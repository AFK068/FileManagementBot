using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
namespace FileManagementBot;

/// <summary>
/// Class responsible for managing the Telegram bot's logic.
/// </summary>
internal class TelegramBotLogics
{
    // Telegram bot token.
    private string _telegramBotToken = "7152407194:AAF14ffPSVRlF68aZcXSj_ZXZKrpETNqSDM";
    
    // Instance of UserInteractionProcessing for handling user interactions.
    private readonly UserInteractionProcessing _userInteractionProcessing = new UserInteractionProcessing();
    
    /// <summary>
    /// Enumeration representing different states of bot interactions.
    /// </summary>
    internal enum StatesEnum
    {
        Message,
        Document,
        Filter
    }
    
    /// <summary>
    /// Starts the Telegram bot.
    /// </summary>
    internal void StartBot()
    {
        var botClient = new TelegramBotClient(_telegramBotToken);
        using CancellationTokenSource cts = new ();
        
        ReceiverOptions receiverOptions = new ()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        
        botClient.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions: receiverOptions, cancellationToken: cts.Token);
        
        Console.ReadLine();
        cts.Cancel();
    }
    
    /// <summary>
    /// Handles updates asynchronously.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="update">The update received from Telegram.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            // Define a different state for each user.
            StatesEnum сurrentState = UserStateManager.GetInstance().GetUserState(GetChatId(update));
            
            // Process messages outside of CallbackQuery and documents.
            if (update.Type == UpdateType.Message && update?.Message?.Text != null && сurrentState == StatesEnum.Message)
            {
                await ProcessingUserData.GetInstance().HandleMessage(botClient, update.Message);
                
                Logging.GetLogger()!.LogInformation("Message chat id : {0} ; User sent a message : {1} ", update.Message.Chat.Id, update.Message.Text);
                return;
            }
            
            // Process messages for filtering.
            if (update.Type == UpdateType.Message && update?.Message?.Text != null && сurrentState == StatesEnum.Filter)
            {
                await ProcessingUserData.GetInstance().HandleMessageForFilter(botClient, update.Message);
                
                Logging.GetLogger()!.LogInformation("Message chat id : {0} ; User sent a message for filtering : {1} ", update.Message.Chat.Id, update.Message.Text);
                return;
            }

            if (update.Type == UpdateType.Message && update?.Message?.Text != null && сurrentState == StatesEnum.Document)
            {
                await ProcessingUserData.GetInstance().HandleMessageInMenu(botClient, update.Message);
                
                Logging.GetLogger()!.LogInformation("Message chat id : {0} ; User sent a message during menu processing : {1} ", update.Message.Chat.Id, update.Message.Text);
                return;
            }
        
            // Process CallbackQuery for documents only.
            if (update.Type == UpdateType.CallbackQuery && (сurrentState == StatesEnum.Document || сurrentState == StatesEnum.Filter))
            {
                await _userInteractionProcessing.HandleCallbackQuery(botClient, update.CallbackQuery!);
                return;
            }

            // Document processing.
            if (update.Message != null && update.Message.Document != null)
            { 
                UserStateManager.GetInstance().UsersStates[update.Message.Chat.Id] = StatesEnum.Document;
                Logging.GetLogger()!.LogInformation("Message chat id: {0} ; User sent a document : {1} ",update.Message.Chat.Id, update.Message.Document.FileName);
                
                var data = await _userInteractionProcessing.SaveAndParseDocument(botClient, update, cancellationToken);
            
                if (data == null)
                    return;

                UserStateManager.GetInstance().GetUserGasStationObjects(update.Message.Chat.Id, data);
                await HandleInlineKeyboard.GetInstance().ProvideChoiceSortOrFilter(botClient, update.Message, update.CallbackQuery!);
            }   
        }
        catch (Exception)
        {
            // If an error occurs, do not process the buttons.
            UserStateManager.GetInstance().UsersStates[update.Message!.Chat.Id] = StatesEnum.Message;
            Logging.GetLogger()!.LogError("Bot id : {0} ; Message chat id : {1} ; Error while processing commands.", botClient.BotId, update.Message.Chat.Id);
        }
    }
    
    /// <summary>
    /// Get chat id.
    /// </summary>
    /// <param name="update">The update received from Telegram.</param>
    /// <returns>User chat id.</returns>
    private long GetChatId(Update update)
    {
        long chatId;
        
        if (update.Message != null)
        {
            chatId = update.Message.Chat.Id;
        }
        else if (update.CallbackQuery != null)
        {
            chatId = update.CallbackQuery.Message!.Chat.Id;
        }
        else
        {
            chatId = 0;
        }

        return chatId;
    }
    
    /// <summary>
    /// Handles errors that occur during polling in the Telegram bot.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="exception">The exception that occurred during polling.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException 
                => $"Ошибка в работе с API: \n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        
        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}
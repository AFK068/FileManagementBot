using Telegram.Bot;
using Telegram.Bot.Types;
using FileWork;
namespace FileManagementBot;

/// <summary>
/// Handles general commands related to file management in the Telegram bot.
/// </summary>
internal class HandleGeneralCommands
{
    // Singleton instance of HandleGeneralCommands.
    private static HandleGeneralCommands _sHandleGeneralCommands;
    
    /// <summary>
    /// Gets the singleton instance of HandleGeneralCommands.
    /// </summary>
    /// <returns>The singleton instance of HandleGeneralCommands.</returns>
    internal static HandleGeneralCommands GetInstance()
    {
        if (_sHandleGeneralCommands == null)
            _sHandleGeneralCommands = new HandleGeneralCommands();

        return _sHandleGeneralCommands;
    }
    
    /// <summary>
    /// Handles sorting based on user's choice.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleSort(ITelegramBotClient botClient, Message message)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceSideSorting(botClient, message);
    }
    
    /// <summary>
    /// Handles filtering based on user's choice.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleFilter(ITelegramBotClient botClient, Message message)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceFiledForFilter(botClient, message);
    }
    
    /// <summary>
    /// Handles sorting in ascending order based on user's choice.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleSortAscending(ITelegramBotClient botClient, Message message)
    {
        UserStateManager.GetInstance().usersGasStationsLastUpdate[message.Chat.Id] = 
            DataManager.GetInstance().SortObjects(UserStateManager.GetInstance().usersGasStationObjects[message.Chat.Id], DataManager.GasStationEnum.TestDate);
        
        await HandleInlineKeyboard.GetInstance().ProvideChoiceFileFormatForSort(botClient, message);
    }
    
    /// <summary>
    /// Handles sorting in descending order based on user's choice.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleSortDescending(ITelegramBotClient botClient, Message message)
    {
        UserStateManager.GetInstance().usersGasStationsLastUpdate[message.Chat.Id] = 
            DataManager.GetInstance().SortObjects(UserStateManager.GetInstance().usersGasStationObjects[message.Chat.Id], DataManager.GasStationEnum.TestDate, true);
        
        await HandleInlineKeyboard.GetInstance().ProvideChoiceFileFormatForSort(botClient, message);
    }
    
    /// <summary>
    /// Handles filtering based on District field.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleFilterDistrict(ITelegramBotClient botClient, Message message)
    {
        UserStateManager.GetInstance().UsersStates[message.Chat.Id] = TelegramBotLogics.StatesEnum.Filter;
        await botClient.SendTextMessageAsync(message.Chat.Id, "\ud83d\udd0e Введите данные для фильтрации по полю District:");
        
        UserStateManager.GetInstance().GetUserGasStationFirstEnum(message.Chat.Id,DataManager.GasStationEnum.District );
        UserStateManager.GetInstance().GetUserGasStationSecondEnum(message.Chat.Id, DataManager.GasStationEnum.None);
    }
          
    /// <summary>
    /// Handles filtering based on Owner field.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleFilterOwner(ITelegramBotClient botClient, Message message)
    {
        UserStateManager.GetInstance().UsersStates[message.Chat.Id] = TelegramBotLogics.StatesEnum.Filter;
        await botClient.SendTextMessageAsync(message.Chat.Id, "\ud83d\udd0e Введите данные для фильтрации по полю Owner:");

        UserStateManager.GetInstance().GetUserGasStationFirstEnum(message.Chat.Id, DataManager.GasStationEnum.Owner);
        UserStateManager.GetInstance().GetUserGasStationSecondEnum(message.Chat.Id, DataManager.GasStationEnum.None);
    }
    
    /// <summary>
    /// Handles filtering based on AdmArea and Owner fields.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleFilterAdmAreaAndOwner(ITelegramBotClient botClient, Message message)
    {
        UserStateManager.GetInstance().UsersStates[message.Chat.Id] = TelegramBotLogics.StatesEnum.Filter;
        await botClient.SendTextMessageAsync(message.Chat.Id,"Введите данные для фильтрации по полям AdmArea и Owner \ud83d\udd0e " + 
                                                             ":\nПервая строка поле AdmArea\nВторая строка поле Owner");

        UserStateManager.GetInstance().GetUserGasStationFirstEnum(message.Chat.Id, DataManager.GasStationEnum.AdmArea);
        UserStateManager.GetInstance().GetUserGasStationSecondEnum(message.Chat.Id, DataManager.GasStationEnum.Owner);
    }
    
    /// <summary>
    /// Handles sending a JSON file.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleSendJsonFile(ITelegramBotClient botClient, Message message)
    {
        await DocumentProcessing.SendDocumentJsonFile(botClient, message, UserStateManager.GetInstance().usersGasStationsLastUpdate[message.Chat.Id]);
    }

    /// <summary>
    /// Handles sending a CSV file.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleSendCsvFile(ITelegramBotClient botClient, Message message)
    {
        await DocumentProcessing.SendDocumentCsvFile(botClient, message, UserStateManager.GetInstance().usersGasStationsLastUpdate[message.Chat.Id]);
    }
    
    /// <summary>
    /// Handles going back to the previous step in the bot's logic.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleBackToPreviousStep(ITelegramBotClient botClient, Message message)
    {
        UserStateManager.GetInstance().UsersStates[message.Chat.Id] = TelegramBotLogics.StatesEnum.Document;
        
        var inlineKeyboard = await MenuStack.GetInstance().PopInlineKeyboardMarkupFromMenuStack();
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Выбирите действие:", replyMarkup: inlineKeyboard);
    }
    
    /// <summary>
    /// Handles the selection of file format for universal sort and performs sorting accordingly.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    /// <param name="reverse">Indicates whether to reverse the sorting order.</param>
    internal async Task HandleFileFormatToDownloadForUniversalSort(ITelegramBotClient botClient, Message message, bool reverse)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceFileFormatForSort(botClient, message);

        UserStateManager.GetInstance().usersGasStationsLastUpdate[message.Chat.Id] = 
            DataManager.GetInstance().SortObjects(UserStateManager.GetInstance().usersGasStationObjects[message.Chat.Id], 
            UserStateManager.GetInstance().usersLastFieldForSort[message.Chat.Id], reverse);
    }

    /// <summary>
    /// Handles the selection of sort side for a specific field and performs sorting accordingly.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    /// <param name="field">The gas station enum field for sorting.</param>
    internal async Task HandleSortSide(ITelegramBotClient botClient, Message message, DataManager.GasStationEnum field)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceSortSide(botClient, message);
        UserStateManager.GetInstance().usersGasStationsLastUpdate[message.Chat.Id] = 
            DataManager.GetInstance().SortObjects(UserStateManager.GetInstance().usersGasStationObjects[message.Chat.Id], field, true);
        
        UserStateManager.GetInstance().GetUserLastFieldForSort(message.Chat.Id, field);
    }
    
    /// <summary>
    /// Handles the selection of universal sort and provides choices for all sort fields.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task HandleUniversalSort(ITelegramBotClient botClient, Message message)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceForAllSortFields(botClient, message);
    }
}
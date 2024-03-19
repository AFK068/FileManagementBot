using FileWork;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace FileManagementBot;

/// <summary>
/// Class responsible for processing user interactions.
/// </summary>
internal class UserInteractionProcessing
{
    // Array containing fields of gas stations for processing.
    private DataManager.GasStationEnum[] _fieldsOfGasStation = ((DataManager.GasStationEnum[])Enum.GetValues(typeof(DataManager.GasStationEnum)))
        .Take(Enum.GetValues(typeof(DataManager.GasStationEnum)).Length - 1).ToArray();
    
    
    /// <summary>
    /// Generates a handler for universal filter field.
    /// </summary>
    /// <param name="field">Gas station field for filtering.</param>
    /// <returns>Handler for universal filter field.</returns>
    private Func<ITelegramBotClient, Message, Task> GenerateUniversalFilterFieldHandler(DataManager.GasStationEnum field)
    {
        return (botClient, message) => HandleUniversalFilterCommands.GetInstance().HandleFirstFieldMoreDetailedFiltering(botClient, message, field);
    }
    
    /// <summary>
    /// Generates a handler for universal sort field.
    /// </summary>
    /// <param name="field">Gas station field for sorting.</param>
    /// <returns>Handler for universal sort field.</returns>
    private Func<ITelegramBotClient, Message, Task> GenerateUniversalSortFieldHandler(DataManager.GasStationEnum field)
    {
        return (botClient, message) => HandleGeneralCommands.GetInstance().HandleSortSide(botClient, message, field);
    }
    
    /// <summary>
    /// Generates a handler for universal filter for second field.
    /// </summary>
    /// <param name="field">Gas station field for filtering.</param>
    /// <returns>Handler for universal filter for second field.</returns>
    private Func<ITelegramBotClient, Message, Task> GenerateUniversalFilterForSecondFieldHandler(DataManager.GasStationEnum field)
    {
        return (botClient, message) => HandleUniversalFilterCommands.GetInstance().HandleSecondFieldMoreDetailedFiltering(botClient, message, field);
    }
    
    /// <summary>
    /// Saves and parses the document received from user.
    /// </summary>
    /// <param name="botClient">Telegram bot client instance.</param>
    /// <param name="update">Update object containing the document information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of gas stations parsed from the document.</returns>
    internal async Task<List<GasStation>> SaveAndParseDocument(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var fileId = update.Message.Document.FileId;
        var fileInfo = await botClient.GetFileAsync(fileId);
        var filePath = fileInfo.FilePath;

        // Build the path to the file.
        var documentName = "Downloaded files";
        var destinationFilePath = Path.Combine(documentName, $"{update.Message.Document.FileName}");

        TelegramBotDataValidator telegramBotDataValidator = new TelegramBotDataValidator();
        
        // Check file extension and get fileExtension.
        (bool correctFileFormatExtension, string fileExtension) = await telegramBotDataValidator.CheckFileExtension(botClient, update, destinationFilePath);

        if (correctFileFormatExtension)
        {
            try
            {
                var data = await telegramBotDataValidator.ProcessValidFile(botClient, filePath, destinationFilePath, fileExtension, cancellationToken);
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "\u2705 Файл успешно обработан.", replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);

                return data;
            }
            catch (Exception ex)
            {
                Logging.GetLogger()!.LogError("Bot id : {0} ; Error while processing file. ; File path : {1} ", botClient.BotId, filePath);
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, ex.Message, cancellationToken: cancellationToken);
            }
        }

        return null!;
    }
    
    /// <summary>
    /// Handles callback queries received from user interactions.
    /// </summary>
    /// <param name="botClient">Telegram bot client instance.</param>
    /// <param name="callbackQuery">Callback query object.</param>
    internal async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        var commandHandlers = new Dictionary<string, Func<ITelegramBotClient, Message, Task>>
        {
            // Separate filtering and sorting.
            { "Sorting", HandleGeneralCommands.GetInstance().HandleSort },
            { "Filtration", HandleGeneralCommands.GetInstance().HandleFilter },
            
            // Sorting by two fields.
            { "SortTestDateAscending", HandleGeneralCommands.GetInstance().HandleSortAscending },
            { "SortTestDateDescending", HandleGeneralCommands.GetInstance().HandleSortDescending },
            
            // Filter by three fields.
            { "FilterDistrict", HandleGeneralCommands.GetInstance().HandleFilterDistrict },
            { "FilterOwner", HandleGeneralCommands.GetInstance().HandleFilterOwner},
            { "FilterAdmAreaAndOwner", HandleGeneralCommands.GetInstance().HandleFilterAdmAreaAndOwner },
            
            // Back commands and file format.
            { "Back", HandleGeneralCommands.GetInstance().HandleBackToPreviousStep },
            { "SendJSONFile", HandleGeneralCommands.GetInstance().HandleSendJsonFile },
            { "SendCSVFile", HandleGeneralCommands.GetInstance().HandleSendCsvFile },
            
            // Universal sorting.
            { "UniversalSort", HandleGeneralCommands.GetInstance().HandleUniversalSort },
            { "SortAscendingForUniversalSide", (botClient, message) => HandleGeneralCommands.GetInstance().HandleFileFormatToDownloadForUniversalSort(botClient, message, false) },
            { "SortDescendingForUniversalSide", (botClient, message) => HandleGeneralCommands.GetInstance().HandleFileFormatToDownloadForUniversalSort(botClient, message, true)},
            
            // Universal filtering.
            { "MoreDetailedFiltering", HandleUniversalFilterCommands.GetInstance().HandleMoreDetailedFiltering },
            { "UniversalFilterTheSameField", (botClient, message) => HandleUniversalFilterCommands.GetInstance().HandleSecondFieldMoreDetailedFiltering(botClient, message, HandleUniversalFilterCommands.s_firstFilterField) }
        };
        
        // Generate universal sort field.
        foreach (DataManager.GasStationEnum field in _fieldsOfGasStation)
        {
            commandHandlers.Add($"UniversalSortField_{field}", GenerateUniversalSortFieldHandler(field));
        }
        
        // Generate universal filter field.
        foreach (DataManager.GasStationEnum field in _fieldsOfGasStation)
        {
            commandHandlers.Add($"UniversalFilterField_{field}", GenerateUniversalFilterFieldHandler(field));
        }
        
        // Generate universal filter for second field.
        foreach (DataManager.GasStationEnum field in _fieldsOfGasStation)
        {
            commandHandlers.Add($"UniversalFilterSecondField_{field}", GenerateUniversalFilterForSecondFieldHandler(field));
        }
        
        // Handle function calls.
        if (commandHandlers.TryGetValue(callbackQuery.Data, out var handler))
        {
            try
            {
                await handler(botClient, callbackQuery.Message);
                Logging.GetLogger()!.LogInformation("Bot id : {0} ; The user clicked: {1}", botClient.BotId, callbackQuery.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Неизвестная команда: {callbackQuery.Data}");
        }
    }
}
using Telegram.Bot;
using Telegram.Bot.Types;
using FileWork;
namespace FileManagementBot;

/// <summary>
/// Handles general commands related to file management in the Telegram bot.
/// </summary>
public class HandleGeneralCommands
{
    // Represents the last field used for sorting.
    private  DataManager.GasStationEnum s_lastField;
    
    // Singleton instance of HandleGeneralCommands.
    private static HandleGeneralCommands _sHandleGeneralCommands;
    
    // List of gas stations.
    internal static List<GasStation> gasStations;
    
    // List of gas stations after the last update.
    internal static List<GasStation> gasStationsLastUpdate;
    
    /// <summary>
    /// Gets the singleton instance of HandleGeneralCommands.
    /// </summary>
    /// <returns>The singleton instance of HandleGeneralCommands.</returns>
    public static HandleGeneralCommands GetInstance()
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
        gasStationsLastUpdate = DataManager.GetInstance().SortObjects(gasStations, DataManager.GasStationEnum.TestDate);
        
        await HandleInlineKeyboard.GetInstance().ProvideChoiceFileFormatForSort(botClient, message);
    }
    
    /// <summary>
    /// Handles sorting in descending order based on user's choice.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleSortDescending(ITelegramBotClient botClient, Message message)
    {
        gasStationsLastUpdate = DataManager.GetInstance().SortObjects(gasStations, DataManager.GasStationEnum.TestDate, true);
        
        await HandleInlineKeyboard.GetInstance().ProvideChoiceFileFormatForSort(botClient, message);
    }
    
    /// <summary>
    /// Handles filtering based on District field.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleFilterDistrict(ITelegramBotClient botClient, Message message)
    {
        TelegramBotLogics.сurrentState = TelegramBotLogics.StatesEnum.Filter;
        await botClient.SendTextMessageAsync(message.Chat.Id, "\ud83d\udd0e Введите данные для фильтрации по полю District:");
        
        ProcessingUserData.gasStationFirstEnum = DataManager.GasStationEnum.District;
        ProcessingUserData.gasStationSecondEnum = null;
    }
          
    /// <summary>
    /// Handles filtering based on Owner field.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleFilterOwner(ITelegramBotClient botClient, Message message)
    {
        TelegramBotLogics.сurrentState = TelegramBotLogics.StatesEnum.Filter;
        await botClient.SendTextMessageAsync(message.Chat.Id, "\ud83d\udd0e Введите данные для фильтрации по полю Owner:");
        
        ProcessingUserData.gasStationFirstEnum = DataManager.GasStationEnum.Owner;
        ProcessingUserData.gasStationSecondEnum = null;
    }
    
    /// <summary>
    /// Handles filtering based on AdmArea and Owner fields.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleFilterAdmAreaAndOwner(ITelegramBotClient botClient, Message message)
    {
        TelegramBotLogics.сurrentState = TelegramBotLogics.StatesEnum.Filter;
        await botClient.SendTextMessageAsync(message.Chat.Id,"Введите данные для фильтрации по полям AdmArea и Owner \ud83d\udd0e " + 
                                                             ":\nПервая строка поле AdmArea\nВторая строка поле Owner");
        
        ProcessingUserData.gasStationFirstEnum = DataManager.GasStationEnum.AdmArea;
        ProcessingUserData.gasStationSecondEnum = DataManager.GasStationEnum.Owner;
    }
    
    /// <summary>
    /// Handles sending a JSON file.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleSendJsonFile(ITelegramBotClient botClient, Message message)
    {
        await DocumentProcessing.SendDocumentJsonFile(botClient, message, gasStationsLastUpdate);
    }

    /// <summary>
    /// Handles sending a CSV file.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleSendCsvFile(ITelegramBotClient botClient, Message message)
    {
        await DocumentProcessing.SendDocumentCsvFile(botClient, message, gasStationsLastUpdate);
    }
    
    /// <summary>
    /// Handles going back to the previous step in the bot's logic.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleBackToPreviousStep(ITelegramBotClient botClient, Message message)
    {
        TelegramBotLogics.сurrentState = TelegramBotLogics.StatesEnum.Document;
        
        var inlineKeyboard = await MenuStack.GetInstance().PopInlineKeyboardMarkupFromMenuStack();
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Выбирите действие:", replyMarkup: inlineKeyboard);
    }
    
    /// <summary>
    /// Handles sorting based on user's choice of file format.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    /// <param name="reverse">Specifies whether sorting should be in reverse order.</param>
    internal async Task HandleFileFormatToDownloadForUniversalSort(ITelegramBotClient botClient, Message message, bool reverse)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceFileFormatForSort(botClient, message);

        gasStationsLastUpdate = DataManager.GetInstance().SortObjects(gasStations, s_lastField, reverse);
    }
    
    /// <summary>
    /// Handles sorting based on user's choice of sort side.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    /// <param name="field">The field to sort by.</param>
    internal async Task HandleSortSide(ITelegramBotClient botClient, Message message, DataManager.GasStationEnum field)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceSortSide(botClient, message);

        gasStationsLastUpdate = DataManager.GetInstance().SortObjects(gasStations, field, true);
        s_lastField = field;
    }
    
    /// <summary>
    /// Handles providing choices for universal sorting.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleUniversalSort(ITelegramBotClient botClient, Message message)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceForAllSortFields(botClient, message);
    }
}
using Telegram.Bot;
using Telegram.Bot.Types;
using FileWork;
namespace FileManagementBot;

/// <summary>
/// Handles universal filter commands in the Telegram bot.
/// </summary>
public class HandleUniversalFilterCommands
{
    // Represents the first filter field.
    internal static DataManager.GasStationEnum s_firstFilterField;
    
    // Singleton instance of HandleUniversalFilterCommands.
    private static HandleUniversalFilterCommands _sHandleUniversalFilterCommands;
    
    /// <summary>
    /// Gets the singleton instance of HandleUniversalFilterCommands.
    /// </summary>
    /// <returns>The singleton instance of HandleUniversalFilterCommands.</returns>
    public static HandleUniversalFilterCommands GetInstance()
    {
        if (_sHandleUniversalFilterCommands == null)
            _sHandleUniversalFilterCommands = new HandleUniversalFilterCommands();

        return _sHandleUniversalFilterCommands;
    }
    
    /// <summary>
    /// Handles more detailed filtering.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleMoreDetailedFiltering(ITelegramBotClient botClient, Message message)
    {
        await HandleInlineKeyboard.GetInstance().ProvideChoiceUniversalFilterForFirstField(botClient, message);
    }
    
    /// <summary>
    /// Handles more detailed filtering for the first field.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    /// <param name="field">The field to filter by.</param>
    internal async Task HandleFirstFieldMoreDetailedFiltering(ITelegramBotClient botClient, Message message, DataManager.GasStationEnum field)
    {
        ProcessingUserData.gasStationFirstEnum = field;

        await HandleInlineKeyboard.GetInstance().ProvideChoiceUniversalFilterForSecondField(botClient, message);
    }
    
    /// <summary>
    /// Handles more detailed filtering for the second field.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received from the user.</param>
    /// <param name="field">The field to filter by.</param>
    internal async Task HandleSecondFieldMoreDetailedFiltering(ITelegramBotClient botClient, Message message, DataManager.GasStationEnum field)
    {
        ProcessingUserData.gasStationSecondEnum = field;
        TelegramBotLogics.сurrentState = TelegramBotLogics.StatesEnum.Filter;

        if (ProcessingUserData.gasStationFirstEnum == ProcessingUserData.gasStationSecondEnum)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, $"\ud83d\udd0e Введите данные для фильтрации по полю {field}");
            return;
        }
        
        await botClient.SendTextMessageAsync(message.Chat.Id, $"Введите данные для фильтрации по полям {ProcessingUserData.gasStationFirstEnum} и " +
                                                              $"{ProcessingUserData.gasStationSecondEnum } \ud83d\udd0e " + 
                                                              $":\nПервая строка поле {ProcessingUserData.gasStationFirstEnum}\nВторая строка поле {ProcessingUserData.gasStationSecondEnum }");
    }
}
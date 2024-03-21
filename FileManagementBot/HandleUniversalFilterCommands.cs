using Telegram.Bot;
using Telegram.Bot.Types;
using FileWork;
namespace FileManagementBot;

/// <summary>
/// Handles universal filter commands in the Telegram bot.
/// </summary>
internal class HandleUniversalFilterCommands
{
    // Singleton instance of HandleUniversalFilterCommands.
    private static HandleUniversalFilterCommands s_handleUniversalFilterCommand;
    
    /// <summary>
    /// Gets the singleton instance of HandleUniversalFilterCommands.
    /// </summary>
    /// <returns>The singleton instance of HandleUniversalFilterCommands.</returns>
    internal static HandleUniversalFilterCommands GetInstance()
    {
        if (s_handleUniversalFilterCommand == null)
            s_handleUniversalFilterCommand = new HandleUniversalFilterCommands();

        return s_handleUniversalFilterCommand;
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
        UserStateManager.GetInstance().GetUserGasStationFirstEnum(message.Chat.Id, field);

        await HandleInlineKeyboard.GetInstance().ProvideChoiceUniversalFilterForSecondField(botClient, message);
    }
    
    /// <summary>
    /// Handles more detailed filtering based on the second field of a gas station enum.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    /// <param name="field">The gas station enum field.</param>
    internal async Task HandleSecondFieldMoreDetailedFiltering(ITelegramBotClient botClient, Message message, DataManager.GasStationEnum field)
    {
        UserStateManager.GetInstance().GetUserGasStationSecondEnum(message.Chat.Id, field);
        UserStateManager.GetInstance().UsersStates[message.Chat.Id] = TelegramBotLogics.StatesEnum.Filter;

        if (UserStateManager.GetInstance().UsersGasStationFirstEnumForFilter[message.Chat.Id] == UserStateManager.GetInstance().UsersGasStationSecondEnumForFilter[message.Chat.Id])
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, $"\ud83d\udd0e Введите данные для фильтрации по полю {field}");
            return;
        }
        
        await botClient.SendTextMessageAsync(message.Chat.Id, $"Введите данные для фильтрации по полям {UserStateManager.GetInstance().UsersGasStationFirstEnumForFilter[message.Chat.Id]} и " +
                                                              $"{UserStateManager.GetInstance().UsersGasStationSecondEnumForFilter[message.Chat.Id] } \ud83d\udd0e " + 
                                                              $":\nПервая строка поле {UserStateManager.GetInstance().UsersGasStationFirstEnumForFilter[message.Chat.Id]}\nВторая ст" +
                                                              $"рока поле {UserStateManager.GetInstance().UsersGasStationSecondEnumForFilter[message.Chat.Id] }");
    }
}
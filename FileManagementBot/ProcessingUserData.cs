using FileWork;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace FileManagementBot;

/// <summary>
/// Class responsible for processing user data and managing interactions with the Telegram bot.
/// </summary>
internal class ProcessingUserData
{
    // Singleton instance of the ProcessingUserData class.
    private static ProcessingUserData s_ProcessingUserData;

    // String containing the welcome message for the bot.
    private string _stringForMessageStart = "\ud83d\udc4b Здравствуйте, я телеграм бот \ud83e\udd16 способоный производить выборку по полям и сортировать файлы определенного формата.\n\n" +
                                          "\ud83d\udccd Для более подробной инструкции по использованию отправьте сообщение /help\n\n" +
                                          "\u25b6\ufe0f Для начала обработки пришлите файл формата .csv или .json.";

    // String containing the help message for the bot.
    private string _stringForMessageHelp = "\ud83d\udccd После того как вы отправите файл, он пройдет проверку на корректность.\n\n" +
                                           "\ud83d\udee0 Корректный формат заголовков для формата .csv:\n\n" +
                                           "1\ufe0f\u20e3 Первая строка:\n" +
                                           "\"ID\";\"FullName\";\"global_id\";\"ShortName\";\"AdmArea\";\"District\";\"Address\";\"Owner\";\"TestDate\";\"geodata_center\";\"geoarea\";\n\n" +
                                           "2\ufe0f\u20e3 Вторая строка:\n" +
                                           "\"Код\";\"Полное официальное наименование\";\"global_id\";\"Сокращенное наименование\";\"Административный округ\";\"Район\";\"Адрес\";\"На" +
                                           "именование компании\";\"Дата проверки\";\"geodata_center\";\"geoarea\";\n\n" +
                                           "\u2705 После успешной обработки вам будет предоставлено меню с действиями.\n\n" +
                                           "\u25b6\ufe0f При вызове меню ваши сообщения не будут обрабатываться, только если вы захотите произвести фильтрацию.\n\n" +
                                           "\u25c0\ufe0f В каждый момент вы можете вернуться назад и обработать последний отправленный файл другими функциями.\n\n" +
                                           "\ud83d\uddc2 При выборе формата файла вы можете получить обработанный файл в 2 форматах: .json и .csv.\n\n" +
                                           "\u2764\ufe0f Приятного использования, With love from AFK1";
    
    /// <summary>
    /// Gets the singleton instance of the ProcessingUserData class.
    /// </summary>
    /// <returns>The singleton instance of the ProcessingUserData class.</returns>
    internal static ProcessingUserData GetInstance()
    {
        if (s_ProcessingUserData == null)
            s_ProcessingUserData = new ProcessingUserData();

        return s_ProcessingUserData;
    }
    
    /// <summary>
    /// Sends information about invalid input to the user.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="message">The message from the user.</param>
    private async Task SendInformationAboutWrongInput(ITelegramBotClient botClient, Message message)
    {
        ReplyKeyboardMarkup keyboard = new(new[]
        {
            new KeyboardButton[] { "/start", "/help" }
        })
        {
            ResizeKeyboard = true
        };
        
        await botClient.SendTextMessageAsync(message.Chat.Id, "\u274c Неизвестная команда.", replyMarkup: keyboard);
    }
    
    /// <summary>
    /// Handles messages sent by the user.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="message">The message sent by the user.</param>
    internal async Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
        if (message.Text == "/start")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, _stringForMessageStart);
        }
        else if (message.Text == "/help")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, _stringForMessageHelp);
        }
        else
        {
            await SendInformationAboutWrongInput(botClient, message);
        }
    }

    /// <summary>
    /// Handles the message in the menu, providing appropriate responses based on the message content.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="message">The message received from the user.</param>
    internal async Task HandleMessageInMenu(ITelegramBotClient botClient, Message message)
    {
        if (message.Text == "/help")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, _stringForMessageHelp);
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,"\ud83d\uded1 Вы начали взаимодействие с меню, ваши сообщения не обрабатываются.\n\n" +
                                                                 "\ud83d\udd04 Если хотите посмотреть инструкции по использованию отправьте сообщение /help");
        }
    }
    
    /// <summary>
    /// Handles messages sent by the user for filtering.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="message">The message sent by the user.</param>
    internal async Task HandleMessageForFilter(ITelegramBotClient botClient, Message message)
    {
        var messageFromUser = message.Text;
        DataManager dataManage = new DataManager();
        
        if (!string.IsNullOrEmpty(messageFromUser))
        {
            try
            {
                var data = dataManage.FilterObject(UserStateManager.GetInstance().usersGasStationObjects[message.Chat.Id], 
                    UserStateManager.GetInstance().usersgasStationFirstEnumForFilter[message.Chat.Id], messageFromUser, 
                    UserStateManager.GetInstance().usersgasStationSecondEnumForFilter[message.Chat.Id]);
                
                UserStateManager.GetInstance().GetUserGasStationsLastUpdate(message.Chat.Id, data);

                await botClient.SendTextMessageAsync(message.Chat.Id, "Найдены совпадения.");
                await HandleInlineKeyboard.GetInstance().ProvideChoiceFileFormatForFilter(botClient, message);
            }
            catch (Exception ex)
            {
                Logging.GetLogger()!.LogError("Bot id : {0} ; Error while searching for a suitable object ; Message from user : {1} ", botClient.BotId, messageFromUser);
                await botClient.SendTextMessageAsync(message.Chat.Id, $"\u274c {ex.Message}\n\n\ud83d\udd03 Повторите попытку поиска:");
            }
        }
    }
}
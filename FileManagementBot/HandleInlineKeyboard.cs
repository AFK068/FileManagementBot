using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace FileManagementBot;

/// <summary>
/// Handles inline keyboard operations for file management in the Telegram bot.
/// </summary>
internal class HandleInlineKeyboard
{
    // Singleton instance of HandleInlineKeyboard.
    private static HandleInlineKeyboard s_handleInlineKeyboard;
    
    /// <summary>
    /// Gets the singleton instance of HandleInlineKeyboard.
    /// </summary>
    /// <returns>The singleton instance of HandleInlineKeyboard.</returns>
    internal static HandleInlineKeyboard GetInstance()
    {
        if (s_handleInlineKeyboard == null)
            s_handleInlineKeyboard = new HandleInlineKeyboard();

        return s_handleInlineKeyboard;
    }
    
    /// <summary>
    /// Provides choice for sorting or filtering.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    /// <param name="callbackQuery">The callback query received.</param>
    internal async Task ProvideChoiceSortOrFilter(ITelegramBotClient botClient, Message message, CallbackQuery callbackQuery)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Произвести сортировку.", "Sorting"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Произвести фильтрацию.", "Filtration"),
            }
        });
        
        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Выберите действие:", replyMarkup: inlineKeyboard);
        await MenuStack.GetInstance().PushInlineKeyboardMarkupInMenuStack(inlineKeyboard);
    }
    
    /// <summary>
    /// Provides choice for sorting side.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task ProvideChoiceSideSorting(ITelegramBotClient botClient, Message message)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Отсортировать поле TestDate по возрастанию даты.", "SortTestDateAscending"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Отсортировать поле TestDate по убыванию даты.", "SortTestDateDescending"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Перейти ко всем поля.", "UniversalSort"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("\u2b05\ufe0f Вернуться назад \u2b05\ufe0f", "Back"),
            } 
        });
        
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Выберите действие:", replyMarkup: inlineKeyboard);
        await MenuStack.GetInstance().PushInlineKeyboardMarkupInMenuStack(inlineKeyboard);
    }
    
    /// <summary>
    /// Provides choice for filtering field.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task ProvideChoiceFiledForFilter(ITelegramBotClient botClient, Message message)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("District", "FilterDistrict"),
                InlineKeyboardButton.WithCallbackData("Owner", "FilterOwner")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("AdmArea и Owner одновременно", "FilterAdmAreaAndOwner")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Перейти к более подробной фильтрации", "MoreDetailedFiltering"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("\u2b05\ufe0f Вернуться назад \u2b05\ufe0f", "Back"),
            } 
        });
        
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Произвести фильтрацию по полю:", replyMarkup: inlineKeyboard);
        await MenuStack.GetInstance().PushInlineKeyboardMarkupInMenuStack(inlineKeyboard);
    }
    
    /// <summary>
    /// Provides choice for first field in universal filter.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task ProvideChoiceUniversalFilterForFirstField(ITelegramBotClient botClient, Message message)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("ID", "UniversalFilterField_Id"),
                InlineKeyboardButton.WithCallbackData("Full name", "UniversalFilterField_FullName"),
                InlineKeyboardButton.WithCallbackData("Global Id", "UniversalFilterField_GlobalId"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Short name", "UniversalFilterField_ShortName"),
                InlineKeyboardButton.WithCallbackData("AdmArea", "UniversalFilterField_AdmArea"),
                InlineKeyboardButton.WithCallbackData("District", "UniversalFilterField_District"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Address", "UniversalFilterField_Address"),
                InlineKeyboardButton.WithCallbackData("Owner", "UniversalFilterField_Owner"),
                InlineKeyboardButton.WithCallbackData("TestDate", "UniversalFilterField_TestDate"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Geodata center", "UniversalFilterField_GeodataCenter"),
                InlineKeyboardButton.WithCallbackData("Geoarea", "UniversalFilterField_Geoarea")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("\u2b05\ufe0f Вернуться назад \u2b05\ufe0f", "Back")
            }
        });
        
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Выберите первое поле для сортировки.", replyMarkup: inlineKeyboard);
        await MenuStack.GetInstance().PushInlineKeyboardMarkupInMenuStack(inlineKeyboard);
    }
    
    /// <summary>
    /// Provides choice for second field in universal filter.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task ProvideChoiceUniversalFilterForSecondField(ITelegramBotClient botClient, Message message)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("ID", "UniversalFilterSecondField_Id"),
                InlineKeyboardButton.WithCallbackData("Full name", "UniversalFilterSecondField_FullName"),
                InlineKeyboardButton.WithCallbackData("Global Id", "UniversalFilterSecondField_GlobalId"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Short name", "UniversalFilterSecondField_ShortName"),
                InlineKeyboardButton.WithCallbackData("AdmArea", "UniversalFilterSecondField_AdmArea"),
                InlineKeyboardButton.WithCallbackData("District", "UniversalFilterSecondField_District"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Address", "UniversalFilterSecondField_Address"),
                InlineKeyboardButton.WithCallbackData("Owner", "UniversalFilterSecondField_Owner"),
                InlineKeyboardButton.WithCallbackData("TestDate", "UniversalFilterSecondField_TestDate"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Geodata center", "UniversalFilterSecondField_GeodataCenter"),
                InlineKeyboardButton.WithCallbackData("Geoarea", "UniversalFilterSecondField_Geoarea")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("По выбрану полю", "UniversalFilterTheSameField"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("\u2b05\ufe0f Вернуться назад \u2b05\ufe0f", "Back")
            }
        });
        
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Выберите второе поле для фильтрации или только по выбранному полю.", replyMarkup: inlineKeyboard);
        await MenuStack.GetInstance().PushInlineKeyboardMarkupInMenuStack(inlineKeyboard);
    }
    
    /// <summary>
    /// Provides choice for file format in sorting.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task ProvideChoiceFileFormatForSort(ITelegramBotClient botClient, Message message)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("JSON", "SendJSONFile"),
                InlineKeyboardButton.WithCallbackData("CSV", "SendCSVFile")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("\u2b05\ufe0f Вернуться назад \u2b05\ufe0f", "Back")
            }
        });
        
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Выбрать формат файла.", replyMarkup: inlineKeyboard);
        await MenuStack.GetInstance().PushInlineKeyboardMarkupInMenuStack(inlineKeyboard);
    }

    /// <summary>
    /// Provides choice for file format in filtering.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task ProvideChoiceFileFormatForFilter(ITelegramBotClient botClient, Message message)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("JSON", "SendJSONFile"),
                InlineKeyboardButton.WithCallbackData("CSV", "SendCSVFile")
            }
        });
        
        await botClient.SendTextMessageAsync(message.Chat.Id, "Выбрать формат файла.", replyMarkup: inlineKeyboard);
    }
    
    /// <summary>
    /// Provides choice for sorting side in universal filter.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task ProvideChoiceSortSide(ITelegramBotClient botClient, Message message)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("\ud83d\udd3c По возрастанию \ud83d\udd3c",
                    "SortAscendingForUniversalSide"),
                
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("\ud83d\udd3d По убыванию \ud83d\udd3d",
                    "SortDescendingForUniversalSide")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("\u2b05\ufe0f Вернуться назад \u2b05\ufe0f", "Back")
            }
        });
        
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Выберите сторону сортировки.", replyMarkup: inlineKeyboard);
        await MenuStack.GetInstance().PushInlineKeyboardMarkupInMenuStack(inlineKeyboard);
    }
    
    /// <summary>
    /// Provides choice for all sort fields in universal filter.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    internal async Task ProvideChoiceForAllSortFields(ITelegramBotClient botClient, Message message)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("ID", "UniversalSortField_Id"),
                InlineKeyboardButton.WithCallbackData("Full name", "UniversalSortField_FullName"),
                InlineKeyboardButton.WithCallbackData("Global Id", "UniversalSortField_GlobalId"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Short name", "UniversalSortField_ShortName"),
                InlineKeyboardButton.WithCallbackData("AdmArea", "UniversalSortField_AdmArea"),
                InlineKeyboardButton.WithCallbackData("District", "UniversalSortField_District"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Address", "UniversalSortField_Address"),
                InlineKeyboardButton.WithCallbackData("Owner", "UniversalSortField_Owner"),
                InlineKeyboardButton.WithCallbackData("TestDate", "UniversalSortField_TestDate"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Geodata center", "UniversalSortField_GeodataCenter"),
                InlineKeyboardButton.WithCallbackData("Geoarea", "UniversalSortField_Geoarea")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("\u2b05\ufe0f Вернуться назад \u2b05\ufe0f", "Back")
            }
        });
        
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Выбрать поле для сортировки.", replyMarkup: inlineKeyboard);
        await MenuStack.GetInstance().PushInlineKeyboardMarkupInMenuStack(inlineKeyboard);
    }
}
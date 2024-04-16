namespace FileManagementBot;

/// <summary>
/// The entry point class for the File Management Bot application.
/// </summary>
class Program
{
    /// <summary>
    /// The entry point method for the application.
    /// </summary>
    static void Main()
    {   
        TelegramBotLogics telegramBotLogics = new TelegramBotLogics();
        
        Logging logging = new Logging();
        
        telegramBotLogics.StartBot();
    }
    
}

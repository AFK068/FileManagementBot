using Telegram.Bot;
using Telegram.Bot.Types;
using FileWork;
namespace FileManagementBot;

/// <summary>
/// Class responsible for validating and processing data received from Telegram updates.
/// </summary>
public class TelegramBotDataValidator
{
    /// <summary>
    /// Checks if the file extension is valid and sends an error message if it is not.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="update">The update containing information about the message.</param>
    /// <param name="destinationFilePath">The destination file path.</param>
    /// <returns>A tuple containing a boolean indicating whether the file extension is valid and the file extension itself.</returns>
    internal async Task<(bool, string)> CheckFileExtension(ITelegramBotClient botClient, Update update, string destinationFilePath)
    {
        string fileExtension = Path.GetExtension(destinationFilePath);


        if (fileExtension != ".json" && fileExtension != ".csv")
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"\u274c Файлы формата {fileExtension} недопустимы.\nРазрешенные форматы: .json или .csv ");
            return (false, "");
        }
        
        return (true, fileExtension);
    }

    /// <summary>
    /// Processes a valid file based on its extension.
    /// </summary>
    /// <param name="botClient">The Telegram bot client instance.</param>
    /// <param name="filePath">The file path to download from.</param>
    /// <param name="destinationFilePath">The destination file path.</param>
    /// <param name="fileExtension">The extension of the file.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A list of GasStation objects parsed from the file.</returns>
    internal async Task<List<GasStation>> ProcessValidFile(ITelegramBotClient botClient, string filePath, string destinationFilePath, string fileExtension, CancellationToken cancellationToken)
    {
        await using Stream fileStream = System.IO.File.Create(destinationFilePath);
        await botClient.DownloadFileAsync(filePath, fileStream, cancellationToken);

        List<GasStation> gasStations = null;
        
        if (fileExtension.ToLower() == ".csv")
        {
            gasStations = await ProcessCSVFile(fileStream);
        }
        else if (fileExtension.ToLower() == ".json")
        {
            gasStations = await ProcessJSONFile(fileStream);
        }

        return gasStations;
    }
    
    /// <summary>
    /// Processes a CSV file and returns a list of GasStation objects.
    /// </summary>
    /// <param name="fileStream">The stream of the CSV file.</param>
    /// <returns>A list of GasStation objects parsed from the CSV file.</returns>
    private async Task<List<GasStation>> ProcessCSVFile(Stream fileStream)
    {
        CSVProcessing csvProcessing = new CSVProcessing();
        return csvProcessing.Read(fileStream);
    }

    /// <summary>
    /// Processes a JSON file and returns a list of GasStation objects.
    /// </summary>
    /// <param name="fileStream">The stream of the JSON file.</param>
    /// <returns>A list of GasStation objects parsed from the JSON file.</returns>
    private async Task<List<GasStation>> ProcessJSONFile(Stream fileStream)
    {
        JSONProcessing jsonProcessing = new JSONProcessing();
        return jsonProcessing.Read(fileStream);
    }
}
using FileWork;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace FileManagementBot;

/// <summary>
/// Provides methods for processing documents and sending them in various formats.
/// </summary>
internal static class DocumentProcessing
{
    /// <summary>
    /// Sends a JSON file containing gas station data.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    /// <param name="gasStations">The list of gas stations to include in the JSON file.</param>
    internal static async Task SendDocumentJsonFile(ITelegramBotClient botClient, Message message, List<GasStation> gasStations)
    {
        try
        {
            JSONProcessing jsonProcessing = new JSONProcessing();
            Stream stream = jsonProcessing.Write(gasStations);

            await using (stream)
            {
                await botClient.SendDocumentAsync(message.Chat.Id, new InputFileStream(stream, "Updated data.json"), caption: "\ud83d\uddc2 Обновленные данные:");
                Logging.GetLogger()!.LogInformation($"Message chat id : {0} ; File sent by user. ; File name - \"Updated data.json\"", message.Chat.Id);
            }
        }
        catch (Exception ex)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, ex.Message);
            Logging.GetLogger()!.LogError($"Message chat id : {0} ; Error while sending file.", message.Chat.Id);
        }
    }
    
    /// <summary>
    /// Sends a CSV file containing gas station data.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">The message received.</param>
    /// <param name="gasStations">The list of gas stations to include in the CSV file.</param>
    internal static async Task SendDocumentCsvFile(ITelegramBotClient botClient, Message message, List<GasStation> gasStations)
    {
        try
        {
            CSVProcessing csvProcessing = new CSVProcessing();
            Stream stream = csvProcessing.Write(gasStations);

            await using (stream)
            {
                await botClient.SendDocumentAsync(message.Chat.Id, new InputFileStream(stream, "Updated data.csv"), caption: "\ud83d\uddc2 Обновленные данные:"); 
                Logging.GetLogger()!.LogInformation($"Message chat id : {0} ; File sent by user ; File name - \"Updated data.csv\"", message.Chat.Id);
            }
        }
        catch (Exception ex)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, ex.Message);
            Logging.GetLogger()!.LogError($"Message chat id : {0} ; Error while sending file.", message.Chat.Id);
        }
    }
}
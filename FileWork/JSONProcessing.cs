using System.Text.Encodings.Web;
using System.Text.Json;
namespace FileWork;

/// <summary>
/// Provides methods for writing and reading JSON data to/from a stream.
/// </summary>
public class JSONProcessing
{
    /// <summary>
    /// Writes a list of gas stations to a stream as JSON data.
    /// </summary>
    /// <param name="gasStations">The list of gas stations to write.</param>
    /// <returns>The stream containing the JSON data.</returns>
    /// <exception cref="ArgumentException">Thrown when the input list of gas stations is null or empty.</exception>
    /// <exception cref="Exception">Thrown when there is an error serializing the data to the stream.</exception>
    public Stream Write(List<GasStation> gasStations)
    {
        if (gasStations == null || gasStations.Count == 0)
        {
            throw new ArgumentException("Нет ни одного объекта для записи.");
        }
        
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            
            var memoryStream = new MemoryStream();
            JsonSerializer.SerializeAsync(memoryStream, gasStations, options);
            
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        catch (Exception ex)
        {
            throw new Exception("Не удалось преобразовать данные в файл.", ex);
        }
    }
    
    /// <summary>
    /// Reads JSON data from a stream and deserializes it into a list of gas stations.
    /// </summary>
    /// <param name="stream">The stream containing the JSON data.</param>
    /// <returns>The list of gas stations deserialized from the JSON data.</returns>
    /// <exception cref="IOException">Thrown when there is an I/O error while reading from the stream.</exception>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="Exception">Thrown when there is an error deserializing the JSON data or validating it.</exception>
    public  List<GasStation> Read(Stream stream)
    {
        try
        {
            stream.Position = 0;
            using var reader = new StreamReader(stream);

            string jsonData = reader.ReadToEnd();
            List<GasStation>? gasStations = JsonSerializer.Deserialize<List<GasStation>>(jsonData);

            DataValidator dataValidator = new DataValidator();
            
            dataValidator.CheckObjectCount(gasStations);
            dataValidator.CheckStateObjects(gasStations!);

            return gasStations!;
        }
        catch (IOException ex)
        {
            throw new IOException("Ошибка ввода-вывода при чтении файла.", ex);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Файл некорректного формата.", ex);
        }
    }
}
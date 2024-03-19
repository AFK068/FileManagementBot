using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace FileWork;
public class CSVProcessing
{
    // First line headers in csv file.
    private string _firstLineHeaders = "\"ID\";\"FullName\";\"global_id\";\"ShortName\";\"AdmArea\";\"District\";" +
                                       "\"Address\";\"Owner\";\"TestDate\";\"geodata_center\";\"geoarea\";\n";
    
    // Second line headers in csv file.
    private string _secondLineHeaders = "\"Код\";\"Полное официальное наименование\";\"global_id\";\"Сокращенное наименование\";" +
                                        "\"Административный округ\";\"Район\";\"Адрес\";\"Наименование компании\";\"Дата проверки\"" +
                                        ";\"geodata_center\";\"geoarea\";\n";
    
    /// <summary>
    /// Writes gas station data to a CSV file stored in a memory stream.
    /// </summary>
    /// <param name="gasStations">The list of gas stations to write.</param>
    /// <returns>A memory stream containing the CSV file data.</returns>
    /// <exception cref="ArgumentException">Thrown when the list of gas stations is null or empty.</exception>
    /// <exception cref="Exception">Thrown when there is an error converting data to the file format.</exception>
    public Stream Write(List<GasStation>? gasStations)
    {
        if (gasStations == null || gasStations.Count == 0)
        {
            throw new ArgumentException("Нет ни одного объекта для записи.");
        }
        
        var memoryStream = new MemoryStream();
        try
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false,
                ShouldQuote = (ShouldQuoteArgs field) => true
            };
            
            using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
            using (var csvWriter = new CsvWriter(writer, csvConfig))
            {
                // Печетаем заголовки.
                writer.Write(_firstLineHeaders);
                writer.Write(_secondLineHeaders);
                
                csvWriter.WriteRecords(gasStations);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        catch (Exception ex)
        {
            memoryStream.Dispose();
            throw new Exception("Не удалось преобразовать данные в файл.", ex);
        }
    }
    
    /// <summary>
    /// Reads gas station data from a CSV file stored in a memory stream.
    /// </summary>
    /// <param name="stream">The memory stream containing the CSV file data.</param>
    /// <returns>The list of gas stations read from the CSV file.</returns>
    /// <exception cref="IOException">Thrown when there is an I/O error while reading the file.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the provided stream is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the file format is incorrect or there are invalid arguments.</exception>
    /// <exception cref="Exception">Thrown when there is an error reading the file.</exception>
    public List<GasStation> Read(Stream stream)
    {
        try
        {
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false
            });
            
            // Check for headers.
            DataValidator dataValidator = new DataValidator();
            dataValidator.ValidateHeader(reader);
            
            csvReader.Context.RegisterClassMap<GasStationMap>();
            var gasStations =  csvReader.GetRecords<GasStation>().ToList();
            
            // Check for the number of elements.
            dataValidator.CheckObjectCount(gasStations);
            dataValidator.CheckStateObjects(gasStations);
            
            return gasStations;
        }
        catch (IOException ex)
        {
            throw new IOException("Ошибка ввода-вывода при чтении файла.", ex);
        }
        catch (ArgumentNullException ex)
        {
            throw new ArgumentNullException(ex.Message, ex);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Файл некорректного формата.", ex);
        }
    }
    
    /// <summary>
    /// Gas station map.
    /// </summary>
    private sealed class GasStationMap : ClassMap<GasStation>
    {
        public GasStationMap()
        {
            Map(m => m.Id).Name("ID");
            Map(m => m.FullName).Name("FullName");
            Map(m => m.GlobalId).Name("global_id");
            Map(m => m.ShortName).Name("ShortName");
            Map(m => m.AdmArea).Name("AdmArea");
            Map(m => m.District).Name("District");
            Map(m => m.Address).Name("Address");
            Map(m => m.Owner).Name("Owner");
            Map(m => m.TestDate).Name("TestDate").TypeConverterOption.Format("dd.MM.yyyy");
            Map(m => m.GeodataCenter).Name("geodata_center");
            Map(m => m.Geoarea).Name("geoarea");
        }
    }
}
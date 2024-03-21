namespace FileWork;

/// <summary>
/// Provides methods for validating data related to gas stations.
/// </summary>
public class DataValidator
{
    /// <summary>
    /// Checks if the provided list of gas stations is null or empty.
    /// </summary>
    /// <param name="gasStations">The list of gas stations to check.</param>
    /// <exception cref="ArgumentException">Thrown when the list of gas stations is null or empty.</exception>
    internal void CheckObjectCount(List<GasStation>? gasStations)
    {
        if (gasStations == null || gasStations.Count == 0)
        {
            throw new ArgumentException("В файле не найдено ни одного подходящего объекта.");
        }
    }

    /// <summary>
    /// Checks if the provided string contains exactly two words.
    /// </summary>
    /// <param name="filterField">The string to check.</param>
    /// <exception cref="ArgumentException">Thrown when the string does not contain exactly two words.</exception>
    internal void CheckStringForTwoWords(string filterField)
    {
        string[] words = filterField.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
        
        if (words.Length != 2)
        {
            throw new ArgumentException("Строка для фильтрации некорректного формата.");
        }
    }
    
    /// <summary>
    /// Validates the header of a CSV file.
    /// </summary>
    /// <param name="streamReader">The stream reader for the CSV file.</param>
    /// <exception cref="ArgumentNullException">Thrown when the file contains fewer than 2 lines.</exception>
    /// <exception cref="ArgumentException">Thrown when the headers in the file are in an incorrect format.</exception>
    internal void ValidateHeader(StreamReader streamReader)
    {
        string?[] firstTwoLines = new string?[2];
        for (int i = 0; i < 2; i++)
        {
            firstTwoLines[i] = streamReader.ReadLine();
            if (firstTwoLines[i] == null)
            {
                throw new ArgumentNullException(firstTwoLines[i],"Некорректный формат файла: в файле менее 2 строк.");
            }
        }
        
        string expectedFirstRowHeader = "\"ID\";\"FullName\";\"global_id\";\"ShortName\";\"AdmArea\";\"District\";" +
                                        "\"Address\";\"Owner\";\"TestDate\";\"geodata_center\";\"geoarea\";";
            
        string expectedSecondRowHeader = "\"Код\";\"Полное официальное наименование\";\"global_id\";\"Сокращенное наименование\";" +
                                         "\"Административный округ\";\"Район\";\"Адрес\";\"Наименование компании\";\"Дата проверки\";" +
                                         "\"geodata_center\";\"geoarea\";";
        
        if (!(string.Equals(firstTwoLines[0], expectedFirstRowHeader) &&
              string.Equals(firstTwoLines[1], expectedSecondRowHeader)))
        {
            throw new ArgumentException("Заголовки в файле некорректного формата.");
        }
    }

    /// <summary>
    /// Checks if all fields of gas stations are empty or have default values.
    /// </summary>
    /// <param name="gasStations">The list of gas stations to check.</param>
    /// <exception cref="ArgumentException">Thrown when all fields are empty or have default values.</exception>
    internal void CheckStateObjects(List<GasStation> gasStations)
    {
        bool allFieldsIsEmpty = gasStations.All(station =>
        {
            var type = typeof(GasStation);
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType == typeof(int))
                {
                    if ((int)property.GetValue(station)! != 0)
                        return false;
                }
                else if (property.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrEmpty((string)property.GetValue(station)!))
                        return false;
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    if ((DateTime)property.GetValue(station)! != DateTime.MinValue)
                        return false;
                }
            }
            return true;
        });

        bool allFieldsOfTheSameTypeIsEmpty = gasStations.All(station => 
            station.Id == 0 ||
            string.IsNullOrEmpty(station.FullName) ||
            station.GlobalId == 0 ||
            string.IsNullOrEmpty(station.ShortName) ||
            string.IsNullOrEmpty(station.AdmArea) ||
            string.IsNullOrEmpty(station.District) ||
            string.IsNullOrEmpty(station.Address) ||
            string.IsNullOrEmpty(station.Owner) ||
            station.TestDate == DateTime.MinValue);
        
        if (allFieldsIsEmpty)
        {
            throw new ArgumentException("Все поля пусты или имеют значение null.");
        }

        if (allFieldsOfTheSameTypeIsEmpty)
        {
            throw new ArgumentException("Все поля одного типа пусты или имеют значение null");
        }
    }
}
using System.Reflection;
namespace FileWork;

/// <summary>
/// Provides methods for managing gas station data.
/// </summary>
public class DataManager
{
    /// <summary>
    /// Enumeration representing properties of a gas station.
    /// </summary>
    public enum GasStationEnum
    {
        Id,
        FullName,
        GlobalId,
        ShortName,
        AdmArea,
        District,
        Address,
        Owner,
        TestDate,
        GeodataCenter,
        Geoarea,
        AdmAreaAndOwner,
        None
    }
    
    // Provides methods for managing gas station data.
    private static DataManager s_dataManager;
    
    /// <summary>
    /// Gets the singleton instance of the DataManager class.
    /// </summary>
    /// <returns>The singleton instance of the DataManager class.</returns>
    public static DataManager GetInstance()
    {
        if (s_dataManager == null)
            s_dataManager = new DataManager();

        return s_dataManager;
    }
    
    /// <summary>
    /// Sorts the list of gas stations based on the specified filter.
    /// </summary>
    /// <param name="gasStations">The list of gas stations to sort.</param>
    /// <param name="filter">The property to sort by.</param>
    /// <param name="reverse">Indicates whether to sort in reverse order.</param>
    /// <returns>The sorted list of gas stations.</returns>
    /// <exception cref="ArgumentException">Thrown when the list of gas stations is null or empty, or when no field is available for sorting.</exception>
    public List<GasStation> SortObjects(List<GasStation>? gasStations, GasStationEnum filter, bool reverse = false)
    {
        if (gasStations == null || gasStations.Count == 0)
        {
            throw new ArgumentException("Список объектов имеет значение null или пуст.");
        }

        if (gasStations.Any(station => station.TestDate == DateTime.MinValue))
        {
            throw new ArgumentException("Нет ни одного поля для выполнения сортировки.");
        }
        
        PropertyInfo? property = typeof(GasStation).GetProperty(filter.ToString());
        
        if (property == null)
        {
            throw new ArgumentException("Поля с таким названием не существует.");
        }
        
        Func<IEnumerable<GasStation>, IEnumerable<GasStation>> sortMethod = reverse ?
            stations => stations.OrderByDescending(station => property.GetValue(station)) :
            stations => stations.OrderBy(station => property.GetValue(station));

        return sortMethod(gasStations).ToList();
    }
    
    /// <summary>
    /// Filters the list of gas stations based on the specified field and filter value.
    /// </summary>
    /// <param name="gasStations">The list of gas stations to filter.</param>
    /// <param name="field1">The first property to filter by.</param>
    /// <param name="filterField">The value to filter the first property by.</param>
    /// <param name="field2">Optional. The second property to filter by (for two-field filtering).</param>
    /// <returns>The filtered list of gas stations.</returns>
    /// <exception cref="ArgumentException">Thrown when the list of gas stations is null or empty, or when a specified property does not exist in the GasStation class.</exception>
    public List<GasStation> FilterObject(List<GasStation>? gasStations, GasStationEnum field1, string filterField, GasStationEnum field2 = GasStationEnum.None)
    {
        if (gasStations == null || gasStations.Count == 0)
        {
            throw new ArgumentException("Список объектов имеет значение null или пуст.");
        }

        DataValidator dataValidator = new DataValidator();
        List<GasStation> foundedStations = new List<GasStation>();

        PropertyInfo prop1 = typeof(GasStation).GetProperty(field1.ToString())!;
        if (prop1 == null)
        {
            throw new ArgumentException($"Свойство {field1} не найдено в классе GasStation.");
        }

        // Filtering by one field.
        if (field2 == GasStationEnum.None || (field1 == field2))
        {
            var propType = prop1.PropertyType;
            var filterValue = ConvertValue(filterField, propType);

            foundedStations = gasStations.Where(station => prop1.GetValue(station)!.Equals(filterValue)).ToList();
        }
        else
        {
            dataValidator.CheckStringForTwoWords(filterField);
            string[] words = filterField.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            
            PropertyInfo prop2 = typeof(GasStation).GetProperty(field2.ToString()!);
            if (prop2 == null)
            {
                throw new ArgumentException($"Свойство {field2} не найдено в классе GasStation.");
            }
            
            var prop1Type = prop1.PropertyType;
            var prop2Type = prop2.PropertyType;
            var filterValue1 = ConvertValue(words[0], prop1Type);
            var filterValue2 = ConvertValue(words[1], prop2Type);

            // Filtering by two fields.
            foundedStations = gasStations.Where(station => prop1.GetValue(station)!.Equals(filterValue1) && prop2.GetValue(station)!.Equals(filterValue2)).ToList();
        }

        dataValidator.CheckObjectCount(foundedStations);
        return foundedStations;
    }
    
    /// <summary>
    /// Converts the given string value to the specified target type.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="targetType">The type to which the value should be converted.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="ArgumentException">Throw argument exception while uncorrected type massage.</exception>
    private object ConvertValue(string value, Type targetType)
    {
        try
        {
            return Convert.ChangeType(value, targetType);
        }
        catch (FormatException)
        {
            string expectedTypeDescription;
            if (targetType == typeof(int))
            {
                expectedTypeDescription = "целое число";
            }
            else if (targetType == typeof(DateTime))
            {
                expectedTypeDescription = "дата";
            }
            else
            {
                expectedTypeDescription = targetType.Name;
            }

            throw new ArgumentException($"Некорректный формат значения: '{value}'. Ожидалось значение типа : {expectedTypeDescription}.");
        }
    }
}
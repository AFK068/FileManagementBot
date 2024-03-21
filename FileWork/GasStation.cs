using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
namespace FileWork;

/// <summary>
/// Represents a gas station entity.
/// </summary>
public class GasStation
{
    // The unique identifier of the gas station.
    private int _id;
    
    // The full name of the gas station.
    private string _fullName;
    
    // The global identifier of the gas station.
    private int _globalId;
    
    // The short name of the gas station.
    private string _shortName;
    
    // The administrative area of the gas station.
    private string _admArea;
    
    // The district of the gas station.
    private string _district;
    
    // The address of the gas station.
    private string _address;
    
    // The owner of the gas station.
    private string _owner;
    
    // The test date of the gas station.
    private DateTime _testDate;
    
    // The geodata center of the gas station.
    private string _geodataCenter;
    
    // The geoarea of the gas station.
    private string _geoarea;
    
    /// <summary>
    /// Gets or sets the unique identifier of the gas station.
    /// </summary>
    [JsonPropertyName("ID")]
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    
    /// <summary>
    /// Gets or sets the full name of the gas station.
    /// </summary>
    [JsonPropertyName("FullName")]
    public string FullName
    {
        get { return _fullName; }
        set { _fullName = value; }
    }
    
    /// <summary>
    /// Gets or sets the global identifier of the gas station.
    /// </summary>
    [JsonPropertyName("global_id")]
    public int GlobalId
    {
        get { return _globalId; }
        set { _globalId = value; }
    }
    
    /// <summary>
    /// Gets or sets the short name of the gas station.
    /// </summary>
    [JsonPropertyName("ShortName")]
    public string ShortName
    {
        get { return _shortName; }
        set { _shortName = value; }
    }
    
    /// <summary>
    /// Gets or sets the administrative area of the gas station.
    /// </summary>
    [JsonPropertyName("AdmArea")]
    public string AdmArea
    {
        get { return _admArea; }
        set { _admArea = value; }
    }
    
    /// <summary>
    /// Gets or sets the district of the gas station.
    /// </summary>
    [JsonPropertyName("District")]
    public string District
    {
        get { return _district; }
        set { _district = value; }
    }
    
    /// <summary>
    /// Gets or sets the address of the gas station.
    /// </summary>
    [JsonPropertyName("Address")]
    public string Address
    {
        get { return _address; }
        set { _address = value; }
    }
    
    /// <summary>
    /// Gets or sets the owner of the gas station.
    /// </summary>
    [JsonPropertyName("Owner")]
    public string Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }
    
    /// <summary>
    /// Gets or sets the test date of the gas station.
    /// </summary>
    [JsonPropertyName("TestDate")]
    [Format("dd.MM.yyyy")]
    public DateTime TestDate
    {
        get { return _testDate; }
        set { _testDate = value; }
    }
    
    /// <summary>
    /// Gets or sets the geodata center of the gas station.
    /// </summary>
    [JsonPropertyName("geodata_center")]
    public string GeodataCenter
    {
        get { return _geodataCenter; }
        set { _geodataCenter = value; }
    }
    
    /// <summary>
    /// Gets or sets the geoarea of the gas station.
    /// </summary>
    [JsonPropertyName("geoarea")]
    public string Geoarea
    {
        get { return _geoarea; }
        set { _geoarea = value; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GasStation"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the gas station.</param>
    /// <param name="fullName">The full name of the gas station.</param>
    /// <param name="globalId">The global identifier of the gas station.</param>
    /// <param name="shortName">The short name of the gas station.</param>
    /// <param name="admArea">The administrative area of the gas station.</param>
    /// <param name="district">The district of the gas station.</param>
    /// <param name="address">The address of the gas station.</param>
    /// <param name="owner">The owner of the gas station.</param>
    /// <param name="testDate">The test date of the gas station.</param>
    /// <param name="geodataCenter">The geodata center of the gas station.</param>
    /// <param name="geoarea">The geoarea of the gas station.</param>
    public GasStation(int id, string fullName, int globalId, string shortName, string admArea, string district,
        string address, string owner, DateTime testDate, string geodataCenter, string geoarea)
    {
        Id = id;
        FullName = fullName;
        GlobalId = globalId;
        ShortName = shortName;
        AdmArea = admArea;
        District = district;
        Address = address;
        Owner = owner;
        TestDate = testDate;
        GeodataCenter = geodataCenter;
        Geoarea = geoarea;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GasStation"/> class with default values.
    /// </summary>
    public GasStation()
    {
        Id = 0;
        FullName = "";
        GlobalId = 0;
        ShortName = "";
        AdmArea = "";
        District = "";
        Address = "";
        Owner = "";
        TestDate = DateTime.MinValue;
        GeodataCenter = "";
        Geoarea = "";
    }
}
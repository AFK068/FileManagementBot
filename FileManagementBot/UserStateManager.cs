namespace FileManagementBot;
using FileWork;

/// <summary>
/// Manages user states and data for the bot.
/// </summary>
internal class UserStateManager
{
    // Singleton instance of UserStateManager.
    private static UserStateManager s_userStateManager;
        
    // Dictionary all user chat id and they current stage.
    internal Dictionary<long, TelegramBotLogics.StatesEnum> UsersStates { get; set; } = new Dictionary<long, TelegramBotLogics.StatesEnum>();
    
    // Dictionary containing user chat IDs and their last field for sorting.
    internal Dictionary<long, DataManager.GasStationEnum> UsersLastFieldForSort { get; set; } = new Dictionary<long, DataManager.GasStationEnum>();
    
    // Dictionary containing user chat IDs and their gas station objects.
    internal  Dictionary<long, List<GasStation>> UsersGasStationObjects { get; set; } = new Dictionary<long, List<GasStation>>();
    
    // Dictionary containing user chat IDs and their gas stations' last update.
    internal Dictionary<long, List<GasStation>> UsersGasStationsLastUpdate{ get; set; } = new Dictionary<long, List<GasStation>>();
    
    // Dictionary containing user chat IDs and their second gas station enum.
    internal Dictionary<long, DataManager.GasStationEnum> UsersGasStationSecondEnumForFilter { get; set; } = new Dictionary<long, DataManager.GasStationEnum>();
    
    // Dictionary containing user chat IDs and their first gas station enum.
    internal Dictionary<long, DataManager.GasStationEnum> UsersGasStationFirstEnumForFilter { get; set; } = new Dictionary<long, DataManager.GasStationEnum>();
    
    /// <summary>
    /// Gets the singleton instance of UserStateManager.
    /// </summary>
    /// <returns>The singleton instance of UserStateManager.</returns>
    internal static UserStateManager GetInstance()
    {
        if (s_userStateManager == null)
            s_userStateManager = new UserStateManager();

        return s_userStateManager;
    }
    
    /// <summary>
    /// Get user state.
    /// </summary>
    /// <param name="chatId">User chat id.</param>
    /// <returns>Current state.</returns>
    internal TelegramBotLogics.StatesEnum GetUserState(long chatId)
    {
        if (!UsersStates.ContainsKey(chatId))
        {
            UsersStates.Add(chatId, TelegramBotLogics.StatesEnum.Message);
        }
        
        return UsersStates[chatId];
    }
    
    /// <summary>
    /// Gets the user's last field for sorting.
    /// </summary>
    /// <param name="chatId">User chat ID.</param>
    /// <param name="field">The last field used for sorting.</param>
    internal void GetUserLastFieldForSort(long chatId, DataManager.GasStationEnum field)
    {
        if (!UsersLastFieldForSort.ContainsKey(chatId))
        {
            UsersLastFieldForSort.Add(chatId, field);
        }

        {
            UsersLastFieldForSort[chatId] = field;
        }
    }
    
    /// <summary>
    /// Gets the user's gas station objects.
    /// </summary>
    /// <param name="chatId">User chat ID.</param>
    /// <param name="gasStations">List of gas stations associated with the user.</param>
    internal void GetUserGasStationObjects(long chatId, List<GasStation> gasStations)
    {
        if (!UsersGasStationObjects.ContainsKey(chatId))
        {
            UsersGasStationObjects.Add(chatId, gasStations);
        }
        else
        {
            UsersGasStationObjects[chatId] = gasStations;
        }
    }
    
    /// <summary>
    /// Gets the user's gas stations' last update.
    /// </summary>
    /// <param name="chatId">User chat ID.</param>
    /// <param name="gasStations">List of gas stations last updated for the user.</param>
    internal void GetUserGasStationsLastUpdate(long chatId, List<GasStation> gasStations)
    {
        if (!UsersGasStationsLastUpdate.ContainsKey(chatId))
        {
            UsersGasStationsLastUpdate.Add(chatId, gasStations);
        }
        else
        {
            UsersGasStationsLastUpdate[chatId] = gasStations;
        }
    }
    
    /// <summary>
    /// Gets the user's second gas station enum.
    /// </summary>
    /// <param name="chatId">User chat ID.</param>
    /// <param name="field">The second gas station enum for the user.</param>
    internal void GetUserGasStationSecondEnum(long chatId, DataManager.GasStationEnum field)
    {
        if (!UsersGasStationSecondEnumForFilter.ContainsKey(chatId))
        {
            UsersGasStationSecondEnumForFilter.Add(chatId, field);
        }
        
        {
            UsersGasStationSecondEnumForFilter[chatId] = field;
        }
    }
    
    /// <summary>
    /// Gets the user's first gas station enum.
    /// </summary>
    /// <param name="chatId">User chat ID.</param>
    /// <param name="field">The first gas station enum for the user.</param>
    internal void GetUserGasStationFirstEnum(long chatId, DataManager.GasStationEnum field)
    {
        if (!UsersGasStationFirstEnumForFilter.ContainsKey(chatId))
        {
            UsersGasStationFirstEnumForFilter.Add(chatId, field);
        }
        
        {
            UsersGasStationFirstEnumForFilter[chatId] = field;
        }
    }
}
using System.Collections.Generic;

public static class Events
{
    public static LevelChangeEvent LevelChangeEvent = new();
    public static InventoryUpdateEvent InventoryUpdateEvent = new();
    public static FightSceneConfiguredEvent FightSceneConfiguredEvent = new();
    public static DataUpdateEvent DataUpdateEvent = new();
    public static StatsChangeEvent EquipmentChangeEvent = new();
    public static LocatorReadyEvent LocatorReadyEvent = new();
}

public class LevelChangeEvent : GameEvent
{
    public int CurrentXP;
    public int XPForNextLevel;
    public int Level;
}

public class InventoryUpdateEvent : GameEvent
{
    public List<InventoryItem<Item>> Items;
}

public class DataUpdateEvent : GameEvent { }
public class StatsChangeEvent : GameEvent
{
    public PlayerStats Stats;
}
public class FightSceneConfiguredEvent : GameEvent { }

public class LocatorReadyEvent : GameEvent { }

using System;
using System.Collections.Generic;

//public class PlayerData
//{
//    public event Action OnDataChanged;

//    //public PlayerData()
//    //{
//    //    Inventory.OnInventoryUpdate.AddListener();
//    //}

//    private int _level;
//    public int Level
//    {
//        get => _level;
//        set
//        {
//            _level = value;
//            OnDataChanged?.Invoke();
//        }
//    }

//    private int _coins;
//    public int Coins
//    {
//        get => _coins;
//        set
//        {
//            _coins = value;
//            OnDataChanged?.Invoke();
//        }
//    }

//    public Inventory Inventory { get; set; }
//    public InventoryItemData Outerwear { get; set; }
//    public InventoryItemData Pants { get; set; }
//    public InventoryItemData Shoes { get; set; }
//    public InventoryItemData Ring { get; set; }

//    public PlayerDataDto ToDto(string userId)
//    {
//        return new PlayerDataDto
//        {
//            UserID = userId,
//            Items = new List<InventoryItemData>(Items),
//            Level = Level,
//            Coins = Coins,
//            Outerwear = Outerwear,
//            Pants = Pants,
//            Shoes = Shoes,
//            Ring = Ring
//        };
//    }
//}

[Serializable]
public class PlayerDataDto
{
    public string UserID;
    public List<InventoryItemData> Items = new();
    public int Level;
    public int Coins;

    public InventoryItemData Outerwear;
    public InventoryItemData Pants;
    public InventoryItemData Shoes;
    public InventoryItemData Ring;
}

[Serializable]
public class UserRegisterDto
{
    public string UserName;
    public string Email;
    public string Password;
}

[Serializable]
public class UserLoginDto
{
    public string UsernameOrEmail;
    public string Password;
}

[Serializable]
public class ApiRegisterResponse
{
    public bool Success;
    public string Token;
    public PlayerDataDto PlayerData;
    public string UsernameError;
    public string EmailError;
    public string PasswordError;
}

[Serializable]
public class ApiLoginResponse
{
    public bool Success;
    public string Token;
    public PlayerDataDto PlayerData;
    public string Error;
}

[Serializable]
public class ApiUpdateResponce
{
    public bool Success;
    public string Error;
}

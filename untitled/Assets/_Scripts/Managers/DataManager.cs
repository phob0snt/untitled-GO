using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class DataManager : MonoBehaviour
{
    [InjectOptional] private readonly Character _character;
    [InjectOptional] private readonly ViewManager _viewManager;
    [Inject] private readonly GameManager _gameManager;
    private List<Item> _itemsRefs = new();
    private CertificateHandlerPass _pass;

    private string _tokenSavePath => Application.persistentDataPath + "/token.txt";
    private string _dataSavePath => Application.persistentDataPath + "/playerData.json";
    private readonly string _itemsSavePath = "Assets/ScriptableObjects";

    private void OnEnable()
    {
        EventManager.AddListener<DataUpdateEvent>((e) => UpdateData());
        LoginLabel.OnRegister.AddListener(Register);
        LoginLabel.OnLogin.AddListener(Login);
    }
    private void OnDisable()
    {
        EventManager.RemoveListener<DataUpdateEvent>((e) => UpdateData());
        LoginLabel.OnRegister.RemoveListener(Register);
        LoginLabel.OnLogin.RemoveListener(Login);
    }

    private async void Awake()
    {
        _pass = new CertificateHandlerPass();
        Debug.Log(_tokenSavePath);
        await LoadItemRefs();
    }

    public void ReInject(DiContainer container) => container.Inject(this);

    private void Register(UserRegisterDto dto) => StartCoroutine(RegisterProcess(dto));

    private void Login(UserLoginDto dto) => StartCoroutine(LoginProcess(dto));

    private async Task LoadItemRefs()
    {
        AsyncOperationHandle<IList<Item>> handle = Addressables.LoadAssetsAsync<Item>("Items", null);

        await handle.Task;

        if (handle.Status ==  AsyncOperationStatus.Succeeded)
        {
            _itemsRefs.AddRange(handle.Result);
            Debug.Log("Items Loaded");
        }
        else Debug.Log("Failed to load Items");

    }

    //private void SyncPlayerData()
    //{
    //    new PlayerDataDto()
    //    {

    //    };
    //  PlayerData.Items = _dataManager.GetInventoryAsData();
    //  PlayerData.Coins = Coins;
    //  PlayerData.Level = Level.CurrentLevel;
    //  PlayerData.Outerwear = _dataManager.ConvertItemToData(Equipment.Outerwear);
    //  PlayerData.Pants = _dataManager.ConvertItemToData(Equipment.Pants);
    //  PlayerData.Shoes = _dataManager.ConvertItemToData(Equipment.Shoes);
    //  PlayerData.Ring = _dataManager.ConvertItemToData(Equipment.Ring);
    //}

    private void UpdateData()
    {
        PlayerDataDto playerData = new();
        InventoryItem<OuterwearItem> outer = _character.Inventory.EquippedOuterwear;
        InventoryItem<PantsItem> pants = _character.Inventory.EquippedPants;
        InventoryItem<ShoesItem> shoes = _character.Inventory.EquippedShoes;
        InventoryItem<RingItem> ring = _character.Inventory.EquippedRing;

        playerData.Outerwear = new InventoryItemData { ID = outer.Item.name, Level = outer.Level, Amount = outer.Amount };
        playerData.Pants = new InventoryItemData { ID = pants.Item.name, Level = pants.Level, Amount = pants.Amount };
        playerData.Shoes = new InventoryItemData { ID = shoes.Item.name, Level = shoes.Level, Amount = shoes.Amount };
        playerData.Ring = new InventoryItemData { ID = ring.Item.name, Level = ring.Level, Amount = ring.Amount };


        foreach (var item in _character.Inventory.Items)
        {
            playerData.Items.Add(new InventoryItemData
            {
                ID = item.Item.name,
                Level = item.Level,
                Amount = item.Amount
            });
        }

        playerData.Coins = _character.Coins;
        playerData.Level = _character.Level.CurrentLevel;
        playerData.UserID = GetCachedPlayerData().UserID;

        StartCoroutine(UpdateDataProcess(playerData));
    }

    public void LoadData(PlayerDataDto playerData)
    {
        CachePlayerData(playerData);
        Inventory inv = _character.Inventory;
        inv.Clear();

        OuterwearItem outer = _itemsRefs.Find(x => x.name == playerData.Outerwear.ID) as OuterwearItem;
        PantsItem pants = _itemsRefs.Find(x => x.name == playerData.Pants.ID) as PantsItem;
        ShoesItem shoes = _itemsRefs.Find(x => x.name == playerData.Shoes.ID) as ShoesItem;
        RingItem ring = _itemsRefs.Find(x => x.name == playerData.Ring.ID) as RingItem;

        InventoryItem<OuterwearItem> outerwear = new InventoryItem<OuterwearItem>
        {
            Item = outer,
            Level = playerData.Outerwear.Level, Amount = playerData.Outerwear.Amount
        };
        _character.Equip(outerwear);
        InventoryItem<PantsItem> pantsItem = new InventoryItem<PantsItem>
        {
            Item = pants,
            Level = playerData.Pants.Level, Amount = playerData.Pants.Amount
        };
        _character.Equip(pantsItem);

        InventoryItem<ShoesItem> shoesItem = new InventoryItem<ShoesItem>
        {
            Item = shoes,
            Level = playerData.Shoes.Level, Amount = playerData.Shoes.Amount
        };
        _character.Equip(shoesItem);

        InventoryItem<RingItem> ringItem = new InventoryItem<RingItem>
        {
            Item = ring,
            Level = playerData.Ring.Level, Amount = playerData.Ring.Amount
        };
        _character.Equip(ringItem);

        List<InventoryItem<Item>> items = new();
        foreach (var itemData in playerData.Items)
        {
            Item item = _itemsRefs.Find(x => x.name == itemData.ID);

            if (item != null)
            {
                InventoryItem<Item> newItem = new InventoryItem<Item>
                {
                    Item = item,
                    Level = itemData.Level,
                    Amount = itemData.Amount
                };
                items.Add(newItem);
            }
        }
        _character.Inventory.LoadInventory(items);
    }

    private string GetToken()
    {
        return EncryptionHelper.Decrypt(File.ReadAllText(_tokenSavePath));
    }

    private void SetToken(string token)
    {
        File.WriteAllText(_tokenSavePath, EncryptionHelper.Encrypt(token));
    }

    //public List<InventoryItemData> GetInventoryAsData()
    //{
    //    List<InventoryItemData> items = new();

    //    foreach (var item in _character.Inventory.Items)
    //    {
    //        items.Add(new InventoryItemData
    //        {
    //            ID = item.Item.name,
    //            Level = item.Level,
    //            Amount = item.Amount
    //        });
    //    }

    //    return items;
    //}

    //public InventoryItemData ConvertItemToData<T>(InventoryItem<T> item) where T : Item
    //{
    //    return new InventoryItemData()
    //    {
    //        ID = item.Item.name,
    //        Level = item.Level,
    //        Amount = item.Amount
    //    };
    //}

    private IEnumerator RegisterProcess(UserRegisterDto dto)
    {
        string json = JsonUtility.ToJson(dto);
        Debug.Log(json);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        using UnityWebRequest www = new("https://localhost:7158/api/Auth/register", UnityWebRequest.kHttpVerbPOST);
        www.certificateHandler = _pass;
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        ApiRegisterResponse responce = JsonUtility.FromJson<ApiRegisterResponse>(www.downloadHandler.text);
        Debug.Log(www.downloadHandler.text);
        if (!responce.Success)
        {
            string usernameError = responce.UsernameError == "" ? "" : responce.UsernameError + "\n";
            string emailError = responce.EmailError == "" ? "" : responce.EmailError + "\n";
            string passwordError = responce.PasswordError == "" ? "" : responce.PasswordError + "\n";
            LoginLabel.Instance.DisplayRegisterError(usernameError + emailError + passwordError);
        }
        else
        {
            yield return _gameManager.LoadAppScene();
            LoadData(responce.PlayerData);
            SetToken(responce.Token);
        }
    }

    private IEnumerator LoginProcess(UserLoginDto dto)
    {
        string json = JsonUtility.ToJson(dto);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        using UnityWebRequest www = new("https://localhost:7158/api/Auth/login", UnityWebRequest.kHttpVerbPOST);
        www.certificateHandler = _pass;
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        ApiLoginResponse responce = JsonUtility.FromJson<ApiLoginResponse>(www.downloadHandler.text);
        if (!responce.Success)
            LoginLabel.Instance.DisplayLoginError(responce.Error);
        else
        {
            yield return _gameManager.LoadAppScene();
            LoadData(responce.PlayerData);
            SetToken(responce.Token);
        }
    }

    private IEnumerator UpdateDataProcess(PlayerDataDto data)
    {
        string json = JsonUtility.ToJson(data);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        using UnityWebRequest www = new("https://localhost:7158/api/Data/update", UnityWebRequest.kHttpVerbPOST);
        www.certificateHandler = _pass;
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Authorization", "Bearer " + GetToken());
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success) yield break;
        Debug.Log("ALE EBLAN1");
        Debug.Log(www.downloadHandler.text);
        Debug.Log("ALE EBLAN2");
        ApiUpdateResponce responce = JsonUtility.FromJson<ApiUpdateResponce>(www.downloadHandler.text);
        if (!responce.Success)
        {
            LoadData(GetCachedPlayerData());
            Debug.Log("anlak");
        }
        else
        {
            Debug.Log(responce.Error);
            CachePlayerData(data);
        }
    }

    private PlayerDataDto GetCachedPlayerData() => JsonUtility.FromJson<PlayerDataDto>(EncryptionHelper.Decrypt(File.ReadAllText(_dataSavePath)));
    private void CachePlayerData(PlayerDataDto data) => File.WriteAllText(_dataSavePath, EncryptionHelper.Encrypt(JsonUtility.ToJson(data)));
}

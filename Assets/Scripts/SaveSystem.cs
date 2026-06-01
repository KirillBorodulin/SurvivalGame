using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[System.Serializable]
public class ItemSaveData
{
    public string itemType;
}

[System.Serializable]
public class InventorySaveData
{
    public ItemSaveData[] items;
}

[System.Serializable]
public class WorldItemData
{
    public float posX;
    public float posY;
    public float posZ;
    public string itemType;
}

[System.Serializable]
public class WorldItemsSaveData
{
    public List<WorldItemData> items = new List<WorldItemData>();
}

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private InputActionReference saveAction;
    [SerializeField] private InputActionReference loadAction;

    public Transform player;
    public Inventory inventory;

    private void Awake()
    {
        saveAction.action.performed += OnSave;
        loadAction.action.performed += OnLoad;
    }

    private void OnEnable()
    {
        saveAction.action.Enable();
        loadAction.action.Enable();
    }

    private void OnDisable()
    {
        saveAction.action.Disable();
        loadAction.action.Disable();
    }

    private void OnDestroy()
    {
        saveAction.action.performed -= OnSave;
        loadAction.action.performed -= OnLoad;
    }

    private void OnSave(InputAction.CallbackContext context)
    {
        // Сохраняем позицию игрока
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);

        // Сохраняем инвентарь
        InventorySaveData invData = new InventorySaveData();
        invData.items = new ItemSaveData[inventory.Capacity];

        for (int i = 0; i < inventory.Capacity; i++)
        {
            invData.items[i] = new ItemSaveData();
            InventoryItem item = inventory[i];

            if (item != null && !(item is EmptyItem))
            {
                invData.items[i].itemType = item.GetType().Name;
            }
            else
            {
                invData.items[i].itemType = "";
            }
        }

        string invJson = JsonUtility.ToJson(invData);
        PlayerPrefs.SetString("InventorySave", invJson);

        // Сохраняем все предметы на карте
        WorldItemsSaveData worldData = new WorldItemsSaveData();
        DroppedItem[] allItems = Object.FindObjectsByType<DroppedItem>(FindObjectsSortMode.None);

        foreach (DroppedItem droppedItem in allItems)
        {
            WorldItemData worldItem = new WorldItemData();
            worldItem.posX = droppedItem.transform.position.x;
            worldItem.posY = droppedItem.transform.position.y;
            worldItem.posZ = droppedItem.transform.position.z;
            worldItem.itemType = droppedItem.item.GetType().Name;
            worldData.items.Add(worldItem);
        }

        string worldJson = JsonUtility.ToJson(worldData);
        PlayerPrefs.SetString("WorldItemsSave", worldJson);

        PlayerPrefs.Save();

        Debug.Log($"СОХРАНЕНО: позиция {player.position}, инвентарь и {allItems.Length} предметов на карте");
    }

    private void OnLoad(InputAction.CallbackContext context)
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            Debug.Log("Нет сохранений!");
            return;
        }

        // Загружаем позицию игрока
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float z = PlayerPrefs.GetFloat("PlayerZ");
        Vector3 pos = new Vector3(x, y, z);

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.position = pos;
            cc.enabled = true;
        }
        else
        {
            player.position = pos;
        }

        // Удаляем все предметы на карте
        DroppedItem[] existingItems = Object.FindObjectsByType<DroppedItem>(FindObjectsSortMode.None);
        foreach (DroppedItem item in existingItems)
        {
            Destroy(item.gameObject);
        }

        // Очищаем инвентарь
        for (int i = 0; i < inventory.Capacity; i++)
        {
            inventory.Drop(i);
        }

        // Загружаем инвентарь
        if (PlayerPrefs.HasKey("InventorySave"))
        {
            string invJson = PlayerPrefs.GetString("InventorySave");
            InventorySaveData invData = JsonUtility.FromJson<InventorySaveData>(invJson);

            for (int i = 0; i < invData.items.Length && i < inventory.Capacity; i++)
            {
                if (!string.IsNullOrEmpty(invData.items[i].itemType))
                {
                    InventoryItem item = CreateItem(invData.items[i].itemType);
                    if (item != null)
                    {
                        inventory.Add(item, i);
                    }
                }
            }
        }

        // Загружаем предметы на карте
        if (PlayerPrefs.HasKey("WorldItemsSave"))
        {
            string worldJson = PlayerPrefs.GetString("WorldItemsSave");
            WorldItemsSaveData worldData = JsonUtility.FromJson<WorldItemsSaveData>(worldJson);

            foreach (WorldItemData worldItem in worldData.items)
            {
                InventoryItem item = CreateItem(worldItem.itemType);
                if (item != null)
                {
                    Vector3 itemPos = new Vector3(worldItem.posX, worldItem.posY, worldItem.posZ);
                    SpawnWorldItem(item, itemPos);
                }
            }
        }

        Debug.Log($"ЗАГРУЖЕНО: позиция {pos}");
    }

    private InventoryItem CreateItem(string itemType)
    {
        switch (itemType)
        {
            case "Apple":
                return new Apple();
            case "Chestplate":
                return new Chestplate();
            case "Coin":
                return new Coin();
            case "LittleRock":
                return new LittleRock();
            default:
                Debug.LogWarning($"Неизвестный тип предмета: {itemType}");
                return null;
        }
    }

    private void SpawnWorldItem(InventoryItem item, Vector3 position)
    {
        if (item.droppedPrefab != null)
        {
            GameObject obj = Instantiate(item.droppedPrefab, position, Quaternion.identity);
            DroppedItem dropped = obj.GetComponent<DroppedItem>();
            if (dropped != null)
            {
                dropped.item = item;
            }
        }
    }
}
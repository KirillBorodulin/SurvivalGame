using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField, Range(1, 64)]
    private int capacity = 10;
    [SerializeField]
    public UnityEvent<InventoryItem, int> onItemsChanged = new();

    public int Capacity => capacity;

    public InventoryItem this[int index]
    {
        get
        {
            if (items == null || items.Length != capacity)
                Resize(capacity);

            return items[index];
        }
        set
        {
            if (items == null || items.Length != capacity)
                Resize(capacity);

            items[index] = value;
            onItemsChanged.Invoke(value, index);
        }
    }

    private InventoryItem[] items;

    public void Add(DroppedItem item)
    {
        if (Add(item.item))
        {
            Destroy(item.gameObject);
        }
    }
    public bool Add(InventoryItem item, int index)
    {
        if (this[index] == null)
        {
            this[index] = item;
            return true;
        } 

        return false;
    }
    public bool Add(InventoryItem item)
    {
        for (var i = 0; i < items.Length; i++)
        {
            if (this[i] == null)
            {
                this[i] = item;
                return true;
            } 
        }

        return false;
    }
    public void Resize(int newSize)
    {
        capacity = newSize;

        var newArray = new InventoryItem[newSize];

        if (items != null)
        {
            for (var i = 0; i < items.Length; i++)
            {
                if (i < newSize)
                {
                    newArray[i] = items[i];
                }
                else
                {
                    Drop(i);
                }
            }
        }

        items = newArray;
    }
    public DroppedItem DropAsObject(int index)
    {
        return DropAsObject(index, transform.position, transform.rotation);
    }
    public DroppedItem DropAsObject(int index, Vector3 position, Quaternion rotation)
    {
        var item = this[index];

        if (item == null)
            return null;

        var prefab = item.droppedPrefab;
        var droppedObject = Instantiate(prefab, position, rotation);
        var dropped = droppedObject.GetComponent<DroppedItem>();
        dropped.item = item;
        this[index] = null;

        return dropped;
    }

    public void Drop(int index)
    {
        this[index] = null;
    }
}
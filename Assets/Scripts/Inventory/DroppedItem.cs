
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    public InventoryItem item;

    private void Interact(GameObject obj)
    {
        if (obj.TryGetComponent<Inventory>(out var inventory))
        {
            inventory.Add(this);
        }
    } 
}
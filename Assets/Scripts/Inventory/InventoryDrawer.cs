using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryDrawer : MonoBehaviour
{
    [SerializeField]
    private UIDocument document;
    
    [SerializeField]
    private VisualTreeAsset element;

    [Space]
    [SerializeField]
    private Inventory inventory;

    private List<VisualElement> elements = new();

    [SerializeField]
    private Transform dropPoint;

    public void Refresh()
    {
        Clear();

        for (var i = 0; i < inventory.Capacity; i++)
        {
            var container = document.rootVisualElement.Q("container");
            var slotElement = element.Instantiate();
            var button = slotElement.Q<Button>();

            container.Add(slotElement);
            elements.Add(slotElement);
            slotElement.dataSource = inventory[i];

            var index = i;

            button.clicked += () =>
            {
                inventory.DropAsObject(index, dropPoint.position, dropPoint.rotation);
            };
        }
    }
    public void Clear()
    {
        foreach (var element in elements)
        {
            element.parent.Remove(element);
        }

        elements.Clear();
    }
    private void OnEnable()
    {
        inventory.onItemsChanged.AddListener(OnChangedEvent);

        Refresh();
    }
    private void OnDisable()
    {
        inventory.onItemsChanged.RemoveListener(OnChangedEvent);

        Clear();
    }   
    private void OnChangedEvent(InventoryItem item, int index)
    {
        if (elements.Count != inventory.Capacity)
        {
            Refresh();
            return;
        }

        elements[index].dataSource = item ?? EmptyItem.empty;
    }
}

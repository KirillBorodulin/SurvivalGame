using System;
using Unity.Properties;
using UnityEngine;

[Serializable]
public abstract class InventoryItem
{
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Godlike
    }

    public string name;
    public float weight;
    public Rarity rarity;

    [CreateProperty]
    public virtual Texture2D icon => Resources.Load<Texture2D>("Icons/" + GetType().Name);

    public virtual GameObject droppedPrefab => Resources.Load<GameObject>("Prefabs/Dropped/" + GetType().Name);
}
public sealed class EmptyItem : InventoryItem
{
    public static EmptyItem empty = new EmptyItem();
    
    [CreateProperty]
    public override Texture2D icon => null;

    public override GameObject droppedPrefab => null;

    private EmptyItem() { }
}
public interface IStackable
{
    public int count { get; set; }
}
public interface IDurable
{
    public float duration { get; set; }
}

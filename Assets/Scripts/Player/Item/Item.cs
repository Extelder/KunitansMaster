using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory")]
public class Item : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite Icon;
    public int MaxAmount;
    public bool CanSplited = true;
    public bool CanActivated;
    public GameObject Prefab;
}
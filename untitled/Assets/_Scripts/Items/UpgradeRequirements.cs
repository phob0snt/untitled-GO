using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/UpgradeRequirements")]
public class UpgradeRequirements : ScriptableObject
{
    public int Level;
    public List<UpgradeComponentDictionary> RequiredComponents;
    public int RequiredCoins;
}

[Serializable]
public struct UpgradeComponentDictionary
{
    public UpgradeComponent UpgradeComponent;
    [Min(1)] public int Amount;
}

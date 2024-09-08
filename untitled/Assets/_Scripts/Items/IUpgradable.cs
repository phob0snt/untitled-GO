using System;
using System.Collections.Generic;
using UnityEngine.Events;

public interface IUpgradable
{
    const int MAX_UPGRADE_LVL = 15;
    public int UpgradeLevel { get; }
    public List<UpgradeRequirements> UpgradeRequirements { get; }
    public void Upgrade();
}

[Serializable]
public struct UpgradeValues
{
    public int PercentageStatsIncrease;
    private void Calculate()
    {

    }
}

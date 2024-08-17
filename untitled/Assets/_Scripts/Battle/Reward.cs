using System;

[Serializable]
public class Reward
{
    public int CoinsAmount { get; private set; }
    public int HyperCoinsAmount { get; private set; }
    public int XPAmount { get; private set; }

    public Reward(int coins, int hyperCoins, int XP)
    {
        CoinsAmount = coins;
        HyperCoinsAmount = hyperCoins;
        XPAmount = XP;
    }
}

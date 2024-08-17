using System;

[Serializable]
public class Progress
{
    public int Level { get; private set; }
    public int Coins { get; private set; }
    public int HyperCoins { get; private set; }
}

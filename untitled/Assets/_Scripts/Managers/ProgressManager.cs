using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public int Level { get; private set; }
    public int LevelProgress { get; private set; }
    public int Coins { get; private set; }
    public int HyperCoins { get; private set; }

    public void AddCoins(int amount)
    {
        if (amount > 0)
            Coins += amount;
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;
        LevelProgress += amount;
    }

    public bool TryToSpendCoins(int amount)
    {
        if (Coins - amount >= 0)
        {
            Coins -= amount;
            return true;
        }
        else return false;
    }

    public void AddHyperCoins(int amount)
    {
        if (amount > 0)
            HyperCoins += amount;
    }

    public bool TryToSpendHyperCoins(int amount)
    {
        if (HyperCoins - amount >= 0)
        {
            HyperCoins -= amount;
            return true;
        }
        else return false;
    }
}

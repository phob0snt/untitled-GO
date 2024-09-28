using UnityEngine;
using UnityEngine.Events;

public class Level
{
    public UnityEvent OnLevelUp = new();
    public int CurrentLevel { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;
    public int XPToNextLevel { get; private set; } = 180;

    public void AddXP(int xp)
    {
        CurrentXP += xp;
        if (CurrentXP >= XPToNextLevel)
            IncreaseLevel();
    }

    public void IncreaseLevel()
    {
        CurrentXP -= XPToNextLevel;
        CurrentLevel++;
        XPToNextLevel = CalculateNextLevelXP();
        OnLevelUp.Invoke();
    }

    private int CalculateNextLevelXP()
    {
        return (int)(1 / 9 * Mathf.Pow(CurrentLevel, 2) * 1000);
    }
}

using UnityEngine;

public class Level
{
    public int CurrentLevel { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;
    public int XPToNextLevel { get; private set; } = 180;

    public void AddXP(int xp)
    {
        CurrentXP += xp;
        if (CurrentXP >= XPToNextLevel)
            IncreaseLevel();
        else
            EventManager.Broadcast(FormEvent());
    }

    public int GetBaseStreamCapacity()
    {
        return 300;
    }

    public int GetBaseHP()
    {
        return 1200;
    }

    public void IncreaseLevel()
    {
        Debug.Log("INCREASE LEVE " + CurrentXP + "" + XPToNextLevel);
        CurrentXP -= XPToNextLevel;
        CurrentLevel++;
        XPToNextLevel = CalculateNextLevelXP();
        EventManager.Broadcast(FormEvent());
    }

    private int CalculateNextLevelXP()
    {
        return (int)(1f / 9f * Mathf.Pow(CurrentLevel, 2) * 1000);
    }

    private LevelChangeEvent FormEvent()
    {
        return new LevelChangeEvent()
        {
            CurrentXP = CurrentXP,
            XPForNextLevel = XPToNextLevel,
            Level = CurrentLevel,
        };
    }
}

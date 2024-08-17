using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnFightSceneLoaded = new();
    public PlayerStats PlayerStats { get; private set; }
    public BattleData CurrentBattleData { get; private set; }

    public void UpdatePlayerStats(PlayerStats playerStats)
    {
        PlayerStats = playerStats;
    }

    public void LoadFightScene(BattleData battleData)
    {
        CurrentBattleData = battleData;
        StartCoroutine(SwitchScene("FightScene"));
    }


    private IEnumerator SwitchScene(string sceneName)
    {
        AsyncOperation operaion = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!operaion.isDone)
        {
            Debug.Log("Loading");
            yield return null;
        }
        OnFightSceneLoaded.Invoke();
    }
}

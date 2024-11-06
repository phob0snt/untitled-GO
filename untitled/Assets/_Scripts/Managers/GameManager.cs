using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnFightSceneLoaded = new();
    public PlayerStats PlayerStats { get; private set; }
    public BattleData CurrentBattleData { get; private set; }

    private bool _fightSceneConfigured = false;

    private void OnEnable()
    {
        EventManager.AddListener((FightSceneConfiguredEvent _) => _fightSceneConfigured = true);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener((FightSceneConfiguredEvent _) => _fightSceneConfigured = true);
    }

    public void UpdatePlayerStats(PlayerStats playerStats)
    {
        PlayerStats = playerStats;
    }


    public IEnumerator LoadAppScene()
    {
        AsyncOperation operaion = SceneManager.LoadSceneAsync("AppScene", LoadSceneMode.Single);
        while (!operaion.isDone)
        {
            Debug.Log("Loading");
            yield return null;
        }
    }


    public IEnumerator LoadFightScene(BattleData data)
    {
        CurrentBattleData = data;
        AsyncOperation operaion = SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
        while (!operaion.isDone)
        {
            Debug.Log("Loading");
            yield return null;
        }
        while (!_fightSceneConfigured)
        {
            yield return null;
        }
        Debug.Log("Loaded");
        _fightSceneConfigured = false;
    }
}

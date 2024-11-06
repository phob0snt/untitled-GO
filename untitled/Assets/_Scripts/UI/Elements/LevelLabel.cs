using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LevelLabel : UIElement
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private Image _xpBackground;
    [SerializeField] private TMP_Text _xpText;
    [SerializeField] private Button _levelButton;
    [SerializeField] private UICircle _levelCircle;

    private Sequence _xpShowSequence;

    public override void Initialize()
    {
        EventManager.AddListener<LevelChangeEvent>(UpdateLevelDisplay);
        _levelButton.onClick.AddListener(() => _xpShowSequence.Restart());
        ConfigureDoTween();
    }

    private void ConfigureDoTween()
    {
        _xpShowSequence = DOTween.Sequence()
            .Append(_xpText.DOFade(1f, 0.1f))
            .AppendInterval(3)
            .Append(_xpText.DOFade(0f, 1f))
            .Pause()
            .SetAutoKill(false);
    }

    private void OnDestroy()
    {
        _levelButton.onClick.RemoveListener(() => _xpShowSequence.Restart());
        EventManager.RemoveListener<LevelChangeEvent>(UpdateLevelDisplay);
    }

    private void UpdateLevelDisplay(LevelChangeEvent e)
    {
        _level.text = e.Level.ToString();
        _levelCircle.Progress = (float)Math.Round((float)e.CurrentXP / e.XPForNextLevel, 2);
        _xpText.text = $"{e.CurrentXP} / {e.XPForNextLevel} XP";
    }
}

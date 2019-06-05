using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : Panel
{
    [SerializeField]
    private Text _level;

    [SerializeField]
    private ParticleSystem _confetti;

    public void UpdateLevelNum(int level)
    {
        _level.text = $"LEVEL {level}";
    }

    public void SwitchSound()
    {

    }

    public void RemoveAds()
    {

    }

    public void RestorePurchases()
    {

    }

    public void Leaderboards()
    {

    }

    public void Skins()
    {

    }
    
    protected override void OnOpen()
    {
        //PlayerController.active = false;
        _confetti.Play(true);
        Invoke("OnNextLevel", 1);
    }


    public void OnNextLevel()
    {
        Close(() =>
        {
            panelsManager.Get<GameplayUI>().Open(null, false);
        }, false);
        //PlayerController.active = true;   
    }
}

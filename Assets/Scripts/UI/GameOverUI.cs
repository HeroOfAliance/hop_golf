using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : Panel
{

    [SerializeField]
    private ParticleSystem _confetti;
    [SerializeField]
    private Image _header;
    [SerializeField]
    private Image _headerShade;

    public void UpdateLevelNum(int level)
    {
        var color = ColorCode.instance.GetColors(level + 1);
        _header.color = color.uiColor;
        _headerShade.color = new Color(color.uiColor.r, color.uiColor.g, color.uiColor.b, 0.25f);
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

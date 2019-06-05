using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorizedUnits : Controller
{
    [SerializeField]
    private List<Image> _fullColor;
    [SerializeField]
    private List<Image> _halfColor;
    [SerializeField]
    private List<Image> _quarterColor;

    [SerializeField]
    private List<Text> _fullColoredLabels;

    public void UpdateColors(Color color)
    {
        var half = new Color(color.r, color.g, color.b, 0.5f);
        var quarter = new Color(color.r, color.g, color.b, 0.25f);

        foreach (var item in _fullColor)
        {
            item.color = color;
        }

        foreach (var item in _fullColoredLabels)
        {
            item.color = color;
        }

        foreach (var item in _halfColor)
        {
            item.color = half;
        }

        foreach (var item in _quarterColor)
        {
            item.color = quarter;
        }
    }
}

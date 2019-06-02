using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelsManager : Controller
{
    [SerializeField]
    private List<Panel> _panels;

    internal T Get<T>() where T : Panel
    {
        return _panels.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
    }
}

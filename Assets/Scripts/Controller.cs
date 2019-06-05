using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private static PlayerController _playerController;
    public static PlayerController playerController
    {
        get
        {
            if (!_playerController) { _playerController = GetController<PlayerController>(); }
            return _playerController;
        }
    }

    private static GameController _gameController;
    public static GameController gameController
    {
        get
        {
            if (!_gameController) { _gameController = GetController<GameController>(); }
            return _gameController;
        }
    }

    private static Field _field;
    public static Field field
    {
        get
        {
            if (!_field) { _field = GetController<Field>(); }
            return _field;
        }
    }


    private static PanelsManager _panelsManager;
    public static PanelsManager panelsManager
    {
        get
        {
            if (!_panelsManager) { _panelsManager = GetController<PanelsManager>(); }
            return _panelsManager;
        }
    }

    private static ColorizedUnits _uiColors;
    public static ColorizedUnits uiColors
    {
        get
        {
            if (!_uiColors) { _uiColors = GetController<ColorizedUnits>(); }
            return _uiColors;
        }
    }

    private static Routiner _routiner;
    public static Routiner routiner
    {
        get
        {
            if (!_routiner) { _routiner = GetController<Routiner>(); }
            return _routiner;
        }
    }

    public static T GetController<T>() where T : Controller
    {
        if (!_controllers.ContainsKey(typeof(T)))
        {
            return default;
        }

        return (T)_controllers[typeof(T)];
    }

    private static Dictionary<Type, Controller> _controllers = new Dictionary<Type, Controller>();

    protected virtual void Awake()
    {
        _controllers[GetType()] = this;
    }

    protected virtual void OnDestroy()
    {
        if (_controllers.ContainsKey(GetType()) && _controllers[GetType()] == this)
        {
            _controllers.Remove(GetType());
        }
    }
}

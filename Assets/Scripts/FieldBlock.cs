using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject _blocker;
    [SerializeField]
    private GameObject _void;
    [SerializeField]
    private GameObject _portal;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _empty;


    [SerializeField]
    private MeshRenderer _obstacle;
    [SerializeField]
    private MeshRenderer _ground;
    [SerializeField]
    private MeshRenderer _hole;
    [SerializeField]
    private TutorHand _tutorHand;

    public LevelData.FiledUnit unit { get; private set; }
    public FieldBlockType type { get; private set; }

    public void SetUnit(LevelData.FiledUnit unit)
    {
        this.unit = unit;
    }

    public void SetType(FieldBlockType type)
    {
        this.type = type;

        _blocker.SetActive(type == FieldBlockType.Blocker);
        _empty.gameObject.SetActive(type != FieldBlockType.Void);
        
        _void.SetActive(type == FieldBlockType.Void);
        _portal.SetActive(type == FieldBlockType.Portal);


        if (type == FieldBlockType.Void || type == FieldBlockType.Portal)
        {
            _empty.gameObject.SetActive(false);
        }
        else
        {
            _empty.gameObject.SetActive(true);
        }

        switch (type)
        {
            case FieldBlockType.Blocker:
                break;
            case FieldBlockType.Empty:
                break;
            case FieldBlockType.Void:
                break;
            case FieldBlockType.Portal:
                break;
        }
    }


    public void SetStartNode(bool startNode, int ? tutor)
    {
        if (startNode)
        {
            SetType(FieldBlockType.Empty);
        }
        _player.SetActive(startNode);

        if (startNode && tutor.HasValue)
        {
            _tutorHand.Init(tutor.Value);
        }
    }

    internal void SetColors(ColorCode.ColorPair codes)
    {
        _obstacle.material.color = codes.obstacles;
        _hole.material.color = codes.ground;
        _ground.material.color = codes.ground;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Field))]
public class FieldCustomEditor : Editor
{
    private static bool _editor;
    private static int _level;
    private static LevelData _levelAsset;
    private static bool _isLevelOpen;
    private static BrushType _brush;

    private static Vector2Int _levelSize = new Vector2Int(5,5);

    private enum BrushType
    {
        Empty,
        Blocker,
        Void,
        Player,
        Portal
    }

    private Field field => (Field) target;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!_editor)
        {

            if (GUILayout.Button("Open Editor"))
            {
                _editor = true;
                field.SetEditMode(true);
            }

            return;
        }

        if (_editor && GUILayout.Button("Close Editor"))
        {
            _editor = false;
            _isLevelOpen = false;
            field.SetEditMode(false);
        }


        if (_isLevelOpen)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("close"))
            {
                _isLevelOpen = false;
                field.Close(null, true);
            }

            if (GUILayout.Button("save"))
            {
                File.WriteAllText($"{Application.dataPath}/Resources/Levels/{_level}.json", JsonUtility.ToJson(field.levelData));
                AssetDatabase.Refresh();
                field.Restart();
            }

            if (GUILayout.Button("save & close"))
            {
                _isLevelOpen = false;
                File.WriteAllText($"{Application.dataPath}/Resources/Levels/{_level}.json", JsonUtility.ToJson(field.levelData));
                AssetDatabase.Refresh();
                field.Close(null, true);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Pick a brush:");

            if (EditorGUILayout.Toggle(" Empty", _brush == BrushType.Empty)) { _brush = BrushType.Empty; };
            if (EditorGUILayout.Toggle(" Blocker", _brush == BrushType.Blocker)) { _brush = BrushType.Blocker; };
            if (EditorGUILayout.Toggle(" Void", _brush == BrushType.Void)) { _brush = BrushType.Void; };
            if (EditorGUILayout.Toggle(" Portal", _brush == BrushType.Portal)) { _brush = BrushType.Portal; };
            if (EditorGUILayout.Toggle(" Player", _brush == BrushType.Player)) { _brush = BrushType.Player; };


        }
        else
        {
            var oldLevel = _level;
            _level = EditorGUILayout.IntField(_level);

            if (_level == 0)
            {
                _level = 1;
            }

            if (_level != oldLevel)
            {
                ReloadAsset();
            }

            if (_levelAsset != null)
            {
                if (GUILayout.Button("Open"))
                {
                    field.Open(_levelAsset, null);
                    _isLevelOpen = true;
                }
            }
            else
            {
                _levelSize = EditorGUILayout.Vector2IntField("Size", _levelSize);
                if (GUILayout.Button("Create"))
                {

                    var level = LevelData.Create(_levelSize);

                    File.WriteAllText($"{Application.dataPath}/Resources/Levels/{_level}.json", JsonUtility.ToJson(level));
                    AssetDatabase.Refresh();
                    ReloadAsset();
                    field.Open(_levelAsset, null);
                    _isLevelOpen = true;
                }
            }
        }
    }


    void OnEnable()
    {
        EditorApplication.update += Update;
    }

    void Update()
    {
        if (!_editor || !_isLevelOpen)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastAll(ray, 100.0f);

            foreach (var hit in hits)
            {
                var block = hit.collider.GetComponent<FieldBlock>();

                if (block)
                {
                    switch (_brush)
                    {
                        case BrushType.Blocker:
                            _levelAsset[block.unit.X, block.unit.Y].Type = FieldBlockType.Blocker;
                            block.SetType(FieldBlockType.Blocker);
                            break;
                        case BrushType.Empty:
                            _levelAsset[block.unit.X, block.unit.Y].Type = FieldBlockType.Empty;
                            block.SetType(FieldBlockType.Empty);
                            break;
                        case BrushType.Void:
                            _levelAsset[block.unit.X, block.unit.Y].Type = FieldBlockType.Void;
                            block.SetType(FieldBlockType.Void);
                            break;
                        case BrushType.Player:
                            foreach (var item in _levelAsset.Units)
                            {
                                item.IsStart = false;
                            }
                            _levelAsset[block.unit.X, block.unit.Y].IsStart = true;
                            _levelAsset[block.unit.X, block.unit.Y].Type = FieldBlockType.Empty;
                           
                            field.Restart();
                            break;
                        case BrushType.Portal:
                            foreach (var item in _levelAsset.Units)
                            {
                                if (item.Type == FieldBlockType.Portal)
                                {
                                    item.Type = FieldBlockType.Empty;
                                }
                            }
                            _levelAsset[block.unit.X, block.unit.Y].Type = FieldBlockType.Portal;
                            field.Restart();
                            break;
                    }
                    break;
                }
            }
        }

    }

    void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    private void ReloadAsset()
    {
        _levelAsset = null;
        var asset = Resources.Load<TextAsset>($"Levels/{_level}");

        if (asset)
        {
            _levelAsset = JsonUtility.FromJson<LevelData>(asset.text);
            _levelAsset.Num = _level;
        }
    }
}

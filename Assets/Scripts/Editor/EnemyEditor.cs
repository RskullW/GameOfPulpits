using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    private Enemy _enemy;
    private bool _isVisibleListMovePoints;

    public void OnEnable()
    {
        _enemy = (Enemy)target;
        _isVisibleListMovePoints = true;
    }

    public override void OnInspectorGUI()
    {
        _enemy.Health = EditorGUILayout.FloatField("Health", _enemy.Health);
        _enemy.Damage = EditorGUILayout.FloatField("Health", _enemy.Damage);
        _enemy.AttackRange = EditorGUILayout.FloatField("Attack range", _enemy.AttackRange);
        _enemy.CooldownAttack = EditorGUILayout.FloatField("Cooldown Attack", _enemy.CooldownAttack);
        _enemy.RunSpeed = EditorGUILayout.FloatField("Run speed", _enemy.RunSpeed);
        _enemy.WalkSpeed = EditorGUILayout.FloatField("Walk speed", _enemy.WalkSpeed);
        GUILayout.Space(10);
        EditorGUILayout.Toggle("Enemy see player", _enemy.IsVisiblePlayer);
        EditorGUILayout.Toggle("Enemy attacks player", _enemy.IsAttack);
        GUILayout.Space(20);
        _enemy.IsMovingArea = EditorGUILayout.Toggle("Is Moving Area", _enemy.IsMovingArea);
        GUILayout.Space(10);
        if (_enemy.IsMovingArea)
        {
            if (!_isVisibleListMovePoints)
            {
                _isVisibleListMovePoints = EditorGUILayout.Toggle("Open list", _isVisibleListMovePoints);
            }

            else
            {
                _isVisibleListMovePoints = EditorGUILayout.Toggle("Closed list", _isVisibleListMovePoints);
            }

            if (GUILayout.Button("Add a new element", GUILayout.Height(20)))
            {
                _enemy.MovePoints.Add(_enemy.gameObject);
                _isVisibleListMovePoints = true;
            }

            if (_isVisibleListMovePoints)
            {
                for (int i = 0; i < _enemy.MovePoints.Count; ++i)
                {

                    if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        _enemy.MovePoints.Remove(_enemy.MovePoints[i]);
                        break;
                    }

                    _enemy.MovePoints[i] = (GameObject)EditorGUILayout.ObjectField("Move point", _enemy.MovePoints[i],
                        typeof(GameObject), true);
                }
            }
        }

        if (GUI.changed)
        {
            SetObject(_enemy.gameObject);
        }
    }
    public static void SetObject(GameObject gameObject)
    {
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }
}

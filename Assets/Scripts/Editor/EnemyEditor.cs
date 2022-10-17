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
        DrawConditions();
        
        GUILayout.Space(10);
        EditorGUILayout.Toggle("Enemy see player", _enemy.IsVisiblePlayer);
        EditorGUILayout.Toggle("Enemy attacks player", _enemy.IsAttack);
        
        GUILayout.Space(20);
        _enemy.IsMovingArea = EditorGUILayout.Toggle("Is Moving Area", _enemy.IsMovingArea);
        GUILayout.Space(10);
        
        if (_enemy.IsMovingArea)
        { 
            _isVisibleListMovePoints = EditorGUILayout.Foldout(_isVisibleListMovePoints,
                "Move list [Count " + _enemy.MovePoints.Count +"]", true);
            if (_isVisibleListMovePoints)
            {
                
                for (int i = 0; i < _enemy.MovePoints.Count; ++i)
                {
                    EditorGUILayout.BeginVertical("box");
                    if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        _enemy.MovePoints.Remove(_enemy.MovePoints[i]);
                        break;
                    }

                    _enemy.MovePoints[i] = (GameObject)EditorGUILayout.ObjectField("Move point", _enemy.MovePoints[i],
                        typeof(GameObject), true);
                    
                    EditorGUILayout.EndVertical();
                }
                
                if (GUILayout.Button("Add a new element", GUILayout.Height(20)))
                {
                    _enemy.MovePoints.Add(_enemy.gameObject);
                    _isVisibleListMovePoints = true;
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

    private void DrawConditions()
    {
        _enemy.TypeEnemy = (TypeEnemy)EditorGUILayout.EnumPopup("Type enemy", _enemy.TypeEnemy);
        _enemy.Health = EditorGUILayout.FloatField("Health", _enemy.Health);
        _enemy.Damage = EditorGUILayout.FloatField("Damage", _enemy.Damage);
        _enemy.AttackRange = EditorGUILayout.FloatField("Attack range", _enemy.AttackRange);
        _enemy.MinCooldownAttack = EditorGUILayout.FloatField("Minimal cooldown attack", _enemy.MinCooldownAttack);
        _enemy.MaxCooldownAttack = EditorGUILayout.FloatField("Maximum cooldown attack", _enemy.MaxCooldownAttack);
        _enemy.RunSpeed = EditorGUILayout.FloatField("Run speed", _enemy.RunSpeed);
        _enemy.WalkSpeed = EditorGUILayout.FloatField("Walk speed", _enemy.WalkSpeed);
    }
}

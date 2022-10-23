using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    [SerializeField] private PlayerController3D _player;
    [SerializeField] private Enemy3D _enemy;
    [SerializeField] private bool _isPlayer;
    private bool _isAttack;

    private void Start()
    {
        _isAttack = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerStay(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isPlayer)
        {
            if (other.gameObject.tag == "Enemy" && _player.IsAttack && !_isAttack)
            {
                _isAttack = true;
                _enemy.TakeDamage(_player.GetDamage());
                Debug.Log("Player caused damage Enemy:" + _player.GetDamage());

            }

            else if (!_player.IsAttack)
            {
                _isAttack = false;
            }
        }

        else
        {
            if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Tip")  && _enemy.IsAttack && !_isAttack)
            {
                var damage = _enemy.GetDamage();

                _isAttack = true;

               
                if (_player.IsDefend)
                {
                    damage /= 4;
                }

                _player.TakeDamage(damage);
                
                Debug.Log("Enemy caused damage player:" + _player.GetDamage());
            }

            else if (!_enemy.IsAttack)
            {
                _isAttack = false;
            }
        }
    }
}

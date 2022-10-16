using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour
{
    public bool IsMovingArea;
    public bool IsVisiblePlayer = false;
    public bool IsAttack = false;

    public float AttackRange;
    public float Health;
    public float CooldownAttack;
    public float WalkSpeed;
    public float RunSpeed;
    public float Damage;

    public List<GameObject> MovePoints;

    private bool _isMovement;
    private Animator _animator;
    private Transform _playerTransform;

    private float _cooldown;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        IsAttack = false;
        _isMovement = false;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void StartWalk()
    {
        _animator.SetBool("Walk", true);
    }

    private void FixedUpdate()
    {

        if (_isMovement)
        {
            MovementLogic();
            AttackLogic();
        }

    }

    private void MovementLogic()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) >= 6f)
        {
            _animator.SetBool("RunUp", true);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z-Time.deltaTime*WalkSpeed);
            IsVisiblePlayer = false;
        } 
        
        else if (Vector3.Distance(transform.position, _playerTransform.position) > AttackRange + 0.5f)
        {
            IsVisiblePlayer = true;

            float xPlayer = _playerTransform.position.x, zPlayer = _playerTransform.position.z;
            float xEnemy = transform.position.x, zEnemy = transform.position.z;

            if (xPlayer+0.1 < xEnemy)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("RunRight", false);
                _animator.SetBool("RunUp", false);
                _animator.SetBool("RunDown", false);
                _animator.SetBool("RunLeft", true);
                xEnemy -= Time.deltaTime*RunSpeed;
            }
            
            else if (xPlayer - 0.1 > xEnemy)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("RunRight", true);
                _animator.SetBool("RunUp", false);
                _animator.SetBool("RunDown", false);
                _animator.SetBool("RunLeft", false);
                xEnemy += Time.deltaTime*RunSpeed;
            }

            if (zEnemy-0.1 > zPlayer)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("RunRight", false);
                _animator.SetBool("RunUp", true);
                _animator.SetBool("RunDown", false);
                _animator.SetBool("RunLeft", false);
                zEnemy -= Time.deltaTime*RunSpeed;
            }

            else if (zEnemy + 0.1 < zPlayer) 
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("RunRight", false);
                _animator.SetBool("RunUp", false);
                _animator.SetBool("RunDown", true);
                _animator.SetBool("RunLeft", false);
                zEnemy += Time.deltaTime*RunSpeed;
            }

            transform.position = new Vector3(xEnemy, transform.position.y, zEnemy);
        }
        
        else if (!IsAttack && Vector3.Distance(transform.position, _playerTransform.position) <= AttackRange + 0.5f)
        {
            IsAttack = true;
            _cooldown = CooldownAttack;
            Debug.Log("Wolf: take damage a " + _playerTransform.gameObject.name + "\nDamage = " + Damage);
        }
    }

    private void AttackLogic()
    {
        if (_cooldown > 0f && IsAttack)
        {
            _cooldown -= Time.deltaTime;
            
        }
        
        else if (_cooldown <= 0 && IsAttack)
        {
            IsAttack = false;
        }
    }

    public void SetMovement(bool isMovement)
    {
        _isMovement = isMovement;
    }
}
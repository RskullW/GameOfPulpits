using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour
{
    public event Action OnCauseDamage;
    public event Action OnDied;
    
    public TypeEnemy TypeEnemy;
    public bool IsMovingArea;
    public bool IsVisiblePlayer = false;
    public bool IsAttack = false;

    public float AttackRange;
    public float Health;
    public float MinCooldownAttack;
    public float MaxCooldownAttack;
    public float WalkSpeed;
    public float RunSpeed;
    public float Damage;
    public float StartHealth => _startHealth;
    public bool IsMovement => _isMovement;
    public List<GameObject> MovePoints;

    private bool _isMovement;
    private Animator _animator;
    private Transform _playerTransform;
    private float _startHealth;
    private bool _isFirstVisiblePlayer;
    private float _cooldown;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        IsAttack = false;
        _isMovement = false;
        _startHealth = Health;
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
        
        Vector3 tempDirection = (_playerTransform.position - gameObject.transform.position).normalized;
        Quaternion tempLookRotation = Quaternion.LookRotation(new Vector3(tempDirection.x, 90.0f, tempDirection.z));
        transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, tempLookRotation, Time.deltaTime * 2f);
        
        if (Vector3.Distance(transform.position, _playerTransform.position)-AttackRange >= 5f)
        {
            _animator.SetBool("RunUp", true);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z-Time.deltaTime*WalkSpeed);
            IsVisiblePlayer = false;
        } 
        
        else if (Vector3.Distance(transform.position, _playerTransform.position) > AttackRange)
        {

            if (!IsVisiblePlayer)
            {
                IsVisiblePlayer = true;

                if (_isFirstVisiblePlayer)
                {
                    AudioManager.Instance.PlaySound("StartFight");
                }
            }

            float xPlayer = _playerTransform.position.x, zPlayer = _playerTransform.position.z;
            float xEnemy = transform.position.x, zEnemy = transform.position.z;

            if (xPlayer+0.1 < xEnemy)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("RunUp", true);
                xEnemy -= Time.deltaTime*RunSpeed;
            }
            
            else if (xPlayer - 0.1 > xEnemy)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("RunUp", true);
                xEnemy += Time.deltaTime*RunSpeed;
            }

            if (zEnemy-0.1 > zPlayer)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("RunUp", true);
                zEnemy -= Time.deltaTime*RunSpeed;
            }

            else if (zEnemy + 0.1 < zPlayer) 
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("RunUp", true);
                zEnemy += Time.deltaTime*RunSpeed;
            }

            transform.position = new Vector3(xEnemy, transform.position.y, zEnemy);
        }
        
        else if (!IsAttack && Vector3.Distance(transform.position, _playerTransform.position) <= AttackRange)
        {
            IsAttack = true;
            OnCauseDamage?.Invoke();
            _cooldown = UnityEngine.Random.Range(MinCooldownAttack, MaxCooldownAttack);

            if (TypeEnemy == TypeEnemy.Wolf)
            {
                AudioManager.Instance.PlaySound("WolfAttack");
            }
            
            else if (TypeEnemy == TypeEnemy.People)
            {
                AudioManager.Instance.PlaySound("Attack2");
            }
            
            else if (TypeEnemy == TypeEnemy.King)
            {
                AudioManager.Instance.PlaySound("Attack3");
            }
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
    public void SetPosition(Vector3 position)
    {
        transform.position = Vector2.Lerp(transform.position, position, RunSpeed * 2);
    }
    public void SetIsFirstVisiblePlayer(bool isVisiblePlayer)
    {
        _isFirstVisiblePlayer = isVisiblePlayer;
    }

    public void SetHealth(float health)
    {
        Health = health;
        
        if (Health <= 0)
        {
            OnDied?.Invoke();    
        }
    }
}

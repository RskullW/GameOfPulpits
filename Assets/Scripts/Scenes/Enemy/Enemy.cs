using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour
{
    public event Action OnCauseDamage;
    public event Action OnDied;
    
    public TypeEnemy TypeEnemy;
    public NavMeshAgent _Agent;
    public bool IsMovingArea;
    public bool IsVisiblePlayer = false;
    public bool IsAttack = false;

    public float StoppingDistance;
    public float RetreatDistance;
    
    public GameObject _arrow;
    public float AttackRange;
    public float Health;
    public float MinCooldownAttack;
    public float MaxCooldownAttack;
    public float MinCooldownBlock;
    public float MaxCooldownBlock;
    public float WalkSpeed;
    public float RunSpeed;
    public float Damage;
    public float StartHealth => _startHealth;
    public bool IsMovement => _isMovement;
    public List<GameObject> MovePoints;

    private bool _isMovement;
    private bool _isBlocked;
    private Animator _animator;
    private Transform _playerTransform;
    private float _startHealth;
    private bool _isFirstVisiblePlayer;
    private float _cooldown;

    void Start()
    {
        _animator = GetComponent<Animator>();
        IsAttack = false;
        _isMovement = true;
        _startHealth = Health;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        if (TypeEnemy == TypeEnemy.Outlaw || TypeEnemy == TypeEnemy.People)
        {
            IsVisiblePlayer = true;
        }

        Debug.Log("IS MOVE: " + IsMovement);
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

        if (TypeEnemy == TypeEnemy.Wolf)
        {
            WolfUpdate();
        }
        
        else if (TypeEnemy == TypeEnemy.Outlaw)
        {
            OutlawUpdate();
        }
        
    }

    private void WolfUpdate()
    {
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

            AudioManager.Instance.PlaySound("WolfAttack");
        }
    }

    private void OutlawUpdate()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) > StoppingDistance)
        {
            IsAttack = false;
            _Agent.SetDestination(_playerTransform.position);
        }

        else if (Vector3.Distance(transform.position, _playerTransform.position) < RetreatDistance)
        {
            IsAttack = false;
            Vector3 dirToPlayer = transform.position - _playerTransform.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;

            _Agent.SetDestination(newPos);
        }
        
        if (_cooldown <= 0)
        {
            IsAttack = true;
            StartCoroutine(StartAttackOutlaw());
            
            _cooldown = UnityEngine.Random.Range(MinCooldownAttack, MaxCooldownAttack);
        }

        else
        {
            _cooldown-= Time.deltaTime;
        }
    }

    private void PeopleUpdate()
    {
        if (_isMovement && Vector3.Distance(transform.position, _playerTransform.position) > StoppingDistance)
        {
            IsAttack = false;
            _Agent.SetDestination(_playerTransform.position);
        }

        else if ( _isMovement && Vector3.Distance(transform.position, _playerTransform.position) < RetreatDistance)
        {
            IsAttack = false;
            Vector3 dirToPlayer = transform.position - _playerTransform.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;

            _Agent.SetDestination(newPos);
        }
        
        if (_cooldown <= 0 && _isMovement && !_isBlocked)
        {
            IsAttack = true;
            StartCoroutine(StartAttackOutlaw());
            
            _cooldown = UnityEngine.Random.Range(MinCooldownAttack, MaxCooldownAttack);
        }

        else
        {   
            _cooldown-= Time.deltaTime;
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

        if (TypeEnemy == TypeEnemy.Outlaw)
        {
            if (!(Health <= 0)) return;
            OnDied?.Invoke();
            StartCoroutine(StartDieOutlaw());
        }

        else if (TypeEnemy == TypeEnemy.Wolf && Health <= 0)
        {
            OnDied?.Invoke();
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (!(Health <= 0)) return;

        if (TypeEnemy == TypeEnemy.Outlaw)
        {
            OnDied?.Invoke();
            _isMovement = false;
            StartCoroutine(StartDieOutlaw());
        }
        
        else if (TypeEnemy == TypeEnemy.Wolf && Health<=0)
        {
            OnDied?.Invoke();
        }
    }

    private IEnumerator StartDieOutlaw()
    {
        _animator.SetBool("OutlawDie", true);
        
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }

    private IEnumerator StartAttackOutlaw()
    {
        _animator.SetBool("OutlawRun", false);
        _animator.SetBool("OutlawAttack", true);
        yield return new WaitForSeconds(1);
        Instantiate(_arrow, transform.position, quaternion.Euler(90, transform.eulerAngles.y, transform.eulerAngles.z));
        AudioManager.Instance.PlaySound("OutlawAttack");
        yield return new WaitForSeconds(0.1f);
        _animator.SetBool("OutlawRun", true);
        _animator.SetBool("OutlawAttack", false);
    }
}

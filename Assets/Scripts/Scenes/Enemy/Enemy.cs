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
    public NavMeshAgent Agent;
    public bool IsMovingArea;
    public bool IsVisiblePlayer = false;
    public bool IsAttack = false;

    public float StoppingDistance;
    public float RetreatDistance;
    public float VisibleDistance;
    
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
    public List<GameObject> MovePoints;

    public bool IsMovement;
    public bool IsBlocked => _isBlocked;
    private bool _isBlocked;
    private Animator _animator;
    private Transform _playerTransform;
    private float _startHealth;
    private bool _isFirstVisiblePlayer;
    private float _cooldown;
    private float _cooldownBlock;
    private ushort _indexMovePoints;


    void Start()
    {
        _animator = GetComponent<Animator>();
        IsAttack = false;
        _startHealth = Health;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _indexMovePoints = 0;
    }

    private void FixedUpdate()
    {
        if (TypeEnemy == TypeEnemy.Outlaw || TypeEnemy == TypeEnemy.People)
        {
            IsVisiblePlayer = true;
        }

        if (IsMovement)
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
        
        else if (TypeEnemy == TypeEnemy.People)
        {
            PeopleUpdate();
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
            Agent.SetDestination(_playerTransform.position);
        }

        else if (Vector3.Distance(transform.position, _playerTransform.position) < RetreatDistance)
        {
            IsAttack = false;
            Vector3 dirToPlayer = transform.position - _playerTransform.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;

            Agent.SetDestination(newPos);
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
        if (Vector3.Distance(transform.position, _playerTransform.position) > VisibleDistance && IsMovingArea && !_isFirstVisiblePlayer) 
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (_indexMovePoints == MovePoints.Count)
                {
                    _indexMovePoints = 0;
                }

                Agent.SetDestination(MovePoints[_indexMovePoints].transform.position);
                _indexMovePoints++;
            }
        }

        else if (IsMovement && Vector3.Distance(transform.position, _playerTransform.position) > StoppingDistance 
                            && (Vector3.Distance(transform.position, _playerTransform.position) <= VisibleDistance || _isFirstVisiblePlayer))
        {
            IsVisiblePlayer = true;

            if (!_isFirstVisiblePlayer)
            {
                _isFirstVisiblePlayer = true;
                AudioManager.Instance.PlaySound("StartFight");
            }
            Agent.SetDestination(_playerTransform.position);

            if (!_isBlocked)
            {
                _animator.SetBool("PeopleRun", true);
                _animator.SetBool("PeopleAttack", false);
                _animator.SetBool("PeopleBlock", false);
            }
        }

        else if ( IsMovement && Vector3.Distance(transform.position, _playerTransform.position) < RetreatDistance)
        {
            if (!_isBlocked)
            {
                _animator.SetBool("PeopleRun", true);
                _animator.SetBool("PeopleAttack", false);
                _animator.SetBool("PeopleBlock", false);
            }

            Vector3 dirToPlayer = transform.position - _playerTransform.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;

            Agent.SetDestination(newPos);
        }

        else if (IsMovement && Vector3.Distance(transform.position, _playerTransform.position) >= RetreatDistance &&
            Vector3.Distance(transform.position, _playerTransform.position) <= StoppingDistance)
        {
            AttackLogicPeople();
        }

        if (_cooldownBlock > 0)
        {
            _cooldownBlock -= Time.deltaTime;

            if (_cooldownBlock <= 0f)
            {
                _isBlocked = false;
            }
        }

        if (_cooldown > 0)
        {   
            _cooldown-= Time.deltaTime;
        }
    }

    private void AttackLogicPeople()
    {
        if (_cooldownBlock <= 0 && IsMovement && IsAttack && _cooldown <= 0)
        {
            IsAttack = false;
            _isBlocked = true;
            
            _animator.SetBool("PeopleBlock", true);
            _animator.SetBool("PeopleRun", false);
            _animator.SetBool("PeopleAttack", false);
            
            _cooldownBlock = UnityEngine.Random.Range(MinCooldownBlock, MaxCooldownBlock);
        }
        
        if (_cooldown <= 0 && IsMovement && !_isBlocked)
        {
            IsAttack = true;
            OnCauseDamage?.Invoke();
            AudioManager.Instance.PlaySound("Attack2");
            _animator.SetBool("PeopleAttack", true);
            _animator.SetBool("PeopleRun", false);
            _animator.SetBool("PeopleBlock", false);

            _cooldown = UnityEngine.Random.Range(MinCooldownAttack, MaxCooldownAttack);
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
        IsMovement = isMovement;
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

        if (!(Health <= 0)) return;

        if (TypeEnemy == TypeEnemy.Outlaw)
        {
            OnDied?.Invoke();
            StartCoroutine(StartDieOutlaw());
        }

        else if (TypeEnemy == TypeEnemy.Wolf && Health <= 0)
        {
            OnDied?.Invoke();
        }
        
        else if (TypeEnemy == TypeEnemy.People && Health <= 0)
        {
            OnDied?.Invoke();
            SetMovement(false);
            StartCoroutine(StartDiePeople());
        }
        
    }

    public void TakeDamage(float damage)
    {
        if (TypeEnemy == TypeEnemy.People && _isBlocked)
        {
            damage /= 4;
        }
        
        Health -= damage;

        if (!(Health <= 0)) return;

        if (TypeEnemy == TypeEnemy.Outlaw)
        {
            OnDied?.Invoke();
            IsMovement = false;

            if (gameObject.activeSelf)
            {
                StartCoroutine(StartDieOutlaw());
            }
        }
        
        else if (TypeEnemy == TypeEnemy.Wolf && Health<=0)
        {
            OnDied?.Invoke();
        }

        else if (TypeEnemy == TypeEnemy.People && Health <= 0)
        {
            OnDied?.Invoke();
            StartCoroutine(StartDiePeople());
        }
    }

    private IEnumerator StartDieOutlaw()
    {
        _animator.SetBool("OutlawDie", true);
        
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }

    private IEnumerator StartDiePeople()
    {
        _animator.SetBool("PeopleRun", false);
        _animator.SetBool("PeopleAttack", false);
        _animator.SetBool("PeopleBlock", false);
        _animator.SetBool("PeopleDie", true);

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

    public void SetActiveNavMeshAgent(bool value)
    {
        if (Agent != null)
        {
            Agent.enabled = value;
        }
    } 
}

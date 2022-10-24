using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy3D : MonoBehaviour
{
    public event Action OnDeath;
    
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldownAttack;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _runDistance;
    [SerializeField] private bool _isMovement;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _pointRecoveryHealth;
    [SerializeField] private Image _healthBarEnemy;
    [SerializeField] private float _maxCooldownUseMedic;
    [SerializeField] private float _minCooldownUseMedic;
    

    private NavMeshAgent _agent;
    private Animator _animator;

    private float _localCooldownAttack;
    private float _localCooldownUseMedic;
    private float _maxHealth;
    private bool _isAttack;
    private bool _isRun;
    private bool _isWalk;
    private bool _isIdle;
    private bool _isCauseDamage;
    
    public bool IsAttack => _isAttack;
    public bool IsRun => _isRun;
    public bool IsWalk => _isWalk;
    public bool IsIdle => _isIdle;
    public bool IsMovement => _isMovement;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _maxHealth = _health;
        _healthBarEnemy.fillAmount = _health / _maxHealth;
        _localCooldownUseMedic = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMovement)
        {
            MovementLogic();
            UseMedic();
        }
    }

    private void UseMedic()
    {
        if (_health * 2 <= _maxHealth && _localCooldownUseMedic <= 0f)
        {
            _localCooldownUseMedic = Random.Range(_minCooldownUseMedic, _maxCooldownUseMedic);

            _health += _pointRecoveryHealth;

            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
            
            _healthBarEnemy.fillAmount = _health / _maxHealth;
            Debug.Log("Enemy used health");
            AudioManager.Instance.PlaySound("UseHealth");
        }

        else if (_localCooldownUseMedic>=0f)
        {
            _localCooldownUseMedic -= Time.deltaTime;
        }
    }
    public void SetMovement(bool value)
    {
        _isMovement = value;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        _healthBarEnemy.fillAmount = _health / _maxHealth;

        if (_health <= 0f)
        {
            OnDeath?.Invoke();
        }
    }

    public float GetDamage()
    {
        return _damage;
    }
    
    private void ResetAnimation()
    {
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isRun", false);
        _animator.SetBool("isAttack", false);
        _animator.SetBool("isWalk", false);
    }

    private void ResetAction()
    {
        _isAttack = _isIdle = _isRun = _isWalk = false;
    }

    private void SetAnimation(string name, bool value)
    {
        ResetAnimation();
        _animator.SetBool(name, value);
    }
    void MovementLogic()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) >= _runDistance)
        {
            ResetAction();
            _isIdle = true;
            
            SetAnimation("isWalk", true);
            _agent.SetDestination(_playerTransform.position);
        }

        else if (Vector3.Distance(transform.position, _playerTransform.position) < _runDistance 
                 && Vector3.Distance(transform.position, _playerTransform.position) > _attackDistance)
        {
            ResetAction();
            _isRun = true;
            SetAnimation("isRun", true);
            _agent.SetDestination(_playerTransform.position);

        }

        else if (Vector3.Distance(transform.position, _playerTransform.position) <= _attackDistance && _localCooldownAttack<=0f)
        {
            ResetAction();
            _isAttack = _isCauseDamage = true;
            _localCooldownAttack = _cooldownAttack;
            SetAnimation("isAttack", true);
            _agent.SetDestination(_playerTransform.position);
        }

        else
        {
            ResetAction();
            _isIdle = true;
            SetAnimation("isIdle", true);
        }

        if (_localCooldownAttack > 0f)
        {
            _localCooldownAttack -= Time.deltaTime;

            if (_localCooldownAttack <= 0f)
            {
                _isAttack = false;
            }
        }
    }
}

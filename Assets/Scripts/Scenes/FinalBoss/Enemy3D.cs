using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy3D : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldownAttack;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _runDistance;
    [SerializeField] private bool _isMovement;
    [SerializeField] private Transform _playerTransform;
    private NavMeshAgent _agent;
    private Animator _animator;

    private float _localCooldownAttack;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMovement)
        {
            MovementLogic();
        }
    }

    public void SetMovement(bool value)
    {
        _isMovement = value;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
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

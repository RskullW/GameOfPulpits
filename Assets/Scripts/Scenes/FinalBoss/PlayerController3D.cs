using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController3D : MonoBehaviour
{
    [SerializeField] private bool _isMovement;
    [SerializeField] private float _speed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _amountOfMedicine;
    [SerializeField] private float _levelGun;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _cooldownAttack;
    private float _localCooldownAttack;
    private bool _isAttack;
    private bool _isDefend;
    private bool _isDeath;
    private bool _isGetHit;
    private bool _isRun;
    private bool _isWalk;
    private bool _isIdle;
    
    public bool IsAttack => _isAttack;
    public bool IsDefend => _isDefend;
    public bool IsDeath => _isDeath;
    public bool IsGetHit => _isGetHit;
    public bool IsRun => _isRun;
    public bool IsWalk => _isWalk;
    public bool IsIdle => _isIdle;
    
    private Rigidbody _rigidbody;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMovement)
        {
            MovementLogic();
            InputLogic();
        }
    }

    private void MovementLogic()
    {
        if (_isDefend || _isAttack)
        {
            return;
        }
        
        float h = Input.GetAxis("Horizontal") * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            SetAnimation("isIdle", true);
        }

        else if (IsRun)
        {
            h *= _runSpeed;
            v *= _runSpeed;
            SetAnimation("isRun", true);
        }

        else
        {
            h *= _speed;
            v *= _speed;
            SetAnimation("isWalk", true);
        }

        transform.Translate(h, 0, v);
    }
    
    private void InputLogic()
    {
        // RUN

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isRun = true;
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isRun = false;
        }

        if (Input.GetKey(KeyCode.Mouse0) && !IsAttack)
        {
            _isAttack = true;
            _localCooldownAttack = _cooldownAttack;
            SetAnimation("isAttack", true);
        }

        else if (_isAttack)
        {
            ResetAnimation();
            _localCooldownAttack -= Time.deltaTime;

            if (_localCooldownAttack <= 0f)
            {
                _isAttack = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _isDefend = true;
            SetAnimation("isDefend", true);
        }
        
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _isDefend = false;
        }
    }

    private void ResetAnimation()
    {
        _animator.SetBool("isRun", false);
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isDeath", false);
        _animator.SetBool("isAttack", false);
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isDefend", false);
        _animator.SetBool("isGetHit", false);
    }

    private void SetAnimation(string name, bool value)
    {
        ResetAnimation();

        if (!_animator.GetBool(name))
        {
            _animator.SetBool(name, value);
        }
    }
    public void SetMovement(bool value)
    {
        _isMovement = value;
    }

    public bool GetMovement()
    {
        return _isMovement;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
    }

    public float GetDamage()
    {
        return _damage;
    }
    
}

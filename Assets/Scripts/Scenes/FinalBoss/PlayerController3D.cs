using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController3D : MonoBehaviour
{
    public event Action OnDeath;
    [SerializeField] private bool _isMovement;
    [SerializeField] private float _speed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _amountOfMedicine;
    [SerializeField] private float _levelGun;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _cooldownAttack;
    [SerializeField] private float _cooldownUseHealth;

    [SerializeField] private float _pointRecoveryHealth;
    // INTERFACE
    [SerializeField] private GameObject _interface;
    [SerializeField] private Image _healthBarPlayer;
    [SerializeField] protected Image _circleFill;
    [SerializeField] private TextMeshProUGUI _textMedicine;
    [SerializeField] private TextMeshProUGUI _textLevelGun;
    
    [SerializeField] private Color _colorCausePlayer;
    [SerializeField] private Color _colorCauseEnemy;
    
    private bool _isAttack;
    private bool _isDefend;
    private bool _isDeath;
    private bool _isGetHit;
    private bool _isRun;
    private bool _isWalk;
    private bool _isIdle;
    private bool _isActiveDialog;
    
    private float _localCooldownUseHealth;
    private float _localCooldownAttack;

    public bool IsAttack => _isAttack;
    public bool IsDefend => _isDefend;
    public bool IsDeath => _isDeath;
    public bool IsGetHit => _isGetHit;
    public bool IsRun => _isRun;
    public bool IsWalk => _isWalk;
    public bool IsIdle => _isIdle;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        if (SaveManager.IsHaveData || SaveManager.IsWasSave)
        {
            _levelGun = SaveManager.LevelGun;
            _amountOfMedicine = SaveManager.AmountOfMedicine;
            _health = SaveManager.Health;
        }

        if (_textLevelGun != null)
        {
            _textLevelGun.text = _levelGun.ToString();
        }
        
        if (_textMedicine != null)
        {
            _textMedicine.text = _amountOfMedicine.ToString();
        }
        
        if (_health <= 0)
        {
            _health = 20f;
        }

        _healthBarPlayer.fillAmount = _health / _maxHealth;


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
        if (_isDefend)
        {
            return;
        }
        
        float h = Input.GetAxis("Horizontal") * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            SetAnimation("isIdle", true);
            AudioManager.Instance.StopSoundWalk();
        }

        else if (IsRun)
        {
            h *= _runSpeed;
            v *= _runSpeed;
            SetAnimation("isRun", true);
            AudioManager.Instance.PlaySoundWalk("RunConcrete");
        }

        else
        {
            h *= _speed;
            v *= _speed;
            SetAnimation("isWalk", true);
            AudioManager.Instance.PlaySoundWalk("RunConcrete");
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
            
            AudioManager.Instance.PlaySound("Attack4");
        }

        else if (_isAttack)
        {
            _localCooldownAttack -= Time.deltaTime;

            if (_localCooldownAttack <= 0f)
            {
                _isAttack = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !_isAttack)
        {
            _isDefend = true;
            SetAnimation("isDefend", true);
        }
        
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _isDefend = false;
        }
        
        if (_amountOfMedicine > 0 && Input.GetKey(KeyCode.Q) && _localCooldownUseHealth <= 0f)
        {
            AudioManager.Instance.PlaySound("UseHealth");

            _amountOfMedicine -= 1;
            _health += _pointRecoveryHealth;
            _healthBarPlayer.fillAmount = _health / _maxHealth;

            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }

            _localCooldownUseHealth = _cooldownUseHealth;

            if (_textMedicine != null)
            {
                _textMedicine.text = _amountOfMedicine.ToString();

            }

            if (_circleFill != null)
            {
                _circleFill.fillAmount = 1f;
            }
        }

        if (_localCooldownUseHealth > 0f)
        {
            _localCooldownUseHealth -= Time.deltaTime;
            if (_circleFill != null)
            {
                _circleFill.fillAmount = _localCooldownUseHealth / _cooldownUseHealth;
            }
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

    public void SetActiveDialog(bool value)
    {
        _isActiveDialog = value;

        if (_interface != null)
        {
            if (_isActiveDialog)
            {
                SetMovement(false);
                _interface.gameObject.SetActive(false);
            }

            else
            {
                _interface.gameObject.SetActive(true);
            }
        }
    }

    public bool GetMovement()
    {
        return _isMovement;
    }

    public void TakeDamage(float damage)
    {
        if (!_isDeath)
        {
            if (_isDefend)
            {
                damage /= 4;
            }

            Debug.Log("Enemy caused damage player:" + damage);

            _health -= damage;

            _healthBarPlayer.fillAmount = _health / _maxHealth;

            if (_health <= 0f)
            {
                _isDeath = true;

                SetMovement(false);
                SetAnimation("isDeath", true);
                OnDeath?.Invoke();
                AudioManager.Instance.PlaySoundDeath();
            }

            else
            {
                if (!_isDefend)
                {
                    SetAnimation("isGetHit", true);
                }

                AudioManager.Instance.PlaySound("TakeDamage");
            }
        }
    }

    public float GetDamage()
    {
        return _damage;
    }

}

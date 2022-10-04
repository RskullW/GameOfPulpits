using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public event Action OnSellerGun;
    public event Action OnSellerMedic;
    public event Action OnSellerItems;
    public event Action OnMessageClick;
    
    [SerializeField] private float _durationCooldown;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _damage;

    private Position _position;
    private Animator _animator;
    private Rigidbody _rigidbody;
    
    private bool _isAttack;
    private bool _isShowMessage;
    
    private bool _isSellerItems;
    private bool _isSellerMedic;
    private bool _isSellerGuns;

    
    void Start()
    {
        _position = Position.Down;
        _isAttack = false;
        
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        _isShowMessage = _isSellerGuns = _isSellerItems = _isSellerMedic = false;
        
    }
    void SetAnimations()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _animator.SetBool("isUpRun", false);
            _animator.SetBool("isLeftRun", true);
            _animator.SetBool("isDownRun", false);
            _animator.SetBool("isRightRun", false);
            _position = Position.Left;
        }

        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _animator.SetBool("isUpRun", false);
            _animator.SetBool("isLeftRun", false);
            _animator.SetBool("isDownRun", false);
            _animator.SetBool("isRightRun", true);
            _position = Position.Right;
        }

        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            _animator.SetBool("isUpRun", true);
            _animator.SetBool("isLeftRun", false);
            _animator.SetBool("isDownRun", false);
            _animator.SetBool("isRightRun", false);
            _position = Position.Up;
        }

        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            _animator.SetBool("isUpRun", false);
            _animator.SetBool("isLeftRun", false);
            _animator.SetBool("isDownRun", true);
            _animator.SetBool("isRightRun", false);
            _position = Position.Down;
        }

        if (Input.GetKey(KeyCode.LeftControl) && !_isAttack)
        {
            if (_animator.GetBool("isLeftRun") && !_animator.GetBool("isLeftAttack"))
            {
                _animator.SetBool("isLeftAttack", true);
                StartCoroutine(StartAttackTime());
                _isAttack = true;
            }
            
            else if (_animator.GetBool("isRightRun")  && !_animator.GetBool("isRightAttack"))
            {
                _animator.SetBool("isRightAttack", true);
                StartCoroutine(StartAttackTime());
                _isAttack = true;
            }
            
            else if (_animator.GetBool("isDownRun") && !_animator.GetBool("isDownAttack"))
            {
                _animator.SetBool("isDownAttack", true);
                StartCoroutine(StartAttackTime());
                _isAttack = true;
            }
            
            else if (_animator.GetBool("isUpRun") && !_animator.GetBool("isUpAttack"))
            {
                _animator.SetBool("isUpAttack", true);
                StartCoroutine(StartAttackTime());
                _isAttack = true;
            }
        }
        
        else
        {
            _animator.SetBool("isUpAttack", false);
            _animator.SetBool("isDownAttack", false);
            _animator.SetBool("isRightAttack", false);
            _animator.SetBool("isLeftAttack", false);
        }
    }
    void FixedUpdate()
    {
        SetAnimations();
        MovementLogic();
        InputLogic();
    }

    private void InputLogic()
    {
        if (_isShowMessage && Input.GetKey(KeyCode.E))
        {
            if (_isSellerGuns)
            {
                OnSellerGun?.Invoke();
            }
            
            else if (_isSellerItems)
            {
                OnSellerItems?.Invoke();
            }

            else
            {
                OnSellerMedic?.Invoke();
            }
        }
    }
    private void MovementLogic()
    {
        if (!_isAttack)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");

            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
            transform.Translate(movement * _playerSpeed * Time.fixedDeltaTime);
        }

    }
    IEnumerator StartAttackTime()
    {
        yield return new WaitForSeconds(_durationCooldown);
        _isAttack = false;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "PointSeller")
        {
            if (!_isShowMessage)
            {
                if (collider.gameObject.name == "SellerMedic")
                {
                    _isSellerMedic = true;
                }
                
                else if (collider.gameObject.name == "SellerGuns")
                {
                    _isSellerGuns = true;
                }

                else
                {
                    _isSellerItems = true;
                }
                _isShowMessage = true;
                OnMessageClick?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log("OUT");

        if (collider.tag == "PointSeller")
        {
            if (_isShowMessage)
            {
                _isShowMessage = _isSellerGuns = _isSellerItems = _isSellerMedic = false;
                OnMessageClick?.Invoke();
            }
        }
    }
}

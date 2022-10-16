using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public event Action OnCauseDamage;
    public event Action OnSetMoney;
    public event Action OnSetGarbage;
    public event Action OnSetMedicine;
    public event Action OnSellerGun;
    public event Action OnSellerMedic;
    public event Action OnSellerItems;
    public event Action OnCloseDialogWithSellers;
    public event Action OnMessageClick;

    [SerializeField] protected int _health;
    [SerializeField] protected int _money;
    [SerializeField] protected int _amountOfMedicine;
    [SerializeField] protected int _levelGun;
    [SerializeField] protected int _amountOfGarbage;
    
    [SerializeField] protected float _durationCooldown;
    [SerializeField] protected float _playerSpeed;
    [SerializeField] protected float _damage;

    protected Position _position;
    protected Animator _animator;

    protected bool _isAttack;
    protected bool _isShowMessage;

    protected bool _isSellerItems;
    protected bool _isSellerMedic;
    protected bool _isSellerGuns;
    
   
    protected bool _causeDamage;
    protected bool _isActiveDialogue;
    


    void Start()
    {
        _position = Position.Down;
        _isAttack = false;

        _animator = GetComponent<Animator>();

        _isShowMessage = _isSellerGuns = _isSellerItems = _isSellerMedic = false;

    }

    IEnumerator SetAnimations()
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
                _isAttack = _causeDamage = true;
                StartCoroutine(StartAttackTime());
                AudioManager.Instance.PlaySound("Attack1");
            }

            else if (_animator.GetBool("isRightRun") && !_animator.GetBool("isRightAttack"))
            {
                _animator.SetBool("isRightAttack", true);
                _isAttack = _causeDamage = true;
                StartCoroutine(StartAttackTime());
                AudioManager.Instance.PlaySound("Attack1");

            }

            else if (_animator.GetBool("isDownRun") && !_animator.GetBool("isDownAttack"))
            {
                _animator.SetBool("isDownAttack", true);
                _causeDamage = _isAttack = true;
                StartCoroutine(StartAttackTime());
                AudioManager.Instance.PlaySound("Attack1");

            }

            else if (_animator.GetBool("isUpRun") && !_animator.GetBool("isUpAttack"))
            {
                _animator.SetBool("isUpAttack", true);
                _isAttack = _causeDamage = true;
                StartCoroutine(StartAttackTime());
                AudioManager.Instance.PlaySound("Attack1");

            }
            
        }

        else if (!_isAttack)
        {
            _animator.SetBool("isUpAttack", false);
            _animator.SetBool("isDownAttack", false);
            _animator.SetBool("isRightAttack", false);
            _animator.SetBool("isLeftAttack", false);
        }
        
        yield return new WaitForSeconds(0.5f);
    }

    void FixedUpdate()
    {
        if (!_isActiveDialogue)
        {
            StartCoroutine(SetAnimations());
            MovementLogic();
            InputLogic();
        }

        else
        {
            ProcessDialogClicks();
        }
    }

    protected void ProcessDialogClicks()
    {
        if (!_isActiveDialogue) return;
        
        if (_isSellerGuns)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                if (_money >= 25)
                {
                    _money -= 25;
                    _levelGun++;
                    OnSetMoney?.Invoke();

                    AudioManager.Instance.PlaySound("Money");
                }
            }

            else if (Input.GetKey(KeyCode.Alpha2))
            {
                Application.OpenURL("https://www.playgwent.com/ru");

                _isActiveDialogue = false;
                OnCloseDialogWithSellers?.Invoke();
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                _isActiveDialogue = false;
                OnCloseDialogWithSellers?.Invoke();
            }
        }

        else if (_isSellerItems)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                Debug.Log(_amountOfGarbage);
                if (_amountOfGarbage > 0)
                {
                    _money += 2*_amountOfGarbage;
                    _amountOfGarbage = 0;
                    OnSetMoney?.Invoke();
                    OnSetGarbage?.Invoke();
                    AudioManager.Instance.PlaySound("Money");
                }
            }
            
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                _isActiveDialogue = false;
                OnCloseDialogWithSellers?.Invoke();
            }
        }

        else if (_isSellerMedic)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                if (_money >= 5)
                {
                    _money -= 5;
                    _amountOfMedicine++;
                    OnSetMoney?.Invoke();
                    OnSetMedicine?.Invoke();
                    AudioManager.Instance.PlaySound("Money");
                }
            }

            else if (Input.GetKey(KeyCode.Alpha2))
            {
                Application.OpenURL("https://www.youtube.com/watch?v=-ix-RldHz0g");

                _isActiveDialogue = false;
                OnCloseDialogWithSellers?.Invoke();
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                _isActiveDialogue = false;
                OnCloseDialogWithSellers?.Invoke();
            }
        }
    }

    protected void InputLogic()
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

    protected void MovementLogic()
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
        
        yield return new WaitForSeconds(_durationCooldown/2);
        AudioManager.Instance.PlaySound("Attack1");
        yield return new WaitForSeconds(_durationCooldown / 2);
        _isAttack = false;
    }

    protected void OnTriggerStay(Collider collider)
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
        
        if (collider.tag == "Enemy" && _isAttack && _causeDamage)
        {
            OnCauseDamage?.Invoke();
            _causeDamage = false;
        }

    }

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "ExitLevel")
        {
            var position = transform.position;
            position.z += 2;
            SaveManager.LoadData(position, _money, _health, "1200", _levelGun, _amountOfGarbage, _amountOfMedicine);
            SceneManager.LoadScene("MainMap");
        }

    }

    protected void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "PointSeller")
        {
            if (_isShowMessage)
            {
                _isShowMessage = _isSellerGuns = _isSellerItems = _isSellerMedic = false;
                OnMessageClick?.Invoke();
            }
        }
    }

    public void SetActiveDialogue(bool isActive)
    {
        _isActiveDialogue = isActive;
    }

    public void SetHealth(int health)
    {
        _health = health;
    }

    public int GetHealth()
    {
        return _health;
    }
    public void SetMoney(int money)
    {
        _money = money;
    }
    public int GetMoney()
    {
        return _money;
    }
    public void SetAmountMedicine(int amountOfMedicine)
    {
        _amountOfMedicine = amountOfMedicine;
    }
    public int GetAmountMedicine()
    {
        return _amountOfMedicine;
    }
    public void SetLevelGun(int levelGun)
    {
        _levelGun = levelGun;
    }
    public int GetLevelGun()
    {
        return _levelGun;
    }
    public void SetAmountGarbage(int amountOfGarbage)
    {
        _amountOfGarbage = amountOfGarbage;
    }
    public int GetAmountGarbage()
    {
        return _amountOfGarbage;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetDefaultAnimation()
    {
        _animator.SetBool("isUpRun", false);
        _animator.SetBool("isLeftRun", false);
        _animator.SetBool("isDownRun", true);
        _animator.SetBool("isRightRun", false);
        _position = Position.Down;
    }
}

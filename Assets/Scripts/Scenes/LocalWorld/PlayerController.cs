using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _health;
    [SerializeField] protected int _money;
    [SerializeField] protected int _amountOfMedicine;
    [SerializeField] protected int _levelGun;
    [SerializeField] protected int _amountOfGarbage;
    
    [SerializeField] protected float _durationCooldown;
    [SerializeField] protected float _playerSpeed;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _pointRecoveryHealth;
    [SerializeField] protected float _cooldownUseHealth;
    [SerializeField] protected Image _circleFill;
    
    [SerializeField] private Image _healthBarPlayer;
    [SerializeField] private TextMeshProUGUI _textStatusBar;
    [SerializeField] private TextMeshProUGUI _textMedicine;
    [SerializeField] private TextMeshProUGUI _textLevelGun;
    protected Position _position;
    protected Animator _animator;

    protected bool _isAttack;
    protected bool _isShowMessage;

    protected bool _isSellerItems;
    protected bool _isSellerMedic;
    protected bool _isSellerGuns;
    
   
    protected bool _causeDamage;
    protected bool _isActiveDialogue;

    protected float _localCooldownUseHealth;


    void Start()
    {
        _position = Position.Down;
        _isAttack = false;
        _animator = GetComponent<Animator>();

        _isShowMessage = _isSellerGuns = _isSellerItems = _isSellerMedic = false;
        
        _levelGun = SaveManager.LevelGun;
        _amountOfGarbage = SaveManager.AmountOfGarbage;
        _amountOfMedicine = SaveManager.AmountOfMedicine;
        _health = SaveManager.Health;
        _money = SaveManager.Money;
        
        _textLevelGun.text = _levelGun.ToString();
        _textMedicine.text = _amountOfMedicine.ToString();

        if (_health <= 0)
        {
            _health = 10f;
        }
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
            
            _healthBarPlayer.fillAmount = _health / _maxHealth;
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

        if (_amountOfMedicine > 0 && Input.GetKey(KeyCode.Q) && _localCooldownUseHealth <= 0f)
        {
            AudioManager.Instance.PlaySound("UseHealth");
            
            _amountOfMedicine -= 1;

            _health += _pointRecoveryHealth;
            
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }

            _localCooldownUseHealth = _cooldownUseHealth;
            _textMedicine.text = _amountOfMedicine.ToString();


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
                _circleFill.fillAmount = _localCooldownUseHealth/_cooldownUseHealth;
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
        yield return new WaitForSeconds(_durationCooldown / 4);
        _textStatusBar.text = "";
        yield return new WaitForSeconds(_durationCooldown / 4);
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

            if (MenuManager.Language == Language.Rus)
            {
                _textStatusBar.text = "Вы нанесли врагу " + _damage + " единиц урона.";
            }

            else
            {
                _textStatusBar.text = "You have dealt " + _damage + " points of damage to the enemy";
            }

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

    public float GetHealth()
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
        _animator.SetBool("isUpAttack", false);
        _animator.SetBool("isUpRun", false);
        _animator.SetBool("isLeftRun", false);
        _animator.SetBool("isLeftAttack", false);
        _animator.SetBool("isRightRun", false);
        _animator.SetBool("isRightAttack", false);
        
        _animator.SetBool("isDownRun", true);
        _animator.SetBool("isDownAttack", false);
        _position = Position.Down;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }
}

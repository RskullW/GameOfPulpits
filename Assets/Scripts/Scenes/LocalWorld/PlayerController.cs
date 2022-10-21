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

    [SerializeField] private GameObject _interface;
    [SerializeField] private Image _healthBarPlayer;
    [SerializeField] private TextMeshProUGUI _textMedicine;
    [SerializeField] private TextMeshProUGUI _textLevelGun;
    
    [Space]
    [SerializeField] private List<TextMeshProUGUI> _textsLogBar;

    [SerializeField] private Color _colorCausePlayer;
    [SerializeField] private Color _colorTakeDamagePlayer;
    
    [Space]
    protected Position _position;
    protected Animator _animator;

    protected bool _isAttack;
    protected bool _isShowMessage;

    protected bool _isBlock;

    [SerializeField] protected float _cooldownBlock;
    protected float _localCooldownBlock;
    
    protected bool _isSellerItems;
    protected bool _isSellerMedic;
    protected bool _isSellerGuns;
    
   
    protected bool _isCauseDamage;
    protected bool _isActiveDialogue;
    
    protected float _localCooldownUseHealth;

    protected float _cooldownBuyItem;
    protected float _localCooldownBuyItem;
    
    protected void Start()
    {
        _cooldownBuyItem = 0.2f;
        _position = Position.Down;
        _isAttack = false;
        _animator = GetComponent<Animator>();

        _isShowMessage = _isSellerGuns = _isSellerItems = _isSellerMedic = false;

        if (SaveManager.IsHaveData || SaveManager.IsWasSave)
        {
            _levelGun = SaveManager.LevelGun;
            _amountOfGarbage = SaveManager.AmountOfGarbage;
            _amountOfMedicine = SaveManager.AmountOfMedicine;
            _health = SaveManager.Health;
            _money = SaveManager.GetMoney();

            if (SceneManager.GetActiveScene().name == "Castle Player")
            {
                transform.position = SaveManager.PositionPlayer;
            }
            
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
        if (_textsLogBar != null)
        {
            SetDefaultLogsBar();
        }
    }

    protected void SetDefaultLogsBar()
    {

        foreach (var textMeshProUGUI in _textsLogBar)
        {
            textMeshProUGUI.text = "";
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
                _isAttack = _isCauseDamage = true;
                StartCoroutine(StartAttackTime());
                AudioManager.Instance.PlaySound("Attack1");
            }

            else if (_animator.GetBool("isRightRun") && !_animator.GetBool("isRightAttack"))
            {
                _animator.SetBool("isRightAttack", true);
                _isAttack = _isCauseDamage = true;
                StartCoroutine(StartAttackTime());
                AudioManager.Instance.PlaySound("Attack1");

            }

            else if (_animator.GetBool("isDownRun") && !_animator.GetBool("isDownAttack"))
            {
                _animator.SetBool("isDownAttack", true);
                _isCauseDamage = _isAttack = true;
                StartCoroutine(StartAttackTime());
                AudioManager.Instance.PlaySound("Attack1");

            }

            else if (_animator.GetBool("isUpRun") && !_animator.GetBool("isUpAttack"))
            {
                _animator.SetBool("isUpAttack", true);
                _isAttack = _isCauseDamage = true;
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_position == Position.Left || _position == Position.Down)
            {
                    DisableAnimations();
                _animator.SetBool("isBlockLeftDown", true);
                _isBlock = true;
            }
            
            else if (_position == Position.Right || _position == Position.Up)
            {
                DisableAnimations();
                _animator.SetBool("isBlockRightUp", true);
                _isBlock = true;
            }
        }
        
        else if (!Input.GetKey(KeyCode.LeftShift) && _isBlock)
        {
            _isBlock = false;
            DisableAnimations();

            switch (_position)
            {
                case Position.Down: _animator.SetBool("isDownRun", true); break;
                case Position.Up: _animator.SetBool("isUpRun", true); break;
                case Position.Left: _animator.SetBool("isLeftRun", true); break;
                case Position.Right: _animator.SetBool("isRightRun", true); break;
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    public void DisableAnimations()
    {
        _animator.SetBool("isUpRun", false);
        _animator.SetBool("isLeftRun", false);
        _animator.SetBool("isDownRun", false);
        _animator.SetBool("isRightRun", false);
        _animator.SetBool("isRightAttack", false);
        _animator.SetBool("isLeftAttack", false);
        _animator.SetBool("isDownAttack", false);
        _animator.SetBool("isUpAttack", false);
        _animator.SetBool("isBlockLeftDown", false);
        _animator.SetBool("isBlockRightUp", false);
    }
    void FixedUpdate()
    {
        if (!_isActiveDialogue)
        {
            StartCoroutine(SetAnimations());
            MovementLogic();
            InputLogic();

            if (_healthBarPlayer != null)
            {

                _healthBarPlayer.fillAmount = _health / _maxHealth;
            }
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

                    AudioManager.Instance.PlaySound("ImproveGun");
                }
            }

            else if (Input.GetKey(KeyCode.Alpha2))
            {
                if (_amountOfGarbage >= 10)
                {
                    _amountOfGarbage -= 10;
                    _levelGun++;
                    OnSetGarbage?.Invoke();

                    AudioManager.Instance.PlaySound("ImproveGun");
                }
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
                    _money += 2 * _amountOfGarbage;
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
                if (_money >= 5 && _localCooldownBuyItem <= 0f)
                {
                    _money -= 5;
                    _amountOfMedicine++;
                    OnSetMoney?.Invoke();
                    AudioManager.Instance.PlaySound("Money");
                    _localCooldownBuyItem = _cooldownBuyItem;
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

        // COOLDOWN TIME BUY/SELL ITEM
        if (_localCooldownBuyItem >= 0f)
        {
            _localCooldownBuyItem -= Time.deltaTime;
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

    protected void MovementLogic()
    {
        if (!_isAttack)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");

            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            transform.Translate(movement * _playerSpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator StartAttackTime()
    {
        yield return new WaitForSeconds(_durationCooldown / 2);
        AudioManager.Instance.PlaySound("Attack1");
        yield return new WaitForSeconds(_durationCooldown / 4);

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

        if (collider.tag == "Enemy" && _isAttack && _isCauseDamage)
        {
            if (_textsLogBar != null)
            {
                SetLogsBar(GetDamage());
            }
            
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();

            if (enemy.TypeEnemy == TypeEnemy.Outlaw)
            {
                enemy.TakeDamage(GetDamage());
            }

            OnCauseDamage?.Invoke();
            _isCauseDamage = false;
        }
    }

    public void SetLogsBar(float damage)
    {
        string text;

        if (MenuManager.Language == Language.Rus)
        {
            if (_isCauseDamage)
            {
                text = "Вы нанесли врагу " + damage + " единиц урона.";
            }

            else
            {
                text = "Враг нанёс вам " + damage + " единиц урона.";
            }
        }

        else
        {
            if (_isCauseDamage)
            {
                text = "You have dealt " + damage + " points of damage to the enemy";
            }

            else
            {
                text = "The enemy has dealt you " + damage + " damage units.";
            }
        }

        for (int index = _textsLogBar.Count-1; index > 0; --index)
        {
            _textsLogBar[index].text = _textsLogBar[index-1].text;
            _textsLogBar[index].color = _textsLogBar[index-1].color;
        }
        
        _textsLogBar[0].color = (_isCauseDamage) ? _colorCausePlayer : _colorTakeDamagePlayer;
        _textsLogBar[0].text = text;
    }

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "ExitLevel")
        {
            var position = transform.position;
            position.z += 2;
            SaveManager.LoadData(position, _money, _health, _levelGun, _amountOfGarbage, _amountOfMedicine);
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

        if (_interface != null)
        {
            if (_isActiveDialogue)
            {
                _interface.gameObject.SetActive(false);
            }

            else
            {
                _interface.gameObject.SetActive(true);
            }
        }
    }

    public void SetHealth(float health)
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
        return (_levelGun<=0)? _damage:_damage + _damage*(_levelGun*0.5f);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void TakeDamage(float damage)
    {
        if (_isBlock)
        {
            damage /= 4;
        }
        
        _health -= damage;
        

        _isCauseDamage = false;
        SetLogsBar(damage);
    }

    public void SetDefaultAnimation()
    {
        _animator.SetBool("isDeath", false);
        _animator.SetBool("isUpAttack", false);
        _animator.SetBool("isUpRun", false);
        _animator.SetBool("isLeftRun", false);
        _animator.SetBool("isLeftAttack", false);
        _animator.SetBool("isRightRun", false);
        _animator.SetBool("isRightAttack", false);
        _animator.SetBool("isBlockRightUp", false);
        _animator.SetBool("isBlockLeftDown", false);
        
        _animator.SetBool("isDownRun", false);
        _animator.SetBool("isDownAttack", false);
        _position = Position.Down;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public bool GetIsBlock()
    {
        return _isBlock;
    }

    public void SetAnimation(string name, bool isActive)
    {
        _animator.SetBool(name, isActive);
    }
    
    
}

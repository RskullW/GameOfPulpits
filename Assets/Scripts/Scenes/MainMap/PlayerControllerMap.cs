using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerControllerMap : PlayerController
{

    public event Action OnHideMessage;
    public event Action OnShowMessage;
    
    private bool _isPlayerCanMove;
    private bool _isTransformHouse;
    private SpriteRenderer _sprite;

    protected void Start()
    {
        base.Start();
        
        _sprite = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        if (_isPlayerCanMove)
        {
            MovementLogic();
            InputLogic();
        }
    }

    protected void InputLogic()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (_isTransformHouse)
            {
                SaveManager.LoadMapPosition(transform.position);
                SceneManager.LoadScene("Castle Player");
            }
        }
    }
    private void MovementLogic()
    {

        transform.Translate(0, Input.GetAxis("Vertical") * _playerSpeed * Time.deltaTime, 0);
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * _playerSpeed * Time.deltaTime;

        _sprite.flipX = Input.GetAxis("Horizontal") < 0 ? false: true;

    }

    public void SetPlayerCanMove(bool isPlayerCanMove)
    {
        _isPlayerCanMove = isPlayerCanMove;
    }

    public void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.tag == "FirstBoss")
        {
            if (SaveManager.IsFirstBoss != 1)
            {
                SaveManager.LoadMapPosition(transform.position);
                SaveManager.SaveStatsMission("MainMap");
                SceneManager.LoadScene("FirstBoss");
            }
        }
        
        else if (collider.gameObject.tag == "InHouse" && _isPlayerCanMove)
        {
            OnShowMessage?.Invoke();
            _isTransformHouse = true;
        }
    }

    protected void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "InHouse" && _isPlayerCanMove)
        {
            OnHideMessage?.Invoke();
            _isTransformHouse = false;
        }
    }
}

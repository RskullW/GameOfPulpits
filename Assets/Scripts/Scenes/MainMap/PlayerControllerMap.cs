using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerControllerMap : PlayerController
{

    private bool _isPlayerCanMove;
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
    }

}

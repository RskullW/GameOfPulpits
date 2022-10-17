using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerMap : PlayerController
{

    private bool _isPlayerCanMove;
    void FixedUpdate()
    {
        if (_isPlayerCanMove)
        {
            MovementLogic();
        }
    }

    private void MovementLogic()
    {
        float speed = _playerSpeed;
        if (speed <= 0f)
        {
            speed = 0.11f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            speed += 0.1f;
        }
        
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            speed -= 0.1f;
        }
        
        transform.Translate(0, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0);
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);


    }

    public void SetPlayerCanMove(bool isPlayerCanMove)
    {
        _isPlayerCanMove = isPlayerCanMove;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "FirstBoss" && !SaveManager.GetInformationFirstBoss())
        {
            SaveManager.SaveMapPosition(transform.position);
            SaveManager.SaveStatsMission("MainMap");
            SceneManager.LoadScene("FirstBoss");
        }
    }

}

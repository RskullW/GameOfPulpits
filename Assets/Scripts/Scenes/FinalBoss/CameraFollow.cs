using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _mouseSensX = 1f;
    [SerializeField] private float _mouseSensY = 1f;
    [SerializeField] private Transform _target;
    [SerializeField] private PlayerController3D _playerController3D;
    private float _xRotation = 0f;
  
    void Update()
    {
        if (_playerController3D.GetMovement())
        {
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensX * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensY * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -20, 10f);
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _target.Rotate(Vector3.up * mouseX);
        }
    }
}

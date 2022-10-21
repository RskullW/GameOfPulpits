using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    private Transform _player;
    private Vector3 _target;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _target = new Vector3(_player.position.x, _player.position.y, _player.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

        if (transform.position.x == _target.x && transform.position.y == _target.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage(_damage);
        }

        if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

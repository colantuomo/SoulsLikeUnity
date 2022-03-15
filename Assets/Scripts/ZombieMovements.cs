using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovements : MonoBehaviour
{
    public float speed = 5f;
    public float searchRadius = 5f;
    public Transform player;
    private Animator _animator;
    private CharacterController _cc;
    private float _gravity = 9.8f;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        _gravity = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateGravity();
        if (Physics.OverlapSphere(transform.position, searchRadius, 1 << LayerMask.NameToLayer("Player")).Length > 0)
        {
            _animator.SetBool("Running", true);
            RotateTowardsPlayer();
            _cc.Move(transform.forward * speed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("Running", false);
        }
    }

    void Move()
    {
        var directionToWalk = transform.forward;
        // _gravity -= 9.8f * Time.deltaTime;
        // directionToWalk.y -= _gravity * Time.deltaTime;
        Debug.Log(_gravity);
        _cc.Move(directionToWalk * Time.deltaTime * speed);
    }

    void RotateTowardsPlayer()
    {
        var _targetDirection = player.position - transform.position;
        // _targetDirection.y = 0f;
        var _lookRotation = Quaternion.LookRotation(_targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 15f);
    }

    void CalculateGravity()
    {
        // _gravity -= 9.8f * Time.deltaTime;
        // if (!_cc.isGrounded)
        // {
        // }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
       private CharacterController _controller;
       private Transform _camera;
       private float _horizontal;
       private float _vertical;

       [SerializeField] private float _playerSpeed = 5;
       [SerializeField] private float _jumpForce = 6;
       [SerializeField] private Transform _sensorPosition;
       [SerializeField] private float _sensorRadius = 0.2f;
       [SerializeField] private float _groundLayer;
       [SerializeField] private float _turnSmoothVeloity;
       [SerializeField] private float _turnSmoothTime = 0.1f;

       private float _gravity = -9.81f;
       private Vector3 _playerGravity;
       private bool _isGrounded;
       Animator _anim;
       


    // Update is called once per frame
    void Update()
    {
         _horizontal = Input.GetAxisRaw("Horizontal");
         _vertical = Input.GetAxisRaw("Vertical");

         Movement();
         Jump();

    }

    void Awake()
    {
       _controller = GetComponent<CharacterController>();
       _camera = Camera.main.transform;
       _anim = GetComponentInChildren<Animator>();

    }

    void Movement()
    {
       Vector3 direction = new Vector3(_horizontal ,0, _vertical);
        
         if (direction != Vector3.zero)
           {
               float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
               float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVeloity, _turnSmoothTime);
               transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
               Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
               _controller.Move(moveDirection * _playerSpeed * Time.deltaTime);
           }

        _anim.SetFloat("VelX", 0);
        _anim.SetFloat("VelY", direction.magnitude);
    }

    void Jump()
    {
        //_isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
        _playerGravity.y += _gravity * Time.deltaTime;
        _controller.Move (_playerGravity * Time.deltaTime);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -2;
        }

        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpForce * -2 * _gravity);
            _anim.SetBool("IsJumping",! _isGrounded);
        }
    }
}

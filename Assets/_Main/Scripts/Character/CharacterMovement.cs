using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Unity.Collections;

public class CharacterMovement : NetworkBehaviour
{
    //Companents
    public CharacterController _controller;
    public Animator anim;
    public Transform groundChecker;
    public CinemachineVirtualCamera cam;

    //Vertical Movement Values
    public int jumpPower = 1;
    private float _gravity = -9.81f;
    private bool _isGrounded;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private  LayerMask groundMask;
  
    //Horizontal Movement Values
    public float speed = 12f;
    public float movementSmoothTime;
    private float _horizontal;
    private float _vertical;
    private bool _move;
    private float _stamina;
    public float staminaRegen;
    private Vector3 _velocity;
    private Vector2 _currentMovement;
    private Vector2 _smoothMove;
    private float _staminaTimer;

    //First Person Camera
    public float mouseSensivity = 10f;
    private float xRotation = 0.0f;

    //Canvas
    public Image StaminaBar;

    //Crouch Values
    private bool _crouch = false;
    private bool _crouchDelay = false;
   
    //Hiding Values
    public bool playerHiding = false;

    public override void OnStartClient()
    {
        DeathManager.instance.cam.Add(cam);

        if (!isLocalPlayer)
        {
            cam.gameObject.SetActive(false);
        }

        //DeathManager.instance.FindPlayerCam();
        
        Cursor.lockState = CursorLockMode.Locked;
        
        _stamina = 100;
    }


    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Gravity();
        CharacterInputs();
        CheckGrounded();
        Movement();
        UpdaterAnimatorValues();
        CameraController();
    }

    private void CameraController()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
 
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
 
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    private void CharacterInputs()
    {
        _horizontal = Input.GetAxis("Horizontal");
        
        _vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonUp("Jump") && _isGrounded)
        {
            Jump();
        }

        if (Input.GetButtonUp("Crouch"))
        {
            if (!_crouchDelay)
            {
                StartCoroutine(Crouch());
            }
        }

        //Run and Stamina
        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) && _stamina > 0)
        {
            _vertical *= 2;
            _stamina -= Time.deltaTime * staminaRegen;
            StaminaBar.fillAmount = _stamina / 100;
            _staminaTimer = 0;
        }

        if (_vertical != 2 && _stamina < 100)
        {
            if (_staminaTimer < 2.1f)
            {
                _staminaTimer += Time.deltaTime;
            }
            
            if (_staminaTimer >= 2)
            {
                _stamina += Time.deltaTime * staminaRegen;
                StaminaBar.fillAmount = _stamina / 100;
            }
        }
        
        if (_stamina > 100)
        {
            _stamina = 100;
        }
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
    }

    private void Movement()
    {
        Vector2 input = new Vector2(_horizontal, _vertical);
        
        _smoothMove = Vector2.SmoothDamp(_smoothMove, input, ref _currentMovement, movementSmoothTime);
        
        Vector3 move = transform.right * _smoothMove.x + _smoothMove.y * transform.forward;

        _controller.Move(move * speed * Time.deltaTime);

        if (input.magnitude > 0)
        {
            _move = true;
        }

        else
        {
            _move = false;
        }
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(jumpPower * -2 * _gravity);
    }

    private IEnumerator Crouch()
    {
        _crouchDelay = true;
        
        _crouch = !_crouch;
        
        if (_crouch)
        {
            speed /= 2;
            anim.SetBool("crouch", true);
        }

        else if (!_crouch)
        {
            speed *= 2;
            anim.SetBool("crouch", false);
        }
        
        yield return new WaitForSeconds(0.65f);

        _crouchDelay = false;
    }
    
    private void Gravity()
    {
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = 0f;
        }
        
        _velocity.y += _gravity * Time.deltaTime;
        
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void UpdaterAnimatorValues()
    {
        anim.SetBool("grounded", _isGrounded);
        anim.SetBool("move", _move);
        anim.SetFloat("horizontal", _horizontal, 0.2f, Time.deltaTime);
        anim.SetFloat("vertical", _vertical,0.2f, Time.deltaTime);
    }
}

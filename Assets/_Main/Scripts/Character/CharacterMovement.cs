using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Mirror;
using Unity.Collections;

public class CharacterMovement : NetworkBehaviour
{
    //Companents
    public CharacterController _controller;
    public Animator anim;
    public Transform groundChecker;
    public CinemachineVirtualCamera cam;

    //Values
    public int jumpPower;
    public float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private  LayerMask groundMask;
    private Vector3 _velocity;
    [ReadOnly] private bool _isGrounded;
    private bool _move;
    private float _horizontal;
    private float _vertical;
    private Vector2 currentMovement;
    private Vector2 smoothMove;
    public float movementSmoothTime;
    private bool _crouch = false;
    private bool _crouchDelay = false;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            cam.gameObject.SetActive(false);
        }
        // cam.Priority++;
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

        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W))
        {
            _vertical *= 2;
        }
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
    }

    private void Movement()
    {
        Vector2 input = new Vector2(_horizontal, _vertical);
        
        smoothMove = Vector2.SmoothDamp(smoothMove, input, ref currentMovement, movementSmoothTime);
        
        Vector3 move = new Vector3(smoothMove.x, 0 , smoothMove.y);
    
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
        _velocity.y = Mathf.Sqrt(jumpPower * -2 * gravity);
    }

    /*private void Crouch()
    {
        if (_crouch)
        {
            speed /= 2;
            _anim.SetBool("crouch", true);
        }

        else if (!_crouch)
        {
            speed *= 2;
            _anim.SetBool("crouch", false);
        }
    }*/
    
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
        
        _velocity.y += gravity * Time.deltaTime;
        
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

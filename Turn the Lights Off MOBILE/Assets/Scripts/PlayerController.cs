using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ground detection")] public Transform GroundDetectionPoint;
    public float GroundDetectReach;

    [Header("Grab/pull area related stuff")]
    public Transform GrabPoint;
    private bool _wasPreviouslyPulling;

    public float GrabReach;
    public GameObject PullEffect;
    public LayerMask KidLayer;

    [Header("Physics variables")] public float DesiredGravityScale = 40;
    public float Speed = 10;
    public float ThrowForce = 10f;
    public LayerMask GroundLayer;

    [Header("Jump")] public float JumpLimit = 2;
    public float JumpForce = 100;
    private float _jumpsLeft = 0;

    private bool _isFacingRight;
    private bool _canClimb;

    private Joystick _joystick;
    private JoyButton _pullButton;
    private Animator _animator;
    private Rigidbody2D _rb;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _canClimb = false;
        _isFacingRight = false;
        _jumpsLeft = JumpLimit;
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = DesiredGravityScale;
        PullEffect.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_joystick == null)
        {
            _joystick = FindObjectOfType<Joystick>();
        }

        if (_pullButton == null)
        {
            _pullButton = FindObjectOfType<JoyButton>();
        }

        bool isGrounded =
            Physics2D.OverlapCircle(GroundDetectionPoint.transform.position, GroundDetectReach, GroundLayer);

        if (isGrounded && _rb.velocity.y <= 0)
        {
            _jumpsLeft = JumpLimit;
        }

        HandleInputs();

        _animator.SetFloat("XSpeed", Mathf.Abs(_rb.velocity.x));
        _animator.SetFloat("YSpeed", _rb.velocity.y);
    }

    private void HandleInputs()
    {
        HandleActions();
        HandleMovement();
    }

    private void HandleMovement()
    {
        var up = _joystick.Vertical > 0;
        var down = _joystick.Vertical < 0;
        if (_canClimb)
        {
            if (up)
            {
                CanClimb(true);
                _rb.velocity = new Vector2(0, 1) * Speed;
            }
            else if (down)
            {
                CanClimb(true);
                _rb.velocity = new Vector2(0, -1) * Speed;
            }
            else if (Math.Abs(_rb.gravityScale) < 0.05f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0) * Speed;
            }
        }

        var moveInput = _joystick.Horizontal;
        _rb.velocity = new Vector2(moveInput * Speed, _rb.velocity.y);

        if (_isFacingRight && moveInput < 0)
        {
            Flip();
        }
        else if (!_isFacingRight && moveInput > 0)
        {
            Flip();
        }
    }

    private void HandleActions()
    {
        if (_pullButton.IsPressed)
        {
            PullEffect.SetActive(true);
            var grabbed = Physics2D.OverlapCircleAll(GrabPoint.transform.position, GrabReach, KidLayer);
            foreach (var obj in grabbed)
            {
                obj.GetComponent<KidController>().Grabbed(gameObject);
            }

            _wasPreviouslyPulling = true;
        }
        else if (_wasPreviouslyPulling)
        {
            _wasPreviouslyPulling = false;
            PullEffect.SetActive(false);
            var grabbed = Physics2D.OverlapCircleAll(GrabPoint.transform.position, GrabReach, KidLayer);
            foreach (var obj in grabbed)
            {
                obj.GetComponent<KidController>().Thrown(gameObject);
            }
        }
    }

    public Vector2 GetThrowAngle()
    {
        if (_isFacingRight)
        {
            return new Vector2(1, 0.6f) * ThrowForce;
        }

        return new Vector2(-1, 0.6f) * ThrowForce;
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        var scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void CanClimb(bool b)
    {
        _canClimb = b;

        if (b)
        {
            _rb.gravityScale = 0;
        }
        else
        {
            _rb.gravityScale = DesiredGravityScale;
        }
    }
}
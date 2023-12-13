using UnityEngine;
using UnityEngine.InputSystem;

public class IndependentAcceleration : MonoBehaviour
{
    [SerializeField] bool _useTheForce;
    
    Rigidbody2D rb;
    TimeManager _timeManager;
    Transform _transform;
    MyInput _input;
    Vector2 _moveInput;
    bool _waitFrame = false;
    float _previousUnscaledTimeFactor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _transform = transform;
        _timeManager = FindObjectOfType<TimeManager>();
        _previousUnscaledTimeFactor = _timeManager.UnscaledTimeFactor;

        _input = new MyInput();
        _input.Enable();
        _input.Movement.Move.performed += OnMoveInput;
    }

    void Update()
    {
        float targetDirection = GetDirection();
        Quaternion targetAngle = Quaternion.AngleAxis(targetDirection, Vector3.forward);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetAngle, Time.deltaTime * 10f * _timeManager.UnscaledTimeFactor);
    }

    void FixedUpdate()
    {
        if (_previousUnscaledTimeFactor != _timeManager.UnscaledTimeFactor)
        {
            rb.velocity *= _timeManager.UnscaledTimeFactor / _previousUnscaledTimeFactor;
            _previousUnscaledTimeFactor = _timeManager.UnscaledTimeFactor;
        }
        if (_useTheForce)
            UpdateForce(Forward());
        else
            UpdateVelocity(Forward());
    }

    Vector2 Forward()
    {
        float curDirection = _transform.rotation.eulerAngles.z;
        
        Vector2 forward = Vector2.right * -Mathf.Sin(curDirection * Mathf.Deg2Rad) + 
                          Vector2.up * Mathf.Cos(curDirection * Mathf.Deg2Rad);

        return forward;
    }

    void UpdateForce(Vector2 forward)
    {
        float force = 0.8f;
        float accelerationMultiplier = _timeManager.UnscaledTimeFactor * _timeManager.UnscaledTimeFactor;
        rb.AddForce(force * accelerationMultiplier * forward, ForceMode2D.Force);
    }

    void UpdateVelocity(Vector2 forward)
    {
        Vector2 targetVelocity = 3f * _timeManager.UnscaledTimeFactor * forward;
        Vector2 currentVelocity = rb.velocity;
        Vector2 newVelocity = Vector2.Lerp(currentVelocity, targetVelocity, Time.deltaTime * 3f * _timeManager.UnscaledTimeFactor);
        rb.velocity = newVelocity;
    }

    float GetDirection()
    {
        float angle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg - 90f;
        return angle;

    }

    void OnMoveInput(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }
}

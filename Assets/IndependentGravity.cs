using UnityEngine;

public class IndependentGravity : MonoBehaviour
{
    Rigidbody2D rb;
    TimeManager _timeManager;
    float _previousUnscaledTimeFactor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _timeManager = FindObjectOfType<TimeManager>();
        _previousUnscaledTimeFactor = _timeManager.UnscaledTimeFactor;
    }

    void FixedUpdate()
    {
        UpdateGravity();
    }

    void UpdateGravity()
    {
        if (_previousUnscaledTimeFactor == _timeManager.UnscaledTimeFactor)
            return;
        
        rb.velocity *= _timeManager.UnscaledTimeFactor / _previousUnscaledTimeFactor;
        _previousUnscaledTimeFactor = _timeManager.UnscaledTimeFactor;
        float accelerationFactor = _timeManager.UnscaledTimeFactor * _timeManager.UnscaledTimeFactor;
        rb.gravityScale = 1 * accelerationFactor;
    }
}

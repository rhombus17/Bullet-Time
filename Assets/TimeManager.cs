using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float UnscaledTimeFactor => GetUnscaledTimeFactor();
    
    [SerializeField][Range(0f, 1f)]  float timeScale;

    int _lastFrameChanged;
    float _unscaledTimeFactor;
    float _previousUnscaledTimeFactor;
    const float _minUnscaledTimeFactor = 0.000001f;

    void Awake()
    {
        SetTimeScale(timeScale);
    }

    void Update()
    {
        if (Time.timeScale == timeScale)
            return;
        
        SetTimeScale(timeScale);
    }

    void SetTimeScale(float newTimeScale)
    {
        _lastFrameChanged = Time.frameCount;
        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = 0.02f * newTimeScale;
        
        _previousUnscaledTimeFactor = _unscaledTimeFactor;
        _unscaledTimeFactor = 1 / newTimeScale;
    }
    
    float GetUnscaledTimeFactor()
    {
        float factor = Time.frameCount <= _lastFrameChanged ? _previousUnscaledTimeFactor : _unscaledTimeFactor;
        return factor <= _minUnscaledTimeFactor ? _minUnscaledTimeFactor : factor;
    }
}

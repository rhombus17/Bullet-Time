using TMPro;
using UnityEngine;

public class UITimeScaleDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text _displayText;
    
    TimeManager _timeManager;
    float _previousUnscaledTimeFactor;

    void Start()
    {
        _timeManager = FindObjectOfType<TimeManager>();
        UpdateDisplay();
    }

    void Update()
    {
        if (_previousUnscaledTimeFactor == _timeManager.UnscaledTimeFactor)
            return;
        
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        _previousUnscaledTimeFactor = _timeManager.UnscaledTimeFactor;
        _displayText.text = System.String.Format("{0:0.00}", Time.timeScale);
    }
}

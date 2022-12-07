using System.Collections;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public SpawnPuzzleEvent puzzleEvent;
    
    private KeyCode _keyCode;
    
    private bool _startQte;
    private bool _isPressed;
    private bool _result;

    private bool _startPulse;
    private float _timeBetweenPulse;
    private int _pulseCount;
    private int _initPulseCount;
    
    private bool _startHold;
    private bool _isHolding;
    private float _holdTimer;
    
    public void CallQuickTimeEvent(KeyCode keycode, float delay = 0, float activeTime = 1, Vector3 position = new Vector3())
    {
        _keyCode = keycode;
        StartCoroutine(QuickTimeEvent(position, keycode, activeTime));
    }

    public void CallPulse(KeyCode keycode, int pulseCount, float delay = 1, float activeTime = 1, float timeBetweenPulse = 1, Vector3 position = new Vector3())
    {
        _keyCode = keycode;
        _timeBetweenPulse = timeBetweenPulse;
        _pulseCount = pulseCount;
        _initPulseCount = 0;
        Invoke(nameof(Pulse), delay);
    }

    public void CallHold(KeyCode keycode, float holdDuration, float delay = 1)
    {
        _keyCode = keycode;
        _holdTimer = holdDuration;
        Invoke(nameof(Hold), delay);
    }

    private IEnumerator QuickTimeEvent(Vector3 position, KeyCode keyCode, float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        
        Debug.Log("Press " + _keyCode);
        puzzleEvent.SpawnPuzzleWorldSpace(position, keyCode.ToString());
        _startQte = true;
        Invoke(nameof(ActiveTimeCounter), 2);
    }

    private void Pulse()
    {
        if (_initPulseCount >= _pulseCount)
        {
            return;
        }
        
        Debug.Log("Press " + _keyCode);
        _startPulse = true;
        StartCoroutine(PulseDelay(_timeBetweenPulse));
        _initPulseCount++;
    }

    private void Hold()
    {
        Debug.Log("Hold " + _keyCode);
        _startHold = true;
    }

    private void ActiveTimeCounter()
    {
        _startQte = false;
        if(!_isPressed) EventFinished(false);
    }

    private void Update()
    {
        QteUpdate();
        HoldUpdate();
    }

    private void EventFinished(bool result, bool pressed = false)
    {
        if (!pressed)
        {
            Debug.LogWarning("Didn't Pressed");
            Debug.LogWarning("Event Finished");
            puzzleEvent.Finished(false);
            return;
        }

        Debug.Log(result ? "TRUE" : "FALSE");

        if (result)
        {
            puzzleEvent.Finished(true);
        }

        else
        {
            puzzleEvent.Finished(false);
        }

        Debug.LogWarning("Event Finished");
    }

    private void HoldUpdate()
    {
        if (_startHold)
        {
            if (Input.anyKeyDown)
            {
                _isPressed = true;
                
                if (Input.GetKeyDown(_keyCode))
                {
                    _isHolding = true;
                }
                else
                {
                    _isHolding = false;
                    _result = false;
                    _startHold = false;
                    EventFinished(_result, _isPressed);
                }
            }
            
            if (Input.GetKeyUp(_keyCode))
            {
                _isHolding = false;
                _result = false;
                _startHold = false;
                EventFinished(_result, _isPressed);
            }

            _holdTimer -= Time.deltaTime;

            if (_holdTimer <= 0)
            {
                _startHold = false;
                EventFinished(_isHolding, _isPressed);
            }
        }
    }

    private void QteUpdate()
    {
        if (_startQte)
        {
            if (Input.anyKeyDown)
            {
                _isPressed = true;
                
                if (Input.GetKeyDown(_keyCode))
                {
                    _result = true;
                    _startQte = false;
                    EventFinished(_result, _isPressed);
                }
                else
                {
                    _result = false;
                    _startQte = false;
                    EventFinished(_result, _isPressed);
                }
            }
        }
    }

    private IEnumerator PulseDelay(float timeBetweenPulse)
    {
        if (_startPulse)
        {
            if (Input.anyKeyDown)
            {
                _isPressed = true;
                
                if (Input.GetKeyDown(_keyCode))
                {
                    _result = true;
                    EventFinished(_result, _isPressed);
                }
                else
                {
                    _result = false;
                    EventFinished(_result, _isPressed);
                }
            }
        }

        yield return new WaitForSeconds(1f);

        if(!_isPressed) Debug.LogWarning("Didn't Pressed");
        _startPulse = false;
        
        
        yield return new WaitForSeconds(timeBetweenPulse);
        
        Pulse();
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light)), AddComponentMenu("Misc/Adjustable Light")]
public class AdjustableLight : MonoBehaviour
{
    public enum LightTypes
    {
        NONE,
        FLICK,
        OPENCLOSE
    }
    
    
    [Header("SETTINGS")]
    [Space(5)]
    [SerializeField] private bool startWithMinValue;
    [SerializeField] private bool rotate;
    [SerializeField] private Vector3 rotateAxis;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float flickRate;
    [SerializeField] private float studderRate;
    [SerializeField] private int minFlickCount;
    [SerializeField] private int maxFlickCount;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;
    [SerializeField] private LightTypes type;

    private Light _light;
    private float _rate;

    private Coroutine _flickCoroutine;

    private void Awake()
    {
        _light = GetComponent<Light>();

        if (startWithMinValue)
        {
            _light.intensity = minValue; 
        }
        else
        {
            _light.intensity = maxValue;
        }
        
        _rate = flickRate;
        
        if(type == LightTypes.FLICK) Adjust();
    }

    private void Update()
    {
        _rate -= Time.deltaTime;

        if (rotate)
        {
            var rotation = Time.deltaTime * rotateAxis * rotateSpeed;
            transform.Rotate(rotation);
        }
        
        if (_rate <= 0 && type != LightTypes.FLICK)
        {
            Adjust();
            ResetValues();
        }
    }

    private void Adjust()
    {
        switch (type)
        {
            case LightTypes.FLICK:
                StartCoroutine(Flicker());
                break;

            case LightTypes.OPENCLOSE:
                OpenClose();
                break;
            
            case LightTypes.NONE:
                break;
                
        }
    }

    private void OpenClose()
    {
        if (_light.intensity == minValue)
        {
            _light.intensity = maxValue;
        }
        else
        {
            _light.intensity = minValue;
        }
    }

    private IEnumerator Flicker()
    {
        yield return new WaitForSeconds(flickRate);
        
        var count = 0;

        var flickCount = Random.Range(minFlickCount, maxFlickCount + 1);
        
        yield return Flick(count, flickCount);

    }

    private IEnumerator Flick(int count, int flickCount)
    {
        if (count >= flickCount)
        {
            yield return Flicker();
        }
        
        OpenClose();

        yield return new WaitForSeconds(studderRate / 2);

        OpenClose();

        yield return new WaitForSeconds(studderRate / 2);

        count++;

        yield return Flick(count, flickCount);
    }

    private void ResetValues()
    {
        _rate = flickRate;
    }
}

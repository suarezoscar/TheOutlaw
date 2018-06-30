using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image content;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleBar();
	}
    void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = fillAmount;
        }
        
    }
    private float Map(float value,float inMin,float inMax,float outMin, float outMax)
    {
        //return fillAmount / MaxValue;
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBarScript : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void SetMaxBarValue(float max_value)
    {
        slider.maxValue = max_value;
        slider.value = max_value;
    }
    // Update is called once per frame
    public void SetBarValue(float value)
    {
        slider.value = value;
    }
}

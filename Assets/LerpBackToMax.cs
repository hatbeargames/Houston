using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LerpBackToMax : MonoBehaviour
{
    private Slider slider;
    public float lerpSpeed = 1.0f;  // Control the speed of the lerp
    public float delayBeforeLerp = 3.0f;  // Delay in seconds before the value starts lerping back
    public PlayerStats playerStats;
    private Coroutine lerpCoroutine = null;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void StartLerpBack()
    {
        // Stop any existing lerp coroutine
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }

        // Start a new lerp coroutine after a delay
        lerpCoroutine = StartCoroutine(LerpBackAfterDelay());
    }

    IEnumerator LerpBackAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLerp);
        
        while (slider.value < slider.maxValue)
        {
            float _value = Mathf.Lerp(slider.value, slider.maxValue, lerpSpeed * Time.deltaTime);
            slider.value = _value;
            
            switch (this.gameObject.tag)
            {
                case "Energy":
                    playerStats.currentEnergy = _value;
                    break;
                case "Thrusters":
                    playerStats.currentThrusters = _value;
                    break;
                default:
                    break;
            }
            yield return null;
        }
    }
    public void StopLerpBack()
    {
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
            lerpCoroutine = null;
        }
    }
}
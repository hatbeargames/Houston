using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //PRIVATE SERIALIZED FIELDS
    [SerializeField] int startingHealth = 100;
    [SerializeField] int startingThrusters = 100;
    [SerializeField] int startingEnergy = 100;
    [SerializeField] StatsBarScript healthBar;
    [SerializeField] StatsBarScript ThrusterBar;
    [SerializeField] StatsBarScript EnergyBar;
    [SerializeField] LerpBackToMax thrusterLerpScript;
    [SerializeField] LerpBackToMax energyLerpScript;
    //PUBLIC INTS FOR CHECKING
    public int currentHealth;
    public float currentThrusters;
    public float currentEnergy;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = startingEnergy;
        currentHealth = startingHealth;
        currentThrusters = startingThrusters;
        healthBar.SetMaxBarValue(startingHealth);
        ThrusterBar.SetMaxBarValue(startingThrusters);
        EnergyBar.SetMaxBarValue(startingEnergy);
        thrusterLerpScript = ThrusterBar.GetComponent<LerpBackToMax>();
        energyLerpScript = EnergyBar.GetComponent<LerpBackToMax>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TakeEnergyDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TakeThrusterDamage(20);
        }
        
    }
    void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthBar.SetBarValue(currentHealth);
    }
    void TakeEnergyDamage(int dmg)
    {
        currentEnergy -= dmg;
        EnergyBar.SetBarValue(currentEnergy);
        energyLerpScript.StartLerpBack();
    }
    void TakeThrusterDamage(int dmg)
    {
        currentThrusters -= dmg;
        ThrusterBar.SetBarValue(currentThrusters);
        thrusterLerpScript.StartLerpBack();
    }
    public void ConsumeThrusters(float amount)
    {
        currentThrusters = Mathf.Max(0, currentThrusters - amount);
        ThrusterBar.SetBarValue(currentThrusters);
        thrusterLerpScript.StartLerpBack(); // Trigger the lerp back
    }
    public void StopThrusterBarLerpBack()
    {
        thrusterLerpScript.StopLerpBack();
    }

    public void ConsumeEnergy(float amount)
    {
        currentEnergy = Mathf.Max(0, currentEnergy - amount);
        EnergyBar.SetBarValue(currentEnergy);
        energyLerpScript.StartLerpBack(); // Trigger the lerp back
    }
    public void StopEnergyBarLerpBack()
    {
        energyLerpScript.StopLerpBack();
    }
}

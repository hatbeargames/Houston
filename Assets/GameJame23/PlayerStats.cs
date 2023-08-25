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
    [SerializeField] float AuxillaryEnergyThreshold = .99f;
    [SerializeField] float AuxillaryThrusterThreshold = .99f;
    //PUBLIC INTS FOR CHECKING
    public int currentHealth;
    public float currentThrusters;
    public float currentEnergy;
    public bool isDead = false;

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
        if(currentHealth <= 0)
        {
            //Debug.Log("Is Dead");
        }
    }
    public void TakeDamage(int dmg)
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
    public float GetMaxThrust()
    {
        return startingThrusters;
    }
    public float GetMaxEnergy()
    {
        return startingEnergy;
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
    public void UpgradeAuxillaryThrusters()
    {
        AuxillaryThrusterThreshold = .5f;
    }
    public void UpgradeAuxillaryEnergy()
    {
        AuxillaryEnergyThreshold = .5f;
    }
    public float GetAuxillaryEnergyThreshold() 
    {
        return AuxillaryEnergyThreshold;
    }
    public float GetAuxillaryThrusterThreshold()
    {
        return AuxillaryThrusterThreshold;
    }
}

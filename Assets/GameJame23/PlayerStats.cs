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
    [SerializeField] float AuxillaryEnergyThreshold = .9f;
    [SerializeField] float AuxillaryThrusterThreshold = .9f;
    [SerializeField] float altitude;
    [SerializeField] float speed;
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
        if(currentHealth <= 0 || currentThrusters <= 0)
        {
            //Debug.Log("Is Dead");
            isDead = true;
            StopThrusterBarLerpBack();
            StopEnergyBarLerpBack();
        }
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
    public float GetMaxHealth()
    {
        return startingHealth;
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
    public void SetAltitude(float alt)
    {
        altitude = alt;
    }
    public void SetSpeed(float sped)
    {
        speed = sped;
    }
    public float GetAltitude()
    {
        return altitude;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public void AddToHealth(int healthPickup)
    {
        currentHealth += healthPickup;
        healthBar.SetBarValue(currentHealth);
    }
    public void AddToEnergy(int energyPickup)
    {
        currentEnergy += energyPickup;
        EnergyBar.SetBarValue(currentEnergy);
    }
    public void AddToThrusters(int fuelPickup)
    {
        currentThrusters += fuelPickup;
        ThrusterBar.SetBarValue(currentThrusters);
    }
}

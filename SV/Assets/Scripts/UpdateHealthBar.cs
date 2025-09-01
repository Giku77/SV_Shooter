using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthBar : LivingEntity
{
    public Slider healthSlider;
    public LivingEntity EntityHealth;

    private void Awake()
    {
        if (EntityHealth != null && healthSlider != null)
        {
            EntityHealth.OnDmg += UpdateHealth;
        }
    }


    private void UpdateHealth()
    {
        if (EntityHealth != null)
        {
            if (EntityHealth.Isdead)
            {
                healthSlider.gameObject.SetActive(false);
                return;
            }
            healthSlider.value = EntityHealth.Health / EntityHealth.MaxHealth;
            //Debug.Log("Health Bar Updated: " + EntityHealth.Health + "/" + EntityHealth.MaxHealth);
        }
    }

    //private void Update() 
    //{
    //    if (EntityHealth != null)
    //    {
    //        if (EntityHealth.Isdead)
    //        {
    //            healthSlider.gameObject.SetActive(false);
    //            return;
    //        }
    //        healthSlider.value = EntityHealth.Health / EntityHealth.MaxHealth;
    //        //Debug.Log("Health Bar Updated: " + EntityHealth.Health + "/" + EntityHealth.MaxHealth);
    //    }
    //}

}

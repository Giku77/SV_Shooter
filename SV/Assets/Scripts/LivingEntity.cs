using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float MaxHealth = 100f;
    public float Health { get; protected set; }
    public bool Isdead { get; protected set; }

    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        Debug.Log("LivingEntity onEnable");
        Health = MaxHealth;
        Isdead = false;
    }
    public virtual void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Health -= damage;
        Debug.Log("Damage Taken: " + damage + ", Health Left: " + Health);
        if (Health <= 0 && !Isdead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Isdead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
    }
}

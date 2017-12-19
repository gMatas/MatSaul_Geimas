using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    public Collider2D MeleeWeapon;
    public float MaxHealth;
    public float Health;

    public bool isDead = false;


    // Use this for initialization
    void Start ()
    {
        MeleeWeapon.enabled = false;
        Health = MaxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isDead)
        {
            Death();
        }
	}

    public void ActivateWeapon()
    {
        MeleeWeapon.enabled = true;
    }

    public void DeactivateWeapon()
    {
        MeleeWeapon.enabled = false;
    }

    private void Death()
    {
        Debug.Log("Enemy is dead!");
        Destroy(GetComponent<EnemyAI>());
        Destroy(gameObject);
    }

    private void TakeDamage(float damage, float criticalHitRatio)
    {
        var chance = (float)(Random.Range(0.01f, 1f));  // returns value between [0.01 ... 1] inclusive
        
        if (criticalHitRatio >= chance)
        {
            Health = 0;
        }
        else
        {
            Health -= damage;
        }
        if (Health <= 0)
        {
            isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Mobile Weapon"))
        {
            var playerWeaponController = collision.GetComponent<ProjectileController>();
            TakeDamage(playerWeaponController.Damage, playerWeaponController.CriticalHitRatio);
        }
    }
}

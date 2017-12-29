using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    public EnemyAnimation MyAnimator;
    public Collider2D MeleeWeapon;
    public float MaxHealth;
    public float Health;

    public bool isDead = false;
    private bool _takeDamage = false;
    private ProjectileController _playerWeaponController;



    // Use this for initialization
    void Start ()
    {
        MeleeWeapon.enabled = false;
        Health = MaxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_takeDamage)
        {
            TakeDamage(_playerWeaponController.Damage, _playerWeaponController.CriticalHitRatio);
            _playerWeaponController = null;
            _takeDamage = false;
        }
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

    bool animationsWasRunned = false;

    private void Death()
    {
        Debug.Log("Enemy is dead!");
        Destroy(GetComponent<EnemyAI>());
        //if(!animationsWasRunned)
        //{
        //    animationsWasRunned = true;
        //    MyAnimator.PlayDeath();
        //}
        //if (!MyAnimator.Anim.isPlaying && animationsWasRunned)
        //{ 
            
        //}
        Destroy(gameObject);
    }

    private void TakeDamage(float damage, float criticalHitRatio)
    {
        var chance = (Random.Range(1, 101));  // returns value between [0.01 ... 1] inclusive
        Debug.Log("Chance: " + chance.ToString());
        Debug.Log("CritHit: " + (criticalHitRatio*100).ToString());
        Debug.Log("damage done: " + damage.ToString());

        if (criticalHitRatio * 100 > chance)
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
            _playerWeaponController = collision.GetComponent<ProjectileController>();
            _takeDamage = true;
        }
    }
}

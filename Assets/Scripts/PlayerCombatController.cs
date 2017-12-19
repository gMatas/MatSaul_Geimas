using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
	public Transform FiringPoint;
	public LayerMask WhatToHit;
	public Object ThrowingKnifePrefab;
	public float ThrowingKnifeSpeed;
	public float ThrowingKnifeDamage;
    public float ThrowingKnifeCriticalHitRatio;
    public float ThrowingKnifeHitRadius;

    public bool isDead = false;

	private PlayerController _playerController;
	private GameObject _projectilePrefab;
	private ProjectileController _projectileController;
	private CircleCollider2D _projectileCollider;
	
	
	// Use this for initialization
	private void Start ()
	{
		_playerController = GetComponent<PlayerController>();
		
		_projectilePrefab = (GameObject) ThrowingKnifePrefab;
		_projectileController = _projectilePrefab.GetComponent<ProjectileController>();
		_projectileCollider = _projectilePrefab.GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	private void Update ()
	{
        if (isDead)
        {
            Death();
        }

		// Fire projectile
		if (Input.GetKeyDown(KeyCode.F))
		{
			_projectileController.FireToRight = _playerController.PlayerIsFacingRight();
			_projectileController.WhatToHit = WhatToHit;
			_projectileController.Speed = ThrowingKnifeSpeed;
			_projectileController.Damage = ThrowingKnifeDamage;
            _projectileController.CriticalHitRatio = ThrowingKnifeCriticalHitRatio;
            _projectileController.PointOfOrigin = FiringPoint;
			
			_projectileCollider.radius = ThrowingKnifeHitRadius;
			Instantiate(_projectilePrefab);
			FiringPoint.DetachChildren();
		}
	}

    private void Death()
    {
        // TODO: Death sequence
        Debug.Log("You are dead now!");
        Destroy(_playerController);
        //return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Receive a hit
        if (collision.CompareTag("Enemy Weapon"))
        {
            isDead = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusController : MonoBehaviour 
{
	public int MaxHealth;

	private int _health;
	private bool _isDead;


	// Use this for initialization
	void Start () 
	{
		_health = MaxHealth;
		_isDead = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_isDead)
		{
			Death();
		}
	}

	private void Death()
	{
		// TODO: Death sequence
	}

	public void AddDamage(int damage)
	{
		_health -= damage;
		if (_health <= 0)
		{
			_isDead = true;
		}
	}
}
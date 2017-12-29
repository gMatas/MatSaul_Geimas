using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Transform PointOfOrigin;
    public float MaxDistanceFromOrigin;
	public LayerMask WhatToHit;
	public bool FireToRight;
	public float Speed;
	public float Damage;
    public float CriticalHitRatio;
    
    private Rigidbody2D _projectileBody;
	private float _velocity;
	private float _damage;


	// Use this for initialization
	private void Start ()
	{
        transform.position = PointOfOrigin.position;
        _projectileBody = GetComponent<Rigidbody2D>();
        if (FireToRight)
        {
            _velocity = Speed;
        }
        else
        {
            _velocity = -Speed;
            var rotation = transform.rotation;
            rotation.z *= -1;
            transform.rotation = rotation;
        }
	}
	
	// Update is called once per frame
	private void Update ()
	{
        var distanceFromOrigin = Mathf.Abs(transform.position.x - PointOfOrigin.position.x);
        if (distanceFromOrigin > MaxDistanceFromOrigin)
        {
            Destroy(gameObject);
        }

		var projectileVelocity = _projectileBody.velocity;
		projectileVelocity.x = _velocity;
		_projectileBody.velocity = projectileVelocity;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (((1 << collision.gameObject.layer) & WhatToHit) != 0)
        {
            Debug.Log("We have a hit!");
            Destroy(gameObject);
        }
    }
}

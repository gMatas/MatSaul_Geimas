using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;

[RequireComponent(typeof(PlayerController))]
public class PlayerCameraController : MonoBehaviour
{
	public GameObject PlayerCamera;
	public float HorizontallDistanceToCameraTarget;
	public float MaxHorizontalDistaceFromCamera;
	public float AllowedDistanceError;
	public float TimeToTravel;
	private Rigidbody2D _cameraBody;
	private PlayerController _playerController;
	private float _cameraTargetX;
	private bool _playerIsFacingRight = false;
	
	
	private void Start ()
	{
		_cameraBody = PlayerCamera.GetComponent<Rigidbody2D>();
		_playerController = GetComponent<PlayerController>();

		var cameraPosition = PlayerCamera.transform.position;
		cameraPosition.x = transform.position.x;
		PlayerCamera.transform.position = cameraPosition;
	}
	
	private void FixedUpdate()
	{
		AdjustCameraTarget();
		FollowHorizontally(_cameraTargetX);
	}

	private void AdjustCameraTarget()
	{
		_playerIsFacingRight = _playerController.PlayerIsFacingRight();
		if (_playerIsFacingRight)
		{
			_cameraTargetX = transform.position.x + HorizontallDistanceToCameraTarget;
		}
		else
		{
			_cameraTargetX = transform.position.x - HorizontallDistanceToCameraTarget;
		}
	}

	private void FollowHorizontally(float targetX)
	{
		var distance = targetX - PlayerCamera.transform.position.x;
		var absoluteDistance = Mathf.Abs(distance);
		if (absoluteDistance > AllowedDistanceError && absoluteDistance < MaxHorizontalDistaceFromCamera && TimeToTravel > 0)
		{
			var velocity = distance / TimeToTravel;
			var cameraVelocity = _cameraBody.velocity;
			cameraVelocity.x = velocity;
			_cameraBody.velocity = cameraVelocity;
		}
		else
		{
			var cameraPosition = PlayerCamera.transform.position;
			cameraPosition.x = targetX;
			PlayerCamera.transform.position = cameraPosition;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public Text countText;

    public bool IsModel3D = true;
	public float MoveSpeed;
	public float JumpSpeed;
	public Transform GroundCheck;
	public Transform WallCheck;
	public LayerMask WhatIsGround;
	public LayerMask WhatIsWall;
	public float SlideSpeed;
    public string EnemyHitTag = "Enemy Weapon";

    public static int count;

    private Rigidbody2D _playerBody;
    private PlayerCombatController _combatController;
	private bool _toRight;
	private bool _isFacingRight = true;
	private bool _doMove = false;
	private bool _isMoving = false;
	private bool _doFlip = false;
	private int _jumpCount = 0;
	private int _maxJumpCount = 2;
	private bool _doJump = false;
	private bool _isGrounded;
	private Transform [] _wallChecks;
	private bool _isWallContact;
	public static bool _isWallGrip;
	private float _wallGripReleaseTimestamp;
	private float _wallGripReleaseCooldown = 0.3f;


    // Use this for initialization
    private void Start()
	{
		_playerBody = GetComponent<Rigidbody2D>();
        _combatController = GetComponent<PlayerCombatController>();
        count = 0;
        SetCountText();
        var wallChecksCount = WallCheck.transform.childCount;
		_wallChecks = new Transform[wallChecksCount];
		for (var i = 0; i < wallChecksCount;)
			 _wallChecks[i] = WallCheck.transform.GetChild(i++);

		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player Body"), LayerMask.NameToLayer("Enemy Body"), true);
	}

	private void FixedUpdate()
	{
		MoveWithKeyboard();		// DEBUG: moving with dirrection keys
		JumpWithKeyBoard();		// DEBUG: Jumping with key 
//		 SynchronizeMovementControlEvents();
		
		_isWallContact = true;
		foreach (var wallcheck in _wallChecks) 
			_isWallContact &= Physics2D.OverlapPoint(wallcheck.position, WhatIsWall);
		_isGrounded = Physics2D.OverlapPoint(GroundCheck.position, WhatIsGround);
		
		// Stops any horizontal inertive movement
		var playerHorizontalVelocity = _playerBody.velocity;
		playerHorizontalVelocity.x = 0;
		_playerBody.velocity = playerHorizontalVelocity;

		if (_isGrounded || _isWallGrip)
		{
			_jumpCount = 0;
		} 
		if (_doFlip) 
		{
			Flip();
		}
		if (_doJump)
		{
			Jump();
		}
		if (_isMoving)
		{
			Movement();
		}
		WallGrip();
	}



	private void Flip()
	{
		// Flips the player to face the opposite direction depending on if it's 2D or 3D model
		var scale = transform.localScale;
		if (IsModel3D) 
			scale.x *= -1;
		else 
			scale.x *= -1;
		transform.localScale = scale;
		_isFacingRight = !_isFacingRight;
		_doFlip = false;
	}

	private void Movement()	
	{
		// Move the player in a correct direction at a consistent speed
		var velocity = _playerBody.velocity;
		velocity.x = MoveSpeed;
		velocity.x *= _isFacingRight ? 1 : -1;	// Sets direction to the movement
		_playerBody.velocity = velocity;
	}

	private void Jump()
	{
		if (_jumpCount < _maxJumpCount)
		{
			var isReleaseCooldownOver = _wallGripReleaseTimestamp < Time.time;
			if (_isWallGrip && isReleaseCooldownOver)
			{
				_wallGripReleaseTimestamp = Time.time + _wallGripReleaseCooldown;
				_isWallGrip = false;
			}

			var velocity = _playerBody.velocity;
			velocity.y = JumpSpeed;
			_playerBody.velocity = velocity;
			_jumpCount++;
		}
		_doJump = false;
	}

	private void WallGrip()
	{
		var wallcheckPosition = WallCheck.localPosition;
		var isWallcheckInFront = wallcheckPosition.x > 0; 
		var isReleaseCooldownOver = _wallGripReleaseTimestamp < Time.time;

		if (_isWallContact && !_isGrounded) 	// On contact with wall & not reaching ground
		{
			if (_isMoving && isReleaseCooldownOver) 
			{
				// Flips player & flips wall checks to the opposite side of where player is looking	
				Flip();
				wallcheckPosition.x = isWallcheckInFront ? -wallcheckPosition.x : wallcheckPosition.x;
				WallCheck.localPosition = wallcheckPosition;
				_isWallGrip = true;
			}
			if (_isWallGrip) 
			{
				// Slides player down the wall
				var playerVelocity = _playerBody.velocity;
				playerVelocity.y = -SlideSpeed;
				_playerBody.velocity = playerVelocity;
				return;
			}	
		}
        if ((!_isWallGrip || !_isWallContact || _isGrounded) && !isWallcheckInFront)    // Flips back wallcheck to it's original side & releases wall grip
        {
            wallcheckPosition.x = -wallcheckPosition.x;
            WallCheck.localPosition = wallcheckPosition;
            _isWallGrip = false;
        }
	}

	private void SynchronizeMovementControlEvents()
	{
		_isMoving = _doMove;								// indicates that player should move
		_doFlip = _isFacingRight ^ _toRight && _doMove; 	// demands to flip player facing direction if we want to move to an opposite direction
	}
	
	public bool PlayerIsFacingRight()
	{
		return _isFacingRight;
	}


	public void OnPressingJumpButton()
	{
		_doJump = true;
	}
	
	public void OnPressingMovementStartButton(bool toRight)
	{
		_doMove = true;
		_toRight = toRight;
	}

	public void OnPressingMovementStopButton()
	{
		_doMove = false;
	}	
	
	private void MoveWithKeyboard()
	{
		var moveLeft = Input.GetKey(KeyCode.LeftArrow);
		var moveRight = Input.GetKey(KeyCode.RightArrow);			
		_isMoving = moveLeft ^ moveRight;
		_doFlip = _isMoving && _isFacingRight ^ moveRight;
	}

	private void JumpWithKeyBoard()
	{
		_doJump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Coin pick-ip
        if (other.gameObject.CompareTag("Pick up"))
        {
            count = count + 1;
            SetCountText();
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            other.gameObject.SetActive(false);
        }
    }
  
    void SetCountText()
    {
        countText.text = "Gold: " + count.ToString();
    }

}
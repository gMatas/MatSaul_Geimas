using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//www.youtube.com/watch?v=wXTFcXUNFkM&index=24&list=PLa5_l08N9jzN6RpyixHkXP90IW6gaVVV1

public class EnemyAI : MonoBehaviour
{
    public Animator anim;
    public float WalkSpeed;
    public float RunSpeed;

    public Collider2D PatrolArea;
    public Transform Target;
    public float AttackRange;
    public float AwarenessRange;

    private Rigidbody2D _enemyBody;
    private bool _isFacingRight = true;

    private EnemyCombatController _combatController;
    private PatrolRouteService _patrolRouteService;
    private bool _returnToPatrolArea = false;
    private bool _isEnemyInPatrolArea;
    private bool _isTargetInPatrolArea;
    private bool _isAttacking = false;
    private bool _isEngaging = false;
    private float _distanceToTarget;

    // Use this for initialization
    void Start ()
    {
        _enemyBody = GetComponent<Rigidbody2D>();
        _patrolRouteService = PatrolArea.GetComponent<PatrolRouteService>();
        _combatController = GetComponent<EnemyCombatController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        _isTargetInPatrolArea = _patrolRouteService.IsPlayerInArea();

        var attackAnimationRunning = false;
        if (_isAttacking)
        {
            Attack();
            _isAttacking = false;
        }
        else if (!attackAnimationRunning)
        {
            _isAttacking = false;
            _combatController.DeactivateWeapon();
            Patrol();
        }
    }

    // Attack animation and sequence
    void Attack()
    {
        _combatController.ActivateWeapon();
    }

    void Walk()
    {
        var velocity = _enemyBody.velocity;
        velocity.x = _isFacingRight ? WalkSpeed : -WalkSpeed;
        _enemyBody.velocity = velocity;
    }

    void Run()
    {
        var velocity = _enemyBody.velocity;
        velocity.x = _isFacingRight ? RunSpeed : -RunSpeed;
        _enemyBody.velocity = velocity;
    }

    void Flip()
    {
        var localScale = transform.localScale;
        localScale.x = -localScale.x;
        transform.localScale = localScale;
        _isFacingRight = !_isFacingRight;
    }

    void Patrol()
    {
        if (_isEnemyInPatrolArea)
        {
            _distanceToTarget = Target.position.x - transform.position.x; // if + to right | if - to left
            var absoluteDistanceToTarget = Mathf.Abs(_distanceToTarget);
            var targetOutsideAwarenessRange = absoluteDistanceToTarget >= AwarenessRange;

            if (_isTargetInPatrolArea && !targetOutsideAwarenessRange)
            {
                var targetIsInTheRight = _distanceToTarget > 0 || (_isFacingRight && _distanceToTarget == 0);
                if ((_isFacingRight && targetIsInTheRight) || (!_isFacingRight && !targetIsInTheRight))                   
                {
                    if (absoluteDistanceToTarget < AttackRange)             // Attack target
                    {
                        _isAttacking = true;
                    }
                    else if (absoluteDistanceToTarget < AwarenessRange)     // Start chasing
                    {
                        _isEngaging = true;
                        Run();
                    }
                    else
                    {
                        Walk();
                    }
                }
                else if (_isEngaging)
                {
                    Flip();
                }
                else
                {
                    Walk();
                }
            } 
            else
            {
                _isEngaging = false;
                _isAttacking = false;
                Walk();
            }
            _returnToPatrolArea = false;
        }
        else if (!_returnToPatrolArea)
        {
            Flip();
            _returnToPatrolArea = true;
        }
        else
        {
            Walk();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.Equals(PatrolArea))
        {
            _isEnemyInPatrolArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.Equals(PatrolArea))
        {
            _isEnemyInPatrolArea = false;
        }
    }


}

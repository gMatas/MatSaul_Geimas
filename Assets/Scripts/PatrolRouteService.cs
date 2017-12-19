using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRouteService : MonoBehaviour
{
    public LayerMask WhatIsPlayer;

    private bool _isPlayerInArea;
    private bool _hasEntered;
    private bool _hasExited;


    // Syncronises _isPlayerInArea flag
    private void Update()
    {
        if (_hasEntered)
        {
            _isPlayerInArea = true;
        }
        else if (_hasExited)
        {
            _isPlayerInArea = false;
        }
        _hasEntered = false;
        _hasExited = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & WhatIsPlayer) != 0)
        {
            _hasEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & WhatIsPlayer) != 0)
        {
            _hasExited = true;
        }
    }

    public bool IsPlayerInArea()
    {
        return _isPlayerInArea;
    }
}

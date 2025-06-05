using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float Speed = 15.0f;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    public Animator animator;


    private float dirSpeed;

    public GameObject CanvasReference;

    void Start()
    {
        CanvasReference = GameObject.Find("CanvasPlayer1");
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
    }

    void Update()
    {
        _movement = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _movement.y = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _movement.y = -1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _movement.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _movement.x = 1;
        }

        _movement = _movement.normalized;
    }

    [Obsolete("Obsolete")]
    private void FixedUpdate()
    {
        _rb.velocity = _movement * Speed;

        if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
        {
            dirSpeed = Mathf.Abs(_movement.x);
        }
        else
        {
            dirSpeed = Mathf.Abs(_movement.y);
        }

        if ((_movement.x > 0 && transform.localScale.x < 0) ||
            (_movement.x < 0 && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        animator.SetFloat("velocity", dirSpeed);

    }

}
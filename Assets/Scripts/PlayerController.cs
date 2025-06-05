using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings: ")]
    [SerializeField] private float Speed = 15.0f;
    private Vector2 _movement;
    [SerializeField] private bool canMove;
    [SerializeField] private string canvasName;

    //Components
    private Animator animator;
    private Rigidbody2D _rb;

    private float dirSpeed;
    public GameObject canvasReference;
    private CanvasHandler canvasHandler;

    private IInteractable<CanvasHandler> _interactable;
    private IInteractable<PlayerController> _playerController;

    public CanvasHandler CanvasHandler => canvasHandler;
    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }


    //HARDCODED - NOT USING UNITY INPUT SYSTEM
    [Serializable]
    public struct ControlStruct
    {
        public KeyCode code;
        public string actionName;
    }

    [SerializeField] private List<ControlStruct> controlList = new List<ControlStruct>();

    public Dictionary<string, ControlStruct> controls = new Dictionary<string, ControlStruct>();

    //HARDCODED - NOT USING UNITY INPUT SYSTEM


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        InitializeControls();
        canvasReference = GameObject.Find(canvasName);
        canvasHandler = canvasReference.GetComponent<CanvasHandler>();
        canvasHandler.SetOwner(this);
        _rb.freezeRotation = true;
        canMove = true;
    }

    void Update()
    {
        _movement = Vector2.zero;

        if (CanMove)
        {
            if (Input.GetKey(GetControl("MoveForward")))
            {
                _movement.y = 1;
            }
            if (Input.GetKey(GetControl("MoveBackward")))
            {
                _movement.y = -1;
            }
            if (Input.GetKey(GetControl("MoveLeft")))
            {
                _movement.x = -1;
            }
            if (Input.GetKey(GetControl("MoveRight")))
            {
                _movement.x = 1;
            }

            _movement = _movement.normalized;

            if (Input.GetKeyDown(GetControl("Interact")))
            {
                if (_interactable != null)
                {
                    _interactable.Interact(canvasReference.GetComponent<CanvasHandler>());
                }

                else if (_playerController != null)
                {
                    _playerController.Interact(this);
                }
            }
        }
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {

            if (collision.TryGetComponent<IInteractable<CanvasHandler>>(out var interactable))
            {
                _interactable = interactable;
            }

            else if (collision.TryGetComponent<IInteractable<PlayerController>>(out var controller))
            {
                _playerController = controller;
            }
            else
            {
                Debug.LogWarning("Collider does not implement IInteractable<CanvasHandler>");
 
            }

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            _interactable = null;
            _playerController = null;
        }
    }

    //HARDCODED - NOT USING UNITY INPUT SYSTEM
    private void InitializeControls()
    {
       foreach (ControlStruct cs in controlList)
        {
            controls.Add(cs.actionName, cs);
        }
    }

    public KeyCode GetControl(string actionName)
    {
        if (controls.TryGetValue(actionName, out var value))
        {
            return value.code;
        }

        return KeyCode.None;
    }
    //HARDCODED - NOT USING UNITY INPUT SYSTEM
}
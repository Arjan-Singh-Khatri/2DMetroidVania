using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputSystemImplement : MonoBehaviour
{

    public NewControls playerControl;
    private Rigidbody2D rigidbody2;
    private PlayerInput playerInput;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speedForce = 5f;

    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();  
        playerControl = new NewControls();
        playerControl.PlayerInput.Enable();
        //playerControl.PlayerInput.Movement.performed += PlayerMovement;
        playerControl.PlayerInput.Jump.performed += JumpMovement;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movementVector = playerControl.PlayerInput.Movement.ReadValue<Vector2>();
        rigidbody2.AddForce(Vector2.right * movementVector.x * speedForce, ForceMode2D.Force);
    }

    //private void Playermovement(InputAction.CallbackContext context)
    //{
    //    Vector2 movementVector = playerControl.PlayerInput.Movement.ReadValue<Vector2>();
    //    rigidbody2.AddForce(Vector2.right * movementVector.x * speedForce, ForceMode2D.Force);

    //}

    void JumpMovement(InputAction.CallbackContext context)
    {
        rigidbody2.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
    }

}

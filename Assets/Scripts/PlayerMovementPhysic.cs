using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovementPhysic : MonoBehaviour
{
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float rotationSpeed = 200f;
    [SerializeField]
    private float DashPower = 5f;
    
    private Rigidbody physicsBody;
    private Animator animator;
    private bool isGrounded;
    private Camera mainCamera;
    

    private float mouseX;

    [SerializeField]
    private bool useJoystick = false;

    [SerializeField]
    private Button DashButton;


    private void Start()
    {
        DashButton.onClick.AddListener(OnDashButtonClicked);

        animator = GetComponent<Animator>();
        physicsBody = physicsBody ?? GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        

        InputManager.Instance.RegisterOnDashInput(Dash, true);
    }

    private void OnDestroy() 
    {
        InputManager.Instance.RegisterOnDashInput(Dash, false);
    }

    private void Update()
    {
        Vector3 movementInput = Vector3.zero;
        if (useJoystick)
        {
            movementInput = new Vector3(UIManager.Instance.Joystick.Direction.x, 0.0f, UIManager.Instance.Joystick.Direction.y);
        }
        else
        {
            movementInput = InputManager.Instance.MovementInput;
        }
        
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = mainCamera.transform.right;

        Vector3 movement = (forward * movementInput.z + right * movementInput.x).normalized;

        float moveSpeed = movement.magnitude;
        
        animator.SetFloat("speed", moveSpeed);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        animator.SetBool("isGrounded", isGrounded);

        if (moveSpeed > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    private void Dash(InputAction.CallbackContext callbackContext)
    {
        if (isGrounded)
        {
            physicsBody.AddForce(Vector3.up * DashPower, ForceMode.Impulse);
            animator.SetTrigger("Dash");
        }
    }

    private void OnDashButtonClicked()
    {
        if (isGrounded)
        {
            physicsBody.AddForce(Vector3.up * DashPower, ForceMode.Impulse);
            animator.SetTrigger("Dash");
        }
    }
    
}

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

    public Vector3 moveDir; // Vector3 pour le mouvement en 3D

    private void Start()
    {
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

        // Utiliser le joystick ou les entrées clavier (par exemple, les touches WASD ou les flèches)
        if (useJoystick)
        {
            movementInput = new Vector3(UIManager.Instance.Joystick.Direction.x, 0.0f, UIManager.Instance.Joystick.Direction.y);
        }
        else
        {
            movementInput = InputManager.Instance.MovementInput;
        }

        // Récupérer la direction en fonction de la caméra
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0; // Nous ignorons l'axe Y de la caméra pour éviter une inclinaison
        forward.Normalize();
        Vector3 right = mainCamera.transform.right;

        // Définir le mouvement en fonction de l'entrée
        moveDir = (forward * movementInput.z + right * movementInput.x).normalized;

        // Déplacer le joueur
        physicsBody.velocity = new Vector3(moveDir.x * speed, physicsBody.velocity.y, moveDir.z * speed);

        // Animer le joueur
        animator.SetFloat("walk", moveDir.magnitude);

        // Vérifier si le joueur est au sol
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        animator.SetBool("isGrounded", isGrounded);

        if (moveDir.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
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

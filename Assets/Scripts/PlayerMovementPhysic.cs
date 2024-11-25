using System.Collections;
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
    private float dashPower = 10f;
    [SerializeField]
    private float dashDuration = 0.2f;
    [SerializeField]
    private float dashCooldown = 1f;

    [SerializeField]
    private GameObject smokePrefab; // Le prefab de fumée
    [SerializeField]
    private Transform smokeSpawnPoint; // Point où la fumée sera créée

    private Rigidbody physicsBody;
    private Animator animator;
    private Camera mainCamera;

    [SerializeField]
    private bool useJoystick = false;

    [SerializeField]
    private Button DashButton;

    private bool isDashing = false;
    private float lastDashTime;

    private void Start()
    {
        if (DashButton != null)
        {
            DashButton.onClick.AddListener(PerformDash);
        }

        animator = GetComponent<Animator>();
        physicsBody = physicsBody ?? GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        InputManager.Instance.RegisterOnDashInput(PerformDash, true);
    }

    private void OnDestroy()
    {
        InputManager.Instance.RegisterOnDashInput(PerformDash, false);
    }

    private void Update()
    {
        if (isDashing) return;

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

        physicsBody.velocity = new Vector3(movement.x * speed, physicsBody.velocity.y, movement.z * speed);

        animator.SetFloat("walk", moveSpeed);

        if (moveSpeed > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    private void PerformDash(InputAction.CallbackContext callbackContext)
    {
        PerformDash();
    }

    private void PerformDash()
    {
        if (isDashing || Time.time < lastDashTime + dashCooldown) 
        {
            Debug.Log("Dash prevented due to conditions");
            return;
        }

        SpawnSmokeEffect();
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector3 dashDirection = transform.forward;
        physicsBody.velocity = dashDirection * dashPower;

        animator.SetTrigger("Dash");

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    private void SpawnSmokeEffect()
    {
        if (smokePrefab != null && smokeSpawnPoint != null)
        {
            Instantiate(smokePrefab, smokeSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Smoke prefab or spawn point is not assigned.");
        }
    }
}


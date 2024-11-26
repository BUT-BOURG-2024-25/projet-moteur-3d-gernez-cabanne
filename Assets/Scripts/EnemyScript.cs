using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float deathDelay = 2.5f;

    [Header("Combat Settings")]
    [SerializeField] private int health = 1;
    [SerializeField] private int xpReward = 10;

    [Header("References")]
    [SerializeField] private BoxCollider attackCollider;

    private Vector3 movementDirection;
    private Rigidbody rb;
    private Animator animator;
    private bool isDead = false;

    private Transform player;
    private PlayerHealth playerHealth;
    private PlayerExperience playerExperience;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerExperience = player.GetComponent<PlayerExperience>();
        }

        if (attackCollider == null)
        {
            Debug.LogError("Le BoxCollider pour la détection de collision n'est pas assigné!");
        }
        else
        {
            attackCollider.isTrigger = true;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (player)
        {
            movementDirection = (player.position - transform.position).normalized;
        }

        rb.MovePosition(rb.position + movementDirection * (speed * Time.fixedDeltaTime));

        float moveSpeed = movementDirection.magnitude;
        animator.SetFloat("EnemyWalk", moveSpeed);

        if (moveSpeed > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        gameObject.tag = "Untagged";
        gameObject.layer = 0;

        animator.SetTrigger("EnemyDeath");

        rb.isKinematic = true;
        //GameManager.Instance.RemoveEnemyFromList(gameObject);

        // Récompense XP
        GrantXP();

        Destroy(gameObject, deathDelay);
    }

    private void GrantXP()
    {
        if (playerExperience != null)
        {
            playerExperience.AddXP(xpReward);
        }
        else
        {
            Debug.LogWarning("Le joueur n'a pas de script PlayerExperience assigné.");
        }
    }
}

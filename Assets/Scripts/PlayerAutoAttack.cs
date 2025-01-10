using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoAttack : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int projectilesPerAttack = 3;
    [SerializeField] private float spreadAngle = 45f;

    private Animator animator;
    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;
        animator = GetComponent<Animator>();

        if (attackTimer >= attackCooldown)
        {
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy && closestEnemy.CompareTag("Enemy"))
            {
                Attack(closestEnemy);
                attackTimer = 0f;
            }
        }
    }

    public void AddProjectile()
    {
        projectilesPerAttack++;
    }

    private GameObject FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.gameObject;
                }
            }
        }

        return closestEnemy;
    }

    private void Attack(GameObject target)
    {
        if (!projectilePrefab || !projectileSpawnPoint) return;

        animator.SetTrigger("PlayerAttack");

        if (projectilesPerAttack == 1)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            FireProjectile(direction);
        }
        else
        {
            float angleStep = spreadAngle / (projectilesPerAttack - 1);
            float angleOffset = -spreadAngle / 2;

            for (int i = 0; i < projectilesPerAttack; i++)
            {
                float currentAngle = angleOffset + i * angleStep;

                Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
                Vector3 direction = rotation * (target.transform.position - transform.position).normalized;

                FireProjectile(direction);
            }
        }
    }

    private void FireProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
        AutoProjectile autoProjectile = projectile.GetComponent<AutoProjectile>();
        if (autoProjectile)
        {
            autoProjectile.SetTargetDirection(direction);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float attackRange = 10f;
    [SerializeField]
    private float attackCooldown = 1f;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private LayerMask enemyLayer;
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

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        
        animator.SetTrigger("PlayerAttack");

        AutoProjectile autoProjectile = projectile.GetComponent<AutoProjectile>();
        if (autoProjectile)
        {
            autoProjectile.SetTarget(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

using UnityEngine;

public class ExplosionAbility : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 4;
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private bool active = false;

    [Header("Layer Settings")]
    [SerializeField] private LayerMask enemyLayer;

    private float cooldownTimer;

    void Update()
    {
        if(!active){return;};

        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= cooldownTime)
        {
            TriggerExplosion();
            cooldownTimer = 0f;
        }
    }

    private void TriggerExplosion()
    {
        if (explosionPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosionEffect, 2f);
        }

        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider enemy in enemiesHit)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.TakeDamage(explosionDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

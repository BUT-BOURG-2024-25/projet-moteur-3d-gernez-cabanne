using UnityEngine;

public class AutoProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    public int BaseDamage { get; set; } = 1;

    [SerializeField]
    private float lifetime = 5f;
    [SerializeField]
    private GameObject enemyHitEffectPrefab;

    private Vector3 targetDirection;
    private bool hasDirection = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (hasDirection)
        {
            transform.position += targetDirection * (speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }
    }

    public void SetTargetDirection(Vector3 direction)
    {
        if (direction.magnitude > 0)
        {
            targetDirection = direction.normalized;
            hasDirection = true;
        }
    }

    public void IncreaseDamage(int amount)
    {
        BaseDamage += amount; 
        Debug.Log($"Projectile damage increased: {BaseDamage}");
    }

    private void InstantiateEnemyHitEffect(Vector3 enemyPosition)
    {
        if (enemyHitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(enemyHitEffectPrefab, enemyPosition, Quaternion.identity);
            hitEffect.transform.position = new Vector3(hitEffect.transform.position.x, hitEffect.transform.position.y + 0.3f, hitEffect.transform.position.z);
            Destroy(hitEffect, 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance != null && GameManager.Instance.activeEnemies.Contains(other.gameObject))
        {
            EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.TakeDamage(BaseDamage);
                InstantiateEnemyHitEffect(other.transform.position);
                Destroy(gameObject);
            }
        }
    }
}

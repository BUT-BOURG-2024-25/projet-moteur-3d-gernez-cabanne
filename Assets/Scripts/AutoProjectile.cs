using UnityEngine;

public class AutoProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float lifetime = 5f;
    [SerializeField]
    private GameObject enemyHitEffectPrefab; 

    private GameObject target;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * (speed * Time.deltaTime);

        transform.LookAt(target.transform);
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void InstantiateEnemyHitEffect(Vector3 enemyPosition)
    {
        GameObject hitEffect = Instantiate(enemyHitEffectPrefab, enemyPosition, Quaternion.identity);
        hitEffect.transform.position = new Vector3(hitEffect.transform.position.x, hitEffect.transform.position.y + 0.3f, hitEffect.transform.position.z);
        Destroy(hitEffect, 0.5f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;


                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.TakeDamage(1);
                    Destroy(gameObject);

                    if (enemyHitEffectPrefab != null)
                    {
                        InstantiateEnemyHitEffect(other.transform.position);
                    }
                }

        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class OrbitalsManager : MonoBehaviour
{
    [Header("Orbitals Settings")]
    [SerializeField] private GameObject orbitalPrefab;
    [SerializeField] private float orbitalRadius = 2f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private int damage = 1;

    private List<GameObject> orbitals = new List<GameObject>();
    private List<float> angles = new List<float>();


    void Update()
    {
        RotateOrbitals();
    }

    public void AddOrbital()
    {
        if (orbitalPrefab == null)
        {
            Debug.LogError("Aucun prefab d'orbital assigné!");
            return;
        }

        float angle = (orbitals.Count) * Mathf.PI * 2f / (orbitals.Count + 1);
        angles.Add(angle);

        Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitalRadius;
        GameObject orbital = Instantiate(orbitalPrefab, transform.position + spawnPosition, Quaternion.identity);
        orbitals.Add(orbital);

        OrbitalDamage orbitalDamage = orbital.GetComponent<OrbitalDamage>();
        if (orbitalDamage != null)
        {
            orbitalDamage.SetDamage(damage);
        }

        UpdateOrbitalAngles();
    }

    private void UpdateOrbitalAngles()
    {
        for (int i = 0; i < orbitals.Count; i++)
        {
            angles[i] = i * Mathf.PI * 2f / orbitals.Count;
        }
    }

    private void RotateOrbitals()
    {
        for (int i = 0; i < orbitals.Count; i++)
        {
            if (orbitals[i] != null)
            {
                angles[i] += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
                Vector3 newPosition = new Vector3(Mathf.Cos(angles[i]), 0, Mathf.Sin(angles[i])) * orbitalRadius;
                orbitals[i].transform.position = transform.position + newPosition;

                orbitals[i].transform.LookAt(transform.position);
                orbitals[i].transform.Rotate(0, 90, 0);
            }
        }
    }
}

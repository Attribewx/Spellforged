using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDestruction : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(explosionPrefab)
            Instantiate(explosionPrefab);
        Destroy(gameObject);
    }
}

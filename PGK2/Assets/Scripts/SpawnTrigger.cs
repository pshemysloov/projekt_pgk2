using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField]
    private EnemyAI enemyPrefab;
    [SerializeField]
    private Vector3 spawnPoint = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
    }
}

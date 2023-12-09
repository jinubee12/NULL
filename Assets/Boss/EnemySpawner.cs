using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{


    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private bool canSpawn = true;
    private void Start(){
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }



    
}

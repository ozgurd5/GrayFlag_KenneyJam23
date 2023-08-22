using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ada5EnemyEnabler : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float waitTime = 5f;
    
    [Header("Info - No Touch")]
    [SerializeField] private List<GameObject> enemies;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            enemies.Add(child.gameObject);
        }
        
        foreach (GameObject enemy in enemies) enemy.SetActive(false);
        PlayerColorEnabler.OnAllColorEnabled += EnableEnemiesMethod;
    }

    private void EnableEnemiesMethod()
    {
        StartCoroutine(EnableEnemies());
    }

    private IEnumerator EnableEnemies()
    {
        yield return new WaitForSeconds(waitTime);
        foreach (GameObject enemy in enemies) enemy.SetActive(true);
    }
    
    private void OnDestroy()
    {
        PlayerColorEnabler.OnAllColorEnabled -= EnableEnemiesMethod;
    }
}
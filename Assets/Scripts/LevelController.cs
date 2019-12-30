using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject portal;


    int enemyCount = 0;

    private void Start()
    {

    }

    public void SpawnPortal(Vector2 position)
    {
        Instantiate(portal, position, Quaternion.identity);
    }

    public void SetEnemyCount(int c)
    {
        enemyCount = c;
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }
}

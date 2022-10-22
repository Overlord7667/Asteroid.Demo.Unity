using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numberOfAsteroids;
    public int levelNumber = 1;
    public GameObject asteroid;
    public AlienScript alien;

    public void UpdateNumberOfAsteroids(int change )
    {
        numberOfAsteroids += change;

        if(numberOfAsteroids <=0)
        {
            Invoke("StartNewLevel", 3f);
        }
    }
    void StartNewLevel()
    {
        levelNumber++;

        for (int i = 0; i < levelNumber*2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-27.4f,27.4f),12f);
            Instantiate(asteroid,spawnPosition,Quaternion.identity);
            numberOfAsteroids++;
        }
        alien.NewLevel();
    }
}

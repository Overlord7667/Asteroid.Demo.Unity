using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numberOfAsteroids;
    public int levelNumber = 1;
    public GameObject asteroid;

    public void UpdateNumberOfAsteroids(int change)
    {
        numberOfAsteroids += change;

        if(numberOfAsteroids <= 0)
        {
            //Start new level
            Invoke("StartNewLevel", 3f);
        }
    }

    public void StartNewLevel()
    {
        levelNumber++;

        //Spawn new asteroids
        for (int i = 0; i < levelNumber*2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-27.9f, 27.9f), 18.9f);
            Instantiate(asteroid, spawnPosition, Quaternion.identity);
            numberOfAsteroids++;
        }
    }

    public bool CheckForHighScore(int score)
    {
        int highScore = PlayerPrefs.GetInt("highscore");
        if(score > highScore)
        {
            return true;
        }
        return false;
    }
}

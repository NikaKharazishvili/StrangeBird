using UnityEngine;
using static Constants;

// Partial class for managing saving and loading of game stats and settings
public partial class GameManager : MonoBehaviour
{
    void LoadStats()
    {
        // Load high score and total deaths, update UI
        highScore = PlayerPrefs.GetInt("HighestScore", 100);
        scoreText.text = currentScore + " / " + highScore;
        totalDeaths = PlayerPrefs.GetInt("TotalDeaths");

        // Load coins and skill level, update UI
        coin = PlayerPrefs.GetInt("Coin");
        coinText.text = coin.ToString();
        skill1Level = PlayerPrefs.GetInt("Skill1Level");

        // Load general settings (volume, game difficulty, fps-showing option, bird-spawning option)
        AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1);
        difficulty = PlayerPrefs.GetInt("Difficulty");
        showFpsOption = PlayerPrefs.GetInt("ShowFps") == 1;
        fpsText.gameObject.SetActive(showFpsOption);
        spawnBirdsOption = PlayerPrefs.GetInt("SpawnBirds") == 1;

        // Load values based on difficulty
        obstaclesSpawnDelay = difficulty == 0 ? EasyObstacleSpawnDelay : difficulty == 1 ? MediumObstacleSpawnDelay : HardObstacleSpawnDelay;
        Obstacle.SetSpeed(difficulty);  // Set static speed for all obstacles
        Coin.SetSpeed(difficulty);  // Set static speed for all coins
    }

    void SaveStats()
    {
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.SetInt("HighestScore", highScore);
        PlayerPrefs.SetInt("TotalDeaths", totalDeaths);
        PlayerPrefs.Save();
    }

    void OnApplicationQuit() => SaveStats();
}
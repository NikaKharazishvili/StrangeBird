using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using static Utils;
using static GameConstants;

// Manages core gameplay systems including scoring, spawning, and game state
public class GameManager : MonoBehaviour
{
    public int difficulty;
    [SerializeField] Player player;
    [SerializeField] GameObject menu, loadingMenu;
    [SerializeField] Image loadingBar;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] Movable[] coinsToPool, obstaclesToPool, birdsToPool;
    [SerializeField] TextMeshProUGUI coinText, timerText, scoreText, deathText, fpsText;
    [SerializeField] SpriteRenderer background;
    [SerializeField] Transform sunMoonIcon;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip newHighScoreSound, uiSelectSound;
    [SerializeField] int minutes = 0, hours = 12, score, highScore, coin, currentCoins, totalDeaths, backgroundSelected, skill1Level;
    [SerializeField] float obstaclesSpawnDelay;
    bool newHighScore, spawnBirdsOption;

    void Start()
    {
        LoadStats();
        coinText.text = coin.ToString();
        scoreText.text = score + " / " + highScore;

        spawnBirdsOption = PlayerPrefs.GetInt("SpawnBirds") == 1;
        if (spawnBirdsOption) InvokeRepeating(nameof(BirdsPool), 5f, 5f);

        fpsText.gameObject.SetActive(PlayerPrefs.GetInt("ShowFps") == 1);

        obstaclesSpawnDelay = difficulty == 0 ? EasyObstaclesSpawnDelay : difficulty == 1 ? MediumObstaclesSpawnDelay : HardObstaclesSpawnDelay;
        InvokeRepeating(nameof(ObstaclesPool), obstaclesSpawnDelay, obstaclesSpawnDelay);
        InvokeRepeating(nameof(DayNightCycle), DayNightCycleDelay, DayNightCycleDelay);  // Day/Night cycle and increase of score by difficulty
        InvokeRepeating(nameof(GainScore), GainScoreDelay, GainScoreDelay);
    }

    void LoadStats()
    {
        coin = PlayerPrefs.GetInt("Coin");
        highScore = PlayerPrefs.GetInt("HighestScore", 100);
        totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        backgroundSelected = PlayerPrefs.GetInt("BackgroundSelected");
        skill1Level = PlayerPrefs.GetInt("Skill1Level");
        AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume");
        difficulty = PlayerPrefs.GetInt("Difficulty");
    }

    void SaveStats()
    {
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.SetInt("HighestScore", highScore);
        PlayerPrefs.SetInt("TotalDeaths", totalDeaths);
        PlayerPrefs.Save();
    }
    void OnApplicationQuit() => SaveStats();

    void CoinsPool()
    {
        foreach (Movable coin in coinsToPool)
        {
            if (!coin.gameObject.activeInHierarchy)
            {
                coin.gameObject.SetActive(true);
                return;
            }
        }
    }

    void ObstaclesPool()
    {
        // Spawn Coin by chance from skill1Level
        if (skill1Level == 1) { if (PercentChanceSuccess(Skill1Level1CoinSpawnChance)) CoinsPool(); }
        else if (skill1Level == 2) { if (PercentChanceSuccess(Skill1Level2CoinSpawnChance)) CoinsPool(); }
        else if (skill1Level == 3) CoinsPool();

        foreach (Movable obstacle in obstaclesToPool)
        {
            if (!obstacle.gameObject.activeInHierarchy)
            {
                obstacle.gameObject.SetActive(true);
                return;
            }
        }
    }

    void BirdsPool()
    {
        // 40% chance to spawn moving birds (decorative)
        if (PercentChanceSuccess(BirdSpawnChance))
        {
            foreach (Movable bird in birdsToPool)
            {
                if (!bird.gameObject.activeInHierarchy)
                {
                    bird.gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    public void TakeCoin()
    {
        coin += 1;
        currentCoins += 1;
        coinText.text = coin.ToString();
    }

    void GainScore()
    {
        score += difficulty == 0 ? 2 : difficulty == 1 ? 3 : 4;  // Increase score at a time by difficulty
        if (score > highScore)
        {
            highScore = score;
            if (!newHighScore)
            {
                audioSource.PlayOneShot(newHighScoreSound);
                newHighScore = true;
            }
        }
        else newHighScore = false;
        scoreText.text = score + " / " + highScore;
        fpsText.text = "Fps: " + Mathf.RoundToInt(1 / Time.deltaTime).ToString();
    }

    void DayNightCycle()
    {
        // Timer
        minutes = (minutes + 1) % 60;
        hours = minutes == 0 ? (hours + 1) % 24 : hours;
        timerText.text = $"{hours:00}:{minutes:00}";

        float timeOfDay = (hours + (minutes / 60f)) / 24f;  // Calculate time as a value between 0 and 1 (0 is midnight, 0.5 is noon, 1 is midnight)
        float adjustedTimeOfDay = (timeOfDay + 0.5f) % 1f;  // Adjust timeOfDay to ensure 12:00 is the peak daylight

        Color dayColor = new Color(1f, 1f, 1f);  // White (max light)
        Color nightColor = new Color(0f, 0f, 0f);  // Black (no light)

        if (adjustedTimeOfDay < 0.5f) background.color = Color.Lerp(dayColor, nightColor, adjustedTimeOfDay * 2);  // Morning to afternoon (dayColor to nightColor)
        else background.color = Color.Lerp(nightColor, dayColor, (adjustedTimeOfDay - 0.5f) * 2);  // Afternoon to morning (nightColor to dayColor)

        // Rotate the sun & moon icon based on time of day
        float rotationAngle = (timeOfDay * 360f + 180f) % 360f;  // Calculate the rotation angle starting from 180 degrees
        sunMoonIcon.localRotation = Quaternion.Euler(0, 0, rotationAngle);  // Apply the rotation to the RectTransform
    }

    public void CanSpawn(bool isActive)
    {
        if (isActive)  // Respawn
        {
            // Disable active objects from pools
            foreach (Movable coin in coinsToPool) coin.gameObject.SetActive(false);
            foreach (Movable obstacle in obstaclesToPool) obstacle.gameObject.SetActive(false);
            foreach (Movable bird in birdsToPool) bird.gameObject.SetActive(false);

            obstaclesSpawnDelay = difficulty == 1 ? 1.5f : difficulty == 2 ? 1.5f : 1f;
            if (spawnBirdsOption) InvokeRepeating(nameof(BirdsPool), 5f, 5f);
            InvokeRepeating(nameof(ObstaclesPool), obstaclesSpawnDelay, obstaclesSpawnDelay);
            InvokeRepeating(nameof(DayNightCycle), DayNightCycleDelay, DayNightCycleDelay);
            InvokeRepeating(nameof(GainScore), GainScoreDelay, GainScoreDelay);
            score = 0;
            scoreText.text = score + " / " + highScore;
        }
        else  // Death
        {
            // Stop Coin and Obstacle movement (Birds keep flying)
            foreach (Movable coin in coinsToPool) coin.Move(Movable.MoveDirection.None);
            foreach (Movable obstacle in obstaclesToPool) obstacle.Move(Movable.MoveDirection.None);

            // Disable spawning
            if (spawnBirdsOption)
            {
                CancelInvoke(nameof(BirdsPool));
                foreach (Bird bird in birdsToPool) bird.FlyAwayAfterPlayerDeath();
            }
            CancelInvoke(nameof(ObstaclesPool));
            CancelInvoke(nameof(DayNightCycle));
            CancelInvoke(nameof(GainScore));

            // Show death menu and update stats
            totalDeaths += 1;
            deathText.text = $"Total Deaths: {totalDeaths}\nHigh Score: {highScore}\nCoins Collected This Round: {currentCoins}";
            currentCoins = 0;
            this.Wait(0.5f, () => menu.SetActive(true));
        }
    }

    public void MenuSelection(int index)
    {
        audioSource.PlayOneShot(uiSelectSound);
        if (index == 1)  // Play
        {
            player.Respawn();
            menu.SetActive(false);
        }
        else if (index == 2)  // Menu
        {
            SaveStats();
            foreach (Transform child in menu.transform) child.gameObject.SetActive(false);
            loadingMenu.SetActive(true);
            StartCoroutine(LoadSceneAsync());
        }
        else if (index == 3) QuitApplication();  // Exit
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(0);
        operation.allowSceneActivation = false;  // Prevent auto scene switch

        // Wait until the scene is fully loaded in the background
        while (operation.progress < 0.9f)
        {
            float progress = operation.progress / 0.9f;  // Normalize to 0-1
            loadingText.text = $"Loading {Mathf.RoundToInt(progress * 100)}%";
            loadingBar.fillAmount = progress;
            yield return null;
        }

        // Scene is ready - show 100% briefly then activate
        loadingText.text = "Loading 100%";
        loadingBar.fillAmount = 1f;
        // yield return new WaitForSeconds(0.2f);  // Brief moment to show 100% (optional)
        operation.allowSceneActivation = true;  // Switch to the loaded scene
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Utils;
using static Constants;

// Manages core gameplay systems including scoring, spawning, and game state
public partial class GameManager : MonoBehaviour
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
        SubscribeToPlayerEvents();  // OnDeath, OnRespawn, OnCoinTake
        LoadStats();  // Load saved stats, player preferences, and game settings
        StartGameplayLoops();  // Initialize core gameplay loops (spawning Obstacles, Coins, Birds, Day/Night cycle, and score gain)
    }

    void OnApplicationQuit() => SaveStats();

    void StartGameplayLoops()
    {
        if (spawnBirdsOption) InvokeRepeating(nameof(BirdsPool), 5f, 5f);  // Spawn birds as well if option is enabled
        InvokeRepeating(nameof(ObstaclesPool), obstaclesSpawnDelay, obstaclesSpawnDelay);
        // Even though Coins spawn with the same delay as Obstacles, they are positioned to the right of Obstacles to avoid overlap
        InvokeRepeating(nameof(CoinsPool), obstaclesSpawnDelay, obstaclesSpawnDelay);
        InvokeRepeating(nameof(DayNightCycle), DayNightCycleInterval, DayNightCycleInterval);  // Affects background color, sun/moon icon rotation and timer
        InvokeRepeating(nameof(GainScore), ScoreGainInterval, ScoreGainInterval);  // Increase score over time based on difficulty
    }

    // Spawn a coin from the pool based on skill level and random chance
    void CoinsPool()
    {
        // Spawn coin based on skill level
        if (skill1Level == 1 && !PercentChanceSuccess(Skill1Level1CoinChance)) return;
        if (skill1Level == 2 && !PercentChanceSuccess(Skill1Level2CoinChance)) return;

        foreach (Movable coin in coinsToPool)
        {
            if (!coin.gameObject.activeInHierarchy)
            {
                coin.gameObject.SetActive(true);
                return;
            }
        }
    }

    // Spawn an obstacle from the pool
    void ObstaclesPool()
    {
        foreach (Movable obstacle in obstaclesToPool)
        {
            if (!obstacle.gameObject.activeInHierarchy)
            {
                obstacle.gameObject.SetActive(true);
                return;
            }
        }
    }

    // Spawn a bird from the pool with a 40% chance
    void BirdsPool()
    {
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

    // Increment score over time based on difficulty
    void GainScore()
    {
        score += difficulty == 0 ? EasyScoreIncrement : difficulty == 1 ? MediumScoreIncrement : HardScoreIncrement;
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

    // Handle day/night cycle: update timer, background color, and sun/moon rotation
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

    // Handle menu selection input (Play, Menu, Quit)
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
            StartCoroutine(LoadSceneAsync("Menu", loadingBar, loadingText));
        }
        else if (index == 3) QuitApplication();  // Exit
    }
}
using UnityEngine;

// Partial class for saving and loading game stats and settings using PlayerPrefs
public partial class MenuManager : MonoBehaviour
{
    void Awake() => LoadStats();
    void OnApplicationQuit() => SaveStats();

    void SaveStats()
    {
        // Save selected cosmetics
        PlayerPrefs.SetInt("BirdSelected", birdSelected);
        PlayerPrefs.SetInt("BackgroundSelected", backgroundSelected);
        PlayerPrefs.SetInt("ObstacleSelected", obstacleSelected);

        // Save bought cosmetics and skills
        SaveBoolArray("BirdsBought", birdsBought);
        SaveBoolArray("BackgroundsBought", backgroundsBought);
        SaveBoolArray("ObstaclesBought", obstaclesBought);
        PlayerPrefs.SetInt("Skill1Level", skill1Level);
        PlayerPrefs.SetInt("Skill2Level", skill2Level);

        // Save options
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.SetFloat("GlobalVolume", AudioListener.volume);
        PlayerPrefs.SetInt("SpawnBirds", spawnBirds);
        PlayerPrefs.SetInt("ShowFps", showFps);

        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.Save();
    }

    void LoadStats()
    {
        // Load cosmetics. First style is always unlocked(and selected if no other selection) by default
        birdsBought[0] = true;
        backgroundsBought[0] = true;
        obstaclesBought[0] = true;
        birdSelected = PlayerPrefs.GetInt("BirdSelected");
        backgroundSelected = PlayerPrefs.GetInt("BackgroundSelected");
        obstacleSelected = PlayerPrefs.GetInt("ObstacleSelected");

        // Load bought cosmetics and skills
        LoadBoolArray("BirdsBought", birdsBought);
        LoadBoolArray("BackgroundsBought", backgroundsBought);
        LoadBoolArray("ObstaclesBought", obstaclesBought);
        skill1Level = PlayerPrefs.GetInt("Skill1Level");
        skill2Level = PlayerPrefs.GetInt("Skill2Level");

        // Load options
        difficulty = PlayerPrefs.GetInt("Difficulty");
        difficultyTexts[difficulty].color = Color.yellow;

        AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1);
        soundsCheckmark.sprite = AudioListener.volume == 1 ? spriteAtlas.GetSprite("Checkmark_Enabled") : spriteAtlas.GetSprite("Checkmark_Disabled");

        spawnBirds = PlayerPrefs.GetInt("SpawnBirds", 1);
        birdsCheckmark.sprite = spawnBirds == 1 ? spriteAtlas.GetSprite("Checkmark_Enabled") : spriteAtlas.GetSprite("Checkmark_Disabled");

        showFps = PlayerPrefs.GetInt("ShowFps");
        fpsText.gameObject.SetActive(PlayerPrefs.GetInt("ShowFps") == 1);
        if (showFps == 1) InvokeRepeating(nameof(ShowFps), 0, 1f);
        fpsCheckmark.sprite = showFps == 1 ? spriteAtlas.GetSprite("Checkmark_Enabled") : spriteAtlas.GetSprite("Checkmark_Disabled");

        // Load coins (100 by default)
        coin = PlayerPrefs.GetInt("Coin", 100);
        coinText.text = coin.ToString();
    }

    // Helper to save a bool array as ints
    void SaveBoolArray(string keyPrefix, bool[] array)
    {
        for (int i = 1; i < array.Length; i++)  // start from 1, first is always unlocked
            PlayerPrefs.SetInt($"{keyPrefix}{i}", array[i] ? 1 : 0);
    }

    // Helper to load a bool array from PlayerPrefs
    void LoadBoolArray(string keyPrefix, bool[] array)
    {
        for (int i = 1; i < array.Length; i++)
            array[i] = PlayerPrefs.GetInt($"{keyPrefix}{i}", 0) == 1;
    }
}
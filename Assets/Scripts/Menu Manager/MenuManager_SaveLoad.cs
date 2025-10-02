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
        for (int i = 1; i < birdsBought.Length; i++) PlayerPrefs.SetInt($"BirdsBought{i}", birdsBought[i] ? 1 : 0);
        for (int i = 1; i < backgroundsBought.Length; i++) PlayerPrefs.SetInt($"BackgroundsBought{i}", backgroundsBought[i] ? 1 : 0);
        for (int i = 1; i < obstaclesBought.Length; i++) PlayerPrefs.SetInt($"ObstaclesBought{i}", obstaclesBought[i] ? 1 : 0);
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
        birdSelected = PlayerPrefs.GetInt("BirdSelected", 0);
        backgroundSelected = PlayerPrefs.GetInt("BackgroundSelected", 0);
        obstacleSelected = PlayerPrefs.GetInt("ObstacleSelected", 0);

        // Load bought cosmetics and skills
        for (int i = 1; i < birdsBought.Length; i++) birdsBought[i] = PlayerPrefs.GetInt($"BirdsBought{i}", 0) == 1;
        for (int i = 1; i < backgroundsBought.Length; i++) backgroundsBought[i] = PlayerPrefs.GetInt($"BackgroundsBought{i}", 0) == 1;
        for (int i = 1; i < obstaclesBought.Length; i++) obstaclesBought[i] = PlayerPrefs.GetInt($"ObstaclesBought{i}", 0) == 1;
        skill1Level = PlayerPrefs.GetInt("Skill1Level", 1);
        skill2Level = PlayerPrefs.GetInt("Skill2Level", 1);

        // Load options
        difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        difficultyTexts[difficulty].color = Color.yellow;

        AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1);
        soundsCheckmark.sprite = AudioListener.volume == 1 ? spriteAtlas.GetSprite("Checkmark_Enabled") : spriteAtlas.GetSprite("Checkmark_Disabled");

        spawnBirds = PlayerPrefs.GetInt("SpawnBirds", 1);
        birdsCheckmark.sprite = spawnBirds == 1 ? spriteAtlas.GetSprite("Checkmark_Enabled") : spriteAtlas.GetSprite("Checkmark_Disabled");

        showFps = PlayerPrefs.GetInt("ShowFps", 0);
        fpsText.gameObject.SetActive(PlayerPrefs.GetInt("ShowFps", 0) == 1);
        if (showFps == 1) InvokeRepeating(nameof(ShowFps), 0, 1f);
        fpsCheckmark.sprite = showFps == 1 ? spriteAtlas.GetSprite("Checkmark_Enabled") : spriteAtlas.GetSprite("Checkmark_Disabled");

        // Load coins (100 by default)
        coin = PlayerPrefs.GetInt("Coin", 100);
        coinText.text = coin.ToString();
    }
}
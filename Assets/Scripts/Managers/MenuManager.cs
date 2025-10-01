using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using static Utils;
using static GameConstants;
using VInspector;

public class MenuManager : MonoBehaviour
{
    [Tab("Main Menu")]
    [SerializeField] GameObject menu, loadingMenu, shopMenu, optionMenu, aboutText, back;
    [SerializeField] Image loadingBar, background;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip uiSelectSound;

    [Tab("Shop Menu")]
    [SerializeField] SpriteAtlas spriteAtlas;
    ShopType shopType;
    [SerializeField] GameObject shopCosmetics, shopSkills;
    [SerializeField] Image cosmeticPreviewImage;
    [SerializeField] GameObject cosmeticBuyButton;
    [SerializeField] TextMeshProUGUI[] cosmeticStyleTexts;
    [SerializeField] TextMeshProUGUI[] skillLevelTexts;
    [SerializeField] GameObject[] skillBuyButtons;
    int selectedItemIndex, selectedItemCost;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] AudioClip[] buySounds;

    // These stats are saved and loaded using PlayerPrefs
    int coin;
    bool[] birdsBought = new bool[8], backgroundsBought = new bool[8], obstaclesBought = new bool[6];
    int birdSelected, backgroundSelected, obstacleSelected;
    int skill1Level, skill2Level;

    [Tab("Options Menu")]
    [SerializeField] TextMeshProUGUI[] difficultyTexts;
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] GameObject[] soundsCheckmarks, fpsCheckmarks, birdsCheckmarks;

    // These stats are saved and loaded using PlayerPrefs
    int difficulty, showFps, spawnBirds;

    void Start() => LoadStats();

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
        PlayerPrefs.SetInt("ShowFps", showFps);
        PlayerPrefs.SetInt("SpawnBirds", spawnBirds);

        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.Save();
    }

    void LoadStats()
    {
        /* Shop Menu */
        // Load coins (100 by default)
        coin = PlayerPrefs.GetInt("Coin", 100);
        coinText.text = coin.ToString();

        // Load cosmetics (first item is always unlocked and selected by default)
        birdsBought[0] = true; backgroundsBought[0] = true; obstaclesBought[0] = true;
        birdSelected = PlayerPrefs.GetInt("BirdSelected", 0);
        backgroundSelected = PlayerPrefs.GetInt("BackgroundSelected", 0);
        obstacleSelected = PlayerPrefs.GetInt("ObstacleSelected", 0);

        for (int i = 1; i < birdsBought.Length; i++) birdsBought[i] = PlayerPrefs.GetInt($"BirdsBought{i}", 0) == 1;
        for (int i = 1; i < backgroundsBought.Length; i++) backgroundsBought[i] = PlayerPrefs.GetInt($"BackgroundsBought{i}", 0) == 1;
        for (int i = 1; i < obstaclesBought.Length; i++) obstaclesBought[i] = PlayerPrefs.GetInt($"ObstaclesBought{i}", 0) == 1;

        // Load skills (level 1 by default)
        skill1Level = PlayerPrefs.GetInt("Skill1Level", 1);
        skill2Level = PlayerPrefs.GetInt("Skill2Level", 1);

        /* Options Menu */
        // Difficulty (Easy by default)
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        difficultyTexts[difficulty].color = Color.yellow;

        AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1);
        if (AudioListener.volume == 1) soundsCheckmarks[0].SetActive(true);
        else soundsCheckmarks[1].SetActive(true);

        showFps = PlayerPrefs.GetInt("ShowFps", 0);
        if (showFps == 1) fpsCheckmarks[0].SetActive(true);
        else { fpsCheckmarks[1].SetActive(true); fpsText.gameObject.SetActive(false); }
        fpsText.gameObject.SetActive(PlayerPrefs.GetInt("ShowFps", 0) == 1);

        spawnBirds = PlayerPrefs.GetInt("SpawnBirds", 1);
        if (spawnBirds == 1) birdsCheckmarks[0].SetActive(true);
        else birdsCheckmarks[1].SetActive(true);

        if (showFps == 1) InvokeRepeating(nameof(ShowFps), 0, 1f);
    }
    void OnApplicationQuit() { SaveStats(); }

    public void MenuSelection(int index)
    {
        audioSource.PlayOneShot(uiSelectSound);
        if (index == 0)  // Play
        {
            SaveStats();
            foreach (Transform child in menu.transform) child.gameObject.SetActive(false);
            loadingMenu.SetActive(true);
            back.SetActive(false);
            StartCoroutine(LoadSceneAsync());
        }
        else if (index == 1)  // Shop Menu
        {
            background.color = new Color(0.2f, 0.2f, 0.2f);
            menu.SetActive(false);
            shopMenu.SetActive(true);
            back.SetActive(true);
        }
        else if (index == 2)  // Options Menu
        {
            background.color = new Color(0.2f, 0.2f, 0.2f);
            menu.SetActive(false);
            optionMenu.SetActive(true);
            back.SetActive(true);
        }
        else if (index == 3)  // About Text
        {
            background.color = new Color(0.2f, 0.2f, 0.2f);
            menu.SetActive(false);
            aboutText.SetActive(true);
            back.SetActive(true);
        }
        else if (index == 4) QuitApplication();
        else if (index == 5)  // Back to Main Menu
        {
            background.color = Color.white;
            back.SetActive(false);
            menu.SetActive(true);
            shopMenu.SetActive(false);
            shopCosmetics.SetActive(false);
            shopSkills.SetActive(false);
            optionMenu.SetActive(false);
            aboutText.SetActive(false);
        }
    }

    // Opens the cosmetics shop (Birds, Backgrounds, Obstacles) and sets up UI
    public void OpenCosmeticShop(int index)
    {
        audioSource.PlayOneShot(uiSelectSound);
        shopMenu.SetActive(false);
        shopCosmetics.SetActive(true);
        cosmeticBuyButton.SetActive(false);
        shopType = (ShopType)index;

        // Determine item count and set text visibility (how many)
        int itemCount = shopType == ShopType.Bird ? 8 : shopType == ShopType.Background ? 8 : shopType == ShopType.Obstacle ? 6 : 0;
        for (int i = 0; i < cosmeticStyleTexts.Length; i++)
            cosmeticStyleTexts[i].gameObject.SetActive(i < itemCount);

        // Update selected item cost based on shop type
        selectedItemCost = shopType == ShopType.Bird ? BirdCost : shopType == ShopType.Background ? BackgroundCost : ObstacleCost;
        // Set initial selected item index to current selection
        selectedItemIndex = shopType == ShopType.Bird ? birdSelected : shopType == ShopType.Background ? backgroundSelected : obstacleSelected;
        // Adjust preview image size for obstacles (taller)
        cosmeticPreviewImage.rectTransform.sizeDelta = shopType == ShopType.Obstacle ? new Vector2(141, 512) : new Vector2(512, 512);

        UpdateCosmeticItemUI();  // Update item UI (colors, preview image, buy button)
    }

    // Updates cosmetic item UI: text colors (yellow for bought+selected, white for bought, grey for locked), preview image, and buy button
    void UpdateCosmeticItemUI()
    {
        int selectedIndex = shopType == ShopType.Bird ? birdSelected : shopType == ShopType.Background ? backgroundSelected : obstacleSelected;

        for (int i = 0; i < cosmeticStyleTexts.Length; i++)
            if (cosmeticStyleTexts[i].gameObject.activeSelf)
                cosmeticStyleTexts[i].color = i == selectedIndex && (shopType == ShopType.Bird ? birdsBought[i] : shopType == ShopType.Background ? backgroundsBought[i] : obstaclesBought[i]) ? Color.yellow : (shopType == ShopType.Bird ? birdsBought[i] : shopType == ShopType.Background ? backgroundsBought[i] : obstaclesBought[i]) ? Color.white : Color.grey;

        cosmeticPreviewImage.sprite = spriteAtlas.GetSprite(shopType.ToString() + selectedItemIndex);
    }

    // Selects a cosmetic item and updates UI if the item is bought
    public void SelectCosmeticItem(int index)
    {
        audioSource.PlayOneShot(uiSelectSound);
        selectedItemIndex = index;

        // Update selected index based on shop type if the item is bought
        bool isBought = shopType == ShopType.Bird ? birdsBought[index] : shopType == ShopType.Background ? backgroundsBought[index] : obstaclesBought[index];
        if (isBought)  // If bought, select it
        {
            cosmeticBuyButton.SetActive(false);
            if (shopType == ShopType.Bird) birdSelected = index;
            else if (shopType == ShopType.Background) backgroundSelected = index;
            else if (shopType == ShopType.Obstacle) obstacleSelected = index;
        }
        else cosmeticBuyButton.SetActive(true);

        UpdateCosmeticItemUI();
    }

    public void BuySelectedItem()
    {
        if (coin >= selectedItemCost)
        {
            audioSource.PlayOneShot(buySounds[Random.Range(0, buySounds.Length)]);
            coin -= selectedItemCost;
            coinText.text = coin.ToString();
            cosmeticBuyButton.SetActive(false);

            if (shopType == ShopType.Bird) { birdsBought[selectedItemIndex] = true; birdSelected = selectedItemIndex; }
            else if (shopType == ShopType.Background) { backgroundsBought[selectedItemIndex] = true; backgroundSelected = selectedItemIndex; }
            else if (shopType == ShopType.Obstacle) { obstaclesBought[selectedItemIndex] = true; obstacleSelected = selectedItemIndex; }

            UpdateCosmeticItemUI();
        }
    }

    public void OpenSkillShop()
    {
        audioSource.PlayOneShot(uiSelectSound);
        shopMenu.SetActive(false);
        shopSkills.SetActive(true);
        if (skill1Level >= 3) skillBuyButtons[0].SetActive(false);
        if (skill2Level >= 3) skillBuyButtons[1].SetActive(false);
        skillLevelTexts[0].text = skill1Level + " / 3";
        skillLevelTexts[1].text = skill2Level + " / 3";
    }

    public void BuySkill(int index)
    {
        if (coin >= SkillCost)
        {
            audioSource.PlayOneShot(buySounds[Random.Range(0, buySounds.Length)]);
            coin -= SkillCost;
            coinText.text = coin.ToString();
            if (index == 0) { skill1Level += 1; skillLevelTexts[0].text = skill1Level + " / 3"; }
            else if (index == 1) { skill2Level += 1; skillLevelTexts[1].text = skill2Level + " / 3"; }
            if (skill1Level >= 3) skillBuyButtons[0].SetActive(false);
            if (skill2Level >= 3) skillBuyButtons[1].SetActive(false);
        }
    }

    public void OptionsSelection(int index)  // Options Menu
    {
        audioSource.PlayOneShot(uiSelectSound);
        if (index <= 3)  // Change Difficulty
        {
            difficulty = index;
            foreach (TextMeshProUGUI text in difficultyTexts) text.color = Color.white;
            difficultyTexts[index - 1].color = Color.yellow;
        }
        else if (index == 4)  // Toggle Sound
        {
            soundsCheckmarks[0].SetActive(!soundsCheckmarks[0].activeSelf);
            soundsCheckmarks[1].SetActive(!soundsCheckmarks[1].activeSelf);
            if (AudioListener.volume == 1) AudioListener.volume = 0;
            else AudioListener.volume = 1;
        }
        else if (index == 5)  // Toggle Fps
        {
            fpsText.gameObject.SetActive(!fpsText.gameObject.activeSelf);
            fpsCheckmarks[0].SetActive(!fpsCheckmarks[0].activeSelf);
            fpsCheckmarks[1].SetActive(!fpsCheckmarks[1].activeSelf);
            showFps = showFps == 1 ? 0 : 1;
            if (showFps == 1) InvokeRepeating(nameof(ShowFps), 0, 1f);
            else CancelInvoke(nameof(ShowFps));
        }
        else if (index == 6)  // Toggle Birds
        {
            birdsCheckmarks[0].SetActive(!birdsCheckmarks[0].activeSelf);
            birdsCheckmarks[1].SetActive(!birdsCheckmarks[1].activeSelf);
            spawnBirds = spawnBirds == 1 ? 0 : 1;
        }
    }
    void ShowFps() { fpsText.text = "Fps: " + Mathf.RoundToInt(1 / Time.deltaTime).ToString(); }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
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
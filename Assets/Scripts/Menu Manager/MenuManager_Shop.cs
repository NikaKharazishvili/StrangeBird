using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;
using static GameConstants;
using VInspector;

// Partial class for managing the shop menu, including buying and selecting cosmetics and skills
public partial class MenuManager : MonoBehaviour
{
    [Tab("Shop Menu")]
    [SerializeField] SpriteAtlas spriteAtlas;
    public enum ShopType : byte { Bird = 1, Background = 2, Obstacle = 3, Skill = 4 }
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
    bool[] birdsBought = new bool[8], backgroundsBought = new bool[8], obstaclesBought = new bool[6];
    int birdSelected, backgroundSelected, obstacleSelected;
    int skill1Level, skill2Level;
    int coin;

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
}
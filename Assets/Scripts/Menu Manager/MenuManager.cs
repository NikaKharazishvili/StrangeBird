using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Utils;
using VInspector;

// Partial class for managing the Main Menu, including navigation and scene loading
public partial class MenuManager : MonoBehaviour
{
    [Tab("Main Menu")]
    [SerializeField] GameObject menu, loadingMenu, shopMenu, optionMenu, aboutText, back;
    [SerializeField] Image loadingBar, background;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip uiSelectSound;

    enum MenuType : byte { Play = 0, Shop = 1, Options = 2, About = 3, Quit = 4, Back = 5 }

    public void MenuSelection(int index)
    {
        audioSource.PlayOneShot(uiSelectSound);
        switch ((MenuType)index)
        {
            case MenuType.Play: Play(); break;
            case MenuType.Shop: ShowSubMenu(shopMenu); break;
            case MenuType.Options: ShowSubMenu(optionMenu); break;
            case MenuType.About: ShowSubMenu(aboutText); break;
            case MenuType.Quit: QuitApplication(); break;
            case MenuType.Back: ResetMainMenu(); break;
        }
    }

    void Play()
    {
        SaveStats();
        foreach (Transform child in menu.transform) child.gameObject.SetActive(false);
        loadingMenu.SetActive(true);
        back.SetActive(false);
        StartCoroutine(LoadSceneAsync("Game", loadingBar, loadingText));
    }

    void ShowSubMenu(GameObject submenu)
    {
        background.color = new Color(0.2f, 0.2f, 0.2f);
        menu.SetActive(false);
        submenu.SetActive(true);
        back.SetActive(true);
    }

    void ResetMainMenu()
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
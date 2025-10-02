using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using static Utils;
using VInspector;

// Partial class for managing the main menu, including navigation and scene loading
public partial class MenuManager : MonoBehaviour
{
    [Tab("Main Menu")]
    [SerializeField] GameObject menu, loadingMenu, shopMenu, optionMenu, aboutText, back;
    [SerializeField] Image loadingBar, background;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip uiSelectSound;

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
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VInspector;

// Partial class for managing the options menu, including difficulty, sound, FPS display, and bird spawning
public partial class MenuManager : MonoBehaviour
{
    [Tab("Options Menu")]
    [SerializeField] TextMeshProUGUI[] difficultyTexts;
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] Image soundsCheckmark, birdsCheckmark, fpsCheckmark;
    int difficulty, spawnBirds, showFps;

    // Updates FPS display text every second when enabled
    void ShowFps() => fpsText.text = $"FPS: {Mathf.RoundToInt(1f / Time.deltaTime)}";

    public enum MenuOption : byte { EasyDifficulty = 0, MediumDifficulty = 1, HardDifficulty = 2, ToggleSound = 3, ToggleBirds = 4, ToggleFps = 5 }
    // Handles option menu selections (difficulty, sound, FPS, birds) using enum-based input
    public void OptionsSelection(int index)
    {
        audioSource.PlayOneShot(uiSelectSound);

        switch ((MenuOption)index)
        {
            case MenuOption.EasyDifficulty:
            case MenuOption.MediumDifficulty:
            case MenuOption.HardDifficulty:
                SetDifficulty(index);
                break;
            case MenuOption.ToggleSound:
                soundsCheckmark.sprite = AudioListener.volume == 1 ? spriteAtlas.GetSprite("Checkmark_Disabled") : spriteAtlas.GetSprite("Checkmark_Enabled");
                AudioListener.volume = AudioListener.volume == 1 ? 0 : 1;
                break;
            case MenuOption.ToggleBirds:
                birdsCheckmark.sprite = spawnBirds == 1 ? spriteAtlas.GetSprite("Checkmark_Disabled") : spriteAtlas.GetSprite("Checkmark_Enabled");
                spawnBirds = spawnBirds == 1 ? 0 : 1;
                break;
            case MenuOption.ToggleFps:
                fpsCheckmark.sprite = showFps == 1 ? spriteAtlas.GetSprite("Checkmark_Disabled") : spriteAtlas.GetSprite("Checkmark_Enabled");
                fpsText.gameObject.SetActive(!fpsText.gameObject.activeSelf);
                showFps = showFps == 1 ? 0 : 1;
                if (showFps == 1) InvokeRepeating(nameof(ShowFps), 0, 1f);
                else CancelInvoke(nameof(ShowFps));
                break;
        }
    }

    // Sets the game difficulty and updates the UI to reflect the current selection
    void SetDifficulty(int index)
    {
        difficulty = index;
        foreach (TextMeshProUGUI text in difficultyTexts) text.color = Color.white;
        difficultyTexts[index].color = Color.yellow;
    }
}
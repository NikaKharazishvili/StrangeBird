using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VInspector;

// Centralized cursor manager to handle hand/arrow cursor for all UI buttons
public sealed class CursorManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hoverSound, clickSound;
    [SerializeField] Texture2D cursorArrow;  // Default cursor
    [SerializeField] Texture2D cursorHand;  // Cursor when hovering over buttons
    [SerializeField] Button[] buttons;

    [Button]
    // Populates the buttons array with all UI buttons in the scene, run once in editor to optimize runtime performance
    void FindAllButtons() => buttons = FindObjectsOfType<Button>(true);

    // Sets up cursor and audio events for UI buttons at runtime (required for event functionality) using the pre-populated buttons array
    void Awake() { foreach (Button button in buttons) AddCursorEvents(button); }

    // Attaches cursor and audio event handlers to a UI button
    void AddCursorEvents(Button button)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        // Enter → hand cursor
        var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((_) =>
        {
            Cursor.SetCursor(cursorHand, Vector2.zero, CursorMode.Auto);
            audioSource.PlayOneShot(hoverSound);
        });
        trigger.triggers.Add(entryEnter);

        // Exit → arrow cursor
        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((_) => Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto));
        trigger.triggers.Add(entryExit);

        // Play click sound, and reset cursor only if the button becomes inactive
        button.onClick.AddListener(() =>
        {
            if (!button.gameObject.activeInHierarchy)
                Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);

            audioSource.PlayOneShot(clickSound);
        });
    }
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Centralized cursor manager to handle hand/arrow cursor for all UI buttons
public class CursorManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hoverSound, clickSound;
    [SerializeField] private Texture2D pointerArrow;  // Default cursor
    [SerializeField] private Texture2D pointerHand;   // Cursor when hovering over buttons

    void Start()
    {
        // Find all active buttons in the scene
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button button in buttons)
            AddCursorEvents(button);
    }

    void AddCursorEvents(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (!trigger) trigger = button.gameObject.AddComponent<EventTrigger>();

        // Pointer Enter → hand cursor
        var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((_) =>
        {
            Cursor.SetCursor(pointerHand, Vector2.zero, CursorMode.Auto);
            audioSource.PlayOneShot(hoverSound);
        });
        trigger.triggers.Add(entryEnter);

        // Pointer Exit → arrow cursor
        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((_) => Cursor.SetCursor(pointerArrow, Vector2.zero, CursorMode.Auto));
        trigger.triggers.Add(entryExit);

        // Play click sound, and reset cursor only if the button becomes inactive
        button.onClick.AddListener(() =>
        {
            if (!button.gameObject.activeInHierarchy)
                Cursor.SetCursor(pointerArrow, Vector2.zero, CursorMode.Auto);

            audioSource.PlayOneShot(clickSound);
        });
    }

    // For dynamically added buttons
    public void RegisterButton(Button button) => AddCursorEvents(button);
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Centralized cursor manager to handle hand/arrow cursor for all UI buttons
public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D arrowCursor;  // Default cursor
    [SerializeField] private Texture2D handCursor;  // Cursor when hovering over buttons

    void Start()
    {
        // Find all active buttons in the scene
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button button in buttons)
            AddCursorEvents(button);
    }

    // Add pointer enter/exit events dynamically
    void AddCursorEvents(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (!trigger) trigger = button.gameObject.AddComponent<EventTrigger>();

        // Pointer Enter → hand cursor
        var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((_) => Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto));
        trigger.triggers.Add(entryEnter);

        // Pointer Exit → arrow cursor
        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((_) => Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.Auto));
        trigger.triggers.Add(entryExit);

        // Reset cursor when button clicked/disabled (to avoid stuck hand cursor bug)
        button.onClick.AddListener(() => Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.Auto));
    }

    // Call this if new buttons are added dynamically during gameplay
    public void RegisterButton(Button button) => AddCursorEvents(button);
}
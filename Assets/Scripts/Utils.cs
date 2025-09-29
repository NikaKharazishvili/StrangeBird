using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

// Static utility methods for common game operations
public static class Utils
{
    // Returns true if a random number between 0-99 is less than the given percent (0-100)
    public static bool PercentChanceSuccess(int percent) => Random.Range(0, 100) < percent;

    // Provides an async awaitable delay method as an extension to MonoBehaviour. Usage: this.Wait(2f, () => { Your codes here });
    public static async void Wait(this MonoBehaviour monoBehaviour, float delay, UnityAction action)
    {
        await Task.Delay((int)(delay * 1000));

        if (monoBehaviour) action?.Invoke();
        else Debug.LogWarning("MonoBehaviour destroyed before wait completed, action not invoked");
    }

    // Exit the application in build, or stop play mode in Unity Editor
    // Note: Application.Quit() is called even in Editor to ensure OnApplicationQuit() callbacks are executed for proper cleanup and saving
    public static void QuitApplication()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
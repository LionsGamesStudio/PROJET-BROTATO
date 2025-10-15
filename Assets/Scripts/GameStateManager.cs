using FluxFramework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the overall game state, including scene transitions and resetting game data.
/// This component is designed to be a persistent singleton.
/// </summary>
public class GameStateManager : FluxMonoBehaviour
{
    // --- Singleton Pattern ---
    public static GameStateManager Instance { get; private set; }

    [Header("Scene Configuration")]
    [Tooltip("The name of your main menu scene in Build Settings.")]
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";

    [Tooltip("The name of your main game scene in Build Settings.")]
    [SerializeField] private string gameSceneName = "GameScene";

    protected override void OnFluxAwake()
    {
        // Enforce the singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Make this object persist across scene loads
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Public method to start a new game. It resets the state and then loads the game scene.
    /// This should be called by UI buttons in the main menu.
    /// </summary>
    public void StartNewGame()
    {
        Debug.Log("Starting new game...");

        Debug.Log("Game state has been cleared. Loading game scene...");
        SceneManager.LoadScene(gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Public method to return to the main menu from the game scene.
    /// </summary>
    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu...");

        // Do 2 time to force register of current scene properties
        SceneManager.LoadScene(mainMenuSceneName);
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
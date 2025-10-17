using UnityEngine;

/// <summary>
/// A simple helper component to be placed on UI Buttons
/// to trigger scene transitions via the GameStateManager.
/// </summary>
public class SceneTransitionButton : MonoBehaviour
{
    /// <summary>
    /// Tells the GameStateManager to start a new game, which includes resetting the state.
    /// Link this to your "Start Game" or "New Game" button's OnClick event.
    /// </summary>
    public void TransitionToGameScene()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.StartNewGame();
        }
        else
        {
            Debug.LogError("GameStateManager not found in the scene. Make sure it exists in your initial scene.");
        }
    }

    /// <summary>
    /// Tells the GameStateManager to return to the main menu.
    /// Link this to your "Quit to Menu" or "Main Menu" button's OnClick event in the game scene.
    /// </summary>
    public void TransitionToMainMenu()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.ReturnToMainMenu();
        }
        else
        {
            Debug.LogError("GameStateManager not found in the scene. Make sure it exists in your initial scene.");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include this to work with UI elements

public class GameManager : MonoBehaviour
{
    // Reference to the Game Over RawImage
    public RawImage gameOverRawImage;

    // Method to be called when the game is over
    public void GameOver()
    {
        Debug.Log("Game Over!");

        // Display the Game Over image
        if (gameOverRawImage != null)
        {
            gameOverRawImage.gameObject.SetActive(true); // Show the Game Over RawImage
        }

        // Optionally reload the scene after a delay
        StartCoroutine(ReloadSceneAfterDelay(2.0f)); // Change the delay time as needed
    }

    // Coroutine to reload the scene after a specified delay
    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
}



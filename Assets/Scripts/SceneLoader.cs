using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour {

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadStartMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadGameOver() {
        StartCoroutine(DelayLevelLoad());
    }

    public void LoadGameScene() {
        SceneManager.LoadScene("Level 1");
    }

    IEnumerator DelayLevelLoad() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Game Over");
    }
}

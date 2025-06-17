using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour
{
    #region Variables
    public static AsyncOperation async;
    public string levelToLoad = "";
    public float loadingDelay;
    #endregion

    #region Unity Base Methods
    void Start()
    {
        // Check the level transition controller async loading option and load the level if set to aysnc direct mode
        if (FindObjectOfType<LevelTransitionController>().loadAsyncDirect)
        {
            // Pause for the set delay time before loading scene
            StartCoroutine(LoadSceneAsync(levelToLoad));
        }
    }
    #endregion

    #region Method to call to load scene
    public void LoadScene(string _levelToLoad)
    {
        StartCoroutine(LoadSceneAsync(_levelToLoad));
    }
    #endregion

    #region Coroutines
    IEnumerator LoadSceneAsync(string sceneName)
    {

        if (FindObjectOfType<LevelTransitionController>().loadAsyncDirect)
        {
            // If async direct mode pause for the set delay time before loading scene
            yield return new WaitForSecondsRealtime(loadingDelay);
        }

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        async = SceneManager.LoadSceneAsync(sceneName);

        // Disable scene activation
        async.allowSceneActivation = false;

        // Wait until scene loaded
        yield return async;
    }
    #endregion
}

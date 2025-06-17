using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class LevelTransitions
{
    public string transitionName;
    public RuntimeAnimatorController animatorController;
}

public class LevelTransition : MonoBehaviour
{
    #region Variables
    [Header("Transitions List")]
    public List<LevelTransitions> transitionsList = new List<LevelTransitions>();

    [Header("Transitions Canvas's")]
    public Canvas simpleFade;
    public Canvas sideWipe;

    [Header("Loading Bar")]
    public Text loadingText;
    public Slider sliderBar;

    [Tooltip("Adjust this to match the loading bar value with the fade delay value." +
        "\nThe higher the fade delay, the higher the multiplier must be set.")]
    public float timeMultiplier = 1.5f;

    [Tooltip("Set this to delay the loading bar from loading instantly." +
    "\nYou will need to sync the fadeDelay & timeMultiplyer settings to get the loading bar to sync if not using the default setting.")]
    public float sliderBarDelay = 1f;

    [HideInInspector]
    public float fadeDelay;

    [HideInInspector]
    public bool useLoadingBar;

    [HideInInspector]
    public bool loadAsyncDirect;

    [HideInInspector]
    public string animationName;

    private Animator animator;
    #endregion

    #region User Methods
    public void LoadLevel(string sceneName)
    {
        // Add your extra animation here
        switch (animationName)
        {
            case "SimpleFade":
                // Set the animation game object to true
                simpleFade.gameObject.SetActive(true);
                // Get & set the runtime animator controller to use the animator controller from the animations list 
                simpleFade.GetComponent<Animator>().runtimeAnimatorController = transitionsList[0].animatorController;
                // Set this game objects animator to the animation game object animator
                animator = simpleFade.GetComponent<Animator>();
                break;

            case "SideWipe":
                sideWipe.gameObject.SetActive(true);
                sideWipe.GetComponent<Animator>().runtimeAnimatorController = transitionsList[1].animatorController;
                animator = sideWipe.GetComponent<Animator>();
                break;

            default:
                break;
        }

        if (loadAsyncDirect)
        {
            // Start the coroutine
            StartCoroutine(LoadMenuAsync());
        }
        else
        {
            StartCoroutine(LoadMenu(sceneName));
        }
    }
    #endregion

    #region Coroutines
    // Load a scene normally
    IEnumerator LoadMenu(string sceneName)
    {
        animator.SetTrigger("FadeOut");

        if (useLoadingBar)
        {
            StartCoroutine(SetslideBarValue());
        }

        // Wait for X amount of seconds before loading the next scene
        yield return new WaitForSecondsRealtime(fadeDelay);

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }

    // Load a scene Asynchronously
    IEnumerator LoadMenuAsync()
    {
        animator.SetTrigger("FadeOut");

        if (useLoadingBar)
        {
            StartCoroutine(SetslideBarValue());
        }

        yield return new WaitForSecondsRealtime(fadeDelay);

        // Activate the next scene
        LoadLevelAsync.async.allowSceneActivation = true;
    }

    // Set the fake loading bar value
    IEnumerator SetslideBarValue()
    {
        yield return new WaitForSecondsRealtime(sliderBarDelay);

        sliderBar.gameObject.SetActive(true);

        float timer = -1f;
        timer += Time.unscaledDeltaTime;

        while (timer < fadeDelay)
        {
            sliderBar.value += Time.unscaledDeltaTime / fadeDelay * timeMultiplier;

            float f = Mathf.Round(sliderBar.value * 100f);

            loadingText.text = f + "%";

            yield return null;
        }
    }
    #endregion
}

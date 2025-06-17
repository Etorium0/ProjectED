using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionController : MonoBehaviour
{
    #region Variables
    // Add your extra transition animations here
    public enum TransitionsIn
    {
        SimpleFade,
        SideWipe
    };

    public enum TransitionsOut
    {
        SimpleFade,
        SideWipe
    };

    [Header("Level Transition Object")]
    // Reference to the level transition component
    public LevelTransition levelTransition;

    [Header("Transition Settings")]
    // Delay in seconds if using as a splash screen
    public float splashDelay;

    // Delay in seconds before loading next scene
    public float fadeDelay;

    // Name of the scene you want to load
    public string sceneToLoad;

    [Header("Transition Options")]
    [Tooltip("Use this option if you want your scene to be a splash screen just set the splash delay to your required length." +
        "\nYou could also use it for cut scenes by setting the splash delay to the length of your cut scene time.")]
    public bool loadOnStart = false;

    // Used to set the tranistion we want to play using a nice dropdown box
    public TransitionsIn transitionInType;

    // Used to set the tranistion we want to play using a nice dropdown box
    public TransitionsOut transitionOutType;

    [Header("Loading Bar Option")]
    public bool useLoadingBar = false;

    [Header("Async Loading Options")]
    [Tooltip("This option will load a level asynchronously from the LoadLevelAsync script attached to the Level Transition Controller." +
        "\nIt will use the levelToLoad & loadingDelay values from the LoadLevelAsync script" +
        "\nTo use triggers to asynchronously load a level see the Async Trigger Demo scene.")]
    public bool loadAsyncDirect = false;

    // Static level transition component for loading levels via a trigger script
    private static LevelTransition _levelTransition;

    private Animator animator;
    private static Animator _animator;
    #endregion

    #region Unity Base Methods
    public void Start()
    {
        // set the private level transition to this one
        _levelTransition = levelTransition;

        SetTransitionIn();

        if (loadOnStart)
        {
            StartCoroutine(SetOutTransition());
        }
    }
    #endregion

    #region Transition Settings
    // Set your transition in settings here
    public void SetTransitionIn()
    {
        // Add your extra transition animations here
        switch (transitionInType)
        {
            // Set the level transition animation name via the enum value
            case TransitionsIn.SimpleFade:
                levelTransition.animationName = "SimpleFade";
                levelTransition.simpleFade.gameObject.SetActive(true);
                levelTransition.simpleFade.gameObject.GetComponent<Animator>().runtimeAnimatorController = levelTransition.transitionsList[0].animatorController;
                animator = levelTransition.simpleFade.gameObject.GetComponent<Animator>();
                break;

            case TransitionsIn.SideWipe:
                levelTransition.animationName = "SideWipe";
                levelTransition.sideWipe.gameObject.SetActive(true);
                levelTransition.sideWipe.gameObject.GetComponent<Animator>().runtimeAnimatorController = levelTransition.transitionsList[1].animatorController;
                animator = levelTransition.sideWipe.gameObject.GetComponent<Animator>();
                break;

            default:
                break;
        }
    }

    // Set your transition out settings here
    public void SetTransitionOut()
    {
        // Add your extra transition animations here
        switch (transitionOutType)
        {
            case TransitionsOut.SimpleFade:
                levelTransition.animationName = "SimpleFade";
                levelTransition.simpleFade.gameObject.SetActive(true);
                levelTransition.simpleFade.gameObject.GetComponent<Animator>().runtimeAnimatorController = levelTransition.transitionsList[0].animatorController;
                animator = levelTransition.simpleFade.gameObject.GetComponent<Animator>();
                break;

            case TransitionsOut.SideWipe:
                levelTransition.animationName = "SideWipe";
                levelTransition.sideWipe.gameObject.SetActive(true);
                levelTransition.sideWipe.gameObject.GetComponent<Animator>().runtimeAnimatorController = levelTransition.transitionsList[1].animatorController;
                animator = levelTransition.sideWipe.gameObject.GetComponent<Animator>();
                break;

            default:
                break;
        }
    }
    #endregion

    #region Public Method For Loading A Scene
    public void LoadScene()
    {
        SetTransitionIn();

        StartCoroutine(SetOutTransition());
    }

    public void LoadScene(string scene)
    {
        sceneToLoad = scene;

        SetTransitionIn();

        StartCoroutine(SetOutTransition());
    }
    #endregion

    #region Static Method For Triggering Level Loading Via Script
    // Set your static transition out settings here
    public static void SetStaticTransitionOut(TransitionsOut transitionsOut)
    {
        // Add your extra transition animations here
        switch (transitionsOut)
        {
            case TransitionsOut.SimpleFade:
                _levelTransition.animationName = "SimpleFade";
                _levelTransition.simpleFade.gameObject.SetActive(true);
                _levelTransition.simpleFade.gameObject.GetComponent<Animator>().runtimeAnimatorController = _levelTransition.transitionsList[0].animatorController;
                _animator = _levelTransition.simpleFade.gameObject.GetComponent<Animator>();
                break;

            case TransitionsOut.SideWipe:
                _levelTransition.animationName = "SideWipe";
                _levelTransition.sideWipe.gameObject.SetActive(true);
                _levelTransition.sideWipe.gameObject.GetComponent<Animator>().runtimeAnimatorController = _levelTransition.transitionsList[1].animatorController;
                _animator = _levelTransition.sideWipe.gameObject.GetComponent<Animator>();
                break;

            default:
                break;
        }
    }

    public static void LoadLevel(string sceneToLoad, float splashDelay, float fadeDelay, TransitionsOut tOut, bool useLoadingBar, bool loadAsyncDirect)
    {
        _levelTransition.StartCoroutine(SetStaticOutTransition(sceneToLoad, splashDelay, fadeDelay, tOut, useLoadingBar, loadAsyncDirect));
    }

    static IEnumerator SetStaticOutTransition(string sceneToLoad, float splashDelay, float fadeDelay, TransitionsOut tOut, bool useLoadingBar, bool loadAsyncDirect)
    {
        _levelTransition.fadeDelay = fadeDelay;
        _levelTransition.useLoadingBar = useLoadingBar;
        _levelTransition.loadAsyncDirect = loadAsyncDirect;

        yield return new WaitForSecondsRealtime(splashDelay);

        _levelTransition.simpleFade.gameObject.SetActive(false);
        _levelTransition.sideWipe.gameObject.SetActive(false);
        SetStaticTransitionOut(tOut);
        _levelTransition.LoadLevel(sceneToLoad);
    }
    #endregion

    #region Coroutine
    IEnumerator SetOutTransition()
    {
        // Set the fade delay
        levelTransition.fadeDelay = fadeDelay;

        // Set the loading bar status
        levelTransition.useLoadingBar = useLoadingBar;

        // Set the async mode
        levelTransition.loadAsyncDirect = loadAsyncDirect;

        // Wait for X amount of seconds before triggering the animation
        yield return new WaitForSecondsRealtime(splashDelay);

        // Deativate the canavs's
        levelTransition.simpleFade.gameObject.SetActive(false);
        levelTransition.sideWipe.gameObject.SetActive(false);

        // Set the out transition
        SetTransitionOut();

        // Load the scene
        levelTransition.LoadLevel(sceneToLoad);
    }
    #endregion
}
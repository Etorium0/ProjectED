using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncLevelLoad : MonoBehaviour
{
    // The scene name we want to load
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Load the level with no splash delay, a 3 second fade delay, the side wipe fade out transition, use the loading bar & use async direct mode.
            // Async Direct Mode will just trigger the level transition and enable the scene as we have already loaded it with the trigger script.
            LevelTransitionController.LoadLevel(sceneToLoad, 0, 3, LevelTransitionController.TransitionsOut.SideWipe, true, true);
        }
    }
}

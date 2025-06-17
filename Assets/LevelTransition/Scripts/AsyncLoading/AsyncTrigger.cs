using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncTrigger : MonoBehaviour
{
    // Scene to load
    public string sceneToLoad;

    // Bool to check if we have entered the trigger zone
    private bool levelTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // If we have not entered the trigger zone load the scene & set the level triggered boo to true so we do not load the scene again
            if (!levelTriggered)
            {
                FindObjectOfType<LoadLevelAsync>().LoadScene(sceneToLoad);
                levelTriggered = true;
            }
        }
    }
}

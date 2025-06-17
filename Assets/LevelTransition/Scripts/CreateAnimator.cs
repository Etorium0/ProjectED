using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnimator : MonoBehaviour
{
    // Creats an animator at runtime and adds it to the object this script is attached to

    void Awake()
    {
        try
        {
            // Create an new animator and set it to this game object
            Animator newAnimator = this.gameObject.AddComponent<Animator>();
            // Set the animator update mode
            newAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        catch (System.Exception)
        {
            Debug.Log("Cant create animator");
        }
    }
}

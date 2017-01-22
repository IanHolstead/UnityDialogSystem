using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public abstract class ActionCutsceneControlBaseClass : MonoBehaviour {

    Camera cameraRef;
    Camera oldCamera;

    protected void Start()
    {
        cameraRef = GetComponent<Camera>();
        gameObject.SetActive(false);
    }

    public void Play()
    {
        SetupCutscene();
    }

    public void Finish()
    {
        CleanupCutscene();
    }

    protected void SetupCutscene()
    {
        //pause game
        gameObject.SetActive(true);
        oldCamera = Camera.current;
        oldCamera.enabled = false;
    }

    protected void CleanupCutscene()
    {
        oldCamera.enabled = true;
        oldCamera = null;
        gameObject.SetActive(false);
    }
}

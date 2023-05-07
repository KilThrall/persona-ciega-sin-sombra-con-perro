using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public const string ON_SCENE_CHANGE_REQUESTED_KEY = "OnSceneChangeRequested";

    [SerializeField]
    private float fadeDuration;

    [SerializeField]
    private GameObject fadeInPrefab;
    [SerializeField]
    private GameObject fadeOutPrefab;

    [Tooltip("Scene you want to transition to when debugging. You can debug the scene change by right clicking the component and selecting \"Change scene\"")]
    [SerializeField]
    private string DebugSceneName;

    private string nextSceneName = null;

    private void Awake()
    {
        ActionsManager.SubscribeToAction(ON_SCENE_CHANGE_REQUESTED_KEY, OnSceneChangeRequested);
    }

    private void Start()
    {
        GameUIManager.Instance.InstanceGameUI(fadeInPrefab);
    }

    private void OnDestroy()
    {
        ActionsManager.UnsubscribeToAction(ON_SCENE_CHANGE_REQUESTED_KEY, OnSceneChangeRequested);
    }

    public void OnSceneChangeRequested(object sceneNameObj)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            return;
        }
        string sceneName = null;
        try
        {
            sceneName = (string)sceneNameObj;

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Tried sending an event for {nameof(PianoUI)} with the wrong type. Error: " + e);
            throw;
        }
        if (string.IsNullOrEmpty(sceneName))
        {
            return;
        }
        nextSceneName = sceneName;
        Invoke(nameof(LoadScene), fadeDuration);
        GameUIManager.Instance.InstanceGameUI(fadeOutPrefab);
        
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(nextSceneName);
        nextSceneName = null;
    }

    [ContextMenu("Change Scene")]
    private void ChangeSceneDebug()
    {
        ActionsManager.InvokeAction(ON_SCENE_CHANGE_REQUESTED_KEY, DebugSceneName);
    }
}

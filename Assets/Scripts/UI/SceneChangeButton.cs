using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Button button;

    private string sceneName;

    public void Setup(string sceneName)
    {
        this.sceneName = sceneName;
        button.onClick.AddListener(ChangeScene);
        text.text = sceneName;
    }

    private void ChangeScene()
    {
        ActionsManager.InvokeAction(SceneTransitionManager.ON_SCENE_CHANGE_REQUESTED_KEY, sceneName);
    }
}

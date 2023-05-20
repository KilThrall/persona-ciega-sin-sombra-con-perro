using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatsUI : MonoBehaviour
{
    [SerializeField]
    private bool areCheatsEnabled = true;

    [SerializeField]
    private GameObject sceneButtonPrefab;
    [SerializeField]
    private Transform scenesListTransform;

    private void Awake()
    {
        if (!areCheatsEnabled)
        {
            gameObject.SetActive(false);
            return;
        }
        SpawnScenesButtons();
    }

    public void ChangeCheatsShownState()
    {
        scenesListTransform.gameObject.SetActive(!scenesListTransform.gameObject.activeSelf);
    }

    private void SpawnScenesButtons()
    {
        var sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            var button = Instantiate(sceneButtonPrefab, scenesListTransform);
            var path = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
            button.GetComponent<SceneChangeButton>().Setup(sceneName);
        }
    }
}

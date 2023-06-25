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

    private Transform dog, blind;

    private string sceneName;

    private CheatTpData cheatData;

    private const string WERE_CHEATS_USED_KEY = "CheatsUsed";
    private const string NEW_CHEAT_LOCATION_X_KEY = "CheatLocationX";
    private const string NEW_CHEAT_LOCATION_Y_KEY = "CheatLocationY";


    public void Setup(string sceneName)
    {
        this.sceneName = sceneName;
        button.onClick.AddListener(ChangeScene);
        text.text = sceneName;
    }

    public void Setup(CheatTpData cheat, Transform dog, Transform blind)
    {
        if (PlayerPrefs.GetInt(WERE_CHEATS_USED_KEY, 0) != 0)
        {
            PlayerPrefs.SetInt(WERE_CHEATS_USED_KEY, 0);
            var newPos = new Vector3(PlayerPrefs.GetFloat(NEW_CHEAT_LOCATION_X_KEY, 0), PlayerPrefs.GetFloat(NEW_CHEAT_LOCATION_Y_KEY, 0), 0);
            dog.position = newPos;
            blind.position = newPos;
        }

        this.dog = dog;
        this.blind = blind;
        this.sceneName = cheat.SceneName;
        button.onClick.AddListener(ChangeScene);
        text.text = cheat.TpName;
        cheatData = cheat;
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(cheatData.SceneName))
        {
            PlayerPrefs.SetInt(WERE_CHEATS_USED_KEY, 1);
            PlayerPrefs.SetFloat(NEW_CHEAT_LOCATION_X_KEY, cheatData.Location.x);
            PlayerPrefs.SetFloat(NEW_CHEAT_LOCATION_Y_KEY, cheatData.Location.y);
        }
        ActionsManager.InvokeAction(SceneTransitionManager.ON_SCENE_CHANGE_REQUESTED_KEY, sceneName);
    }
}

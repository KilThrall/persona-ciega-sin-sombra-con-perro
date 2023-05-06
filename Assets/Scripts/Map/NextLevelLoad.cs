using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevelLoad : MonoBehaviour
{
    public void NextLevel()
    {
        ActionsManager.InvokeAction(SceneTransitionManager.ON_SCENE_CHANGE_REQUESTED_KEY, (SceneManager.GetActiveScene().buildIndex + 1).ToString());
    }
}

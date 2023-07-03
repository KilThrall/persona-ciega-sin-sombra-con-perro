using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    private const string MAIN_MENU_SCENE_NAME = "Main Menu";
    public void ReturnToMainMenuMethod()
    {
        ActionsManager.InvokeAction(SceneTransitionManager.ON_SCENE_CHANGE_REQUESTED_KEY, MAIN_MENU_SCENE_NAME);
    }
}

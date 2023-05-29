using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [SerializeField]
    private Transform uiParent;

    [SerializeField]
    private AudioSource audioSource;

    private List<string> instancedUIs;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Tried instantiating a new UI Manager but UI Manager is already instantiated");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void InstanceGameUI(GameObject prefab)
    {
        if (isUIInstanced(prefab.name))
        {
            return;
        }
        Instantiate(prefab, uiParent);
        instancedUIs.Add(prefab.name);
    }

    public void PlaySoundOneShot(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError($"Tried to play sound but no Audio source is atached to {gameObject.name}");
            return;
        }
        audioSource.PlayOneShot(clip);
    }

    private bool isUIInstanced(string name)
    {
        if (instancedUIs == null)
        {
            instancedUIs = new List<string>();
            return false;
        }

        foreach (var ui in instancedUIs)
        {
            if (string.Equals(name, ui))
            {
                return true;
            }
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCandles : MonoBehaviour
{
    public bool isLit;
    CharacterController charC;
    [SerializeField] GameObject fire;
    [SerializeField] string[] scenes;
    [SerializeField] float levelChangeDelay;
    [SerializeField] int currentScene;
    int maxScene;

    private void Start()
    {
        if (isLit)
            fire.SetActive(true);
        else
            fire.SetActive(false);
    }
    public void LightUp()
    {
        isLit = true;
        fire.SetActive(true);
        StartCoroutine("LevelChange");
    }

    IEnumerator LevelChange()
    {
        yield return new WaitForSeconds(levelChangeDelay);
        if (currentScene == scenes.Length - 1)
            currentScene = 0;
        else
            currentScene++;
        SceneManager.LoadScene(scenes[currentScene]);
    }
}

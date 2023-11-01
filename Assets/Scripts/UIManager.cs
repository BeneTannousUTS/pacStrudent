using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            GameObject ghostTimer = GameObject.FindGameObjectWithTag("ScaredGhostTimer");
            ghostTimer.SetActive(true);
        }
    }
    
    public void LoadLevelOne()
    {
        // Prevent the UIManager from being destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        // Load the WalkingScene by its name or scene build index
        SceneManager.LoadScene("LevelOne");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == SceneManager.GetSceneByName("LevelOne").buildIndex)
        {
            // Find the GameObject with the tag "QuitButton"
            GameObject quitButton = GameObject.FindGameObjectWithTag("ExitButton");

            Button button = quitButton.GetComponent<Button>();

            // Add the QuitGame method as a listener to the button's onClick event
            button.onClick.AddListener(LoadMenu);

            GameObject ghostTimer = GameObject.FindGameObjectWithTag("ScaredGhostTimer");
            if(ghostTimer != null)
            {
                ghostTimer.SetActive(false);
            }
        }
    }
}

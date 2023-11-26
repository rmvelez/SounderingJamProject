using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class pauseMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;

    private GameManager gameManager;

    private VisualElement root;
    private Button resumeButton;
    private Button mainMenuButton;
    private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        root = uiDoc.rootVisualElement;
        resumeButton = root.Q<Button>("Resume");
        mainMenuButton = root.Q<Button>("MainMenu");
        quitButton = root.Q<Button>("Quit");

        root.visible = false;

        gameManager.onGamePause.AddListener(Pause);
        gameManager.onGameResume.AddListener(Resume);

        resumeButton.clicked += StartResume;
        mainMenuButton.clicked += GoToMainMenu;
        quitButton.clicked += () => { Application.Quit();  };
    }

    private void Pause()
    {
        root.visible = true;
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void StartResume()
    {
        gameManager.ResumeGame();
    }

    private void Resume()
    {
        root.visible = false;
    }
}

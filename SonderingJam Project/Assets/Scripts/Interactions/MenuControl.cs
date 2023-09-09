using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    // a list of the avaliable states that the menu can be in
    public enum GameState {MenuState,LearnState,CreditState}
    // reference to the current game state, or menu, that the user is on
    public GameState currentGameState;

    // reference to the main menu
    public GameObject menu;

    // reference to the instructions screen
    public GameObject learn;

    // reference to the credits screen
    public GameObject credit;

    // Start is called before the first frame update
    void Start()
    {
        // when the game starts, the player will see the main menu first
        currentGameState = GameState.MenuState;
        ShowScreen(menu);
    }

    // this function is called on the start button
    // will start the game
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // this function is called on the back button in either the instruction or credits screen
    public void MainMenu()
    {
        // sets the screen to the main menu
        currentGameState = GameState.MenuState;
        ShowScreen(menu);
    }

    // this function is called by the learn button on the main menu screen
    public void InstructionsScreen()
    {
        // shows the instructions screen
        currentGameState = GameState.LearnState;
        ShowScreen(learn);
    }

    // this function is called by the credits button on the main menu screen
    public void CreditsScreen()
    {
        // shows the credis screen
        currentGameState = GameState.CreditState;
        ShowScreen(credit);
    }

    // used to determine which screen to show based on the current game state
    private void ShowScreen(GameObject gameObjectToShow)
    {
        menu.SetActive(false);
        learn.SetActive(false);
        credit.SetActive(false);

        gameObjectToShow.SetActive(true);
    }
}

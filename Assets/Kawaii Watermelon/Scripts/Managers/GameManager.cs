using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header(" Settings ")]
    private GameState gameState;

    [Header(" Actions ")]
    public static Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetMenu()
    {
        SetGameState(GameState.Menu);
    }

    private void SetGame()
    {
        SetGameState(GameState.Game);
    }

    private void SetGameover()
    {
        SetGameState(GameState.Gameover);
    }

    private void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState()
    {
        SetGame();
    }

    public bool IsGameState()
    {
        return gameState == GameState.Game;
    }

    public void SetGameoverState()
    {
        SetGameover();
    }
}
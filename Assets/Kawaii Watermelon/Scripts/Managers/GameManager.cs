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

    void Start()
    {
        SetMenu();
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

    // YENİ EKLENDİ BAŞLANGIÇ
    /// <summary>
    /// Belirtilen saniye kadar bekledikten sonra oyunu bitirir.
    /// FruitManager tarafından çağrılır.
    /// </summary>
    /// <param name="delay">Saniye cinsinden bekleme süresi.</param>
    public void EndGameAfterDelay(float delay)
    {
        // Eğer oyun zaten bitmişse tekrar bitirme.
        if (gameState == GameState.Gameover)
            return;

        StartCoroutine(GameoverCoroutine(delay));
    }

    private IEnumerator GameoverCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetGameoverState();
    }
    // YENİ EKLENDİ SON
}
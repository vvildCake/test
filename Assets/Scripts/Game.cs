using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    
    IGameInput input;
    PlayerController player;

    bool isPaused; 
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        input = new GameInputSimpleKeyboard();
        AssignPlayer();
    }

    void AssignPlayer()
    {
        player = FindObjectOfType<PlayerController>();
        if(player == null)
            throw new Exception($"Couldn't find a player object on this level. Are you sure it exists in the scene {SceneManager.GetActiveScene().name}");
    }

    void Update()
    {
        if(input.IsPausePressed())
            TogglePause();

        if(isPaused)
            return;
        
        player.Move(input.GetMovementDirection(), input.IsJumpPressed(), input.IsCrouchPressed());
    }

    void TogglePause()
    {
        SetPause(!isPaused);
    }

    void SetPause(bool pause)
    {
        Time.timeScale = pause ? 0.0f : 1.0f;
        isPaused = pause;
    }
}

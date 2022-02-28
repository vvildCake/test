using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    IGameInput input;
    public PlayerController Player;
    public Camera Cam;
    public float PlayerTrackingMinX = -3.0f;

    bool isPaused;
    float camDeltaX;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        input = new GameInputSimpleKeyboard();
        AssignPlayer();
        AssignCamera();
    }

    void AssignPlayer()
    {
        if(Player == null)
            Player = FindObjectOfType<PlayerController>();
        if(Player == null)
            ErrorMissingComponent("player");
    }

    void AssignCamera()
    {
        if(Cam == null)
            Cam = Camera.main;
        if(Cam == null)
            ErrorMissingComponent("camera");

        camDeltaX = Cam.transform.position.x - PlayerTrackingMinX;
    }

    void ErrorMissingComponent(string label)
    {
        throw new Exception($"Couldn't find a {label} object on this level. Are you sure it exists in the scene {SceneManager.GetActiveScene().name}");
    }

    void Update()
    {
        if(input.IsPausePressed())
            TogglePause();

        if(isPaused)
            return;
        
        Player.Move(input.GetMovementDirection(), input.IsJumpPressed(), input.IsCrouchPressed());
    }

    //Making camera trail the player in LateUpdate because player's new position is ready by then
    void LateUpdate()
    {
        CameraUpdateTrailing();
    }

    void CameraUpdateTrailing()
    {
        float playerX = Player.transform.position.x;
        if(playerX < PlayerTrackingMinX)
            return;
            
        var camTfm = Cam.transform;
        var camPos = camTfm.position;

        camPos.x = playerX + camDeltaX;

        camTfm.position = camPos;
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

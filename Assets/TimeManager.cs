using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Slider timeBar;
    public PlayerController _player;
    [Range(0.0f, 10.0f)]
    public float _fTime = 10.0f;

    public float _fTimeDelta = 0.5f;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    public void Update()
    {
        timeBar.value = _fTime;
        _fTime -= _fTimeDelta * Time.deltaTime;
        if (_fTime <= 0.0f)
            _player.Health -= 600;       
    }

}

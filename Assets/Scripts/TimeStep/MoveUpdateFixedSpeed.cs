using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpdateFixedSpeed : MonoBehaviour
{
    public float Speed = 1f;
    void Update()
    {
        transform.position += Vector3.right * Speed * Time.deltaTime;
    }
}

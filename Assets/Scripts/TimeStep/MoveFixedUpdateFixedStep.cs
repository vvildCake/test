using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFixedUpdateFixedStep : MonoBehaviour
{
    public float Step = 0.01f;

    void FixedUpdate()
    {
        transform.position += Vector3.right * Step;
    }
}

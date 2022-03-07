using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpdateFixedStep : MonoBehaviour
{
    public float Step = 0.01f;

    void Update()
    {
        transform.position += Vector3.right * Step;
    }
}

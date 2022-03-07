using UnityEngine;

public class MoveFixedUpdateFixedSpeed : MonoBehaviour
{
    public float Speed = 1f;

    void FixedUpdate()
    {
        transform.position += Vector3.right * Speed * Time.fixedDeltaTime;
    }
}

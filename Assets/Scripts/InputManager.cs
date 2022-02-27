using UnityEngine;

//A high-level interface hides implementation details,
//making it easy to swap to another input system/device without affecting the game logic
public interface IGameInput
{
  //System actions
  bool IsPausePressed();
  
  //Player controls
  bool IsJumpPressed();
  bool IsCrouchPressed();
  float GetMovementDirection();
}

public class GameInputSimpleKeyboard : IGameInput
{
  public bool IsPausePressed()
  {
    return Input.GetKeyDown(KeyCode.Escape);
  }

  public bool IsJumpPressed()
  {
    return Input.GetKeyDown(KeyCode.Space);
  }

  public bool IsCrouchPressed()
  {
    return Input.GetKeyDown(KeyCode.LeftControl);
  }

  public float GetMovementDirection()
  {
    if(Input.GetKey(KeyCode.RightArrow))
      return +1.0f;
    
    if(Input.GetKey(KeyCode.LeftArrow))
      return -1.0f;

    return 0.0f;
  }
}
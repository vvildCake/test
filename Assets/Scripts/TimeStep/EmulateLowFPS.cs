using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class EmulateLowFPS : MonoBehaviour
{
  public bool LowFrameRate = false;
  public Text FrameLabel;

  void Update()
  {
    FrameLabel.text = $"Frame: {Time.frameCount}";
    if(LowFrameRate)
      Thread.Sleep(100);
  }
}
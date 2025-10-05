using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanvasUI : MonoBehaviour
{
    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void StartTime()
    {
        Time.timeScale = 1;
    }
}

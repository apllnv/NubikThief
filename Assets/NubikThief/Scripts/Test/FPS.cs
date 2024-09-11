using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] private int _fpsRate;

    private void OnValidate()
    {
        Application.targetFrameRate = _fpsRate;
    }
}

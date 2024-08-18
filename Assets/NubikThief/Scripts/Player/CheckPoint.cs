using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private CheckSide checkSide;

    public CheckSide GetCheckSide()
    {
        return checkSide;
    }
}

public enum CheckSide
{
    Left,
    Right,
    Bottom,
    Top
}

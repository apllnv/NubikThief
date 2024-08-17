using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckCollider : MonoBehaviour
{
    [SerializeField]
    private CheckSide checkSide;

    public CheckSide GerCheckSide()
    {
        return checkSide;
    }
}

public enum CheckSide
{
    Left,
    Right,
    Ground
}

using System;
using JN.Chess;
using UnityEngine;

public class BoardInputHandler : MonoBehaviour, IInputHandler
{
    public BoardGame board;
    
    public void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action callback)
    {
        board.OnSquareSelected(inputPosition);   
    }
}
using System;
using UnityEngine;

public interface IPiece
{
    GameObject gameObject { get; }

    void AddListenerOnPieceSelected(Action<IPiece> action);
    void RemoveListenerOnPieceSelected(Action<IPiece> action);
}

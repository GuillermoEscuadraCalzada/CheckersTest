using System;
using UnityEngine;

public interface IPlayer
{
    public GameObject gameObject { get; }
    // Turn Start
    void AddListenerOnPlayerTurnStart(Action action);
    void RemoveListenerOnPlayerTurnStart(Action action);
    // Turn End
    void AddListenerOnPlayerTurnEnd(Action action);
    void RemoveListenerOnPlayerTurnEnd(Action action);
    // Cancel Play
    void AddListenerOnCancelPlay(Action action);
    void RemoveListenerOnCancelPlay(Action action);
    // Select Piece
    void AddListenerOnPieceSelected(Action<IPiece> action);
    void RemoveListenerOnPieceSelected(Action<IPiece> action);

    void StartTurn();
    void EndTurn();
}
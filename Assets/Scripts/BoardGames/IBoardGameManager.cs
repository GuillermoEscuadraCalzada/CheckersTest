using UnityEngine;

public interface IBoardGameManager
{
    void StartGame();
    void EndGame();
    void StartTurn(IPlayer player);
    void SelectPiece(IPiece piece);
}

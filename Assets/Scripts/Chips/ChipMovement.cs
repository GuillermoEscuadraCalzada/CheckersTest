using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public struct ChipPosition
    { 

        public Vector2Int PositionInBoard { get; set; }
        public Vector2Int Upper_Left(Vector2Int position = new Vector2Int()) =>
            position == Vector2Int.zero ? new Vector2Int(PositionInBoard.x - 1, PositionInBoard.y - 1) 
            : new Vector2Int(position.x - 1, position.y - 1);
        
        public Vector2Int Upper_Right(Vector2Int position = new Vector2Int()) 
            => position == Vector2Int.zero ? new Vector2Int(PositionInBoard.x - 1, PositionInBoard.y + 1)
            : new Vector2Int(position.x - 1, position.y + 1);
        public Vector2Int Lower_Left(Vector2Int position = new Vector2Int()) =>
            position == Vector2Int.zero ? new Vector2Int(PositionInBoard.x + 1, PositionInBoard.y - 1) 
            : new Vector2Int(position.x + 1, position.y - 1);
        public Vector2Int Lower_Right(Vector2Int position = new Vector2Int()) =>
            position == Vector2Int.zero ? new Vector2Int(PositionInBoard.x + 1, PositionInBoard.y + 1) 
            : new Vector2Int(position.x + 1, position.y + 1);
    }

}
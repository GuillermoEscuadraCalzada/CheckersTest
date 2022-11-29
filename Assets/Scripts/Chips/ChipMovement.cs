using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public struct ChipPosition
    { 
        /// <summary>
        /// The position of the chip in the board
        /// </summary>
        public Vector2Int PositionInBoard { get; set; }


        /// Gets the upper left coordinates of indicated vector2 position
        public static Vector2Int GetChipUpperLeftTile(Vector2Int chipPosition) => new(chipPosition.x - 1, chipPosition.y - 1);

        /// Gets the upper right coordinates of indicated vector2 position
        public static Vector2Int GetChipUpperRightTile(Vector2Int chipPosition) => new(chipPosition.x - 1, chipPosition.y + 1);
        
        /// Gets the lower left coordinates of indicated vector2 position
        public static Vector2Int GetChipLowerLeftTile(Vector2Int chipPosition) => new(chipPosition.x + 1, chipPosition.y - 1);

        /// Gets the lower right coordinates of indicated vector2 position
        public static Vector2Int GetChipLowerRightTile(Vector2Int chipPosition) => new(chipPosition.x + 1, chipPosition.y + 1);

        //Gets the upper left position of the chip position
        public Vector2Int Upper_Left() => new(PositionInBoard.x - 1, PositionInBoard.y - 1);
        
        //Gets the upper left position of the chip position
        public Vector2Int Upper_Right() => new (PositionInBoard.x - 1, PositionInBoard.y + 1);

        //Gets the upper left position of the chip position
        public Vector2Int Lower_Left() => new (PositionInBoard.x + 1, PositionInBoard.y - 1);

        //Gets the upper left position of the chip position
        public Vector2Int Lower_Right() => new (PositionInBoard.x + 1, PositionInBoard.y + 1);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public enum PlayerNumber { NONE, ONE, TWO };
    public class Player : MonoBehaviour
    {
        private const int MaxPlayerChips = 12;

        [SerializeField] PlayerNumber playerNumber;

        public List<Chip> playerChips = new List<Chip>(MaxPlayerChips);
        public PlayerNumber PlayerNumber => playerNumber;
    }
}
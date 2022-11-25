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

        public List<Chip> playerChips = new(MaxPlayerChips);
        public Chip SelectedChip { get; set; } = null;
        public int PlayerNumber => (int)playerNumber;

        private void Start()
        {
            ToggleMobilityOfChips();
        }

        public void ToggleMobilityOfChips(Chip chip = null, bool toggleState = true)
        {
            foreach(Chip chip2 in playerChips)
            {
                if (chip != null && chip2 == chip) continue;
                chip2.CanMove = toggleState;
            }
        }

    }
}

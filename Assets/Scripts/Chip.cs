using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public enum Chip_Color
    {
        WHITE,
        BLACK
    }
    public class Chip : MonoBehaviour
    {
        public Chip_Color checkerColor; //El color de la ficha
        [SerializeField] bool isChecker; //Si es una dama o no
        [SerializeField] Player chipPlayer;
        
        /*Hacer sistema para que los tiles tengan su posici�n en el tablero
        *Crear un arreglo de dos dimensiones donde se guardar�n temporalmente los transform de los hijos de las filas.
            *As� se les almacenar� un n�mero para su vector2Int a las casillas
        *Al entrar al trigger, se llamar� a un evento que hablar� con las variables del tablero, fichas y casillas para actualizar su informaci�n.       
        */

        public bool IsChecker { get =>isChecker; set => isChecker = value; }

        public int ChipValueOnBoard => (int)chipPlayer.PlayerNumber;


        private void Awake()
        {
            chipPlayer.playerChips.Add(this);
        }

        public void EvolveFromChipToChecker(bool isEndline, PlayerNumber playerNumber)
        {
            if (isEndline && playerNumber != chipPlayer.PlayerNumber)
                IsChecker = true;
            
        }


    }
}

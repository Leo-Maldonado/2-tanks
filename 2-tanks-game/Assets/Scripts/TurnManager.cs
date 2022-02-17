using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private enum PlayersTurn { Tank1, Tank2 };

    private PlayersTurn turn = PlayersTurn.Tank1;

    // Returns whether it is the given tank's turn (1 represents tank 1 and 2 represents tank 2)
    // -> used ints to avoid weird string errors and annoying syntax of using the enum values
    public bool IsPlayerTurn(int tank)
    {
        if (tank == 1)
        {
            return turn == PlayersTurn.Tank1;
        }
        else
        {
            return turn == PlayersTurn.Tank2;
        }
    }

    // Change turns
    public void ChangeTurns()
    {
        if (turn == PlayersTurn.Tank1)
        {
            turn = PlayersTurn.Tank2;
        }
        else
        {
            turn = PlayersTurn.Tank1;
        }
    }
}

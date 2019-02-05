using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public int GetRowZ(Rows row)
    {
        return CELL_ZS[(int)row];
    }

    public List<int> GetRowXs()
    {
        return CELL_XS;
    }

    public float GetHeight()
    {
        return HEIGHT;
    }

    #region private

    const float HEIGHT = 1f;

    List<int> CELL_XS = new List<int>() { -35, -25, -15, -5, 5, 15, 25, 35 };
    List<int> CELL_ZS = new List<int>() { -35, -25, -15, -5, 5, 15, 25, 35 };

    #endregion
}

public enum Rows
{
    FIRST_ROW,
    SECOND_ROW,
    THIRD_ROW,
    FOURTH_ROW,
    FIFTH_ROW,
    SIXTH_ROW,
    SEVENTH_ROW,
    EIGHTH_ROW
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    [SerializeField] Room room;
    [SerializeField] List<Puzzle> pieces = new List<Puzzle>();

    public void Check()
    {
        bool allActive = true;
        foreach (Puzzle piece in pieces)
        {
            if (!piece.active)
            {
                allActive = false;
            }
        }

        if (allActive)
        {
            room.ReduceObjectives();
        }
    }

}

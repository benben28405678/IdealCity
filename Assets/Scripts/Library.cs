using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library
{
    public static (string, BuildingSize)[] buildingSizes = {
        ("Recharging_Complex", BuildingSize._1x1),
        ("Solar_Energy_Plant", BuildingSize._1x1),
        ("Purifier", BuildingSize._1x2)
    };

    // All possible building sizes, in the format _XxZ.
    public enum BuildingSize
    {
        _1x1, _1x2, _2x2, _3x3
    }

    public static BuildingSize sizeForBuilding(string building)
    {
        foreach((string, BuildingSize) tuplet in buildingSizes)
        {
            if (tuplet.Item1 == building) return tuplet.Item2;
        }

        return BuildingSize._1x1;
    }
}
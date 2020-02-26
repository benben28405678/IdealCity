using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library
{
    public static (string, BuildingSize)[] buildingSizes = {
        ("Recharging_Complex", BuildingSize._1x1),
        ("Solar_Energy_Plant", BuildingSize._1x1),
        ("Purifier", BuildingSize._2x1)
    };

    // All possible building sizes, in the format _XxZ.
    public enum BuildingSize
    {
        _1x1, _1x2, _2x1, _2x2, _3x3
    }
}
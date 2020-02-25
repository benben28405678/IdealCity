﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public enum GraphicsLevel
    {
        LOW, MEDIUM, HIGH
    }

    public GameObject grid;
    public GridAnimation gridAnimation;
    public MonoBehaviour postProcessing;

    public GraphicsLevel graphicsLevel = GraphicsLevel.HIGH;

    // Start is called before the first frame update
    void Start()
    {
        settingsChanged();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void settingsChanged()
    {
        Material toSet = gridAnimation.lowDefMaterial;

        if ((int)graphicsLevel > 0)
        {
            toSet = gridAnimation.material;
        }

        if ((int)graphicsLevel > 1)
        {
            postProcessing.enabled = true;
        }

        for (int i = 0; i < grid.transform.childCount; i++)
        {
            grid.transform.GetChild(i).GetComponent<MeshRenderer>().material = toSet;
        }
    }
}

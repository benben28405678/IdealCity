using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAnimation : MonoBehaviour
{

    /*
     * This script is in charge of managing the animation of the grid. When Build mode is entered, the grid appears. When it's exited, the grid disappears.
     */

    public Material material; // The animation material of the grid
    public Material lowDefMaterial; // The solid material of the grid
    public StateManager manager; // A reference to the StateManager script in the State Manager GameObject

    private float animationTime = 0.0f; // A counter for the time of the animation

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(5.0f, 2.0f * Mathf.Sin(animationTime), 5.0f); // Set the grids position to below the ground when animationTime is 0 and move above the ground when animationTime increases
    }

    private void LateUpdate()
    {
        // Increase animationTime when build mode is on
        if(!manager.isBuilding && animationTime < 3*Mathf.PI/2.0f)
        {
            animationTime += Time.deltaTime * 12.0f;
        }

        // Decrease animationTime when build mode is off
        if (manager.isBuilding && animationTime > 0.1f)
        {
            animationTime -= Time.deltaTime * 12.0f;
        }

        // Make sure animationTime is not negative
        if (animationTime < 0.1f) animationTime = 0.1f;
    }
}

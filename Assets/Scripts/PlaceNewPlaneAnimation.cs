using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceNewPlaneAnimation : MonoBehaviour
{
    /*
     * This script is in charge of the plane that animates under the current building in Build Mode.
     */

    public Material thisMaterial; // The reference to the plane's material
    public StateManager stateManager; // The State Manager

    void LateUpdate()
    {
        
        // Don't bother running this script if we're not in Build Mode.
        if(!stateManager.isBuilding) return;

        float s = 0.95f + 0.05f * Mathf.Abs(Mathf.Sin(3.0f * Time.fixedTime)); // Scale the object over time

        // Animate the color blue and green
        thisMaterial.color = new Color(0.0f, 1.0f, 0.5f + 0.5f * Mathf.Cos(Time.fixedTime));

        // If we're placing on top of a building, animate the color red and yellow
        if (transform.localPosition.y < -4.0f)
        {
            thisMaterial.color = new Color(1.0f, 0.5f + 0.5f * Mathf.Cos(Time.fixedTime), 0.0f);
        }

        Library.BuildingSize size = Library.sizeForBuilding(stateManager.currentlyPlacingName);
        if (size == Library.BuildingSize._1x1) transform.localScale = new Vector3(s, s, s);
        else if (size == Library.BuildingSize._1x2) transform.localScale = new Vector3(s, s, 2.0f * s);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceNew : MonoBehaviour
{
    /*
     * This script manages the new building being placed when in Build Mode.
     * To enable Build Mode, find the State Manager GameObject in the Hierarchy and check the Is Building checkbox. It's TRUE by default when first running the game.
     * Note: The psuedocode references something called a 'current building'. This is the building that is floating and you're finding a spot for it to go.
     */

    public StateManager manager; // A reference to the State Manager
    public GameObject building; // A reference to the building that hovers when you're finding a spot to place it down
    public GameObject mapParentNode; // A reference to the MapElements GameObject. This is how we see where other buildings are, so we can make them red if trying to place on top of them.
    public GameObject plane; // The color-changing plane that is below the building you're currently placing
    public Material hologramMaterial; // A reference to the hologram material that appears on buildings under construction.
    public ParticleSystem particles; // The particles that appear when a building is placed
    public ParticleSystem destroyParticles; // The particles that appear when a building is destroyed due to a placing on top.

    private Vector3 newCloneCenter = new Vector3(); // A position variable for the building clone once a building is placed.
    private GameObject newClone; // The clone building.

    private void Start()
    {
        // Disable particles.
        particles.gameObject.SetActive(false);
        destroyParticles.gameObject.SetActive(false);

        // Choose the type of building to place. For instance, "Solar_Energy_Plant". It's referenced from the State Manager.
        updateBuildingPlacing();
    }

    public void updateBuildingPlacing()
    {
        GameObject go = Resources.Load<GameObject>("Meshes/CityBuildings/Models/" + manager.currentlyPlacingName); // Load the building's model
        Mesh newMesh = go.GetComponent<MeshFilter>().sharedMesh; // Find the model's mesh
        Material newMaterial = go.GetComponent<MeshRenderer>().sharedMaterial; // Find the model's material
        building.GetComponent<MeshFilter>().mesh = newMesh; // Set the current building's mesh to the model's mesh
        building.GetComponent<MeshRenderer>().material = newMaterial; // Set the current building's material to the model's material
    }

    void Update()
    {
        /*
         * We use a Raycast to find the mouse's position in a 3D world.
         */

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Hide all of the current building objects if we're not in Build Mode
        building.GetComponent<MeshRenderer>().enabled = manager.isBuilding;
        plane.GetComponent<MeshRenderer>().enabled = manager.isBuilding;

        // If we're not in build mode, stop here.
        if (!manager.isBuilding) return;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Environment") // Only raytrace objects with the "Environment" tag, i.e. the ground.
            {
                Vector3 destination = hit.point; // This is the point in 3D space where the mouse is. It's going to be the point where the current building will be in space.
                Vector3 planeDestination = new Vector3(0, 0, 0);

                building.transform.localPosition = new Vector3(0.0f, 1.1f + 0.5f * Mathf.Sin(Time.fixedTime * 3.0f), 0.0f); // Hover the building up and down

                // Snap the x position to a multiple of 10 per the grid
                destination.x /= 10.0f;
                destination.x = Mathf.Round(destination.x);
                destination.x *= 10.0f;

                // Snap the z position to a multiple of 10 per the grid
                destination.z /= 10.0f;
                destination.z = Mathf.Round(destination.z);
                destination.z *= 10.0f;

                destination.y = 0.0f;

                // Check all of the other buildings to see if we're placing something on top
                for (int i = 0; i < mapParentNode.transform.childCount; i++)
                {
                    Transform child = mapParentNode.transform.GetChild(i); // Get each child from the Map Parent Node

                    // Check to see if the positions are close enough
                    if (Mathf.Abs(child.position.x - transform.position.x) < 5.0f && Mathf.Abs(child.position.z - transform.position.z) < 5.0f && !Input.GetMouseButton(0))
                    {
                        mapParentNode.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.red; // Make the already existing building red if we're trying to place on top

                        destination.y = 10.0f; // Move the currently placing building up if we're placing on top
                        planeDestination = new Vector3(0, -9.0f, 0); // Move the plane's local position down to equalize with the building moving up
                    }
                    else
                    {
                        // Return the material of the already existing building to normal if we're not placing on top
                        mapParentNode.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = new Color(0.71f, 0.71f, 0.71f);
                    }
                }

                // Smoothly move the building's transform position to the destination position for animation purposes
                transform.position += (destination - transform.position) / 4.0f;
                // Again, smoothly move the plane as well
                plane.transform.localPosition += (planeDestination - plane.transform.localPosition) / 4.0f;

                // Rotate the current building ever so slightly
                building.transform.localRotation = new Quaternion(0.02f * (transform.position.z - destination.z) + 0.02f * Mathf.Sin(Time.fixedTime), 0.0f, 0.02f * (transform.position.x - destination.x) + 0.02f * Mathf.Cos(Time.fixedTime), 1.0f);

                // The script that manages what happens when the mouse is pressed.
                if (Input.GetMouseButtonDown(0))
                {
                    newClone = Instantiate(building); // Make a clone of the building
                    newClone.transform.position = new Vector3(destination.x, 0.0f, destination.z); // Set the clone's position to the position of the current building, but on the ground.
                    newClone.transform.rotation = new Quaternion(); // Reset the rotation of the clone
                    newClone.transform.SetParent(mapParentNode.transform); // Make the clone a child of MapElements

                    newCloneCenter = newClone.transform.localPosition; // Make sure the local position of the clone isn't screwed up

                    // Now we need to check to see if we placed the clone on top of a preexisting building. We need to destroy the old one if so.
                    for (int i = 0; i < mapParentNode.transform.childCount - 1; i++)
                    {
                        Transform child = mapParentNode.transform.GetChild(i);

                        if (Mathf.Abs(child.position.x - newClone.transform.position.x) < 5.0f && Mathf.Abs(child.position.z - newClone.transform.position.z) < 5.0f)
                        {
                            // Make destroy particles when we remove an old building
                            destroyParticles.gameObject.SetActive(true);
                            destroyParticles.transform.position = child.position;
                            destroyParticles.transform.position += new Vector3(0, 3.0f, 0);
                            destroyParticles.time = 0.0f;
                            destroyParticles.Play();

                            Destroy(child.gameObject); // Destroy the old building
                        }

                    }

                }

                // While clicking and holding, you can choose the rotation of the clone building by moving the mouse around.
                if (Input.GetMouseButton(0))
                {
                    // Hide the current building and plane when the mouse is down
                    building.GetComponent<MeshRenderer>().enabled = false;
                    plane.GetComponent<MeshRenderer>().enabled = false;

                    // Only rotate the building if the mouse is 5 units away from the clone. This is to prevent glitchy angles.
                    if ((hit.point - newCloneCenter).magnitude > 5.0f)
                    {
                        // Calculate the angle between the clone's facing position vector and the mouse position vector. Big maths.
                        float theta = Vector3.Angle(new Vector3(newCloneCenter.x - hit.point.x, 0.0f, newCloneCenter.z - hit.point.z), Vector3.left);

                        if (newCloneCenter.z - hit.point.z < 0.0f)
                        {
                            theta *= -1;
                        }

                        // Snap the theta value to a multiple of 90. This is done manually with IF statements.

                        float snapTheta = 180.0f;

                        if (Mathf.Abs(90.0f - theta) < 45.0f)
                        {
                            snapTheta = 270.0f;
                        }

                        if (Mathf.Abs(-90.0f - theta) < 45.0f)
                        {
                            snapTheta = 90.0f;
                        }

                        if (Mathf.Abs(180.0f - theta) < 45.0f || Mathf.Abs(-180.0f - theta) < 45.0f)
                        {
                            snapTheta = 0.0f;
                        }

                        // Animate the rotation of the clone's rotation
                        if ((snapTheta - newClone.transform.rotation.eulerAngles.y) > -180.0f)
                        {
                            newClone.transform.rotation = Quaternion.Euler(0.0f, newClone.transform.rotation.eulerAngles.y + (snapTheta - newClone.transform.rotation.eulerAngles.y) / 4.0f, 0.0f);
                        }
                        else
                        {
                            newClone.transform.localRotation = Quaternion.Euler(0.0f, newClone.transform.localRotation.eulerAngles.y - (snapTheta - newClone.transform.rotation.eulerAngles.y) / 4.0f, 0.0f);
                        }

                        Debug.DrawLine(newCloneCenter, hit.point, Color.red);
                        Debug.DrawLine(newCloneCenter, newCloneCenter + 100.0f * Vector3.left, Color.green);
                    }
                }

                // Place the clone when the mouse is released
                if (Input.GetMouseButtonUp(0))
                {
                    float y = newClone.transform.rotation.eulerAngles.y;

                    // Ensure the clone's rotation is a multiple of 90
                    newClone.transform.rotation = Quaternion.Euler(0.0f, y + 45.0f - (y + 45.0f) % 90.0f, 0.0f);
                    newClone.GetComponent<MeshRenderer>().material = hologramMaterial; // Make the clone have a hologram material.

                    manager.isBuilding = false; // Exit build mode
                    building.GetComponent<MeshRenderer>().enabled = false; // Hide the current building
                    plane.GetComponent<MeshRenderer>().enabled = false; // Hide the plane

                    // Create some cool blue particles
                    particles.gameObject.SetActive(true);
                    particles.transform.position = newClone.transform.position;
                    particles.transform.position += new Vector3(0, 3.0f, 0);
                    particles.time = 0.0f;
                    particles.Play();
                }
            }
        }
    }
}

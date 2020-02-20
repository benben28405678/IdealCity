using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceNew : MonoBehaviour
{
    public StateManager manager;
    public GameObject building;
    public GameObject mapParentNode;
    public GameObject plane;
    public Material hologramMaterial;
    public ParticleSystem particles;
    public ParticleSystem destroyParticles;

    private Vector3 newCloneCenter = new Vector3();
    private GameObject newClone;
    private Material holdMaterial;

    private void Start()
    {
        particles.gameObject.SetActive(false);
    }

    void Update()
    {
        //gameObject.SetActive(manager.isBuilding);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        building.GetComponent<MeshRenderer>().enabled = manager.isBuilding;
        plane.GetComponent<MeshRenderer>().enabled = manager.isBuilding;

        if (!manager.isBuilding)
        {
            return;
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Environment")
            {
                Vector3 destination = hit.point;
                Vector3 planeDestination = new Vector3(0, 0, 0);

                building.transform.localPosition = new Vector3(0.0f, 1.1f + 0.5f * Mathf.Sin(Time.fixedTime * 3.0f), 0.0f);

                destination.x /= 10.0f;
                destination.x = Mathf.Round(destination.x);
                destination.x *= 10.0f;

                destination.z /= 10.0f;
                destination.z = Mathf.Round(destination.z);
                destination.z *= 10.0f;

                destination.y = 0.0f;

                for (int i = 0; i < mapParentNode.transform.childCount; i++)
                {
                    Transform child = mapParentNode.transform.GetChild(i);

                    if (Mathf.Abs(child.position.x - transform.position.x) < 5.0f && Mathf.Abs(child.position.z - transform.position.z) < 5.0f && !Input.GetMouseButton(0))
                    {
                        mapParentNode.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.red;

                        destination.y = 10.0f;
                        planeDestination = new Vector3(0, -9.0f, 0);
                    }
                    else
                    {
                        mapParentNode.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = new Color(0.71f, 0.71f, 0.71f);
                    }
                }

                transform.position += (destination - transform.position) / 4.0f;
                plane.transform.localPosition += (planeDestination - plane.transform.localPosition) / 4.0f;

                building.transform.localRotation = new Quaternion(0.02f * (transform.position.z - destination.z) + 0.02f * Mathf.Sin(Time.fixedTime), 0.0f, 0.02f * (transform.position.x - destination.x) + 0.02f * Mathf.Cos(Time.fixedTime), 1.0f);

                if (Input.GetMouseButtonDown(0))
                {
                    newClone = Instantiate(building);
                    newClone.transform.position = new Vector3(destination.x, 0.0f, destination.z);
                    newClone.transform.rotation = new Quaternion();
                    newClone.transform.SetParent(mapParentNode.transform);

                    newCloneCenter = newClone.transform.localPosition;

                    for (int i = 0; i < mapParentNode.transform.childCount; i++)
                    {
                        Transform child = mapParentNode.transform.GetChild(i);

                        if (Mathf.Abs(child.position.x - transform.position.x) < 5.0f && Mathf.Abs(child.position.z - transform.position.z) < 5.0f && !Input.GetMouseButton(0))
                        {
                            destroyParticles.gameObject.SetActive(true);
                            destroyParticles.transform.position = child.position;
                            destroyParticles.transform.position += new Vector3(0, 3.0f, 0);
                            destroyParticles.time = 0.0f;
                            destroyParticles.Play();

                            Debug.Log("Destroy");

                            Destroy(child.gameObject);
                        }

                    }

                }

                if (Input.GetMouseButton(0))
                {
                    building.GetComponent<MeshRenderer>().enabled = false;
                    plane.GetComponent<MeshRenderer>().enabled = false;

                    if ((hit.point - newCloneCenter).magnitude > 5.0f)
                    {
                        float theta = Vector3.Angle(new Vector3(newCloneCenter.x - hit.point.x, 0.0f, newCloneCenter.z - hit.point.z), Vector3.left);

                        if (newCloneCenter.z - hit.point.z < 0.0f)
                        {
                            theta *= -1;
                        }

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

                if (Input.GetMouseButtonUp(0))
                {
                    float y = newClone.transform.rotation.eulerAngles.y;

                    newClone.transform.rotation = Quaternion.Euler(0.0f, y + 45.0f - (y + 45.0f) % 90.0f, 0.0f);
                    newClone.GetComponent<MeshRenderer>().material = hologramMaterial;

                    manager.isBuilding = false;
                    building.GetComponent<MeshRenderer>().enabled = false;
                    plane.GetComponent<MeshRenderer>().enabled = false;

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

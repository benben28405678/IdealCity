using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceNew : MonoBehaviour
{
    public StateManager manager;
    public GameObject building;
    public GameObject mapParentNode;
    public GameObject plane;
    
    private Vector3 newCloneCenter = new Vector3();
    private GameObject newClone;
    
    void Update()
    {
        //gameObject.SetActive(manager.isBuilding);
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if(!manager.isBuilding)
        {
            return;
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Environment")
            {
                Vector3 destination = hit.point;

                building.transform.localPosition = new Vector3(0.0f, 1.1f + 0.5f * Mathf.Sin(Time.fixedTime * 3.0f), 0.0f);

                destination.x /= 10.0f;
                destination.x = Mathf.Round(destination.x);
                destination.x *= 10.0f;
                
                destination.z /= 10.0f;
                destination.z = Mathf.Round(destination.z);
                destination.z *= 10.0f;

                destination.y = 0.0f;

                transform.position += (destination - transform.position) / 4.0f;

                building.transform.localRotation = new Quaternion(0.02f * (transform.position.z - destination.z) + 0.02f * Mathf.Sin(Time.fixedTime), 0.0f, 0.02f * (transform.position.x - destination.x) + 0.02f * Mathf.Cos(Time.fixedTime), 1.0f);
                
                if(Input.GetMouseButtonDown(0))
                {
                    newClone = Instantiate(building);
                    newClone.transform.position = new Vector3(destination.x, 0.0f, destination.z);
                    newClone.transform.rotation = new Quaternion();
                    newClone.transform.SetParent(mapParentNode.transform);

                    newCloneCenter = newClone.transform.localPosition;
                }
                
                if (Input.GetMouseButton(0))
                {
                    building.GetComponent<MeshRenderer>().enabled = false;
                    plane.GetComponent<MeshRenderer>().enabled = false;

                    //float theta = Mathf.Atan((newCloneCenter.x - hit.point.x)/(newCloneCenter.z - hit.point.z));

                    float theta = Vector3.Angle(new Vector3(newCloneCenter.x - hit.point.x, 0.0f, newCloneCenter.z - hit.point.z), Vector3.left);
                    
                    if(newCloneCenter.z - hit.point.z < 0.0f)
                    {
                        theta *= -1;
                    }

                    //float snapTheta = theta - theta % 90.0f;

                    //snapTheta = theta - (theta % (Mathf.PI / 2.0f));

                    //newClone.transform.localRotation = Quaternion.LookRotation(new Vector3(hit.point.x - newCloneCenter.x, hit.point.z - newCloneCenter.z));

                    float snapTheta = 180.0f;
                    
                    if(Mathf.Abs(90.0f - theta) < 45.0f)
                    {
                        snapTheta = 270.0f;
                    }
                    
                    if(Mathf.Abs(-90.0f - theta) < 45.0f)
                    {
                        snapTheta = 90.0f;
                    }
                    
                    if(Mathf.Abs(180.0f - theta) < 45.0f || Mathf.Abs(-180.0f - theta) < 45.0f)
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
                else if(!manager.isBuilding)
                {
                    building.GetComponent<MeshRenderer>().enabled = true;
                    plane.GetComponent<MeshRenderer>().enabled = true;
                }
                
                if(Input.GetMouseButtonUp(0))
                {
                    float y = newClone.transform.rotation.eulerAngles.y;
                    
                    newClone.transform.rotation = Quaternion.Euler(0.0f, y + 45.0f - (y + 45.0f) % 90.0f, 0.0f);

                    manager.isBuilding = false;
                }
            }
        }
    }
}

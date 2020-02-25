using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /*
     * This script manages the movement of the camera per user input.
     */

    public float panSpeed = 1.0f; // The speed of the camera's pan.
    public float grabSpeed = 1.0f; // The parallax of the grab speed (hold SPACE to pan using Grab)
    public float rotateSpeed = 1.0f; // The rotation speed (press Q, E, R and F)
    public float zoomSpeed = 1.0f; // The zoom speed (scroll)
    public GameObject centerObject; // The object that is the pivot point of the Camera
    public Texture2D panCursor; // The cursor's texture when holding SPACEBAR

    private Vector3 lastMousePosition; // The last position of the mouse, in 3D coords (even though it is really 2D)
    private Vector3 deltaMouse; // The change in position of the mouse. Used for animation of the new building being placed.
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // W key moves camera up
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            centerObject.transform.position += centerObject.transform.forward * panSpeed * Time.deltaTime; //transform.localPosition
        }

        // S key moves camera down
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            centerObject.transform.position += -centerObject.transform.forward * panSpeed * Time.deltaTime;
        }

        // A key moves camera left
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            centerObject.transform.position += -centerObject.transform.right * panSpeed * Time.deltaTime;
        }

        // D key moves camera right
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            centerObject.transform.position += centerObject.transform.right * panSpeed * Time.deltaTime;
        }

        // Q key rotates along Y axis
        if(Input.GetKey(KeyCode.Q)) {
            centerObject.transform.Rotate(0.0f, rotateSpeed, 0.0f, Space.World);
        }

        // E key rotates along Y axis the other way
        if(Input.GetKey(KeyCode.E)) {
            centerObject.transform.Rotate(0.0f, -rotateSpeed, 0.0f, Space.World);
        }

        // R key gives better top down view
        if (Input.GetKey(KeyCode.R) && (centerObject.transform.localRotation.eulerAngles.x < 15.0f || centerObject.transform.localRotation.eulerAngles.x > 180.0f))
        {
            centerObject.transform.Rotate(0.4f * rotateSpeed, 0.0f, 0.0f, Space.Self);
        }

        // F key does the opposite of R key
        if (Input.GetKey(KeyCode.F) && (centerObject.transform.localRotation.eulerAngles.x > 345.0f || centerObject.transform.localRotation.eulerAngles.x < 180.0f))
        {
            centerObject.transform.Rotate(-0.4f * rotateSpeed, 0.0f, 0.0f, Space.Self);
        }

        var camera = GetComponent<Camera>(); // Remember, this script is placed on the Main Camera GameObject in the hierarchy. The camera variable is the actual camera Component.

        //camera.orthographicSize += Input.mouseScrollDelta.y * zoomSpeed / -10.0f;
        camera.fieldOfView += Input.mouseScrollDelta.y * zoomSpeed / -100.0f; // Zooming in/out will change the field of view.

        // Make sure we don't zoom too far in/out.
        if(camera.fieldOfView < 1.0f)
        {
            camera.fieldOfView = 1.0f;
        } else if(camera.fieldOfView > 7.0f)
        {
            camera.fieldOfView = 7.0f;
        }

        // Update deltaMouse
        deltaMouse = Input.mousePosition - lastMousePosition;

        // Pan the screen when SPACEBAR pressed
        if(Input.GetKey(KeyCode.Space))
        {
            Cursor.SetCursor(panCursor, Vector2.zero, CursorMode.Auto);
            transform.localPosition -= grabSpeed * deltaMouse * Time.deltaTime;
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void LateUpdate()
    {
        lastMousePosition = Input.mousePosition;
    }
}

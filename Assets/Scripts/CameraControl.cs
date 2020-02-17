using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float panSpeed = 1.0f;
    public float grabSpeed = 1.0f;
    public float rotateSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public GameObject centerObject;
    public Texture2D panCursor;

    private Vector3 lastMousePosition;
    private Vector3 deltaMouse;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            centerObject.transform.position += centerObject.transform.forward * panSpeed * Time.deltaTime; //transform.localPosition
        }
        
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            centerObject.transform.position += -centerObject.transform.forward * panSpeed * Time.deltaTime;
        }
        
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            centerObject.transform.position += -centerObject.transform.right * panSpeed * Time.deltaTime;
        }
        
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            centerObject.transform.position += centerObject.transform.right * panSpeed * Time.deltaTime;
        }
        
        if(Input.GetKey(KeyCode.Q)) {
            centerObject.transform.Rotate(0.0f, rotateSpeed, 0.0f, Space.World);
        }
        
        if(Input.GetKey(KeyCode.E)) {
            centerObject.transform.Rotate(0.0f, -rotateSpeed, 0.0f, Space.World);
        }

        Debug.Log(centerObject.transform.localRotation.eulerAngles.x);

        if (Input.GetKey(KeyCode.R) && (centerObject.transform.localRotation.eulerAngles.x < 15.0f || centerObject.transform.localRotation.eulerAngles.x > 180.0f))
        {
            centerObject.transform.Rotate(0.4f * rotateSpeed, 0.0f, 0.0f, Space.Self);
        }

        if (Input.GetKey(KeyCode.F) && (centerObject.transform.localRotation.eulerAngles.x > 345.0f || centerObject.transform.localRotation.eulerAngles.x < 180.0f))
        {
            centerObject.transform.Rotate(-0.4f * rotateSpeed, 0.0f, 0.0f, Space.Self);
        }

        var camera = GetComponent<Camera>();

        camera.orthographicSize += Input.mouseScrollDelta.y * zoomSpeed / -10.0f;
        
        if(camera.orthographicSize < 10.0f)
        {
            camera.orthographicSize = 10.0f;
        } else if(camera.orthographicSize > 85.0f)
        {
            camera.orthographicSize = 85.0f;
        }

        deltaMouse = Input.mousePosition - lastMousePosition;
        
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

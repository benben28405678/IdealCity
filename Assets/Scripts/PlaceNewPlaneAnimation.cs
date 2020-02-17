using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceNewPlaneAnimation : MonoBehaviour
{
    public Material thisMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        float s = 0.95f + 0.05f * Mathf.Abs(Mathf.Sin(3.0f * Time.fixedTime));

        thisMaterial.color = new Color(0.0f, 1.0f, 0.5f + 0.5f * Mathf.Cos(Time.fixedTime));

        if (transform.localPosition.y < -2.0f)
        {
            thisMaterial.color = new Color(1.0f, 0.5f + 0.5f * Mathf.Cos(Time.fixedTime), 0.0f);
        }

        transform.localScale = new Vector3(s, s, s);
        
    }
}

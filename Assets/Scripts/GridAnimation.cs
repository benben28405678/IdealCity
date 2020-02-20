using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAnimation : MonoBehaviour
{
    public Material material;
    public Material lowDefMaterial;
    public StateManager manager;

    private float animationTime = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(5.0f, 2.0f * Mathf.Sin(animationTime), 5.0f);

        //enabled = transform.position.y > 0.0f;


    }

    private void LateUpdate()
    {

        if(!manager.isBuilding && animationTime < 3*Mathf.PI/2.0f)
        {
            animationTime += Time.deltaTime * 12.0f;
        }

        if (manager.isBuilding && animationTime > 0.1f)
        {
            animationTime -= Time.deltaTime * 12.0f;
        }

        if (animationTime < 0.1f) animationTime = 0.1f;
    }
}

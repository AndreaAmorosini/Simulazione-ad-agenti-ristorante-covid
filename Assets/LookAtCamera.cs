using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera camerToLookAt;


    // Start is called before the first frame update
    void Start()
    {
        camerToLookAt = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = camerToLookAt.transform.position - transform.position;

        v.x = v.z = 0.0f;
        transform.LookAt(camerToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}

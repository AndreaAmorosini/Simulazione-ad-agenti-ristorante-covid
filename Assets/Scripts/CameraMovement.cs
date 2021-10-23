/*Gestisce i controlli per la telecamera*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float moveSpeed = 0.5f;
    public float scrollSpeed = 10f;
    public float minFov = 15f;
    public float maxFov = 50f;
    private Vector3 moveVector;
    private float fov;

    // Start is called before the first frame update
    void Start()
    {
        moveVector = new Vector3(0, 0, 0);
        fov = Camera.main.fieldOfView;
    }

    private void Update()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.z = Input.GetAxisRaw("Vertical");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.position += moveSpeed * moveVector;
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            fov += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;
        }
    }
}

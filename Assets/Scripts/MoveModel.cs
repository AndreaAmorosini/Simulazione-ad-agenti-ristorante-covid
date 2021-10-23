/*Gestisce il sistema di controllo del model di genere punta-e-clicca*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveModel : MonoBehaviour
{
    NavMeshAgent agent;
    public Camera camera;
    RaycastHit hitInfo = new RaycastHit();
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                agent.destination = hitInfo.point;
            }

            
        }

        
    }
}

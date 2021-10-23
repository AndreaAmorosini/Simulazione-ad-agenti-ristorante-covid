using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarAI : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;
    private CharacterController controller;

    public Path path;

    public float speed = 2;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;

    // Start is called before the first frame update
    void Start()
    {
        //ottiene un riferimento al componente seeker e al CharcterController
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();

        //inizia a calcolare un path alla targetPosition, chiama poi la OnPathComplete quando il path è stato calcolato
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(path == null)
        {
            //non ci sta un path quindi non si fa nulla
            return;
        }

        //controlla in un loop se l'avatar è abbastanza vicino al waypoint corrente per fare switch al prossimo
        reachedEndOfPath = false;
        //la distanza al prossimo wayPoint nel path
        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if(distanceToWaypoint < nextWaypointDistance)
            {
                //controlla se ci sta un altro waypoint o è la fine del percorso
                if(currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                } 
                else
                {
                    //setta una variabile di sttao per indicare che l'agente ha raggiunto la fine del path
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        //rallenta man mano che si avvicina alla fine del path
        //questo valore andrà da 1 a 0 man mano che l'agente si avvicina alla fine del path
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        //direzione verso il prossimo waypoint
        //la normalizza per fare in modo che abbia una lunghezza di un unità di mondo
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        //moltiplica la direzione per la velocità desiderata per ottenere la velocity
        Vector3 velocity = dir * speed * speedFactor;

        //muove l'agente usando il CharacterController
        controller.SimpleMove(velocity);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yai, we got a path back. Did it have an error? " + p.error);

        if (!p.error)
        {
            path = p;
            //resetta il contatore di waypoint
            currentWaypoint = 0;
        }
    }
}

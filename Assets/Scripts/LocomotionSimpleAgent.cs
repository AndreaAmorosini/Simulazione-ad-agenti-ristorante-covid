/*Gestisce l'animazione per il movimento del modello nello spazio*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocomotionSimpleAgent : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        //map worldDeltaPosition to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        //aggiorna la velocità se il tempo avanza
        if(Time.deltaTime > 1e-5f)
        {
            velocity = smoothDeltaPosition / Time.deltaTime;
        }

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        //aggiorna i parametri dell'animazione
        animator.SetBool("isWalking", shouldMove);
        animator.SetFloat("velx", velocity.x);
        animator.SetFloat("vely", velocity.y);

        LookAt lookAt = GetComponent<LookAt>();
        if(lookAt)
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

        //tira il charachter verso l'agent
        /*if(worldDeltaPosition.magnitude > agent.radius)
        {
            agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
        }*/
        
    }

     void OnAnimatorMove()
    {
        //aggiorna la posizione sul movimento dell'animazione usando l'altezza della superficie di navigazioen (la mesh surface)
        Vector3 position = animator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = agent.nextPosition;
    }
}

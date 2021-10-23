using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICliente : MonoBehaviour
{
    
    public GameObject exit;
    public GameObject postiL;
    AIDestinationSetter ai;
    AIPath path;
    Spawner spawner;
    Animator ani;
    PostiPool postiPool;
    GameObject[] postiClienti;
    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<AIPath>();
        ani = GetComponent<Animator>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        ai = GetComponent<AIDestinationSetter>();
        exit = GameObject.Find("Uscita");
        postiClienti = GameObject.FindGameObjectsWithTag("PostiClienti");
        //postiPool = GameObject.FindGameObjectWithTag("ClientePool").GetComponent<PostiPool>();
        StartCoroutine(setDestination());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator setDestination()
    {
        int choice = Random.Range(0, postiClienti.Length);
        Debug.Log("CLIENTE : " + this.name + " choice:" + choice);
        GameObject posto = postiClienti[choice];
        Debug.Log("CLIENTE : " + this.name + " posto nome:" + posto.name);
        if(posto != null)
        {
            if(posto.GetComponent<DisponibilitàPosto>().getDisponibilitàPosto() == true)
            {
                Debug.Log("CLIENTE : " + this.name + " posto disponibile");
                ai.target = posto.transform;
                posto.GetComponent<DisponibilitàPosto>().setDisponibilitàFalse();
                ani.SetBool("isWalking", true);
                yield return new WaitForSeconds(20);
            }
            else
            {
                Debug.Log("CLIENTE : " + this.name + " posto non disponibile");
                StopCoroutine(setDestination());
                //StopCoroutine("setDestination");
                StartCoroutine(setDestination());
                //StartCoroutine("setDestination");
            }
        }
        else
        {
            Debug.Log("CLIENTE : " + this.name + " Posto non trovato");
            StopCoroutine(setDestination());
            //StopCoroutine("setDestination");
            StartCoroutine(setDestination());
            //StartCoroutine("setDestination");
        }
        Debug.Log("CLIENTE : " + this.name + " Me ne vado");
        ai.target = exit.transform;
        posto.GetComponent<DisponibilitàPosto>().setDisponibilitàTrue();
        ani.SetBool("isWalking", true);
        yield return new WaitForSeconds(5);
        if (path.reachedEndOfPath)
        {
            gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }
       
    }

    
}

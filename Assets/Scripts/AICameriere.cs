using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICameriere : MonoBehaviour
{
    public List<Transform> posti;
    public int timer;
    public GameObject exit;
    public GameObject postiL;
    AIDestinationSetter ai;
    bool hasDestination;
    Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        ai = GetComponent<AIDestinationSetter>();
        exit = GameObject.Find("CamerieriSpawn");
        postiL = GameObject.Find("PostiCamerieri");
        foreach (Transform child in postiL.transform)
        {
            posti.Add(child);
        }
        hasDestination = false;
        StartCoroutine(setDestination());
    }

    // Update is called once per frame
    void Update()
    {
      if(hasDestination == false)
        {
            StartCoroutine(setDestination());
        }
        
    }

    IEnumerator setDestination()
    {
        hasDestination = true;
        int choice = Random.Range(0, posti.Count);
        if (posti[choice].GetComponent<DisponibilitàPosto>().isFree == true)
        {
            ai.target = posti[choice];
            posti[choice].GetComponent<DisponibilitàPosto>().setDisponibilitàFalse();
            ani.SetBool("isWalking", true);
            yield return new WaitForSeconds(20);
        }
        else
        {
            StopCoroutine("setDestination");
            StartCoroutine("setDestination");
        }
        ai.target = exit.transform;
        posti[choice].GetComponent<DisponibilitàPosto>().setDisponibilitàTrue();
        ani.SetBool("isWalking", true);
        yield return new WaitForSeconds(15);
        hasDestination = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidController : MonoBehaviour
{

    public int nrContagious;
    public int nrInfected;
    public int InfectionPercentage;
    public int seed;

    // Start is called before the first frame update
    void Start()
    {
        nrContagious = 0;
        nrInfected = 0;
        InfectionPercentage = 50;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addInfected()
    {
        nrInfected++;
    }

    public void addContagious()
    {
        nrContagious++;
    }

    public int getContagious()
    {
        return nrContagious;
    }

    public int getInfected()
    {
        return nrInfected;
    }

    public bool Infected(int infectionPercentage)
    {
        seed++;
        Random.seed = seed;
        return Random.Range(0, 100) < infectionPercentage;
    }

}

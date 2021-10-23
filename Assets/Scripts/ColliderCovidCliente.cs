using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCovidCliente : MonoBehaviour
{
    [HideInInspector]public int InfectionPercentage;
    CovidController covidController;

    private void Start()
    {
        covidController = GameObject.FindGameObjectWithTag("Utility").GetComponent<CovidController>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (covidController.Infected(GetComponentInParent<IAClienteFSM>().infectionPercentage))
        {
            Debug.Log("Infettato");
            GetComponentInParent<IAClienteFSM>().Infected();
        }
    }
}

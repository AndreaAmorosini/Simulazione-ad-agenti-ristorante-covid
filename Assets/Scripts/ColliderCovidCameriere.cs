using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCovidCameriere : MonoBehaviour
{
    CovidController covidController;

    private void Start()
    {
        covidController = GameObject.FindGameObjectWithTag("Utility").GetComponent<CovidController>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (covidController.Infected(GetComponentInParent<IACameriereFSM>().infectionPercentage))
        {
            Debug.Log("Infettato");
            GetComponentInParent<IACameriereFSM>().Infected();
        }
    }
}

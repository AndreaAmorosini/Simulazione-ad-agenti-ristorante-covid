using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavoliOccupatiCounter : MonoBehaviour
{

    public GameObject[] tavoli;
    public List<GameObject> tavoliLiberi;
    public List<GameObject> tavoliPronti;


    // Start is called before the first frame update
    void Start()
    {
        tavoli = GameObject.FindGameObjectsWithTag("Tavoli");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject tavolo in tavoli)
        {
            if (tavolo.GetComponent<StatoTavolo>().isPronto() == true && tavoliPronti.Contains(tavolo) == false)
            {
                tavoliPronti.Add(tavolo);
            }

            if(tavolo.GetComponent<StatoTavolo>().isOccupato() == false && tavoliLiberi.Contains(tavolo) == false)
            {
                tavoliLiberi.Add(tavolo);
            }
        }

    }

    public GameObject getTavoloPronto()
    {
        if(tavoliPronti.Count != 0)
        {
            GameObject tavoloPronto = tavoliPronti[0];
            tavoloPronto.GetComponent<StatoTavolo>().setPronto(false);
            tavoliPronti.RemoveAt(0);
            return tavoloPronto;
        }
        else
        {
            return null;
        }

    }


    public GameObject getTavoloLibero()
    {
        if(tavoliLiberi.Count != 0)
        {
            GameObject tavoloLibero = tavoliLiberi[0];
            tavoloLibero.GetComponent<StatoTavolo>().setOccupato(true);
            tavoliLiberi.RemoveAt(0);
            return tavoloLibero;
        }
        else
        {
            return null;
        }
    }



}

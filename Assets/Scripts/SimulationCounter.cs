using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationCounter : MonoBehaviour
{
    public int nrClientiInAttesa;
    public int nrClientiMangiando;
    public int nrClientiFinito;
    public int nrClientiServiti;

    // Start is called before the first frame update
    void Start()
    {
        nrClientiFinito = 0;
        nrClientiInAttesa = 0;
        nrClientiMangiando = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addClienteInAttesa()
    {
        nrClientiInAttesa++;
    }

    public void addClienteMangiando()
    {
        nrClientiInAttesa--;
        nrClientiMangiando++;
    }

    public void addClienteFinito()
    {
        nrClientiMangiando--;
        nrClientiFinito++;
    }

    public void addClienteServito()
    {
        nrClientiFinito--;
        nrClientiServiti++;
    }
}

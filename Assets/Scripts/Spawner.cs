using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Clienti;
    public GameObject Camerieri;
    public Transform spawnPointClienti1;
    public Transform spawnPointClienti2;
    public Transform spawnPointClienti3;
    public Transform spawnPointClienti4;
    public Transform spawnPointCamerieri;
    public float delay;
    public int maxClienti;
    public int maxCamerieri;
    public GameObject[] tavoli;
    public List<GameObject> tavoliOccupati;
    //public ClientePool ClientePool;
    GameObject utility;
    TavoliOccupatiCounter tavoliOccupatiCounter;
    UIBehaviour uIBehaviour;

    public int numClienti;
    public int numCamerieri;

    public bool isSpawning = false;

    public bool isPaused = true;


    // Start is called before the first frame update
    void Start()
    {
        numClienti = 0;
        numCamerieri = 0;
        tavoli = GameObject.FindGameObjectsWithTag("Tavoli");
        utility = GameObject.FindGameObjectWithTag("Utility");
        tavoliOccupatiCounter = utility.GetComponent<TavoliOccupatiCounter>();
        uIBehaviour = utility.GetComponent<UIBehaviour>();

        //ClientePool = GameObject.FindGameObjectWithTag("ClientePool").GetComponent<ClientePool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (!isSpawning)
            {
                isSpawning = true;
                //StartCoroutine(spawnCliente());
                StartCoroutine(spawnGruppoClienti());
                StartCoroutine(spawnCameriere());
                StartCoroutine(failSafe());
            }

        }
    }

    IEnumerator spawnCliente()
    {
        if(numClienti < maxClienti)
        {
            //Instantiate(Clienti, spawnPointClienti.position, spawnPointClienti.rotation);
            GameObject cliente = ClientePool.SharedInstance.GetPooledObject();
            if(cliente != null)
            {
                cliente.transform.position = spawnPointClienti1.position;
                cliente.transform.rotation = spawnPointClienti1.rotation;
               
                cliente.SetActive(true);
            }
            numClienti++;
            yield return new WaitForSeconds(delay);
            isSpawning = false;
        }
    }

    IEnumerator spawnCameriere()
    {
        foreach (GameObject tavolo in tavoli)
        {
            if (tavolo.GetComponent<StatoTavolo>().isPronto() == true)
            {
                tavoliOccupati.Add(tavolo);
            }
        }

        if (numCamerieri < maxCamerieri && numCamerieri <= tavoliOccupatiCounter.tavoliPronti.Count)
        {
            GameObject tmp = Instantiate(Camerieri, spawnPointCamerieri.position, spawnPointCamerieri.rotation);
            uIBehaviour.camerieri.Add(tmp);
            tmp.name = "Cameriere " + numCamerieri;
            numCamerieri++;
            yield return new WaitForSeconds(delay);
            isSpawning = false;
        }
    }

    IEnumerator spawnGruppoClienti()
    {
        if(numClienti < maxClienti)
        {
            GameObject tavoloLibero = tavoliOccupatiCounter.getTavoloLibero();
            Debug.Log("SPAWNER : TAVOLOLIBERO : tavolo : " + tavoloLibero.name);
            if (tavoloLibero != null)
            {
                if (tavoloLibero.GetComponent<StatoTavolo>().getTipoTavolo() == 1)
                {
                    Debug.Log("SPAWNER : TIPOTAVOLO : " + tavoloLibero.GetComponent<StatoTavolo>().getTipoTavolo());
                    GameObject cliente1 = ClientePool.SharedInstance.GetPooledObject();
                    Debug.Log("SPAWNER : SPAWNGRUPPOCLIENTI : Cliente : " + cliente1.name);
                    GameObject cliente2 = ClientePool.SharedInstance.GetPooledObject();
                    Debug.Log("SPAWNER : SPAWNGRUPPOCLIENTI : Cliente : " + cliente2.name);
                    GameObject cliente3 = ClientePool.SharedInstance.GetPooledObject();
                    Debug.Log("SPAWNER : SPAWNGRUPPOCLIENTI : Cliente : " + cliente3.name);
                    GameObject cliente4 = ClientePool.SharedInstance.GetPooledObject();
                    Debug.Log("SPAWNER : SPAWNGRUPPOCLIENTI : Cliente : " + cliente4.name);

                    if (cliente1 != null && cliente2 != null && cliente3 != null && cliente4 != null)
                    {
                        cliente1.transform.position = spawnPointClienti1.position;
                        cliente1.transform.rotation = spawnPointClienti1.rotation;

                        cliente2.transform.position = spawnPointClienti2.position;
                        cliente2.transform.rotation = spawnPointClienti2.rotation;

                        cliente3.transform.position = spawnPointClienti3.position;
                        cliente3.transform.rotation = spawnPointClienti3.rotation;

                        cliente4.transform.position = spawnPointClienti4.position;
                        cliente4.transform.rotation = spawnPointClienti4.rotation;


                        cliente1.SetActive(true);
                        cliente2.SetActive(true);
                        cliente3.SetActive(true);
                        cliente4.SetActive(true);

                        cliente1.GetComponent<IAClienteFSM>().posto = tavoloLibero.GetComponent<StatoTavolo>().postoCliente1;
                        cliente2.GetComponent<IAClienteFSM>().posto = tavoloLibero.GetComponent<StatoTavolo>().postoCliente2;
                        cliente3.GetComponent<IAClienteFSM>().posto = tavoloLibero.GetComponent<StatoTavolo>().postoCliente3;
                        cliente4.GetComponent<IAClienteFSM>().posto = tavoloLibero.GetComponent<StatoTavolo>().postoCliente4;

                        numClienti += 4;
                        yield return new WaitForSeconds(delay);
                        isSpawning = false;

                    }


                }
                else if (tavoloLibero.GetComponent<StatoTavolo>().getTipoTavolo() == 2)
                {
                    Debug.Log("SPAWNER : TIPOTAVOLO : " + tavoloLibero.GetComponent<StatoTavolo>().getTipoTavolo());
                    GameObject cliente1 = ClientePool.SharedInstance.GetPooledObject();
                    Debug.Log("SPAWNER : SPAWNGRUPPOCLIENTI : Cliente : " + cliente1.name);
                    GameObject cliente2 = ClientePool.SharedInstance.GetPooledObject();
                    Debug.Log("SPAWNER : SPAWNGRUPPOCLIENTI : Cliente : " + cliente2.name);

                    if (cliente1 != null && cliente2 != null)
                    {
                        cliente1.transform.position = spawnPointClienti2.position;
                        cliente1.transform.rotation = spawnPointClienti2.rotation;

                        cliente2.transform.position = spawnPointClienti3.position;
                        cliente2.transform.rotation = spawnPointClienti3.rotation;

                        cliente1.SetActive(true);
                        cliente2.SetActive(true);

                        cliente1.GetComponent<IAClienteFSM>().posto = tavoloLibero.GetComponent<StatoTavolo>().postoCliente1;
                        cliente2.GetComponent<IAClienteFSM>().posto = tavoloLibero.GetComponent<StatoTavolo>().postoCliente2;

                        numClienti += 2;
                        yield return new WaitForSeconds(delay);
                        isSpawning = false;

                    }

                }
            }


        }
    }

    IEnumerator failSafe()
    {
        yield return new WaitForSeconds(10);
        isSpawning = false;
    }

}

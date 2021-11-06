using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{

    public Text textWalkSpeed;
    public Slider sliderWalkSpeed;
    public Text textDelaySpawn;
    public Slider sliderDelaySpawn;
    public Text textNumMaxCamerieri;
    public Slider sliderNumMaxCamerieri;
    public Text textNrContagiosi;
    public Text textNrContagiati;
    public Text textPercentualeInfezione;
    public Text subtextPercentuleInfezione;
    public Slider sliderPercentualeInfezione;
    public Text textPercentualeContagiosi;
    public Text subtextPercentuleContagiosi;
    public Slider sliderPercentualeContagiosi;
    public Button startButton;
    public Text textNrClientiInAttesa;
    public Text textNrClientiServiti;
    public Text textNrClientiFinito;
    public Text textNrClientiServitiTot;
    public GameObject optionsPanel;
    public Text textAmpEmissione;
    public Text textPercorsoMax;
    public Text textAngoloEmissione;
    public Slider sliderAmpEmissione;
    public Slider sliderPercorsoMax;
    public Slider sliderAngoloEmissione;

    ClientePool clientePool;
    public List<GameObject> clienti;
    public List<GameObject> camerieri;
    Spawner spawner;
    CovidController covidController;
    SimulationCounter simulationCounter;
    // Start is called before the first frame update
    void Start()
    {
        clientePool = GameObject.FindGameObjectWithTag("ClientePool").GetComponent<ClientePool>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        covidController = GameObject.FindGameObjectWithTag("Utility").GetComponent<CovidController>();
        simulationCounter = GameObject.FindGameObjectWithTag("Utility").GetComponent<SimulationCounter>();
        startButton.onClick.AddListener(OnButtonPressed);
        StartCoroutine(ottienClienti(1));
    }

    // Update is called once per frame
    void Update()
    {
        textWalkSpeed.text = "Velocita Agenti : " + sliderWalkSpeed.value;
        textDelaySpawn.text = "Delay Spawn : " + sliderDelaySpawn.value;
        textNumMaxCamerieri.text = "Num. Max. Camerieri : " + sliderNumMaxCamerieri.value;
        textPercentualeInfezione.text = "Probabilita' di contagio : " + sliderPercentualeInfezione.value;
        textPercentualeContagiosi.text = "Probabilita' di presenza di contagiosi : " + sliderPercentualeContagiosi.value;
        textNrContagiati.text = "Numero Contagiosi : " + covidController.nrContagious;
        textNrContagiosi.text = "Numero Contagiati : " + covidController.nrInfected;
        textNrClientiInAttesa.text = "Numero Clienti in attesa : " + simulationCounter.nrClientiInAttesa;
        textNrClientiServiti.text = "Numero Clienti Serviti : " + simulationCounter.nrClientiMangiando;
        textNrClientiFinito.text = "Numero Clienti Alzati : " + simulationCounter.nrClientiFinito;
        textNrClientiServitiTot.text = "Numero Clienti serviti in totale : " + simulationCounter.nrClientiServiti;
        textAmpEmissione.text = "Ampiezza di emissione : " + sliderAmpEmissione.value;
        textPercorsoMax.text = "Percorso massimo : " + sliderPercorsoMax.value;
        textAngoloEmissione.text = "Angolo di emissione : " + sliderAngoloEmissione.value;
        foreach (GameObject cliente in clienti)
        {
            cliente.GetComponent<IAClienteFSM>().walkSpeed = sliderWalkSpeed.value;
            cliente.GetComponent<IAClienteFSM>().infectionPercentage = (int)sliderPercentualeInfezione.value;
            cliente.GetComponent<IAClienteFSM>().contagiousPercentage = (int)sliderPercentualeContagiosi.value;
            cliente.GetComponent<IAClienteFSM>().angoloEmissione = (int) sliderAngoloEmissione.value;
            cliente.GetComponent<IAClienteFSM>().ampiezzaEmissione = (int)sliderAmpEmissione.value;
            cliente.GetComponent<IAClienteFSM>().maxPathVirus = sliderPercorsoMax.value;
        }
        foreach(GameObject cameriere in camerieri)
        {
            cameriere.GetComponent<IACameriereFSM>().walkSpeed = sliderWalkSpeed.value;
            cameriere.GetComponent<IACameriereFSM>().infectionPercentage = (int)sliderPercentualeInfezione.value;
            cameriere.GetComponent<IACameriereFSM>().contagiousPercentage = (int)sliderPercentualeContagiosi.value;
            cameriere.GetComponent<IACameriereFSM>().angoloEmissione = (int)sliderAngoloEmissione.value;
            cameriere.GetComponent<IACameriereFSM>().ampiezzaEmissione = (int)sliderAmpEmissione.value;
            cameriere.GetComponent<IACameriereFSM>().maxPathVirus = sliderPercorsoMax.value;
        }
        spawner.delay = sliderDelaySpawn.value;
        spawner.maxCamerieri = (int) sliderNumMaxCamerieri.value;
    }

    IEnumerator ottienClienti(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        clienti = clientePool.clientiRef;
    }


  void OnButtonPressed()
    {
        spawner.isPaused = false;
        optionsPanel.SetActive(false);
        textWalkSpeed.gameObject.SetActive(false);
        textDelaySpawn.gameObject.SetActive(false);
        textNumMaxCamerieri.gameObject.SetActive(false);
        sliderWalkSpeed.gameObject.SetActive(false);
        sliderDelaySpawn.gameObject.SetActive(false);
        sliderNumMaxCamerieri.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        sliderPercentualeInfezione.gameObject.SetActive(false);
        textPercentualeInfezione.gameObject.SetActive(false);
        subtextPercentuleInfezione.gameObject.SetActive(false);
        textAmpEmissione.gameObject.SetActive(false);
        textAngoloEmissione.gameObject.SetActive(false);
        textPercorsoMax.gameObject.SetActive(false);
        sliderAmpEmissione.gameObject.SetActive(false);
        sliderAngoloEmissione.gameObject.SetActive(false);
        sliderPercorsoMax.gameObject.SetActive(false);
        sliderPercentualeContagiosi.gameObject.SetActive(false);
        textPercentualeContagiosi.gameObject.SetActive(false);
        subtextPercentuleContagiosi.gameObject.SetActive(false);
    }
  
}

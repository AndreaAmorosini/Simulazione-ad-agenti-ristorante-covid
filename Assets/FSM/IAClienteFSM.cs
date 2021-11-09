using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class IAClienteFSM : MonoBehaviour
{
    public enum ClienteStates
    {
        RICERCA_POSTO = 0,
        PATH_TO_POSTO = 1,
        MANGIO = 2,
        VADO_A_PAGARE = 3,
        ME_NE_VADO = 4
    }

    public float walkSpeed;
    public GameObject exit;
    public GameObject cassa;
    AIDestinationSetter ai;
    AIPath path;
    Spawner spawner;
    Animator ani;
    GameObject[] postiClienti;
    public GameObject posto;
    int postoChoiche;
    bool imEating = false;
    bool stoPagando = false;
    bool meNeStoAndando = false;
    Sprite spriteStato1;
    Sprite spriteStato2;
    Sprite spriteStato2_5;
    Sprite spriteStato3;
    Sprite spriteStato4;
    Sprite spriteStato5;
    SetStatoUI UI;
    int iterazione = 0;
    ClientePool clientePool;
    public bool isContagious = false;
    public bool isInfected;
    public CovidController covidController;
    public GameObject particleSystem;
    public GameObject covidCollider;
    public int infectionPercentage;
    public int contagiousPercentage;
    public SimulationCounter simulationCounter;

    public int angoloEmissione;
    public int ampiezzaEmissione;
    public float maxPathVirus;

    public GameObject colliderCovid;


    public FSM m_fsm;

    // Start is called before the first frame update
    void Start()
    {
        //crea i tre stati e li aggiunge al fsm
        GetComponentsInChildren<Image>()[1].color = Color.green;
        GetComponentInChildren<ColliderCovidCliente>().gameObject.SetActive(true);
        //contagiousPercentage = Random.Range(0, 100);
        walkSpeed = 1.0f;
        covidController = GameObject.FindGameObjectWithTag("Utility").GetComponent<CovidController>();
        particleSystem = GetComponentInChildren<Illness>().gameObject;
        particleSystem.SetActive(false);
        isContagious = covidController.Infected(contagiousPercentage);
        /*if (isContagious)
        {
            particleSystem.SetActive(true);
            this.Contagious();
            var covid = GetComponentInChildren<Illness>().GetComponentInChildren<ParticleSystem>().shape;
            covid.angle = angoloEmissione;
            Illness.EmissionAngle = ampiezzaEmissione;
            GetComponentInChildren<Illness>().GetComponentInChildren<ParticleSystem>().startLifetime = maxPathVirus;

        }*/
        isInfected = false;
        /*
        if (!isContagious)
        {
            colliderCovid = GetComponentInChildren<ColliderCovidCliente>().gameObject;
        }*/

        colliderCovid = GetComponentInChildren<ColliderCovidCliente>().gameObject;

        //infectionPercentage = Random.Range(0, 100);
        path = GetComponent<AIPath>();
        ani = GetComponent<Animator>();
        UI = this.gameObject.GetComponentInChildren<SetStatoUI>();
        spriteStato1 = Resources.Load<Sprite>("icons/ricercaPosto");
        spriteStato2 = Resources.Load<Sprite>("icons/pathToPosto");
        spriteStato2_5 = Resources.Load<Sprite>("icons/stoAspettando");
        spriteStato3 = Resources.Load<Sprite>("icons/mangiando");
        spriteStato4 = Resources.Load<Sprite>("icons/pagando");
        spriteStato5 = Resources.Load<Sprite>("icons/uscendo");
        clientePool = GameObject.FindGameObjectWithTag("ClientePool").GetComponent<ClientePool>();
        ai = GetComponent<AIDestinationSetter>();
        exit = GameObject.FindGameObjectWithTag("Uscita");
        cassa = GameObject.FindGameObjectWithTag("Cassa");
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        postiClienti = GameObject.FindGameObjectsWithTag("PostiClienti");
        simulationCounter = GameObject.FindGameObjectWithTag("Utility").GetComponent<SimulationCounter>();

        m_fsm = new FSM();
        m_fsm.Add((int)ClienteStates.RICERCA_POSTO, new statoCliente(m_fsm, ClienteStates.RICERCA_POSTO, this));
        m_fsm.Add((int)ClienteStates.PATH_TO_POSTO, new statoCliente(m_fsm, ClienteStates.PATH_TO_POSTO, this));
        m_fsm.Add((int)ClienteStates.MANGIO, new statoCliente(m_fsm, ClienteStates.MANGIO, this));
        m_fsm.Add((int)ClienteStates.VADO_A_PAGARE, new statoCliente(m_fsm, ClienteStates.VADO_A_PAGARE, this));
        m_fsm.Add((int)ClienteStates.ME_NE_VADO, new statoCliente(m_fsm, ClienteStates.ME_NE_VADO, this));

        Init_RicercaPostoState();
        Init_PathToPostoState();
        Init_MangioState();
        Init_VadoAPagareState();
        Init_MeNeVadoState();

        m_fsm.SetCurrentState(m_fsm.GetState((int)ClienteStates.RICERCA_POSTO));


        //setta lo stato corrente (iniziale) dell'FSM
    }

    //fuznione per cambiare lo stato
    public void SetState(ClienteStates tipo)
    {
        m_fsm.SetCurrentState(m_fsm.GetState((int)tipo));
    }

    // Update is called once per frame
    void Update()
    {
        //vengono chiamati gli update dell'FSM qui
        if (m_fsm != null)
        {
            m_fsm.Update();
        }
    }

    void FixedUpdate()
    {
        m_fsm.FixedUpdate();
    }

    //exit functions per la fine dell'FSM
    public void Exit()
    {
        Debug.Log("Cliente con FSM exit");
        m_fsm = null;
    }

    #region Setup and Initialize the various States.

    void Init_RicercaPostoState()
    {
        statoCliente stato = (statoCliente)m_fsm.GetState((int)ClienteStates.RICERCA_POSTO);

        stato.OnEnterDelegate += delegate ()
        {
            if (isInfected)
            {
                isInfected = false;
                GetComponentsInChildren<Image>()[1].color = Color.green;
                colliderCovid.SetActive(true);
                particleSystem.SetActive(false);
            }
            ai.target = null;
            iterazione++;
            meNeStoAndando = false;
            UI.SetSpriteUI(spriteStato1);
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " OnEnter - RICERCAPOSTO");
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnExit - RICERCAPOSTO");
        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            //postoChoiche = Random.Range(0, postiClienti.Length);
            //posto = postiClienti[postoChoiche];

            if (posto != null)
            {
                if (posto.GetComponent<DisponibilitàPosto>().getDisponibilitàPosto() == true)
                {
                    SetState(ClienteStates.PATH_TO_POSTO);
                }
            }


        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }

    void Init_PathToPostoState()
    {
        statoCliente stato = (statoCliente)m_fsm.GetState((int)ClienteStates.PATH_TO_POSTO);

        stato.OnEnterDelegate += delegate ()
        {
            if (covidController.Infected(contagiousPercentage) &&
                covidController.getNrClientiContagiosi() < covidController.nrMaxClientiContagiosi)
            {
                particleSystem.SetActive(true);
                GetComponentsInChildren<Image>()[1].color = Color.yellow;
                Debug.Log("Contagious");
                covidController.addContagious();
                covidController.addClienteContagioso();
                if(GetComponentInChildren<ColliderCovidCliente>() != null)
                {
                    GetComponentInChildren<ColliderCovidCliente>().gameObject.SetActive(false);
                }
                var covid = GetComponentInChildren<Illness>().GetComponentInChildren<ParticleSystem>().shape;
                covid.angle = angoloEmissione;
                Illness.EmissionAngle = ampiezzaEmissione;
                GetComponentInChildren<Illness>().GetComponentInChildren<ParticleSystem>().startLifetime = maxPathVirus;

            }

            //stoScegliendo = false;
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnEnter - PATHTOPOSTO");
            UI.SetSpriteUI(spriteStato2);
            ai.target = posto.transform;
            posto.GetComponent<DisponibilitàPosto>().setDisponibilitàFalse();
            posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.GetComponent<StatoTavolo>().setOccupato(true);
            SetAnimation(true);
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnExit - PATHTOPOSTO");
        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (path.reachedEndOfPath == true)
            {
                LookAtTable(posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.transform);
                posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.GetComponent<StatoTavolo>().setPronto(true);
                SetState(ClienteStates.MANGIO);
            }

        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }

    void Init_MangioState()
    {
        statoCliente stato = (statoCliente)m_fsm.GetState((int)ClienteStates.MANGIO);

        stato.OnEnterDelegate += delegate ()
        {
            //stoAndando = false;
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnEnter - MANGIO");
            UI.SetSpriteUI(spriteStato2_5);
            simulationCounter.addClienteInAttesa();
            SetAnimation(false);
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnExit - MANGIO");
        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.GetComponent<StatoTavolo>().isServito() == true)
            {
                UI.SetSpriteUI(spriteStato3);
                if (imEating == false)
                {
                    simulationCounter.addClienteMangiando();
                    imEating = true;
                    StartCoroutine(Coroutine_wait_mangio(10));
                }


            }
        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }

    void Init_VadoAPagareState()
    {
        statoCliente stato = (statoCliente)m_fsm.GetState((int)ClienteStates.VADO_A_PAGARE);

        stato.OnEnterDelegate += delegate ()
        {
            imEating = false;
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnEnter - VADO_A_PAGARE");
            UI.SetSpriteUI(spriteStato4);
            simulationCounter.addClienteFinito();
            ai.target = cassa.transform;
            posto.GetComponent<DisponibilitàPosto>().setDisponibilitàTrue();
            posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.GetComponent<StatoTavolo>().setOccupato(false);
            posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.GetComponent<StatoTavolo>().setPronto(false);
            posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.GetComponent<StatoTavolo>().setServito(false);
            SetAnimation(true);
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnExit - VADO_A_PAGARE");
        };

        stato.OnUpdateDelegate += delegate ()
        {
            if (stoPagando == false)
            {
                stoPagando = true;
                StartCoroutine(Coroutine_wait_pago(3));
            }

        };
    }

    void Init_MeNeVadoState()
    {
        statoCliente stato = (statoCliente)m_fsm.GetState((int)ClienteStates.ME_NE_VADO);

        stato.OnEnterDelegate += delegate ()
        {
            stoPagando = false;
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnEnter - MENEVADO");
            UI.SetSpriteUI(spriteStato5);
            ai.target = exit.transform;
            SetAnimation(true);
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "OnExit - MENEVADO");

        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (meNeStoAndando == false)
            {
                meNeStoAndando = true;
                StartCoroutine(Coroutine_wait_me_ne_vado(3));
            }

        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }

    public void SetAnimation(bool isWalking)
    {
        switch (isWalking)
        {
            case true:
                {
                    ani.SetBool("isWalking", true);
                    break;
                }
            case false:
                {
                    ani.SetBool("isWalking", false);
                    break;
                }
        }
    }

    public static float Distance(GameObject obj, Vector3 pos)
    {
        return (obj.transform.position - pos).magnitude;
    }

    IEnumerator Coroutine_wait_mangio(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        SetState(ClienteStates.VADO_A_PAGARE);
    }

    IEnumerator Coroutine_wait_me_ne_vado(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "hasReachedEndOfPath MENEVADO : " + path.reachedEndOfPath);
        if (path.reachedEndOfPath)
        {
            simulationCounter.addClienteServito();
            clientePool.pooledObjects.Add(gameObject);
            m_fsm.SetCurrentState(m_fsm.GetState((int)ClienteStates.RICERCA_POSTO));
            spawner.numClienti--;
            gameObject.SetActive(false);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_wait_me_ne_vado(0.5f));
        }

    }

    IEnumerator Coroutine_wait_pago(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Debug.Log(this.gameObject.name + ": " + "Iterazione :" + iterazione + " " + "hasReachedEndOfPath PAGO : " + path.reachedEndOfPath);
        if (path.reachedEndOfPath == true)
        {
            SetAnimation(false);
            SetState(ClienteStates.ME_NE_VADO);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_wait_pago(0.5f));
        }

    }

    IEnumerator wait(float duration, int position)
    {
        yield return new WaitForSecondsRealtime(duration);
        Debug.Log(this.gameObject.name + ": " + "Waited in position : " + position + "for : " + duration);
    }


    void LookAtTable(Transform target)
    {
        StartCoroutine(LookAtTableCoroutine(target));
    }

    IEnumerator LookAtTableCoroutine(Transform target)
    {
        float Speed = 0.3f;
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * Speed;
            yield return null;
        }
    }

    public void Infected()
    {
        isInfected = true;
        GetComponentsInChildren<Image>()[1].color = Color.red;
        covidController.addInfected();
        if(GetComponentInChildren<ColliderCovidCliente>() != null)
        {
            GetComponentInChildren<ColliderCovidCliente>().gameObject.SetActive(false);

        }
        particleSystem.SetActive(true);
    }

    public void Contagious()
    {
        GetComponentsInChildren<Image>()[1].color = Color.yellow;
        Debug.Log("Contagious");
        GameObject.FindGameObjectWithTag("Utility").GetComponent<CovidController>().addContagious();
        GetComponentInChildren<ColliderCovidCliente>().gameObject.SetActive(false);


    }
    



    #endregion
}

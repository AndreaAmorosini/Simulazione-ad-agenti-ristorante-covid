using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Patterns;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class IACameriereFSM : MonoBehaviour
{

    public enum CameriereStates
    {
        CERCO_CLIENTI = 0,
        VADO_AL_TAVOLO = 1,
        PRENDO_ORDINAZIONE = 2,
        PRENDO_IL_CIBO = 3,
        TORNO_AL_TAVOLO = 4,
        SERVO = 5,
        TORNO_IN_CUCINA = 6
    }

    public float walkSpeed;
    public GameObject[] posti;
    public GameObject[] tavoli;
    public GameObject tavolo;
    public GameObject posto;
    public int timer;
    public GameObject exit;
    public GameObject postiL;
    AIDestinationSetter ai;
    AIPath path;
    Animator ani;
    int choice;
    int iterazione = 0;
    bool stoAndando = false;
    bool stoServendo = false;
    bool stoTornando = false;
    bool stoPrendendoOrdini = false;
    bool stoPrendendoCibo = false;
    bool stoTornandoAlTavolo = false;
    Sprite spriteStato1;
    Sprite spriteStato2;
    Sprite spriteStato3;
    Sprite spriteStato4;
    Sprite spriteStato5;
    Sprite spriteStato6;
    Sprite spriteStato7;
    SetStatoUI UI;
    public List<GameObject> tavoliOccupati;
    GameObject utility;
    TavoliOccupatiCounter tavoliOccupatiCounter;
    public bool isContagious;
    public bool isInfected;
    public CovidController covidController;
    public GameObject particleSystem;
    public GameObject covidCollider;
    public int infectionPercentage;



    public FSM m_fsm;

    // Start is called before the first frame update
    void Start()
    {
        walkSpeed = 1.0f;
        GetComponentInChildren<ColliderCovidCameriere>().gameObject.SetActive(true);
        covidController = GameObject.FindGameObjectWithTag("Utility").GetComponent<CovidController>();
        particleSystem = GetComponentInChildren<Illness>().gameObject;
        particleSystem.SetActive(false);
        if (isContagious)
        {
            particleSystem.SetActive(true);
            this.Contagious();
        }
        isInfected = false;
        infectionPercentage = Random.Range(0, 100);
        path = GetComponent<AIPath>();
        ani = GetComponent<Animator>();
        ai = GetComponent<AIDestinationSetter>();
        exit = GameObject.Find("CamerieriSpawn");
        posti = GameObject.FindGameObjectsWithTag("PostiCamerieri");
        tavoli = GameObject.FindGameObjectsWithTag("Tavoli");
        utility = GameObject.FindGameObjectWithTag("Utility");
        tavoliOccupatiCounter = utility.GetComponent<TavoliOccupatiCounter>();

        UI = this.gameObject.GetComponentInChildren<SetStatoUI>();
        spriteStato1 = Resources.Load<Sprite>("icons/ricercaPosto");
        spriteStato2 = Resources.Load<Sprite>("icons/pathToPosto");
        spriteStato3 = Resources.Load<Sprite>("icons/stoPrendendoOrdini");
        spriteStato4 = Resources.Load<Sprite>("icons/prendoIlCibo");
        spriteStato5 = Resources.Load<Sprite>("icons/stoTornando");
        spriteStato6 = Resources.Load<Sprite>("icons/servendo");
        spriteStato7 = Resources.Load<Sprite>("icons/tornoInCucina");

        m_fsm = new FSM();
        m_fsm.Add((int)CameriereStates.CERCO_CLIENTI, new statoCameriere(m_fsm, CameriereStates.CERCO_CLIENTI, this));
        m_fsm.Add((int)CameriereStates.VADO_AL_TAVOLO, new statoCameriere(m_fsm, CameriereStates.VADO_AL_TAVOLO, this));
        m_fsm.Add((int)CameriereStates.PRENDO_ORDINAZIONE, new statoCameriere(m_fsm, CameriereStates.PRENDO_ORDINAZIONE, this));
        m_fsm.Add((int)CameriereStates.PRENDO_IL_CIBO, new statoCameriere(m_fsm, CameriereStates.PRENDO_IL_CIBO, this));
        m_fsm.Add((int)CameriereStates.TORNO_AL_TAVOLO, new statoCameriere(m_fsm, CameriereStates.TORNO_AL_TAVOLO, this));
        m_fsm.Add((int)CameriereStates.SERVO, new statoCameriere(m_fsm, CameriereStates.SERVO, this));
        m_fsm.Add((int)CameriereStates.TORNO_IN_CUCINA, new statoCameriere(m_fsm, CameriereStates.TORNO_IN_CUCINA, this));

        Init_CercoClientiState();
        Init_VadoAlTavolo();
        Init_PrendoOrdinazioneState();
        Init_PrendoIlCiboState();
        Init_TornoAlTavoloState();
        Init_Servo();
        Init_TornoInCucina();

        m_fsm.SetCurrentState(m_fsm.GetState((int)CameriereStates.CERCO_CLIENTI));

    }

    public void SetState(CameriereStates tipo)
    {
        m_fsm.SetCurrentState(m_fsm.GetState((int)tipo));
    }

    // Update is called once per frame
    void Update()
    {
        if (m_fsm != null)
        {
            m_fsm.Update();
        }
    }

    void FixedUpdate()
    {
        m_fsm.FixedUpdate();
    }

    public void Exit()
    {
        Debug.Log("Cliente con FSM exit");
        m_fsm = null;
    }

    void Init_CercoClientiState()
    {
        statoCameriere stato = (statoCameriere)m_fsm.GetState((int)CameriereStates.CERCO_CLIENTI);

        stato.OnEnterDelegate += delegate ()
        {
            string nameNum = Regex.Replace(this.name, "[^0-9]", "");
            Random.seed = int.Parse(nameNum);
            ai.target = null;
            stoTornando = false;
            iterazione++;
            UI.SetSpriteUI(spriteStato1);
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnEnter - CERCO_CLIENTI");
            
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnExit - CERCO_CLIENTI");

        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            tavolo = tavoliOccupatiCounter.getTavoloPronto();
            if (tavolo != null)
            {
                tavolo.GetComponent<StatoTavolo>().setPronto(false);
                tavoliOccupatiCounter.tavoliPronti.Remove(tavolo);
                //choice = Random.Range(0, tavoliOccupati.Count);
                //Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "Scelta :" + choice);
                posto = tavolo.GetComponent<StatoTavolo>().postoCameriere;
                if (posto != null)
                {
                    if (posto.GetComponent<DisponibilitàPosto>().getDisponibilitàPosto() == true)
                    {
                        Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "Posto libero");
                        SetState(CameriereStates.VADO_AL_TAVOLO);
                    }
                }

            }

        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }

    void Init_VadoAlTavolo()
    {
        statoCameriere stato = (statoCameriere)m_fsm.GetState((int)CameriereStates.VADO_AL_TAVOLO);

        stato.OnEnterDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnEnter - VADO_AL_TAVOLO");
            ai.target = posto.transform;
            UI.SetSpriteUI(spriteStato2);
            posto.GetComponent<DisponibilitàPosto>().setDisponibilitàFalse();
            SetAnimation(true);
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " "  + "OnExit - VADO_AL_TAVOLO");

        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (stoAndando == false)
            {
                stoAndando = true;
                StartCoroutine(Coroutine_vado_al_tavolo(2));
            }
        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }

    void Init_PrendoOrdinazioneState()
    {
        statoCameriere stato = (statoCameriere)m_fsm.GetState((int)CameriereStates.PRENDO_ORDINAZIONE);

        stato.OnEnterDelegate += delegate ()
        {
            stoAndando = false;
            UI.SetSpriteUI(spriteStato3);
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnEnter - PRENDO_ORDINAZIONE");
            SetAnimation(false);
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnExit - PRENDO_ORDINAZIONE");

        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (stoPrendendoOrdini == false)
            {
                stoPrendendoOrdini = true;
                StartCoroutine(Coroutine_prendo_ordinazione(5));
            }

        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }


    void Init_PrendoIlCiboState()
    {
        statoCameriere stato = (statoCameriere)m_fsm.GetState((int)CameriereStates.PRENDO_IL_CIBO);

        stato.OnEnterDelegate += delegate ()
        {
            stoPrendendoOrdini = false;
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnEnter - PRENDO_IL_CIBO");
            ai.target = exit.transform;
            UI.SetSpriteUI(spriteStato4);
            //posti[choice].GetComponent<DisponibilitàPosto>().setDisponibilitàTrue();
            SetAnimation(true);

        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnExit - PRENDO_IL_CIBO");

        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (stoPrendendoCibo == false)
            {
                stoPrendendoCibo = true;
                StartCoroutine(Coroutine_prendo_il_cibo(5));
            }

        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }

    void Init_TornoAlTavoloState()
    {
        statoCameriere stato = (statoCameriere)m_fsm.GetState((int)CameriereStates.TORNO_AL_TAVOLO);

        stato.OnEnterDelegate += delegate ()
        {
            stoPrendendoCibo = false;
            ai.target = posto.transform;
            UI.SetSpriteUI(spriteStato5);
            SetAnimation(true);
            //posti[choice].GetComponent<DisponibilitàPosto>().setDisponibilitàFalse();
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnEnter - TORNO_AL_TAVOLO");

        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnExit - TORNO_AL_TAVOLO");

        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (stoTornandoAlTavolo == false)
            {
                stoTornandoAlTavolo = true;
                StartCoroutine(Coroutine_torno_al_tavolo(2));
            }

        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }


    void Init_Servo()
    {
        statoCameriere stato = (statoCameriere)m_fsm.GetState((int)CameriereStates.SERVO);

        stato.OnEnterDelegate += delegate ()
        {
            stoTornandoAlTavolo = false;
            UI.SetSpriteUI(spriteStato6);
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnEnter - SERVO");
            SetAnimation(false);
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnExit - SERVO");

        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (stoServendo == false)
            {
                stoServendo = true;
                StartCoroutine(Coroutine_servo(5));
            }
            
        };

        stato.OnFixedUpdateDelegate += delegate ()
        {

        };
    }

    void Init_TornoInCucina()
    {
        statoCameriere stato = (statoCameriere)m_fsm.GetState((int)CameriereStates.TORNO_IN_CUCINA);

        stato.OnEnterDelegate += delegate ()
        {
            stoServendo = false;
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " +  "OnEnter - TORNO_IN_CUCINA");
            ai.target = exit.transform;
            UI.SetSpriteUI(spriteStato7);
            posto.GetComponent<DisponibilitàPosto>().setDisponibilitàTrue();
            SetAnimation(true);
        };

        stato.OnExitDelegate += delegate ()
        {
            Debug.Log(this.gameObject.name + ": " + "Iterazione : " + iterazione + " " + "OnExit - TORNO_IN_CUCINA");

        };

        stato.OnUpdateDelegate += delegate ()
        {
            path.maxSpeed = walkSpeed;
            if (stoTornando == false)
            {
                stoTornando = true;
                StartCoroutine(Coroutine_torno_in_cucina(5));
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

    IEnumerator Coroutine_servo(float duration)
    {
        yield return new WaitForSeconds(duration);
        tavolo.GetComponent<StatoTavolo>().setServito(true);
        SetState(CameriereStates.TORNO_IN_CUCINA);
    }

    IEnumerator Coroutine_prendo_ordinazione(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetState(CameriereStates.PRENDO_IL_CIBO);
    }


    IEnumerator Coroutine_torno_in_cucina(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        if (path.reachedEndOfPath == true)
        {
            SetAnimation(false);
            m_fsm.SetCurrentState(m_fsm.GetState((int)CameriereStates.CERCO_CLIENTI));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_torno_in_cucina(1));
        }
    }

    IEnumerator Coroutine_prendo_il_cibo(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        if (path.reachedEndOfPath == true)
        {
            SetAnimation(false);
            m_fsm.SetCurrentState(m_fsm.GetState((int)CameriereStates.TORNO_AL_TAVOLO));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_prendo_il_cibo(1));
        }
    }


    IEnumerator Coroutine_vado_al_tavolo(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        if(path.reachedEndOfPath == true)
        {
            LookAtTable(posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.transform);
            SetState(CameriereStates.PRENDO_ORDINAZIONE);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_vado_al_tavolo(1));
        }
    }

    IEnumerator Coroutine_torno_al_tavolo(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        if (path.reachedEndOfPath == true)
        {
            LookAtTable(posto.GetComponent<DisponibilitàPosto>().tavoloAssociato.transform);
            SetState(CameriereStates.SERVO);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_torno_al_tavolo(1));
        }
    }



    void LookAtTable(Transform target)
    {
        StartCoroutine(LookAtTableCoroutine(target));
    }

    IEnumerator LookAtTableCoroutine(Transform target)
    {
        float Speed = 1f;
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
        GetComponentsInChildren<Image>()[1].color = Color.white;
        covidController.addInfected();
        GetComponentInChildren<ColliderCovidCameriere>().gameObject.SetActive(false);
    }

    public void Contagious()
    {
        GetComponentsInChildren<Image>()[1].color = Color.yellow;
        Debug.Log("Contagious");
        GameObject.FindGameObjectWithTag("Utility").GetComponent<CovidController>().addContagious();
        GetComponentInChildren<ColliderCovidCameriere>().gameObject.SetActive(false);

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatoTavolo : MonoBehaviour
{
    public enum TipoTavolo
    {
        daQuattro,
        daDue
    };  //1 : tavolo da quattro posti ;  2 : tavolo da due posti

    public TipoTavolo tipoTavolo;
    public bool Occupato;
    public bool ProntoPerServito;
    public bool Servito;
    public GameObject postoCameriere;
    public GameObject postoCliente1;
    public GameObject postoCliente2;
    public GameObject postoCliente3;
    public GameObject postoCliente4;
    // Start is called before the first frame update
    void Start()
    {
        Occupato = false;
        ProntoPerServito = false;
        Servito = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isOccupato()
    {
        return Occupato;
    }

    public void setOccupato(bool occupato)
    {
        Occupato = occupato;
    }

    public bool isPronto()
    {
        return ProntoPerServito;
    }

    public void setPronto(bool pronto)
    {
        ProntoPerServito = pronto;
    }

    public bool isServito()
    {
        return Servito;
    }

    public void setServito(bool servito)
    {
        Servito = servito;
    }

    public TipoTavolo getTipoTavolo()
    {
        return tipoTavolo;
    }

}

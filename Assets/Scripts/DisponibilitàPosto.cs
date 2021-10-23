using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisponibilitàPosto : MonoBehaviour
{
    
    public bool isFree;
    public GameObject tavoloAssociato;
    // Start is called before the first frame update
    void Start()
    {
        
        isFree = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setDisponibilitàTrue()
    {
        isFree = true;
    }

    public void setDisponibilitàFalse()
    {
        isFree = false;
    }

    public bool getDisponibilitàPosto()
    {
        return isFree;
    }


}

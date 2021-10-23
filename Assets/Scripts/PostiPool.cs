using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostiPool : MonoBehaviour
{
    public GameObject[] postiClienti;
    // Start is called before the first frame update
    void Start()
    {
        postiClienti = GameObject.FindGameObjectsWithTag("PostiClienti");
        for(int i = 0; i < postiClienti.Length; i++)
        {
            postiClienti[i].GetComponent<DisponibilitàPosto>().setDisponibilitàTrue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPostoFree()
    {
        for(int i = 0; i < postiClienti.Length; i++)
        {
            if(postiClienti[i].GetComponent<DisponibilitàPosto>().isFree == true)
            {
                return postiClienti[i];
            }
        }
        return null;
    }

    public int GetIndexPostoFree()
    {
        for (int i = 0; i < postiClienti.Length; i++)
        {
            if (postiClienti[i].GetComponent<DisponibilitàPosto>().isFree == true)
            {
                return i;
            }
        }
        return -1;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientePool : MonoBehaviour
{

    public static ClientePool SharedInstance;
    public List<GameObject> pooledObjects;
    public List<GameObject> clientiRef;
    public GameObject objectToPool;
    public int amountToPool;
    GameObject obj;
    public CovidController covidController;

    // Start is called before the first frame update
    void Start()
    {
        covidController = GameObject.FindGameObjectWithTag("Utility").GetComponent<CovidController>();
        SharedInstance = this;
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.name = "Cliente " + i;
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
            clientiRef.Add(tmp);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if (!(pooledObjects[i].activeInHierarchy))
            {
                obj = pooledObjects[i];
                pooledObjects.RemoveAt(i);
                return obj;
            }
        }
        return null;
    }

}

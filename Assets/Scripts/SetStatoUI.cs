using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStatoUI : MonoBehaviour
{

    Sprite spriteStato1;
    Sprite spriteStato2;
    Sprite spriteStato3;
    Sprite spriteStato4;
    Sprite spriteStato5;
    // Start is called before the first frame update
    void Start()
    {
        spriteStato1 = Resources.Load<Sprite>("icons/ricercaPosto");
        //GameObject.FindGameObjectWithTag("SpriteStato1");
        spriteStato2 = Resources.Load<Sprite>("icons/pathToPosto");
        spriteStato3 = Resources.Load<Sprite>("icons/mangiando");
        spriteStato4 = Resources.Load<Sprite>("icons/pagando");
        spriteStato5 = Resources.Load<Sprite>("icons/uscendo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpriteUI(Sprite spriteUI)
    {
        this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = spriteUI;
    }
}

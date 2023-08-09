using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buble;
    private Animator anim;
    public Sprite chestOpen;
    private bool isOpened;
    private bool inArea;
    void Start()
    {
        inArea = false;
        isOpened = false;
        anim = GetComponent<Animator>();
        buble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && inArea){
            this.GetComponent<SpriteRenderer>().sprite = chestOpen;
            isOpened = true;
            buble.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && !isOpened){
            buble.SetActive(true);
            inArea = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        buble.SetActive(false);
        inArea = false;
    }

}

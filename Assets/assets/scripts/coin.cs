using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    private GameManager gm;
    // Start is called before the first frame update
    private int spawnChance;
    void Start()
    {
        spawnChance = Random.Range(3,4);
        if (spawnChance == 3){
            this.gameObject.SetActive(true);

        }
        else{
            this.gameObject.SetActive(false);
        }
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            gm.SetScore(gm.GetScore() + 10);
            Destroy(this.gameObject);
        }
    }
}

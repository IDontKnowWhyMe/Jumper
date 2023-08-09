using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldActivate : MonoBehaviour
{
    public bool shielded = false;
    public GameObject shield;
    private Animator anim;

    private Animator myAnim;

    private float speed_backup;
    private float jump_b;
    // Start is called before the first frame update
    void Start()
    {
        shield.SetActive(false);
        anim = shield.GetComponent<Animator>();
        myAnim = this.GetComponent<Animator>();
        speed_backup = this.GetComponent<PlayerMovment>().speed;
        jump_b = this.GetComponent<PlayerMovment>().jump;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && this.GetComponent<PlayerMovment>().isGrounded){
            shielded = true;
            this.GetComponent<PlayerMovment>().speed = 0;
            this.GetComponent<PlayerMovment>().jump = 0;
            shield.SetActive(true);
            anim.SetTrigger("Shield");
        }
        if (Input.GetKeyUp(KeyCode.Q) && shielded){
            shielded = false;
            shield.SetActive(false);
            this.GetComponent<PlayerMovment>().speed = speed_backup;
            this.GetComponent<PlayerMovment>().jump = jump_b;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject player;
    public RuntimeAnimatorController[] animators;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)){
            int weapon_index = player.GetComponent<Inventory>().EquipWeapon(1);
            player.GetComponent<Animator>().runtimeAnimatorController = animators[weapon_index];
        }
    }
}

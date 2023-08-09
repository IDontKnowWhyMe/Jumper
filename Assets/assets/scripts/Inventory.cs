using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static int damge = 50;
    public static int HP = 100;
    public static int EquipdWeapon = 1;
    public static int[] Weapons = {0, 1};


    public int EquipWeapon(int weapon){
        for (int i = 0; i < Weapons.Length; i++){
            if (Weapons[i] == weapon){
                EquipdWeapon = weapon;
                return weapon;
            }
        }
        Debug.LogError("There is not such a weapon");
        return -1;

    }

    public int getWeapon(){
        return EquipdWeapon;
    }

    public int getHP(){
        return HP;
    }

    public void setHP(int hp){
        HP = hp;
    }

    public int getDamage(){
        return damge;
    }
}

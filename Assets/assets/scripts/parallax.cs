using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
   private float startPosX;
   private float startPosY;
   public GameObject cam;
   [SerializeField] private float parallaxEffectX;
   [SerializeField] private float parallaxEffectY;
   

   private void Start() {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
   }

   private void Update() {
        float distanceX = (cam.transform.position.x * parallaxEffectX);
        float distanceY = (cam.transform.position.y * parallaxEffectY);
        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);
   }
      

}

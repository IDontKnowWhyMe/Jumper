using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFolow : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private Vector3 landPoint;
    private bool cameraSynch = false;
    public float YOffset;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position  = new Vector3(player.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {   
        float step = speed * Time.deltaTime;
        if (player.transform.position.y - this.transform.position.y > 2.5f){
            transform.position = Vector3.MoveTowards(transform.position,new Vector3(transform.position.x, player.transform.position.y + YOffset, transform.position.z),2*step);
        }
        if (player.GetComponent<PlayerMovment>().isGrounded && player.GetComponent<PlayerMovment>().YSpeed <= 0){
            landPoint = new Vector3(transform.position.x, player.transform.position.y + YOffset, transform.position.z);
            cameraSynch = true;
        }
        if (cameraSynch){
            transform.position = Vector3.MoveTowards(transform.position,landPoint,step);
        }
        if(transform.position == landPoint){
            cameraSynch = false;
        }
    }
    private Vector3 PixelPerfectClamp(Vector3 moveVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(moveVector.x * pixelsPerUnit), Mathf.CeilToInt(moveVector.y * pixelsPerUnit), Mathf.CeilToInt(moveVector.z * pixelsPerUnit));                                         
        return vectorInPixels / pixelsPerUnit;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformOpt : MonoBehaviour
{
    public GameObject camer;
    public enum platformType
    {
        LONG_PLATFORM,
        SHORT_PLATFORM,
        MOVING_PLATFORM
    }
    private platformType type;
    public float cameraBorders;
    public Camera cam;
    public GameObject paltform;
    public float spawnMin;
    public float spawnMax;

    private bool generate = true;

    private float length;
    private float heigth = 0.5f;
    private Color color;

    private GameManager gm;

    private Vector3 spawnPos;

    public GameObject parent;
    public GameObject EnemyDomain;
    public GameObject chest;
    private bool left = true;

    private GameObject empty;

    private bool claimed = false;
    public Sprite[] icebox;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer childRenderer;

    void Start()
    {
        //External obj

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        camer = GameObject.Find("Camera");
        cam = camer.GetComponent<Camera>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        //Internal settings
        int probGen = Random.Range(1, 8);
        if (probGen == 7 && gm.GetScore() >= 100 &&
            parent.GetComponent<PlatformOpt>().type == platformType.SHORT_PLATFORM)
        {
            type = platformType.LONG_PLATFORM;
        }
        else if (gm.GetScore() >= 20 && (probGen == 1 || probGen == 2))
        {
            type = platformType.MOVING_PLATFORM;
        }
        else
        {
            type = platformType.SHORT_PLATFORM;
        }
        SetStart(type);
        spawnPos = newPos();
        this.transform.GetChild(0).localScale = new Vector3(length, heigth, 0);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

    }
    void Update()
    {
        if (type == platformType.MOVING_PLATFORM)
        {
            float step = 2 * Time.deltaTime;
            if (left)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-10 + length / 2, this.transform.position.y, 0), step);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(10 - length / 2, this.transform.position.y, 0), step);
            }
            if (transform.position.x <= -10 + length / 2 + 1 || transform.position.x >= 10 - length / 2 - 1)
            {
                left = !left;
            }
        }
    }

    // Update is called once per frame
    void OnBecameInvisible()
    {
        if (gm.player.transform.position.y > this.transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }

    void OnBecameVisible()
    {
        Debug.Log("geg");
        if (generate)
        {
            GameObject newInst = Instantiate(paltform, spawnPos, Quaternion.Euler(0, 0, 0));
            newInst.GetComponent<PlatformOpt>().parent = this.gameObject;
            generate = false;
        }
    }

    public Vector3 newPos()
    {
        if (this.type == platformType.LONG_PLATFORM)
        {
            return new Vector3(0, transform.position.y + spawnMin, 0);
        }
        float radius = Random.Range(spawnMin, spawnMax);
        float y = Random.Range(radius - 0.5f, radius);
        double x = Mathf.Sqrt(radius * radius - y * y);
        int polarity = Random.Range(0, 2);
        if (polarity == 0)
        {
            x = Mathf.Abs((float)x);
        }
        else
        {
            x = -Mathf.Abs((float)x);
        }
        if (this.transform.position.x + (float)x > camer.transform.position.x + cameraBorders ||
           this.transform.position.x + (float)x < camer.transform.position.x - cameraBorders)
        {
            return new Vector3(transform.position.x - (float)x, transform.position.y + y, 0);
        }
        else
        {
            return new Vector3(transform.position.x + (float)x, transform.position.y + y, 0);
        }

    }

    public void recalcPos(platformType type)
    {
        if (parent == null)
        {
            return;
        }
        if (type == platformType.LONG_PLATFORM)
        {
            if (parent.transform.position.x < 0)
            {
                this.transform.position = new Vector3(10 - length / 2, this.transform.position.y, 0);
            }
            else
            {
                this.transform.position = new Vector3(-10 + length / 2, this.transform.position.y, 0);
            }
        }

    }

    private void SetStart(platformType type)
    {
        switch (type)
        {
            case platformType.LONG_PLATFORM:
                length = 11f;
                heigth = 1f;
                recalcPos(type);
                Instantiate(EnemyDomain, new Vector3(this.transform.position.x,
                this.transform.position.y + 6, 0), Quaternion.Euler(0, 0, 0));
                if (this.transform.position.x < 0)
                {
                    Instantiate(chest, new Vector3(this.transform.position.x,
                    this.transform.position.y + 1.2f, 0), Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    Instantiate(chest, new Vector3(this.transform.position.x,
                    this.transform.position.y +1.2f, 0), Quaternion.Euler(0, 0, 0));
                }
                spriteRenderer.sprite = icebox[2];
                spriteRenderer.sortingOrder = 10;
                childRenderer.enabled = false;
                break;
            case platformType.SHORT_PLATFORM:
                length = 3f;
                color = Color.white;
                spriteRenderer.sprite = icebox[0];
                spriteRenderer.sortingOrder = 10;
                childRenderer.enabled = false;
                break;
            case platformType.MOVING_PLATFORM:
                length = 2f;
                spriteRenderer.sprite = icebox[1];
                spriteRenderer.sortingOrder = 10;
                childRenderer.enabled = false;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            if (!claimed)
            {
                claimed = true;
                gm.SetScore(gm.GetScore() + 10);
            }
            if (gm.player.GetComponent<PlayerMovment>().isGrounded)
            {
                col.collider.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        col.collider.transform.SetParent(null);
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [System.Serializable]
    public class SFX{
        public AudioClip Swing;
        public AudioClip Walk;
        public AudioClip Land;
    }
    private bool Jump = false;
    public GameObject HpBar;
    public LayerMask groundLayer;
    public GameObject groundCL;

    public bool isGrounded;
    public float speed;
    public float jump;
    float moveVelocity;
    public Rigidbody2D rb;
    private AudioSource audioS;
    private Animator anim;

    private Inventory inv;

    private bool inAttack = false;

    public bool shield = false;
    public LayerMask enemis;
    public Transform attackPoint;
    public float attackRange;

    public SFX sfx;

    public float YSpeed;

    public ParticleSystem drop;
 
    private bool dropPar = false;
    private GameManager gm;



    void Start()
    {
        anim = GetComponent<Animator>();
        inv = GetComponent<Inventory>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioS = GetComponent<AudioSource>();
        
        
    }

    void Update()
    {
        YSpeed = GetComponent<Rigidbody2D>().velocity.y;
        //Grounded?
        if (isGrounded){
            Jump = false;
            anim.SetBool("InAir", false);
            if (dropPar && YSpeed  == 0){
                drop.Play();
                audioS.clip = sfx.Land;
                audioS.loop = false;
                audioS.Play();
                dropPar = false;
            }
            
        }else{
            anim.SetBool("InAir", true);
            dropPar = true;
        }
        anim.SetFloat("YSpeed", YSpeed);
        if (isGrounded && !shield)
        {
            //jumping
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W))
            {
                Jump = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
            }

        }
        if (Input.GetMouseButtonDown(0)){
            Attack();
        }
        moveVelocity = 0;
        //Left Right Movement
        if (!shield){
            Move();
        }

    }

    void Move(){
        anim.SetBool("Runing", false);
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            this.transform.localScale = new Vector3(-1,1,1);
            anim.SetBool("Runing", true);
            moveVelocity = -speed;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            this.transform.localScale = new Vector3(1,1,1);
            anim.SetBool("Runing", true);
            moveVelocity = speed;
        }


        GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);
    }

    void Attack(){
        if(inv.getWeapon() == 1 && !inAttack && isGrounded && !Jump){
            anim.SetTrigger("attack");
            StartCoroutine(startAttack());
        }
    }

    public IEnumerator startAttack(){
        inAttack = true;
        float recSpeed = speed;
        float recJump = jump;
        speed = 0;
        jump = 0;
        yield return new WaitForSeconds(0.2f);
        audioS.clip = sfx.Swing;
        audioS.loop = false;
        audioS.Play();
        yield return new WaitForSeconds(0.1f);
        Collider2D [] hitedEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemis);
        foreach (Collider2D enemy in hitedEnemies){
            Debug.Log(enemy.name);
            enemy.GetComponent<EnemyP2P>().GetHit(GetComponent<Inventory>().getDamage());
        }
        yield return new WaitForSeconds(0.3f);
        inAttack = false;
        speed = recSpeed;
        jump = recJump;
    }


    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCL.transform.position, 0.2f, groundLayer);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void GetDamage(int damage){
        GetComponent<Inventory>().setHP(GetComponent<Inventory>().getHP() - damage);
        HpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<Inventory>().getHP() * 0.6f, 10f);
    }

    void OnBecameInvisible()
    {
        gm.GameOver();
    }

}

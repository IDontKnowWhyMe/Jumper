using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyP2P : MonoBehaviour
{
    public Transform AttackPoint;
    public LayerMask playerLayer;
    public float attackRange;
    public Transform startP;
    public Transform endP;
    private Transform target;

    public GameObject HpBar;

    private Animator anim;

    public int HP;
    private Rigidbody2D rb;
    private bool move = true;

    private GameManager gm;
    public int damage;
    private bool canAttack = true;

    public AudioSource hurt;

    
    public float speed;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        target = endP;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (transform.position.x >= endP.transform.position.x){
            target = startP;
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        }
        else if(transform.position.x <= startP.transform.position.x)
        {
            target = endP;
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
        }
        if (move){
            anim.SetBool("move", true);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), step);
        }
        else{
            anim.SetBool("move", false);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
        Gizmos.DrawWireSphere(startP.position, 0.5f);
        Gizmos.DrawWireSphere(endP.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startP.position, endP.position);
    }

    public void Attack(){
        anim.SetTrigger("attack");
        StartCoroutine(AttackEnum());
    }

    public void GetHit(int damage){
        anim.SetTrigger("Hit");
        hurt.Play();
        rb.AddForce(new Vector2(2f*gm.player.transform.localScale.x,2f),ForceMode2D.Impulse);
        HP -= damage;
        StartCoroutine(Knockup());
        HpBar.transform.localScale = new Vector3(HP, HpBar.transform.localScale.y, 1);
        if(HP <= 0){
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death(){
        anim.SetTrigger("Death");
        GameObject.Destroy(HpBar.transform.parent.gameObject);
        this.GetComponent<EnemyP2P>().enabled = false;
        yield return new WaitForSeconds(20f);
        GameObject.Destroy(this.gameObject);
    }

    private IEnumerator Knockup(){
        move = false;
        yield return new WaitForSeconds(0.5f);
        move = true;
    }

    private IEnumerator AttackEnum(){
        move = false;
        yield return new WaitForSeconds(0.3f);
        if (Physics2D.OverlapCircle(AttackPoint.position, attackRange, playerLayer)){
            gm.player.GetComponent<PlayerMovment>().GetDamage(damage);
            yield return new WaitForSeconds(0.3f);
        }
        else{
            anim.SetTrigger("attackCancle");
            move = true;
        }
    }

    private IEnumerator AttackCD(){
        canAttack = false;
        yield return new WaitForSeconds(1.5f);
        canAttack = true;
    }

    private void FixedUpdate() {
        if (Physics2D.OverlapCircle(AttackPoint.position, attackRange, playerLayer)){
            move = false;
            if (canAttack){
                Attack();
                StartCoroutine(AttackCD());
            }
        }
        else
            move = true;
    }



}

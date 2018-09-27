using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour {

    string state = "idle";
    bool canSeePlayer = false;
    static int DEF_BORED = 200;
    static int DEF_TIRED = 200;
    int bored = DEF_BORED;
    int tired = DEF_TIRED;

    float gravity = 100;
    int Damping = 6;

    float Distance = 0f;
    public GameObject Target;
   // public CharacterController controller;
    float moveSpeed = 1.5f;

    float attackTime = 0f;
    float attackRepeatTime = 1.0f;

    float lookAtDistance = 20;
    float chaseRange = 20;
    float attackRange = 2.2f;

    private Vector3 moveDirection = Vector3.zero;


    Animator animationControl;

    // Use this for initialization
    void Start() {
        animationControl = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update() {
        Target = FindClosestPlayer();
        //Debug.Log("state " + bored);
        Distance = Vector3.Distance(Target.transform.position, transform.position);
       // Debug.Log("distance " + Distance);
        if (Distance < lookAtDistance)
        {
            lookAt();
        }

        if (Distance < attackRange)
        {
            attack();

        }
        else if (Distance < chaseRange)
        {
            chase();
        }


    }

    void attack()
    {

        if (Time.time > attackTime)
        {

            animationControl.Play("attack");
            var health = Target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
            var hud = Target.GetComponent<PlayerHUDControl>();
            if(hud != null)
            {
                hud.gotHurtBy(this.gameObject);
            }
            attackTime = Time.time + attackRepeatTime;
        }
    }

    void lookAt()
    {

        var rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
    }

    void chase()
    {
        animationControl.Play("walk");

        moveDirection = new Vector3(0, 0, 1);
        moveDirection *= moveSpeed;
        moveDirection.y -= gravity * Time.deltaTime;
    //    transform.Translate(0, 0, Random.Range(0, 0.03f));
        transform.Translate(moveDirection * Time.deltaTime);
        // controller.Move(moveDirection * Time.deltaTime);
    }

    void ApplyDamage()
    {
        chaseRange += 30;
        moveSpeed += 2;
        lookAtDistance += 40;
    }

    public GameObject FindClosestPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }


}

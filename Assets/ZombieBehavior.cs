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

    Animator animationControl;

    // Use this for initialization
    void Start() {
        animationControl = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log("state " + bored);
        switch (state)
        {
            case ("idle"):
                bored--;
                animationControl.Play("idle");
                if (bored == 0)
                {
                    state = "wandering";
                    tired = DEF_TIRED;
                }
                break;
            case ("walking"):


                break;
            case ("wandering"):
                wander();
                tired--;

                if (tired == 0)
                {
                    state = "idle";
                    bored = DEF_BORED;
                }
                break;
        }
    }


    float angular = 0.0f;

    void wander()
    {
        angular += Random.Range(-0.1f, 0.1f);
        animationControl.Play("walk");
        transform.Rotate(0,angular , 0);
        transform.Translate(0, 0, Random.Range(0, 0.03f));
    }

}

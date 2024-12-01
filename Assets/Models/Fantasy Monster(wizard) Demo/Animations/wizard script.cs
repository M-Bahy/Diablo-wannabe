using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_bahy : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    int state = 0;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(state > 0){
                state--;
                anim.SetInteger("state", state);
            }
            else{
                state = 6;
                anim.SetInteger("state", state);
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(state < 6){
                state++;
                anim.SetInteger("state", state);
            }
            else{
                state = 0;
                anim.SetInteger("state", state);
            }
        }
    }
}

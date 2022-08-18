using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Animator anim=null;
    private Rigidbody2D rb=null;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalkey=Input.GetAxis("Horizontal");
        float xSpeed=0.0f;

        if(horizontalkey>0)
        {
            transform.localScale=new Vector3(5,5,5);
            anim.SetBool("run",true);
            xSpeed=speed;
        }
        else if(horizontalkey<0){
            transform.localScale=new Vector3(-5,5,5);
            anim.SetBool("run",true);
            xSpeed=-speed;
        }
        else{
            anim.SetBool("run",false);
            xSpeed=0.0f;
        }
        rb.velocity=new Vector2(xSpeed,rb.velocity.y);

    }
}

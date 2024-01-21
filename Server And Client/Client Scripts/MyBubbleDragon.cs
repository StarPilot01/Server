using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Define;
public class MyBubbleDragon : BubbleDragon
{

    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        base.Update();

    }

    void CheckInput()
    {



        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            C_RequestMove reqPkt = new C_RequestMove();
            reqPkt.id = _id;
            reqPkt.isRight = true;
            GameManager.Instance.Send(reqPkt);

            //Debug.Log("Down");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            C_RequestMove reqPkt = new C_RequestMove();
            reqPkt.id = _id;
            reqPkt.isRight = false;
            GameManager.Instance.Send(reqPkt);

            // Debug.Log("Down");

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            C_RequestJump reqPkt = new C_RequestJump();
            reqPkt.id = _id;

            GameManager.Instance.Send(reqPkt);


        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            C_MakeBubble reqPkt = new C_MakeBubble();

            bool isRight = GetComponent<SpriteRenderer>().flipX;




            reqPkt.id = _id;
            reqPkt.posX = isRight ? transform.position.x + diffBubblePos : transform.position.x - diffBubblePos;
            reqPkt.posY = transform.position.y;
            reqPkt.isRight = isRight;


            GameManager.Instance.Send(reqPkt);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {

            C_MakeBubble360 reqPkt = new C_MakeBubble360();
            reqPkt.id = _id;

            GameManager.Instance.Send(reqPkt);



        }
        else if(Input.GetKeyDown(KeyCode.V)) 
        {
            C_RequestHeal reqPkt = new C_RequestHeal();
            reqPkt.id = _id;

            GameManager.Instance.Send(reqPkt);
        }
       


        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            C_RequestStop reqPkt = new C_RequestStop();
            reqPkt.id = _id;
            GameManager.Instance.Send(reqPkt);

            //Debug.Log("Up");

        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            C_RequestStop reqPkt = new C_RequestStop();
            reqPkt.id = _id;
            GameManager.Instance.Send(reqPkt);

            //Debug.Log("Up");
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
        }



    }
  
    

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }




}

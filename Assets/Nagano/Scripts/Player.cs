using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 public class Player : MonoBehaviour
 {
     //インスペクターで設定する
     public float speed; //速度
     public float gravity; //重力
     public float jumpSpeed;//ジャンプする速度
     public float jumpHeight;//高さ制限
     public float jumpLimitTime;//ジャンプ制限時間
     public GroundCheck ground; //接地判定
     public GroundCheck head;//頭ぶつけた判定
     public AnimationCurve dashCurve;  //New
     public AnimationCurve jumpCurve;  //New

     //プライベート変数
     private Animator anim = null;
     private Rigidbody2D rb = null;
     private bool isGround = false;
     private bool isJump = false;
     private bool isHead = false;
     private float jumpPos = 0.0f;
     private float dashTime, jumpTime;  //New
     private float beforeKey;  //New

     void Start()
     {
          //コンポーネントのインスタンスを捕まえる
          anim = GetComponent<Animator>();
          rb = GetComponent<Rigidbody2D>();
     }

     void FixedUpdate()
     {
          //接地判定を得る
          isGround = ground.IsGround();
          isHead = head.IsGround();

          //キー入力されたら行動する
          float horizontalKey = Input.GetAxis("Horizontal");
          float xSpeed = 0.0f;
          float ySpeed = -gravity;
          float verticalKey = Input.GetAxis("Jump");

          if (isGround)
          {
              if (verticalKey > 0)
              {
                  ySpeed = jumpSpeed;
                  jumpPos = transform.position.y; //ジャンプした位置を記録する
                  isJump = true;
                  jumpTime = 0.0f;
              }
              else
              {
                  isJump = false;
              }
          }
          else if (isJump)
          {
              //上方向キーを押しているか
　　　　　　　bool pushUpKey = verticalKey > 0;
　　　　　　　//現在の高さが飛べる高さより下か
　　　　　　　bool canHeight = jumpPos + jumpHeight > transform.position.y;
　　　　　　　//ジャンプ時間が長くなりすぎてないか
　　　　　　　bool canTime = jumpLimitTime > jumpTime;

　　　　　　　if (pushUpKey && canHeight && canTime && !isHead)
              {
                  ySpeed = jumpSpeed;
                  jumpTime += Time.deltaTime;
              }
              else
              {
                  isJump = false;
                  jumpTime = 0.0f;
              }
          }

          if (horizontalKey > 0)
          {
              transform.localScale = new Vector3(5, 5, 5);
              anim.SetBool("run", true);
              dashTime += Time.deltaTime;  //New
              xSpeed = speed;
          }
          else if (horizontalKey < 0)
          {
              transform.localScale = new Vector3(-5, 5, 5);
              anim.SetBool("run", true);
              dashTime += Time.deltaTime;  //New
              xSpeed = -speed;
          }
          else
          {
              anim.SetBool("run", false);
              xSpeed = 0.0f;
              dashTime = 0.0f;  //New
          }

          //前回の入力からダッシュの反転を判断して速度を変える New
          if (horizontalKey > 0 && beforeKey < 0)
          {
              dashTime = 0.0f;
          }
          else if (horizontalKey < 0 && beforeKey > 0)
          {
              dashTime = 0.0f;
          }
          beforeKey = horizontalKey;

          //アニメーションカーブを速度に適用 New
          xSpeed *= dashCurve.Evaluate(dashTime);
          if (isJump)
          {
              ySpeed *= jumpCurve.Evaluate(jumpTime);
          }
          anim.SetBool("jump", isJump); //New
          anim.SetBool("ground", isGround); //New
          rb.velocity = new Vector2(xSpeed, ySpeed);
      }
 }
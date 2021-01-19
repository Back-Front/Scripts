using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//コンポーネント強制アタッチ
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //======コンポーネント宣言======
    new Rigidbody rigidbody;
    Animator animator;

    //======変数宣言======
    //キャラ情報
    float speed = 2.5f;//キャラの移動速度

    //移動系
    float horizontal;//X軸入力取得用
    float vertical;//Y軸入力取得用
    Vector3 move;//移動情報
    float angleSpeed = 6f;//プレイヤー回転速度

    //調整用
    int gravity = 100;//キャラにかかる重力

    //Animatorパラメータをハッシュ値に変換
    int walk_id = Animator.StringToHash("walk");

    void Start()
    {
        //コンポーネント取得
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        //キー入力
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //horizontal,verticalの値を代入
        move = new Vector3(horizontal, 0, vertical);

        //カメラの向きに対応した移動方向
        float y = Camera.main.transform.rotation.eulerAngles.y;
        move = Quaternion.Euler(0, y, 0) * move;

        //プレイヤーObjに力を加える（移動）
        rigidbody.velocity = move * speed;

        //歩行時
        if (move.sqrMagnitude > 0)
        {
            //振り向き時、自然な動きになるように
            Quaternion tmpQ = Quaternion.LookRotation(move.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, tmpQ, Time.deltaTime * angleSpeed);
            //歩行アニメーションON
            animator.SetInteger(walk_id, 1);
        }
        //非歩行時
        else
        {
            //歩行アニメーションOFF
            animator.SetInteger(walk_id, 0);
        }
    }
}

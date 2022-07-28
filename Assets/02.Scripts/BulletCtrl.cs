using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    //총알의 파괴력
    public float damage=20.0f;

    //총알 발사 힘
    public float force=1500.0f;

    private Rigidbody rb;
    void Start()
    {
        //Rigidbody 컴포넌트 추출
        rb=GetComponent<Rigidbody>();

        //총알의 전진방향으로 힘(force)을 가한다
        rb.AddForce(transform.forward * force);
    }

    void Update()
    {
        
    }
}

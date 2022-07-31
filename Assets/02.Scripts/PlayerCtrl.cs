using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    //컴포넌트를 캐시처리할 변수
    private Transform tr;
    //Animation 컴포넌트를 저장할 변수
    private Animation anim;

    //이동 속도 변수
    public float MoveSpeed=10.0f;
    public float turnSpeed=80.0f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //컴포넌트를 추출해 변수에 대입
        tr=GetComponent<Transform>();
        anim = GetComponent<Animation>();

        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        float h=Input.GetAxis("Horizontal");
        float v=Input.GetAxis("Vertical");
        float r=Input.GetAxis("Mouse X");

        //Debug.Log("h 수평 입력값은:" + h + " v 수직 입력값은:"+v);

        //Transform 컴폰넌트의 position 속성값을 변경.
        //transform.position += new Vector3(0,0,1);

        //정규화 벡터를 사용한 코드
        //tr.position+=Vector3.forward*1;

        //전후좌우 이동 방향 벡터 계산
        Vector3 MoveDir = (Vector3.forward * v)+(Vector3.right * h);
        
        //Translate 함수를 사용한 이동 로직
        tr.Translate(MoveDir.normalized * MoveSpeed * Time.deltaTime);

        //Vector3.up 축을 기준으로 turnSpeed만큼의 속도로 회전.
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime *r);

        PlayerAnim(h,v);
    }

    void PlayerAnim(float h, float v)
    {
        //키보드의 입력값을 기주능로 동작할 애니메이션 수행
        if(v>=0.1f)
        {
            anim.CrossFade("RunF",0.25f);//전진 애니메이션 실행
        }
        else if(v<=-0.1f)
        {
            anim.CrossFade("RunB",0.25f);//후진 애니메이션 실행
        }
        else if(h>=0.1f)
        {
            anim.CrossFade("RunR",0.25f);//오른쪽 이동 애니메이션 실행
        }
        else if(h<=-0.1f)
        {
            anim.CrossFade("RunL",-0.25f);//왼쪽 이동 애니메이션 실행
        }
        else
        {
            anim.CrossFade("Idle",0.25f);//정지 시 Idle 애니메이션 실행
        }
    }
}

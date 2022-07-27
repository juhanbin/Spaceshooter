using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    //컴포넌트를 캐시처리할 변수
    private Transform tr;
    //이동 속도 변수
    public float MoveSpeed=10.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Transform 컴포넌트를 추출해 변수에 대입
        tr=GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float h=Input.GetAxis("Horizontal");
        float v=Input.GetAxis("Vertical");

        //Debug.Log("h 수평 입력값은:" + h + " v 수직 입력값은:"+v);

        //Transform 컴폰넌트의 position 속성값을 변경.
        //transform.position += new Vector3(0,0,1);

        //정규화 벡터를 사용한 코드
        //tr.position+=Vector3.forward*1;

        //전후좌우 이동 방향 벡터 계산
        Vector3 MoveDir = (Vector3.forward * v)+(Vector3.right * h);
        
        //Translate 함수를 사용한 이동 로직
        tr.Translate(MoveDir.normalized * MoveSpeed * Time.deltaTime);
    }
}

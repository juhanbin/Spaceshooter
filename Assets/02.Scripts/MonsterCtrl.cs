using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//네비게이션 기능을 사용하기 위해 추가해야 하는 네임스페이스
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    //컴포넌트 캐시를 처리할 변수
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;

    void Start()
    {
        //몬스터 Transform 할당
        monsterTr = GetComponent<Transform>();
        //추적 대상인 player의 Transform 할당
        playerTr =  GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        //NamMeshAgent 컴포넌트 할당
        agent = GetComponent<NavMeshAgent>();

        //추적 대상의 위치를 설정하면 바로 추적 시작
        agent.destination = playerTr.position;
    }

    void Update()
    {
        
    }
}

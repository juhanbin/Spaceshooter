using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //몬스터가 출현할 위치를 저장할 배열
    //public Transform[] points;
    //몬스터가 출현할 위치를 저장할 List 타입 변수
    public List<Transform> points = new List<Transform>();

    //몬스터 프리팹을 연결할 변수
    public GameObject monster;

    //몬스터의 생성 간격
    public float createTime =3.0f;

    //게임 종료여부를 저장할 멤버변수
    private bool isGameOver;

    //게임의 종료 여부를 저장할 프로퍼티
    public bool IsGameOver
    {
        get{return isGameOver;}
        set
        {
            isGameOver = value;
            if(isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    void Start()
    {
        //SpawnPointGroup 게임오브젝트의 Transform 컴포넌트 추출
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        //SpawPointGroup 하위에 있는 모든 차일드 게임오브젝트의 Transform 컴포넌트 추출
        //points = spawnPointGroup.GetComponentsInChildren<Transform>();
        //spawnPointGroup?.GetComponentsInChildren<Transform>(points);

        foreach(Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        monster = Resources.Load<GameObject>("Monster");

        //일정한 시간 간격으로 함수를 호출
        InvokeRepeating("CreateMonster",2.0f,createTime);
    }

    void CreateMonster()
    {
        //몬스터의 불규칙한 생성 위치 산출
        int idx = UnityEngine.Random.Range(0,points.Count);

        //몬스터 프리펩 생성
        Instantiate(monster,points[idx].position,points[idx].rotation);
    }
}

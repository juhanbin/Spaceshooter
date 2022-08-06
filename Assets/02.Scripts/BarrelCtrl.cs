using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    //폭발 효과 파티클을 연결할  변수
    public GameObject expEffect;

    //컴포넌트를 저장할 변수
    private Transform tr;
    private Rigidbody rb;

    //총알 맞은 횟수를 누적시킬 변수
    private int hitCount=0;

    //무작위로 적용할 텍스쳐 배열
    public Texture[] Textures;
    //하위에 있는 Mesh Renderer 컴포넌트를 저장할 변수
    private new MeshRenderer renderer;

    //폭발 반경
    public float radius=10.0f;

    //사운드
    public new AudioSource audio;
    public AudioClip boomSound;

    //hp형식으로 폭발 트리거
    private int hp_Barrel = 300;
    private bool dead = false;
    

    void Start()
    {
        tr=GetComponent<Transform>();
        rb=GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

        //하위에 있는 MeshRederer 컴포넌트를 추출
        renderer = GetComponentInChildren<MeshRenderer>();

        //난수 발생
        int idx=UnityEngine.Random.Range(0,Textures.Length);
        //텍스쳐 지정
        renderer.material.mainTexture=Textures[idx];
    }
    //충돌 시 발생하는 콜백 함수
    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.CompareTag("BULLET"))
        {
            if(hp_Barrel > 0){
                hp_Barrel -= 100;
                if(hp_Barrel <= 0)
                {
                    exp();
                }
            }
        }
    }

    void exp(){
        Invoke("ExpBarrel", 0.5f);
    }

    //드럼통을 폭발 시킬 함수
    void ExpBarrel()
    {
        //폭발 효과 파티클 생성
        GameObject exp=Instantiate(expEffect, tr.position, Quaternion.identity);
        //폭발 효과 파티클 5초 후에 제거
        Destroy(exp,5.0f);

        //터지는 소리 효과
        audio.clip = boomSound;
        audio.Play();

        // //Rigidbody 컴포턴트의 mass를 1.0으로 수정해 무게를 가볍게 함
        // rb.mass=1.0f;
        // //위로 솟구치는 힘을 가함
        // rb.AddForce(Vector3.up*1500.0f);

        //간접 폭발력 전달
        IndirectionDamage(tr.position);

        //3초 후에 드럼통 제거
        Destroy(gameObject,3.0f);
    }

    //결과값을 저장할 정적 배열을 미리 선언
    //Collider[] colls = new Collider[30];
    //폭발력을 주변에 전달하는 함수
    void IndirectionDamage(Vector3 pos)
    {
        //주변에 있는 드럼통을 모두 추출
        //가비지 컬렉션이 발생
        Collider[] colls=Physics.OverlapSphere(pos,radius,1<<3);
        Collider[] monsters=Physics.OverlapSphere(pos,radius,1<<7);
        Collider[] players=Physics.OverlapSphere(pos,radius,1<<9);

        //가비지컬렉션이 발생하지 않음
        //Physics.OverlapSphereNonAlloc(pos,radius,colls,1<<3);
        
        foreach(var coll in colls)
        {
            //폭발 범위에 포함된 드럼통의 Rigidbody 컴포넌트 추출
            rb=coll.GetComponent<Rigidbody>();
            //드럼통의 무게를 가볍게 함
            rb.mass=1.0f;
            //freezeRotation 제한값을 해제
            rb.constraints=RigidbodyConstraints.None;
            //폭발력을 전달
            rb.AddExplosionForce(1500.0f,pos,radius, 1200.0f);

            int damage = 300;
            Transform transform = coll.GetComponent<Transform>();
            if(GetDistance(pos, transform.position) > 5){
                int distance = (int) GetDistance(pos, transform.position);
                damage -= distance * 10;
            }
            
            BarrelCtrl br = coll.GetComponent<BarrelCtrl>();
            if(br.hp_Barrel > 0){
                br.hp_Barrel -= damage;
                if(br.hp_Barrel <= 0)
                {
                    br.exp();
                }
            }
        }

        foreach(var monster in monsters){
            int damage = 300;
            Transform transform = monster.GetComponent<Transform>();
            if(GetDistance(pos, transform.position) > 5){
                int distance = (int) GetDistance(pos, transform.position);
                damage -= distance * 10;
            }
            
            MonsterCtrl br = monster.GetComponent<MonsterCtrl>();
            if(br.hp > 0){
                br.hp -= damage;
                if(br.hp<=0)
                {
                    br.state=MonsterCtrl.State.DIE;
                    //몬스터가 사망 했을때 50점을 추가
                    GameManager.instance.DisplayScore(50);
                }
            }
        }
        foreach(var player in players){
            int damage = 300;
            Transform transform = player.GetComponent<Transform>();
            if(GetDistance(pos, transform.position) > 5){
                int distance = (int) GetDistance(pos, transform.position);
                damage -= distance * 10;
            }
            
            PlayerCtrl br = player.GetComponent<PlayerCtrl>();
            if(br.currHp > 0){
                br.currHp -= damage;
                br.DisplayHealth();

                if(br.currHp<=0)
                {
                    br.PlayerDie();
                }
            }
        }
    }

    double GetDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2) + Mathf.Pow(pos1.z - pos2.z, 2));
    }
}

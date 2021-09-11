using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float angleOffset = 0.0f;    
    public Transform prefabExplosion;

    // 基础属性
    public float speedModifier = 0.5f;
    public int hp = 2;

    // 掉落道具
    [SerializeField] private float dropProbability = 1.0f;
    [SerializeField] private Transform prefabDropItem;  // 一种敌人掉落一种道具

    // 发射子弹
    private bool isVisible = false;
    private float FireGap = 1.0f;
    [SerializeField] public Transform prefabBullet;
    private bool fireAllowed = true;  // 用于锁开火协程

    // 贝塞尔移动
    [HideInInspector]
    public Transform[] routes;  // 每个route实际上就是四个贝塞尔曲线控制点，由EnemySpawner决定

    private int routeToGo;
    private float paramT;  // bezier Param t
    private Vector2 positionToGo;
    private Vector2 tangentToGo;  // 切线向量
    private bool coroutineAllowed;
    private Vector2[] controlPoints;

    private void Start() {
        routeToGo = 0;
        paramT = 0.0f;
        coroutineAllowed = true;
        controlPoints = new Vector2[4];
    }
    private void Update()
    {
        // Move
        if(coroutineAllowed){
            StartCoroutine(GoByTheRoute(routeToGo));
        }
        if(fireAllowed && isVisible){
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire(){
        fireAllowed = false;
        while(isVisible){
            Transform bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(FireGap);
        }
        fireAllowed = true;
    }
    private void OnBecameVisible() {
        isVisible = true;
    }
    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            hp -= other.GetComponent<BulletController>().damage;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                Transform transExplosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
                if(Random.Range(0,1) < dropProbability)
                {
                    Instantiate(prefabDropItem, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("DeathSideBoundary"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator GoByTheRoute(int routeIndex){
        coroutineAllowed =false;
        for(int i = 0; i < 4; i++){
            controlPoints[i] = routes[routeIndex].GetChild(i).position;
        }

        while( paramT < 1){
            paramT += Time.deltaTime * speedModifier;
            positionToGo = Bezier();
            tangentToGo = BezierDerivative();
            
            transform.position = positionToGo;
            transform.rotation = Quaternion.Euler(0, 0, angleOffset + Mathf.Rad2Deg * Mathf.Atan2(tangentToGo.y, tangentToGo.x));
            yield return new WaitForEndOfFrame();
        }

        paramT = 0.0f;
        routeToGo += 1;

        if(routeToGo > routes.Length - 1){
            routeToGo = 0;
            coroutineAllowed = false;
        }else{
            coroutineAllowed = true;
        }
    }

    private Vector2 Bezier(){
        return controlPoints[0] * Mathf.Pow(1 - paramT, 3) + 
                3 * controlPoints[1] *  Mathf.Pow(1-paramT, 2) * paramT +
                3 * controlPoints[2] * (1-paramT) * Mathf.Pow(paramT, 2) +
                    controlPoints[3] * Mathf.Pow(paramT, 3) ;
    }
    private Vector2 BezierDerivative(){
        return -3 * controlPoints[0] * Mathf.Pow(1 - paramT, 2) +
                 3 * controlPoints[1] * (Mathf.Pow( 1 - paramT, 2) - 2 * paramT * (1 - paramT)) +
                 3 * controlPoints[2] * (2 * paramT * (1-paramT) - Mathf.Pow(paramT,2)) + 
                 3 * controlPoints[3] * Mathf.Pow(paramT, 2);
    }
}

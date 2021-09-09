using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRoutes : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;  // 每个route实际上就是四个贝塞尔曲线控制点
    private int routeToGo;
    private float paramT;  // bezier Param t
    private Vector2 positionToGo;
    private Vector2 tangentToGo;  // 切线向量

    [SerializeField]
    private float speedModifier = 0.5f;
    private bool coroutineAllowed;
    private Vector2[] controlPoints;

    private void Start() {
        routeToGo = 0;
        paramT = 0.0f;
        coroutineAllowed = true;
        controlPoints = new Vector2[4];
    }
    private void Update() {
        if(coroutineAllowed){
            StartCoroutine(GoByTheRoute(routeToGo));
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
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(tangentToGo.y, tangentToGo.x));
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

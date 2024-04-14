using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Camera : MonoBehaviour
{

    public GameObject Target; // 목표 타겟

    void Start()
    {
        
    }

    void Update()
    {
        if (Target != null)
        {
            // 카메라가 타겟의 위치로 이동
            this.transform.position = Target.transform.position + new Vector3(0, 2f, -10f); // this 는 Controller_Camera 스크립트가 붙을 오브젝트
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Camera : MonoBehaviour
{

    public GameObject Target; // ��ǥ Ÿ��

    void Start()
    {
        
    }

    void Update()
    {
        if (Target != null)
        {
            // ī�޶� Ÿ���� ��ġ�� �̵�
            this.transform.position = Target.transform.position + new Vector3(0, 2f, -10f); // this �� Controller_Camera ��ũ��Ʈ�� ���� ������Ʈ
        }
    }
}

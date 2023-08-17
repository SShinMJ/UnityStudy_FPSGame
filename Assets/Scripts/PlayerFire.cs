using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적 : 마우스 오른쪽 버튼을 눌러 폭탄을 특정 위치에 생성하고, 정면으로 발사.
public class PlayerFire : MonoBehaviour
{
    // 폭탄 게임 오브젝트
    public GameObject bombPrf;
    // 발사 위치
    public GameObject firePosition;
    public float firePower = 5f;

    void Update()
    {
        // 마우스 오른쪽 버튼을 클릭하면
        if(Input.GetMouseButton(1))  // 마우스 왼쪽 : 0, 오른쪽 : 1, 휠 : 2
        {
            GameObject bombObj = Instantiate(bombPrf);
            // 폭탄 발사 위치를 fireposition 값으로 하여 특정 위치에 생성되게 한다.
            bombObj.transform.position = firePosition.transform.position;

            // 폭탄 오브젝트의 rigidBody 값을 가져온 후, 힘(폭팔)을 더한다.
            Rigidbody rigidbody = bombObj.GetComponent<Rigidbody>();
            // 카메라의 정면(forward)방향으로 firePower을 힘을 가진채, 질량의 영향을 받는다.(Impulse: 질량)
            rigidbody.AddForce(Camera.main.transform.forward * firePower, ForceMode.Impulse);
        }
    }
}

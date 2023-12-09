using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIFItem_TreeFruit : MonoBehaviour
{
    private Rigidbody fruitRigid = default;

    private void Start()
    {
        fruitRigid = transform.GetComponent<Rigidbody>();
    }

    // TODO: 중력이 활성화 되지 않는 문제 
    public void ActivateGravity() { fruitRigid.useGravity = true; }
    public void DeactivateGravity() { fruitRigid.useGravity = false; }

    // TODO: True 명령이 들어가지 않는 것 같다...  true는 들어가나 어디서 자꾸 false를 때린다. 
}

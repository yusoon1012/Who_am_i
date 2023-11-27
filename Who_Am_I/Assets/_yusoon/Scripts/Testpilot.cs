using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testpilot : MonoBehaviour
{
    public bool isAction=false;
    public bool isTalk = false;
    private float actionTimer = 0;
    private float actionRate = 0.3f;
    private CharacterController cc;
    private float speed = 5f;
    Vector3 moveDir;
    // Start is called before the first frame update
    void Start()
    {
        cc=GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cc.Move(moveDir*speed*Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(isAction==false)
            {

            isAction = true;
            Debug.Log("¾×¼ÇÁß");
            }
        }
        if(isAction)
        {
            actionTimer += Time.deltaTime;
        }
        if(actionTimer>=actionRate)
        {
            isAction=false;
            actionTimer = 0;
        }
    }
}

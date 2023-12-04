using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    public string npcName;
    // Start is called before the first frame update
    void Start()
    {
        GameEventManager.instance.miscEvent.onNpcTalked += Talk;
        
        npcName = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            QuestManager.Instance.CheckNpcTalked(npcName);
           
        }
    }
    private void Talk(string name)
    {
        Debug.Log(name + "Talk");   
    }

}

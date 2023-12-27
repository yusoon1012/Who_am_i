using UnityEngine;

public class StartNavigation : MonoBehaviour
{
    // 길안내 NPC 트랜스폼
    public Transform npcTf;
    // NPC 컨트롤러 트랜스폼
    public Transform npcControllerTf;

    // 길안내 NPC 가 활성화 된 상태인지 체크
    private bool npcOn = false;
    // NPC 에 접근한 상태인지 체크
    private bool enterNpc = false;

    private void OnTriggerEnter(Collider collision)
    {
        // 길안내 NPC 소환 지점에 플레이어 태그 오브젝트가 들어오면 실행
        if (collision.tag == "Player" && npcOn == false)
        {
            npcOn = true;
            enterNpc = true;
            npcControllerTf.GetComponent<NPCController>().navigationEnterNpc = true;
            npcTf.gameObject.SetActive(true);
        }
        else if (collision.tag == "Player" && npcOn == true && enterNpc == false)
        {
            enterNpc = true;
            npcControllerTf.GetComponent<NPCController>().navigationEnterNpc = true;
        }
    }     // OnTriggerEnter()

    private void OnTriggerExit(Collider collision)
    {
        // 길안내 NPC 소환 지점에 플레이어 태그 오브젝트가 나가면 실행
        if (collision.tag == "Player" && enterNpc == true)
        {
            enterNpc = false;
            npcControllerTf.GetComponent<NPCController>().navigationEnterNpc = false;
        }
    }     // OnTriggerExit()
}

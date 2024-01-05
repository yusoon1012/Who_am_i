using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThisAnimalData : MonoBehaviour
{
    [SerializeField]
    [Header("드랍 아이템")]
    private GameObject testPrefab = default;

    [SerializeField]
    [Header("해당 오브젝트 데이터")]
    public AnimalData data = default;

    private GameObject player = default;
    private NavMeshAgent navigation = default;
    private ThisAnimalAI animalAI = default;
    private Animator animator = default;
    private bool isPlayer = default;

    private SphereCollider collider = default;

    public bool isAction = false;

    private string NAME_CLONE = "(Clone)";
    private string NAME_NULL = "";

    private float MAX_WAIT_TIME = 10;

    private float DROP_FORCE = 10;
    private int DESTRUCTION_TIME = 5;

    private void Start()
    {
        // { 애니메이션 컴포넌트
        animator = GFunc.SetComponent<Animator>(this.gameObject);
        if (animator == null)
        {
            GFunc.SubmitNonFindText<Animator>(this.gameObject);
            return;
        }
        // } 애니메이션 컴포넌트
        // { AI 컴포넌트
        animalAI = GFunc.SetComponent<ThisAnimalAI>(this.gameObject);
        if (animalAI == null)
        {
            GFunc.SubmitNonFindText<ThisAnimalAI>(this.gameObject);
            return;
        }
        // } AI 컴포넌트
        // { AnimalsType 설정
        AnimalsType? type = new AnimalsType();

        type = SetAnimal(this.name);
        if (type == null)
        {
            Debug.LogError("animalsType이 null입니다.");
            return;
        }
        // } AnimalsType 설정
        // { AnimalData 설정
        data = new AnimalData();

        data = SettingAnimal(type.Value);
        if (data == null)
        {
            Debug.LogError("AnimalData가 null입니다.");
            return;
        }
        // } AnimalData 설정
        // { 네비게이션 컴포넌트
        navigation = GFunc.SetComponent<NavMeshAgent>(this.gameObject);
        if (navigation == null)
        {
            GFunc.SubmitNonFindText<NavMeshAgent>(this.gameObject);
            return;
        }

        navigation.speed = data.speed;
        // } 네비게이션 컴포넌트
        // { collider 설정
        collider = GFunc.SetComponent<SphereCollider>(this.gameObject);
        if (collider == null)
        {
            GFunc.SubmitNonFindText<SphereCollider>(this.gameObject);
            return;
        }

        collider.radius = 1;
        // } collider 설정
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(5);

        Hit(1);
    }

    private void Update()
    {
        if (isAction == false)
        {
            if (isPlayer == false)
            {
                animalAI.UnEscape();
            }
            else
            {
                animalAI.Escape(player.transform.position);
            }
        }
    }

    public void Hit(int _damage)
    {
        data.hp = data.hp - _damage;

        if (data.hp <= 0)
        {
            GameObject newObject = Instantiate(testPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            DropItem(newObject.GetComponent<Rigidbody>());

            collider.enabled = false;

            StartCoroutine(Death());
        }
    }

    private void DropItem(Rigidbody _rigidbody)
    {
        _rigidbody.AddForce(Vector3.up * DROP_FORCE, ForceMode.Impulse);

        Vector3 direction = Quaternion.Euler(0, GFunc.RandomValueInt(0, 360), 0) * Vector3.forward;
        _rigidbody.AddForce(direction * DROP_FORCE * 0.1f, ForceMode.Impulse);
    }

    #region 애니메이션
    private IEnumerator Death()
    {
        float timeElapsed = 0.0f;

        animator.SetTrigger("Death");

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {
            timeElapsed += Time.deltaTime;

            if (MAX_WAIT_TIME < timeElapsed)
            {
                Debug.LogError("무한 루프 방어 로직");
                yield break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(DESTRUCTION_TIME);

        Destroy(gameObject);
    }
    #endregion

    #region 충돌 처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayer = true;

            if (player == null) { player = other.gameObject; }

            animalAI.SetCoroutine();
            animalAI.Escape(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayer = false;
        }
    }
    #endregion

    #region 초기화
    //! AnimalsType을 오브젝트 .name과 비교하여 설정
    private AnimalsType? SetAnimal(string _name)
    {
        foreach (AnimalsType type in Enum.GetValues(typeof(AnimalsType)))
        {
            if (type.ToString() == _name.Replace(NAME_CLONE, NAME_NULL))
            {
                return type;
            }
        }

        return null;
    }

    //! 비교한 AnimalsType을 이용해 데이터 저장
    private AnimalData SettingAnimal(AnimalsType _animalsType)
    {
        if (GameManager.instance.animals[_animalsType] == null)
        {
            Debug.LogError("게임매니저의 Animals 딕션어리가 없거나 초기화되지 않았습니다.");
            return null;
        }

        return GameManager.instance.animals[_animalsType];
    }
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    #region members

    #region readonly members
    private readonly string NAME_CLONE = "(Clone)";
    private readonly string NAME_NULL = "";
    private readonly float MIN_ACTION_VALUE = 3f;
    private readonly float MAX_ACTION_VALUE = 5f;
    private readonly float DROP_FORCE = 10f;
    #endregion

    #region private members
    private GameObject dropItem;
    private AnimalData data;
    private NavMeshAgent nav;
    private List<Animator> anis;
    private AnimalsType? type;
    private Coroutine actionCoroutine;
    private Coroutine visibleCoroutine;
    #endregion

    #endregion

    #region Initialization and Setup
    private void Start()
    {
        InitializationDatas();
        InitializationGameObject();
        InitializationComponents();
        if (HasNullReference()) { return; }
        InitializationSetup();
    }

    private void InitializationDatas()
    {
        type = new AnimalsType();
        type = SetAnimal(this.name);
        data = new AnimalData();
        data = SettingAnimal(type.Value);
    }

    private void InitializationGameObject()
    {
        dropItem = GFunc.GetGameObjectToList(GameManager.instance.items, data.drop);
    }

    private void InitializationComponents()
    {
        nav = gameObject.GetComponent<NavMeshAgent>() ? gameObject.GetComponent<NavMeshAgent>() : null;
        anis = GFunc.GetChildComponentList<Animator>(this.gameObject);
    }

    private bool HasNullReference()
    {
        if (nav == null) { GFunc.SubmitNonFindText(this.gameObject, typeof(NavMeshAgent)); return true; }
        if (anis == null) { GFunc.SubmitNonFindText(this.gameObject, typeof(Animator)); return true; }

        return false;
    }

    private void InitializationSetup()
    {
        nav.speed = data.speed;
    }

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
        if (GameManager.instance.animals[_animalsType] != null)
        {
            return GameManager.instance.animals[_animalsType];
        }

        return null;
    }
    #endregion

    #region Animator and Coroutine and Movement
    private void StopTargetCoroutine(Coroutine _targetCoroutine)
    {
        if (_targetCoroutine != null) { _targetCoroutine = null; }
    }

    private void StartRandomCoroutine()
    {
        Debug.Log("하잇");

        switch (GFunc.RandomBool())
        {
            case true: actionCoroutine = StartCoroutine(Move()); break;
            case false: actionCoroutine = StartCoroutine(Wait()); break;
        }
    }

    private void StartDeathCoroutine()
    {
        StopCoroutine(actionCoroutine);
        actionCoroutine = StartCoroutine(Death());
    }

    private void SetGameObject()
    {
        StartCoroutine(SetCoroutine());
    }

    private IEnumerator SetCoroutine()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(data.respawn);

        gameObject.SetActive(true);
        Reset();
    }

    private IEnumerator Move()
    {
        foreach (Animator ani in anis) { ani.SetBool("Move", true); }
        transform.rotation = Quaternion.Euler(0, GFunc.RandomAngle(), 0);

        float timeElapsed = 0.0f;
        float duration = GFunc.RandomValueFloat(MIN_ACTION_VALUE, MAX_ACTION_VALUE);

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            transform.position += Vector3.forward * data.speed * Time.deltaTime;

            yield return null;
        }

        foreach (Animator ani in anis) { ani.SetBool("Move", false); }
        Debug.Log("여긴 들어오니 ?");
        StopTargetCoroutine(actionCoroutine);
    }

    private IEnumerator Wait()
    {
        float time = GFunc.RandomValueFloat(MIN_ACTION_VALUE, MAX_ACTION_VALUE);

        yield return new WaitForSeconds(time);
        Debug.Log("여긴 들어오니 ?");
        StopTargetCoroutine(actionCoroutine);
    }

    private IEnumerator Death()
    {
        foreach (Animator ani in anis) { ani.SetTrigger("Death"); }

        float timeElapsed = 0.0f;
        float duration = MAX_ACTION_VALUE;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        SetGameObject();
    }

    private void DropItem()
    {
        GameObject newObject = Instantiate(dropItem, transform.position + Vector3.up, Quaternion.identity);

        Rigidbody newRig = newObject.GetComponent<Rigidbody>() ? newObject.GetComponent<Rigidbody>() : null;
        if (newRig == null) { GFunc.SubmitNonFindText(newObject, typeof(Rigidbody)); return; }

        Vector3 direction = Quaternion.Euler(0, GFunc.RandomValueInt(0, 360), 0) * Vector3.forward;
        direction.Normalize();

        newRig.AddForce((Vector3.up + direction) * DROP_FORCE, ForceMode.Impulse);
    }

    private void Reset()
    {
        foreach (Animator ani in anis) { ani.SetTrigger("Reset"); }
        data.hp = 1;
    }
    #endregion

    #region Visible
    private void StartVisibleCoroutine()
    {
        visibleCoroutine = StartCoroutine(CoroutineUpdate());
    }

    private IEnumerator CoroutineUpdate()
    {
        while (true)
        {
            Debug.Log(actionCoroutine == null);
            if (actionCoroutine == null) { StartRandomCoroutine(); }
            yield return null;
        }
    }

    private void OnBecameInvisible()
    {
        StopTargetCoroutine(visibleCoroutine);
        StopTargetCoroutine(actionCoroutine);
    }

    private void OnBecameVisible()
    {
        StartVisibleCoroutine();
    }
    #endregion

    #region reference
    public void Hit(int _damage)
    {
        data.hp = data.hp - _damage;

        if (data.hp == 0) { DropItem(); }

        StartDeathCoroutine();
    }
    #endregion
}

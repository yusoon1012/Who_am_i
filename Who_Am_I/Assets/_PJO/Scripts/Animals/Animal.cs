using System;
using System.Collections;
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
    private Animator ani;
    private AnimalsType? type;
    private Coroutine actionCoroutine;
    private Coroutine visibleCoroutine;
    private Rigidbody rig;
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
        ani = gameObject.GetComponent<Animator>() ? gameObject.GetComponent<Animator>() : null;
        rig = gameObject.GetComponent<Rigidbody>() ? gameObject.GetComponent<Rigidbody>() : null;
    }

    private bool HasNullReference()
    {
        if (nav == null) { GFunc.SubmitNonFindText(this.gameObject, typeof(NavMeshAgent)); return true; }
        if (ani == null) { GFunc.SubmitNonFindText(this.gameObject, typeof(Animator)); return true; }
        if (rig == null) { GFunc.SubmitNonFindText(this.gameObject, typeof(Rigidbody)); return true; }

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
    private void StopCoroutine()
    {
        if (actionCoroutine != null) { StopCoroutine(actionCoroutine); }
    }

    private void StartRandomCoroutine()
    {
        if (actionCoroutine != null) { StopCoroutine(actionCoroutine); }

        switch (GFunc.RandomBool())
        {
            case true: actionCoroutine = StartCoroutine(Move()); break;
            case false: actionCoroutine = StartCoroutine(Wait()); break;
        }
    }

    private void StartDeathCoroutine()
    {
        StopCoroutine();
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
        ani.SetBool("Move", true);

        float timeElapsed = 0.0f;
        float duration = GFunc.RandomValueFloat(MIN_ACTION_VALUE, MAX_ACTION_VALUE);

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            rig.AddForce(Vector3.forward * data.speed);

            yield return null;
        }

        ani.SetBool("Move", false);
    }

    private IEnumerator Wait()
    {
        float time = GFunc.RandomValueFloat(MIN_ACTION_VALUE, MAX_ACTION_VALUE);

        yield return new WaitForSeconds(time);
    }

    private IEnumerator Death()
    {
        ani.SetTrigger("Death");

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
        ani.SetTrigger("Reset");
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
            if (actionCoroutine == null) { StartRandomCoroutine(); }
            yield return null;
        }
    }

    private void OnBecameInvisible()
    {
        if (visibleCoroutine != null) { StopCoroutine(visibleCoroutine); }
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

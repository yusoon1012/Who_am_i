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
    private readonly float MIN_ACTION_VALUE = 1f;
    private readonly float MAX_ACTION_VALUE = 3f;
    private readonly float DROP_FORCE = 5f;
    #endregion

    #region public members
    public AnimalData data;
    #endregion

    #region private members
    private GameObject dropItem;
    private NavMeshAgent nav;
    private List<Animator> anis;
    private AnimalsType? type;
    private Coroutine actionCoroutine;
    private Vector3 originScale;
    private bool isVisible;
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
        originScale = this.gameObject.transform.localScale;
        isVisible = false;
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
    private void StopActionCoroutine()
    {
        if (actionCoroutine != null)
        {
            StopCoroutine(actionCoroutine);
        }
    }

    private void StartAliveCoroutine(bool _isAllive)
    {
        StopActionCoroutine();

        if (_isAllive)
        {
            switch (GFunc.RandomBool())
            {
                case true: actionCoroutine = StartCoroutine(Move()); break;
                case false: actionCoroutine = StartCoroutine(Wait()); break;
            }
        }
        else { actionCoroutine = StartCoroutine(Death()); }
    }

    private void ResetGameObject()
    {
        foreach (Animator ani in anis) { ani.SetTrigger("Reset"); }
        data.hp = 1;
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

            nav.SetDestination(transform.position + transform.forward);

            yield return null;
        }

        foreach (Animator ani in anis) { ani.SetBool("Move", false); }

        StartAliveCoroutine(true);
    }

    private IEnumerator Wait()
    {
        float time = GFunc.RandomValueFloat(MIN_ACTION_VALUE, MAX_ACTION_VALUE);

        yield return new WaitForSeconds(time);

        StartAliveCoroutine(true);
    }

    private IEnumerator Death()
    {
        foreach (Animator ani in anis) { ani.SetTrigger("Death"); }

        yield return new WaitForSeconds(5f);

        this.gameObject.transform.localScale = Vector3.one * 0.001f;

        yield return new WaitForSeconds(data.respawn);

        this.gameObject.transform.localScale = originScale;

        ResetGameObject();

        if (isVisible == true) { StartAliveCoroutine(true); }
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

    private IEnumerator IsResurrection()
    {
        while (data.hp == 0)
        {
            yield return null;
        }

        StartAliveCoroutine(true);
    }
    #endregion

    #region Visible
    private void OnBecameInvisible()
    {
        isVisible = false;
        foreach (Animator ani in anis) { ani.SetBool("Move", false); }
        if (data.hp != 0) { StopActionCoroutine(); }
    }

    private void OnBecameVisible()
    {
        isVisible = true;
        if (data.hp == 0) { StartCoroutine(IsResurrection()); }
        else { StartAliveCoroutine(true); }
    }
    #endregion

    #region reference
    public void Hit(int _damage)
    {
        if (data.hp == 0) { return; }

        data.hp = data.hp - _damage;

        if (data.hp == 0) { DropItem(); }

        StartAliveCoroutine(false);
    }
    #endregion
}
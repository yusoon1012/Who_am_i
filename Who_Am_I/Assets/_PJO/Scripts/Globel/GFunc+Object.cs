using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using TMPro;

public static partial class GFunc
{
    /// <summary>현재 활성화된 씬에 있는 루트 오브젝트를 이름으로 찾는 메서드</summary>
    /// <param name="objectName_">찾고자 하는 오브젝트의 이름</param>
    public static GameObject GetRootObject(string objectName_)
    {
        // 현재 활성화된 Unity 씬
        Scene activeScene = SceneManager.GetActiveScene();
        // 씬의 루트에 있는 모든 GameObject 배열로 저장
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        // 루트 GameObject 배열을 순회하여 찾고자 하는 오브젝트를 찾기
        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.name == objectName_)
            {
                return rootObject;
            }
        }
        return null;
    }

    /// <summary>해당 오브젝트의 모든 자식 오브젝트를 List로 저장해 반환하는 메서드</summary>
    /// <param name="object_">자식 오브젝트를 가져올 부모 오브젝트</param>
    public static List<GameObject> GetChildrenObjectsList(this GameObject object_)
    {
        List<GameObject> objects = new List<GameObject>();
        GameObject childObject = default;

        for (int i = 0; i < object_.transform.childCount; i++)
        {
            childObject = object_.transform.GetChild(i).gameObject;
            objects.Add(childObject);
        }
        return objects.Count < 1 ? null : objects;
    }

    /// <summary>해당 오브젝트의 모든 자식 오브젝트를 Array로 저장해 반환하는 메서드</summary>
    /// <param name="object_">자식 오브젝트를 가져올 부모 오브젝트</param>
    public static GameObject[] GetChildrenObjectsArray(this GameObject object_)
    {
        GameObject[] objects = new GameObject[object_.transform.childCount];
        GameObject childObject = default;

        for (int i = 0; i < objects.Length; i++)
        {
            childObject = object_.transform.GetChild(i).gameObject;
            objects[i] = childObject;
        }
        return objects.Length < 1 ? null : objects;
    }

    /// <summary>해당 오브젝트의 자식들 중에서 특정 이름을 가진 오브젝트를 재귀적으로 찾아 반환하는 메서드</summary>
    /// <param name="object_">자식 오브젝트를 가져올 부모 오브젝트</param>
    /// <param name="objectName_">찾고자 하는 오브젝트의 이름</param>
    public static GameObject GetFindChildObject(this GameObject object_, string objectName_)
    {
        GameObject childObject = default;

        for (int i = 0; i < object_.transform.childCount; i++)
        {
            childObject = object_.transform.GetChild(i).gameObject;

            if (childObject.name.Equals(objectName_))
            {
                return childObject;
            }
            else
            {
                childObject = GetFindChildObject(childObject, objectName_);

                if (!(childObject == null || childObject == default)) { return childObject; }
            }
        }
        return null;
    }

    /// <summary>해당 오브젝트의 자식들 중에서 특정 이름을 가진 오브젝트를 재귀적으로 찾아 리스트에 저장 후 반환하는 메서드</summary>
    /// <param name="object_">자식 오브젝트를 가져올 부모 오브젝트</param>
    /// <param name="objectName_">찾고자 하는 오브젝트의 이름</param>
    /// <returns></returns>
    public static List<GameObject> GetFindAllObjects(GameObject object_, string objectName_)
    {
        GameObject childObject = default;
        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < object_.transform.childCount; i++)
        {
            childObject = object_.transform.GetChild(i).gameObject;

            if (childObject.name.Equals(objectName_))
            {
                list.Add(childObject);
            }
            else
            {
                List<GameObject> childList = GetFindAllObjects(childObject, objectName_);
                if (!(childList.Count < 1)) { list.AddRange(childList); }
            }
        }
        return list.Count < 1 ? null : list;
    }

    /// <summary>해당 오브젝트의 자식들 중에서 특정 이름을 가진 오브젝트를 BFS알고리즘을 이용해 찾아 반환하는 메서드</summary>
    /// <param name="object_">자식 오브젝트를 가져올 부모 오브젝트</param>
    /// <param name="objectName_">찾고자 하는 오브젝트의 이름</param>
    public static GameObject GetFindChildObjectBFS(GameObject object_, string objectName_)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        queue.Enqueue(object_);

        while (queue.Count > 0)
        {
            GameObject result = queue.Dequeue();

            if (result.name == objectName_) { return result; }

            for (int i = 0; i < result.transform.childCount; i++)
            {
                queue.Enqueue(result.transform.GetChild(i).gameObject);
            }
        }
        return null;
    }

    /// <summary>해당 오브젝트의 자식들 중에서 특정 이름을 가진 오브젝트를 BFS알고리즘을 이용해 찾아 리스트에 저장 후 반환하는 메서드</summary>
    /// <param name="object_">자식 오브젝트를 가져올 부모 오브젝트</param>
    /// <param name="objectName_">찾고자 하는 오브젝트의 이름</param>
    public static List<GameObject> GetFindAllObjectsBFS(GameObject object_, string objectName_)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        List<GameObject> list = new List<GameObject>();
        queue.Enqueue(object_);

        while (queue.Count > 0)
        {
            GameObject result = queue.Dequeue();

            if (result.name == objectName_) { list.Add(result); }

            for (int i = 0; i < result.transform.childCount; i++)
            {
                queue.Enqueue(result.transform.GetChild(i).gameObject);
            }
        }
        return list.Count < 1 ? null : list;
    }

    /// <summary>
    /// 주어진 이름으로 새로운 오브젝트를 만들고, 여러 개의 컴포넌트를 추가하는 메서드
    /// 사용법: CreateObject(newObject, typeof(SpriteRenderer), typeof(BoxCollider2D));
    /// </summary>
    /// <param name="objectName_">생성될 오브젝트의 이름</param>
    /// <param name="types_">추가할 컴포넌트 타입들의 배열</param>
    public static GameObject CreateObject(string objectName_, params Type[] types_)
    {
        GameObject newObject = new GameObject(objectName_);

        foreach (Type type in types_)
        {
            if (typeof(Component).IsAssignableFrom(type)) { newObject.AddComponent(type); }
            else { DebugNonFindComponentType(type); return null; }
        }
        return newObject;
    }

    /// <summary>주어진 문자열로부터 Type을 찾아 반환하는 메서드</summary>
    /// <param name="type_">찾고자 하는 Type의 이름</param>
    public static Type GetType(string type_)
    {
        string typeName = "UnityEngine." + type_;

        Type type = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .FirstOrDefault(p => p.FullName == typeName);

        return type != null ? type : null;
    }

    /// <summary>Unity에서 기본으로 제공하는 오브젝트를 생성하고 이를 반환하는 메서드</summary>
    /// <param name="type_">Unity에서 기본적으로 제공하는 PrimitiveType</param>
    public static GameObject PrimitiveObject(PrimitiveType type_)
    {
        if (!Enum.IsDefined(typeof(PrimitiveType), type_))
        {
            DebugNonFindPrimitiveType(type_);
            return null;
        }
        GameObject primitiveObject = GameObject.CreatePrimitive(type_);
        return primitiveObject;
    }

    /// <summary>배열을 섞는 셔플 메서드</summary>
    /// <typeparam name="T">배열의 요소 타입</typeparam>
    /// <param name="array_">섞을 배열</param>
    public static T[] ShuffleArray<T>(T[] array_)
    {
        T[] shuffledArray = array_.Clone() as T[];

        for (int i = shuffledArray.Length - 1; 0 <= i; i--)
        {
            int randIndex = RandomValueInt(0, array_.Length - 1);

            T temp = shuffledArray[i];
            shuffledArray[i] = shuffledArray[randIndex];
            shuffledArray[randIndex] = temp;
        }
        return shuffledArray;
    }

    /// <summary>주어진 문자열을 StringBuilder에 추가하고 그 결과를 TextMeshProUGUI에 할당하는 메서드</summary>
    /// <param name="value_">추가할 문자열</param>
    /// <param name="textType_">텍스트를 표시할 컴포넌트</param>
    /// <param name="stringBuilder_">문자열을 빌드하는 데 사용할 StringBuilder</param>
    public static void AppendStringBuilder(string value_, TMP_Text textType_, StringBuilder stringBuilder_)
    {
        if (stringBuilder_ == null) { return; }
        stringBuilder_.Clear();
        stringBuilder_.Append(value_);
        textType_.text = stringBuilder_.ToString();
    }

    /// <summary>주어진 리스트에서 특정 오브젝트를 이름 값으로 찾아오는 메서드</summary>
    /// <param name="list_">오브젝트가 모여 있는 리스트</param>
    /// <param name="value_">찾고 싶은 오브젝트의 이름</param>
    public static GameObject GetObjectToList(List<GameObject> list_, string value_)
    {
        if (list_.Count < 1) { return null; }

        foreach (GameObject gameObject in list_)
        {
            if (gameObject.name == value_)
            {
                return gameObject;
            }
        }
        return null;
    }

    /// <summary>자식 오브젝트의 특정 컴포넌트를 리스트로 가져오는 메서드</summary>
    /// <typeparam name="T">가져올 컴포넌트 타입</typeparam>
    /// <param name="object_">부모 오브젝트</param>
    public static List<T> GetChildObjectComponentToList<T>(this GameObject object_) where T : Component
    {
        List<T> list = new List<T>();

        foreach (Transform childTransform in object_.transform)
        {
            try
            {
                T component = childTransform.GetComponent<T>();
                if (component != null) { list.Add(component); }
            }
            catch (Exception e) { DebugNonFindComponent(childTransform.gameObject, typeof(T)); }
        }
        return list.Count < 1 ? null : list;
    }
}
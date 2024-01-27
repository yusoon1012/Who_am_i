using UnityEngine;
using UnityEditor;
using System;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(RemoveComponent))]
public class RemoveComponentEditor : Editor
{
    #region members

    #region Property members
    SerializedProperty propertyTargetObject;        // 컴포넌트를 제거할 오브젝트가 모여있는 최상위 오브젝트
    SerializedProperty propertyComponentTypeName;   // 제거할 컨포넌트 이름
    #endregion

    #region private members
    private GameObject targetObject;                // 컴포넌트를 제거할 오브젝트가 모여있는 최상위 오브젝트
    private Transform targetTransform;              // 컴포넌트를 제거할 오브젝트가 모여있는 최상위 위치
    private string componentTypeName;               // 제거할 컨포넌트 이름
    private Type componentType;                     // 컴포넌트 타입
    #endregion

    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        propertyTargetObject = serializedObject.FindProperty("propertyTargetObject");
        propertyComponentTypeName = serializedObject.FindProperty("propertyComponentTypeName");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(propertyTargetObject, new GUIContent("컴포넌트를 제거할 오브젝트가 모여있는 최상위 오브젝트"));
        EditorGUILayout.PropertyField(propertyComponentTypeName, new GUIContent("제거할 컨포넌트 이름"));

        // "Remove" 버튼 클릭시 해당 컴포넌트가 존재하는지 확인
        if (GUILayout.Button("Remove"))
        {
            EditorStart();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region Editor Initialization and Setup
    // 초기 데이터 초기화 메서드
    private void EditorStart()
    {
        InitializationObjects();
        InitializationComponents();
        InitializationValue();
        if (HasNullReference())
        { GFunc.DebugError(typeof(RemoveComponentEditor)); return; }

        EditorRemoveComponent(targetTransform);
    }

    // 초기 오브젝트 초기화 메서드
    private void InitializationObjects()
    {
        targetObject = GEFunc.GetPropertyGameObject(propertyTargetObject);
    }

    // 초기 컴포넌트 초기화 메서드
    private void InitializationComponents()
    {
        targetTransform = targetObject.GetComponent<Transform>() ? targetObject.GetComponent<Transform>() : null;
    }

    // 초기 값 초기화 메서드
    private void InitializationValue()
    {
        componentTypeName = GEFunc.GetPropertyString(propertyComponentTypeName);
        componentType = GFunc.GetType(componentTypeName);
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (targetObject == null) { GEFunc.DebugNonFind(propertyTargetObject, SerializedPropertyType.ObjectReference); return true; }
        if (targetTransform == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(Transform)); return true; }
        if (componentTypeName == null) { GEFunc.DebugNonFind(propertyComponentTypeName, SerializedPropertyType.String); return true; }
        if (componentType == null) { GFunc.DebugTypeToString(componentTypeName); return true; }

        return false;
    }
    #endregion

    #region Editor function start
    // 재귀적으로 특정 오브젝트 하위의 특정 컴포넌트를 제거하는 메서드
    private void EditorRemoveComponent(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if(child.gameObject.GetComponent(componentType))
            { 
                RemoveComponent(child);
            }

            if (child.childCount > 0)
            {
                EditorRemoveComponent(child);
            }
        }
    }

    // 한 오브젝트에 같은 컴포넌트가 있는지 체크하고 만약 있다면 제거
    private void RemoveComponent(Transform objectTransform)
    {
        Component[] components = objectTransform.GetComponents(componentType);

        foreach (Component component in components)
        {
            DestroyImmediate(component);
        }
    }
    #endregion
}
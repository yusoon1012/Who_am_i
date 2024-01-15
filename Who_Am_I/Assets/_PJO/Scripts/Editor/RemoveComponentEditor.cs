using UnityEngine;
using UnityEditor;
using System;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(RemoveComponent))]
public class RemoveComponentEditor : Editor
{
    #region Property members
    SerializedProperty targetObject;        // 컴포넌트제거할 오브젝트가 모여있는 최상위 오브젝트
    SerializedProperty componentTypeName;   // 제거할 컨포넌트 이름
    #endregion

    #region private members
    private Transform targetTransform;      // 최상위 오브젝트의 위치
    private Type componentType;             // 컴포넌트 타입
    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
        componentTypeName = serializedObject.FindProperty("componentTypeName");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("최상위 폴더"));
        EditorGUILayout.PropertyField(componentTypeName, new GUIContent("지울 컴포넌트"));

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
        InitializationComponents();
        if (HasNullReference()) { return; }

        EditorRemoveComponent(targetTransform);
    }

    // 초기 컴포넌트 초기화 메서드
    private void InitializationComponents()
    {
        targetTransform = GFuncE.SetTransform(targetObject);
        componentType = GFuncE.GetType(componentTypeName.stringValue);
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (targetTransform == null) { GFuncE.SubmitNonFindText(targetObject, typeof(Transform)); return true; }
        if (IsComponent() == false) { return true; }

        return false;
    }

    // 특정 컴포넌트가 있는지 유효성 검사하는 메서드
    private bool IsComponent()
    {
        if (componentType != null) { Debug.Log($"{componentTypeName.stringValue} is a valid Component type"); return true; }
        else { Debug.Log($"{componentTypeName.stringValue} is not a valid Component type"); return false; }
    }
    #endregion

    #region Editor function start
    // 특정 오브젝트 하위의 특정 컴포넌트 지우는 재귀함수
    private void EditorRemoveComponent(Transform _parent)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Transform child = _parent.GetChild(i);

            if (GFuncE.HasComponent(child.gameObject, componentType))
            {
                RemoveComponent(child);
            }

            if (GFuncE.HasChild(child))
            {
                EditorRemoveComponent(child);
            }
        }
    }

    // 한 오브젝트에 같은 컴포넌트가 있는지 체크하고 지우는 재귀함수
    private void RemoveComponent(Transform _object)
    {
        Component[] components = _object.GetComponents(componentType);

        foreach (Component component_ in components)
        {
            DestroyImmediate(component_);
        }
    }
    #endregion
}
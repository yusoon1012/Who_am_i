using UnityEngine;
using UnityEditor;
using System;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(RemoveComponent))]
public class RemoveComponentEditor : Editor
{
    // 컴포넌트제거할 오브젝트가 모여있는 최상위 오브젝트
    SerializedProperty targetObject;
    // 제거할 컨포넌트 이름
    SerializedProperty componentTypeName;

    // 최상위 오브젝트의 위치
    private Transform targetTransform;
    // 컴포넌트 타입
    private Type componentType;

    private void OnEnable()
    {
        // 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
        componentTypeName = serializedObject.FindProperty("componentTypeName");

        // 오브젝트의 위치 지정
        targetTransform = GFuncE.SetTransform(targetObject);
    }
    
    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("최상위 폴더"));
        EditorGUILayout.PropertyField(componentTypeName, new GUIContent("지울 컴포넌트"));

        /* 
         * IsComponent           : "Remove" 버튼 클릭시 해당 컴포넌트가 존재하는지 확인
        *  RecursiveAllComponent : 재귀적으로 오브젝트의 컴포넌트 제거
        */
        if (GUILayout.Button("Remove"))
        {
            if (IsComponent() == false) { return; }

            RecursiveAllComponent(targetTransform);
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }

    // 특정 컴포넌트가 있는지 유효성 검사하는 함수
    private bool IsComponent()
    {
        // Type 저장
        componentType = GFuncE.GetType(componentTypeName.stringValue);

        // 유효성 검사
        if (componentType != null)
        {
            Debug.Log($"{componentTypeName.stringValue} is a valid Component type");
            return true;
        }
        else
        {
            Debug.Log($"{componentTypeName.stringValue} is not a valid Component type");
            return false;
        }
    }

    // 특정 오브젝트 하위의 특정 컴포넌트 지우는 재귀함수
    private void RecursiveAllComponent(Transform _parent)
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
                RecursiveAllComponent(child);
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
}
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace DS.Windows
{

    public class DialogueEditorWindow : EditorWindow
    {
        [MenuItem("Window/DS/Dialogue Graph")]
        public static void ShowExample()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Graph");
        }
        private void OnEnable()
        {
            AddGraphView();
            AddStyles();
        }
        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView();
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSVariables.uss");
             rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}
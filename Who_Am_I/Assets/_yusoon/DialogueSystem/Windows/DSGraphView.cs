using DS.Elements;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddBackGround();
            AddStyles();
            AddManipulators();
            CreateNode();
        }
        private void AddBackGround()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyles.uss");
            styleSheets.Add(styleSheet);
        }
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
        }
        private void CreateNode()
        {
            DSNode node = new DSNode();
            node.Initialize();
            node.Draw();
            AddElement(node);
        }
    }
}

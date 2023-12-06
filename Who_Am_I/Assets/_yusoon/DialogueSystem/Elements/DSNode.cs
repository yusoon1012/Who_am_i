using System.Collections.Generic;

using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using UnityEngine.UIElements;

    //public class DSNode : Node
    //{
    //    public string dialogueName { get; set; }
    //    public List<string> choices { get; set; }
    //    public string[] text { get; set; }
    //    public DSDialogueType dialogueType { get; set; }
    //    public void Initialize()
    //    {
    //        dialogueName = "Dialogue Name";
    //        choices = new List<string>();
    //        text = new string[]{ "Dialogue Text"};
    //    }
    //    public void ApplyDialogueData(Dialogue dialogue)
    //    {
    //        dialogueName=dialogue.name;
    //        text = dialogue.contexts;
    //    }
    //    public void Draw()
    //    {
    //        TextField dialogueNameTextField = new TextField()
    //        {
    //            value = dialogueName
    //        };
    //        titleContainer.Insert(0,dialogueNameTextField);
    //        Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,typeof(bool));
    //        inputPort.portName = "Dialogue Connection";
    //        inputContainer.Add(inputPort);

    //        VisualElement customDataContainer = new VisualElement();
    //        Foldout textFoldout = new Foldout()
    //        {
    //            text ="Dialogue Text"
    //        };
    //        // 기존의 textFoldout 대신, 배열의 각 텍스트를 TextField로 생성하여 추가
    //        for (int i = 0; i < text.Length; i++)
    //        {
    //            TextField textTextField = new TextField()
    //            {
    //                value = text[i]
    //            };
    //            customDataContainer.Add(textTextField);
    //        }
            
    //        customDataContainer.Add(textFoldout);
    //        extensionContainer.Add(customDataContainer);
    //        RefreshExpandedState();
    //    }
    //}
}

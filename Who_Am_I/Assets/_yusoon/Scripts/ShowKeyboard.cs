using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using BNG;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField inputField;
    public VRKeyboard vrKeyboard;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        
    }

   public void OpenKeyboard()
    {
        vrKeyboard.gameObject.SetActive(true);
        vrKeyboard.AttachedInputField = inputField;
        
    }
    public void CloseKeyboard()
    {
        vrKeyboard.gameObject.SetActive(false);
        vrKeyboard.AttachedInputField = null;
    }
}

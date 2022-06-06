using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSpaceKey : MonoBehaviour
{
    public TMP_InputField inputField;

   
    public void RemoveSpaces()
    {
        inputField.text = inputField.text.Replace(" ", "");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class test_InputFieldOnValidateInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        Debug.Log("inputfield says : " + gameObject.GetComponent<TextMeshProUGUI>().text);
    }
}

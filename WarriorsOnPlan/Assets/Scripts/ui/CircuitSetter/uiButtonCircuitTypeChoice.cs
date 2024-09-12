using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiButtonCircuitTypeChoice : MonoBehaviour
{
    private static List<GameObject> listButtonsCircuitTypeChoice;

    //should be set from unity editor
    public int orderButton;


    public void Awake() {
        if (listButtonsCircuitTypeChoice == null) {
            listButtonsCircuitTypeChoice = new List<GameObject>();
        }

        listButtonsCircuitTypeChoice.Add(gameObject);
    }

    /*
    public void onClick() {
        for (int i=0; i<listButtonsCircuitTypeChoice.Count; i++) {
            if (listButtonsCircuitTypeChoice[i] != gameObject) {
                listButtonsCircuitTypeChoice[i].GetComponent<Button>().interactable = false;
            } else { 
                
            }
        }


    }
    */

    public static void resetButtons() {
        foreach (GameObject obj in listButtonsCircuitTypeChoice) {
            obj.GetComponent<Button>().interactable = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiToolsStorage : MonoBehaviour
{
    private GameObject prefabBubbleTool;

    private List<GameObject> listToolsBubble;

    public void Awake() {
        prefabBubbleTool = Resources.Load<GameObject>("Prefabs/UI/bubbleTool.prefab");
    }

    public void prepareBubbles(dataTool[] parDataToolArray) {


        foreach (dataTool DT in parDataToolArray) { 
            //★ tool 이미지 가져와서 겹치기
        }
    }
}

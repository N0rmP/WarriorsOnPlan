using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class graphicManager : MonoBehaviour
{
    public static graphicManager GM;

    public GameObject vfxSimplePrefab;

    private Stack<GameObject> availableSimples;
    private List<GameObject> occupiedSimples;

    //private Stack<GameObject> availableExplosions;
    //private List<GameObject> occupiedExplosions;

    public void Awake() {
        //singleton
        if (GM == null) {
            GM = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        availableSimples = new Stack<GameObject>();
        occupiedSimples = new List<GameObject>();
        //availableExplosions = new Stack<GameObject>();
        //occupiedExplosions = new List<GameObject>();
    }

    public void prepare(Scene parScene, LoadSceneMode parLoadSceneMode) { 
        
    }
}

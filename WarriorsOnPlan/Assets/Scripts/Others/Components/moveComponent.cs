using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class moveComponent : MonoBehaviour
{
    private Dictionary<GameObject, (Vector3 destination, float multiplier)> dictMovers;
    private Dictionary<GameObject, (Vector3 departure, Vector3 destination, float height, float multiplier, float ratio)> dictParabolaers;
    //https://gist.github.com/ditzel/68be36987d8e7c83d48f497294c66e08

    #region callbacks
    void Awake()
    {
        dictMovers = new Dictionary<GameObject, (Vector3 destination, float multiplier)>();
    }

    void Update()
    {
        float tempDeltaTime = Time.deltaTime;
        makeMove(tempDeltaTime);
    }
    #endregion callbacks

    #region similari_observers
    public void makeMove(float parDeltaTime) {
        Vector3 tempVector = Vector3.zero;
        foreach (GameObject obj in dictMovers.Keys.ToArray()) {
            tempVector = dictMovers[obj].destination - obj.transform.position;
            obj.transform.position += tempVector.normalized * dictMovers[obj].multiplier * parDeltaTime;
            if (tempVector.magnitude <= 0.05f) {
                obj.transform.position = dictMovers[obj].destination;
                removeMover(obj);
            }
        }
    }

    #region add_remove
    public void addMover(GameObject parObj, Vector3 parDestination, float parTime = 1f) {
        float tempCalcMult() {
            return ((parDestination - parObj.transform.position).magnitude / parTime);
        }

        //if parObj already exists as key, update the destination
        if (dictMovers.ContainsKey(parObj)) {
            dictMovers[parObj] = (parDestination, tempCalcMult());
        } else {
            dictMovers.Add(parObj, (parDestination, tempCalcMult()));
        }
    }

    public void removeMover(GameObject parObj) {
        dictMovers.Remove(parObj);
    }
    #endregion add_remove
    #endregion similari_observers
}

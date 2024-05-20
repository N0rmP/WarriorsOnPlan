using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class moveComponent
{
    class parabolaInfo {
        private readonly Vector3 departure;
        private readonly Vector3 destination;
        private readonly float height;
        private readonly float multiplier;
        private float ratio;

        public parabolaInfo(Vector3 parDeparture, Vector3 parDestination, float parHeight, float parMultiplier) {
            departure = parDeparture;
            destination = parDestination;
            height = parHeight;
            multiplier = parMultiplier;
            ratio = 0f;
        }

        public void addRatio(float value) {
            ratio += value;
            Mathf.Clamp(ratio, 0f, 1f);
        }

        public Vector3 getCurrentVector() {
            Vector3 tempVector = Vector3.Lerp(departure, destination, ratio);
            return new Vector3(tempVector.x, tempVector.y + 4 * height * (-ratio * ratio + ratio), tempVector.z);
        }

        public Vector3 getDestination() {
            return destination;
        }
    }

    private Dictionary<GameObject, (Vector3 destination, float multiplier)> dictMovers;
    private Dictionary<GameObject, parabolaInfo> dictParabolaers;
    //https://gist.github.com/ditzel/68be36987d8e7c83d48f497294c66e08

    public moveComponent()
    {
        dictMovers = new Dictionary<GameObject, (Vector3 destination, float multiplier)>();
        dictParabolaers = new Dictionary<GameObject, parabolaInfo>();
    }

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

    public void makeParabola(float parDeltaTime) {
        parabolaInfo tempPI = null;
        foreach (GameObject obj in dictParabolaers.Keys.ToArray()) {
            tempPI = dictParabolaers[obj];
            tempPI.addRatio(parDeltaTime);
            obj.transform.position = tempPI.getCurrentVector();
            if ((tempPI.getDestination() - obj.transform.position).magnitude <= 0.05f) {
                obj.transform.position = tempPI.getDestination();
                removeParabolaer(obj);
            }
        }
    }

    #region add_remove
    public float getMultiplier(Vector3 parDeparture, Vector3 parDestination, float parTime) {
        return (parDestination - parDeparture).magnitude / parTime;
    }

    public void addMover(GameObject parObj, Vector3 parDestination, float parTime = 1f) {
        dictMovers.Remove(parObj);
        dictMovers.Add(parObj, (parDestination, getMultiplier(parDestination, parObj.transform.position, parTime)));
    }

    public void removeMover(GameObject parObj) {
        dictMovers.Remove(parObj);

        if (parObj.TryGetComponent<IMovable>(out IMovable tempIM)) {
            tempIM.whenEndMove();
        }
    }

    public void addParabolaer(GameObject parObj, Vector3 parDestination, float parTime = 1, float parHeight = 4) {
        dictParabolaers.Remove(parObj);
        dictParabolaers.Add(parObj,
                new parabolaInfo(parObj.transform.position, parDestination, parHeight, getMultiplier(parDestination, parObj.transform.position, parTime))
            );
    }

    public void removeParabolaer(GameObject parObj) {
        dictParabolaers.Remove(parObj);

        if (parObj.TryGetComponent<IMovable>(out IMovable tempIM)) {
            tempIM.whenEndMove();
        }
    }
    #endregion add_remove
    #endregion similari_observers
}

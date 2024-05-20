using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumMove {
    stationary,
    linear,
    parabola
}

public class movableObject : MonoBehaviour {
    private Vector3 departure;
    private Vector3 destination;
    private float multiplier;
    private float ratio_;
    private float height;
    private enumMove stateMove;

    private float ratio { 
        get { return ratio_; } 
        set { ratio_ = Mathf.Clamp(value, 0f, 1f); } 
    }

    public void Awake() {
        
    }

    public void Update() {
        float tempDeltaTime = Time.deltaTime;
        switch (stateMove) {
            case enumMove.stationary:
                break;
            case enumMove.linear:
                moveLinear(tempDeltaTime);
                break;
            case enumMove.parabola:
                moveParabola(tempDeltaTime);
                break;
            default:
                break;
        }
    }

    #region similari_observers
    public void moveLinear(float parDeltaTime) {
        Vector3 tempVector = Vector3.zero;
        tempVector = destination - transform.position;
        transform.position += tempVector.normalized * multiplier * parDeltaTime;

        if (tempVector.magnitude <= 0.05f) {
            transform.position = destination;
        }
    }

    public void moveParabola(float parDeltaTime) {
        ratio += parDeltaTime;
        Vector3 tempVector = Vector3.Lerp(departure, destination, ratio);
        transform.position = new Vector3(tempVector.x, tempVector.y + 4 * height * (-ratio * ratio + ratio), tempVector.z);

        if ((destination - transform.position).magnitude <= 0.05f) {
            transform.position = destination;
        }
    }

    #region start_end
    public float getMultiplier(Vector3 parDeparture, Vector3 parDestination, float parTime) {
        return (parDestination - parDeparture).magnitude / parTime;
    }

    public void endMove(bool isArrival = true) {
        stateMove = enumMove.stationary;
        if (isArrival) { transform.position = destination; }

        if (this is IMovableSupplement) { ((IMovableSupplement)this).whenEndMove(); }
    }

    public void startLinearMove(Vector3 parDestination, float parTime = 1f) {
        destination = parDestination;
        multiplier = getMultiplier(parDestination, transform.position, parTime);
        stateMove = enumMove.linear;

        if (this is IMovableSupplement) { ((IMovableSupplement)this).whenStartMove(); }
    }

    

    public void startParabolaMove(Vector3 parDestination, float parTime = 1, float parHeight = 4) {
        destination = parDestination;
        multiplier = getMultiplier(parDestination, transform.position, parTime);
        height = parHeight;
        ratio = 0f;
        stateMove = enumMove.parabola;

        if (this is IMovableSupplement) { ((IMovableSupplement)this).whenStartMove(); }
    }
    #endregion add_remove
    #endregion similari_observers
}

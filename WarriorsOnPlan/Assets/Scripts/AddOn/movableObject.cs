using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumMoveType {
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
    private enumMoveType stateMove;

    private float ratio {
        get { return ratio_; }
        set { ratio_ = Mathf.Clamp(value, 0f, 1f); }
    }

    public void Awake() {
        resetMovableObject();
    }

    public void Update() {
        float tempDeltaTime = Time.deltaTime;
        switch (stateMove) {
            case enumMoveType.stationary:
                break;
            case enumMoveType.linear:
                moveLinear(tempDeltaTime);
                break;
            case enumMoveType.parabola:
                moveParabola(tempDeltaTime);
                break;
            default:
                break;
        }
    }

    private void resetMovableObject() {
        departure = Vector3.zero;
        destination = Vector3.zero;
        multiplier = 1f;
        ratio_ = 0f;
        height = 4f;
        stateMove = enumMoveType.stationary;
    }

    #region similari_observers
    public void moveLinear(float parDeltaTime) {
        Vector3 tempVector = destination - transform.position;
        transform.position += tempVector.normalized * multiplier * parDeltaTime;

        if (tempVector.magnitude <= 0.05f) {
            endMove();
        }
    }

    public void moveParabola(float parDeltaTime) {
        ratio += parDeltaTime;
        Vector3 tempVector = Vector3.Lerp(departure, destination, ratio);
        transform.position = new Vector3(tempVector.x, tempVector.y + 4 * height * (-ratio * ratio + ratio), tempVector.z);

        if ((destination - transform.position).magnitude <= 0.05f) {
            endMove();
        }
    }

    #region start_end
    public float getMultiplier(Vector3 parDeparture, Vector3 parDestination, float parTime) {
        return (parDestination - parDeparture).magnitude / parTime;
    }

    public void endMove(bool isArrival = true) {
        if (isArrival) { transform.position = destination; }
        resetMovableObject();        

        if (this is IMovableSupplement) { ((IMovableSupplement)this).whenEndMove(); }
    }

    public void startLinearMove(Vector3 parDestination, float parTime = 1f) {
        departure = transform.position;
        destination = parDestination;
        multiplier = getMultiplier(parDestination, transform.position, parTime);
        stateMove = enumMoveType.linear;

        if (this is IMovableSupplement) { ((IMovableSupplement)this).whenStartMove(); }
    }

    public void startParabolaMove(Vector3 parDestination, float parTime = 1) {
        departure = transform.position;
        destination = parDestination;
        multiplier = getMultiplier(parDestination, transform.position, parTime);
        ratio = 0f;
        stateMove = enumMoveType.parabola;

        if (this is IMovableSupplement) { ((IMovableSupplement)this).whenStartMove(); }
    }

    public void startParabolaMove(Vector3 parDestination, float parTime = 1, float parHeight = 4) {
        height = parHeight;
        startParabolaMove(parDestination, parTime);
    }
    #endregion add_remove
    #endregion similari_observers
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class navigatorAbst : ICaseUpdateState
{
    public warriorAbst owner { get; private set; }

    // route will be recalculated just before every movement, but can remain only when whole nodes in route have nothing on them
    protected Stack<EDirection> route;

    public navigatorAbst(warriorAbst parOwner) {
        owner = parOwner;
        route = new Stack<EDirection>();
    }

    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source) {
        //set state to idleAttack if target is in range, otherwise move
        return checkIsArrival() ? (this, enumStateWarrior.idleAttack) : (this, enumStateWarrior.move);
    }

    protected abstract bool checkIsArrival();

    public abstract EDirection getNextEDirection();
}

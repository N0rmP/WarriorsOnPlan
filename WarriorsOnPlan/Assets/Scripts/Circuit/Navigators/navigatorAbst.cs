using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class navigatorAbst : caseBase
{

    // route will be recalculated just before every movement, but can remain only when whole nodes in route have nothing on them
    protected Queue<node> route;

    public navigatorAbst() : base(enumCaseType.circuit) {
        route = new Queue<node>();
    }

    protected void polishEDirStackToRouteQueue(node parDeparture, ref Stack<EDirection> parStack, ref Queue<node> parRoute) {
        node tempNode = parDeparture;
        route.Clear();
        while (parStack.Count > 0) {
            tempNode = tempNode.link[(int)parStack.Pop()];
            Debug.Log("route : " + tempNode.coor0 + "," + tempNode.coor1);
            route.Enqueue(tempNode);
        }
    }

    public abstract bool checkIsArrival(Thing owner);

    public abstract node getNextRoute(Thing owner);
}

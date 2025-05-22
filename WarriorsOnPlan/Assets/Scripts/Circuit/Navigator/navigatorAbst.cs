using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

namespace Circuits {
    public abstract class navigatorAbst : circuitAbst<navigatorAbst> {
        // route will be recalculated just before every movement, but can remain only when whole nodes in route have nothing on them
        protected Queue<node> route;

        public navigatorAbst(int[] parParameter) : base(parParameter) {
            route = new Queue<node>();
        }

        public override object Clone() {
            object tempResult = base.Clone();
            ((navigatorAbst)tempResult).initRouteQueue();
            return tempResult;
        }

        protected void polishEDirStackToRouteQueue(node parDeparture, Stack<EDirection> parStack, Queue<node> parRoute) {
            node tempNode = parDeparture;
            route.Clear();
            while (parStack.Count > 0) {
                tempNode = tempNode.link[(int)parStack.Pop()];
                route.Enqueue(tempNode);
            }
        }

        public void initRouteQueue() {
            route = new Queue<node>();
        }

        public void clearRoute() {
            route.Clear();
        }

        public node getNextRoute(Thing owner) {
            return route.Dequeue();
        }

        public virtual bool checkIsRouteValid(Thing owner) {
            if (route.Count == 0) {
                return false;
            }

            foreach (node nd in route) {
                if (nd.thingHere != null) {
                    return false;
                }
            }

            return true;
        }

        public abstract bool checkIsArrival(Thing owner);

        public abstract void calculateNewRoute(Thing owner);

        #region test
        public void testRoute() {
            StringBuilder tempSB = new StringBuilder("- - - - - " + this + " route test - - - - -\n");
            foreach (node n in route) {
                tempSB.Append(n?.coor0);
                tempSB.Append(",");
                tempSB.Append(n?.coor1);
                tempSB.Append(" - ");
            }

            Debug.Log(tempSB.ToString());
        }
        #endregion test
    }
}
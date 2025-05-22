using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public class navigatorBackward : navigatorAbst {
        public navigatorBackward(int[] parParameter) : base(parParameter) {
            code = 1204;
        }

        public override void calculateNewRoute(Thing owner) {
            int tempDestinationX = owner.curPosition.coor0;
            Stack<EDirection> tempStack = new Stack<EDirection>();
            combatManager.CM.GC.BFS(owner.curPosition,
                (x) => {
                    return x.coor0 == tempDestinationX && x.coor1 == (owner.thisSide == enumSide.player ? 0 : combatManager.CM.GC.size1 - 1);
                },
                tempStack);

            polishEDirStackToRouteQueue(owner.curPosition, tempStack, route);
        }

        public override bool checkIsArrival(Thing owner) {
            return owner.curPosition.coor1 == (owner.thisSide == enumSide.player ? 0 : combatManager.CM.GC.size1 - 1);
        }
    }
}
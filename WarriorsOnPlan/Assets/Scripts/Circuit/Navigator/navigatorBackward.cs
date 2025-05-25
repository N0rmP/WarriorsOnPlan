using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public class navigatorBackward : navigatorAbst {
        public navigatorBackward(int[] parParameter) : base(parParameter) {
            code = 1204;
        }

        /*
            �� ���� ������ ��ǥ�� �ٸ� Thing�� �ִٸ� ���� ������ �����Ƿ�, ���� BFS�� �ʿ��ϴ�

            1. (�ڽ��� x ��ǥ, enumSide.player��� 0 �׸��� enumSide.enemy��� combatManager.CM.GC.size1 - 1)�� �����Ѵ�.
            2. ���� �ش� ��ǥ�� thingHere�� �ִٸ� ���� BFS�� �ʿ��ϴ�.
            
            �׸��� �̰Ŷ� navigatorForward ���ؼ� �ٸ� �κ� �� ������ ä���־���� ��
        */
        public override void calculateNewRoute(Thing owner) {
            int tempDestinationX = owner.curPosition.coor0;
            Stack<EDirection> tempStack = new Stack<EDirection>();
            node tempDestination = combatManager.CM.GC[owner.curPosition.coor0, owner.thisSide == enumSide.player ? 0 : combatManager.CM.GC.size1 - 1];
            combatManager.CM.GC.BFS(owner.curPosition,
                (x) => {
                    return x == tempDestination;
                },
                tempStack,
                new Vector2(owner.curPosition.coor0 - tempDestination.coor0, owner.curPosition.coor1 - tempDestination.coor1)
                );

            polishEDirStackToRouteQueue(owner.curPosition, tempStack, route);
        }

        public override bool checkIsArrival(Thing owner) {
            return owner.curPosition.coor1 == (owner.thisSide == enumSide.player ? 0 : combatManager.CM.GC.size1 - 1);
        }
    }
}
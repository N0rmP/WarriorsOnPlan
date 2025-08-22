using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public class navigatorBackward : navigatorAbst {
        public navigatorBackward() {
            // ★ 나중에 navigatorAttackAllWeapon 만들고 1204로 코드 옮기기
            code = 1203;
        }

        /*
            ★ 만약 후퇴할 좌표에 다른 Thing이 있다면 무한 루프에 빠지므로, 세미 BFS가 필요하다

            1. (자신의 x 좌표, enumSide.player라면 0 그리고 enumSide.enemy라면 combatManager.CM.GC.size1 - 1)을 지정한다.
            2. 만약 해당 좌표에 thingHere이 있다면 세미 BFS가 필요하다.
            
            그리고 이거랑 navigatorForward 비교해서 다른 부분 좀 걔한테 채워넣어줘라 야
        */
        public override void calculateNewRoute(Thing owner) {
            int tempDestinationX = owner.curPosition.coor0;
            Stack<EDirection> tempStack = new Stack<EDirection>();
            node tempDestination = combatManager.CM.GC[owner.curPosition.coor0, owner.thisSide == enumSide.player ? 0 : combatManager.CM.GC.size1 - 1];
            combatManager.CM.GC.BFS(owner.curPosition,
                (x) => {
                    return x.coor1 == owner.thisSide switch { 
                        enumSide.player => 0,
                        enumSide.enemy or enumSide.neutral => 6,
                        _ => 6
                    };
                },
                tempStack,
                new Vector2(0, owner.thisSide switch {
                    enumSide.player => -1,
                    enumSide.enemy or enumSide.neutral => 1,
                    _ => 1
                })
                );

            polishEDirStackToRouteQueue(owner.curPosition, tempStack, route);
        }

        public override bool checkIsArrival(Thing owner) {
            return owner.curPosition.coor1 == (owner.thisSide == enumSide.player ? 0 : combatManager.CM.GC.size1 - 1);
        }
    }
}
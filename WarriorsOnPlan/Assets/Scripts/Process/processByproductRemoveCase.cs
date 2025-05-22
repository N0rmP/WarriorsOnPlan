using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

namespace Processes {
    public class processByproductRemoveCase : processByproductAbst {
        private Thing source;
        private caseBase caseTBR;

        public processByproductRemoveCase(Thing parSource, caseBase parCB) {
            source = parSource;
            caseTBR = parCB;
        }

        protected override void actualDO() {
            source.removeCase(caseTBR);
        }

        /*
        protected override void actualUNDO() {
            source.addCase(caseTBR);
        }
        */

        protected override void actualSHOW() {
            base.actualSHOW();

            /*
            ★ 가능하면 이 부분을 combatManager로 옮겨서 모든 process/warrior에 대하여 시행할 것, warrior의 애니메이션이나 투사체 및 이펙트가 아니면 여기 없어야 함
                아무래도 canvasPersonal에 매 process마다 imgEffect를 저장해놨다가 확인하고 갱신하는 코드가 필요해보임, 체력 및 스킬 타이머와 달리 오브젝트 여럿을 검사/갱신해야 하기 때문
                이거 끝나면 processByproductDie에도 동일한 부분 있으니 그것도 함께 신경쓸 것.
                이거 끝나면 각 warrior의 좌표, status를 확인하고 갱신하는 코드도 함께 집어넣을 것.
                대규모 공산데...
            */

            source.removePanelImgEffect(caseTBR);
        }
    }
}
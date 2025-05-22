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
            �� �����ϸ� �� �κ��� combatManager�� �Űܼ� ��� process/warrior�� ���Ͽ� ������ ��, warrior�� �ִϸ��̼��̳� ����ü �� ����Ʈ�� �ƴϸ� ���� ����� ��
                �ƹ����� canvasPersonal�� �� process���� imgEffect�� �����س��ٰ� Ȯ���ϰ� �����ϴ� �ڵ尡 �ʿ��غ���, ü�� �� ��ų Ÿ�̸ӿ� �޸� ������Ʈ ������ �˻�/�����ؾ� �ϱ� ����
                �̰� ������ processByproductDie���� ������ �κ� ������ �װ͵� �Բ� �Ű澵 ��.
                �̰� ������ �� warrior�� ��ǥ, status�� Ȯ���ϰ� �����ϴ� �ڵ嵵 �Բ� ������� ��.
                ��Ը� ���굥...
            */

            source.removePanelImgEffect(caseTBR);
        }
    }
}
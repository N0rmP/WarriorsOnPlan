using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

namespace Processes {
    public class processByproductAddCase : processByproductAbst {
        private Thing source;
        private caseBase caseAdded;

        public processByproductAddCase(Thing parSource, caseBase parCaseAdded) {
            source = parSource;
            caseAdded = parCaseAdded;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            foreach (ICaseBeforeAddCase cb in source.getCaseList<ICaseBeforeAddCase>(true)) {
                cb.onBeforeAddCase(source, caseAdded);
            }
        }

        protected override void actualDO() {
            source.addCase(caseAdded);
        }

        /*
        protected override void actualUNDO() {
            source.removeCase(caseAdded);
        }
        */

        protected override void actualSHOW() {
            base.actualSHOW();

            switch (caseAdded.caseType) {
                case enumCaseType.effect:
                    source.updatePanelImageEff();
                    break;
                case enumCaseType.skill:
                    source.updatePanelSkillTimer();
                    break;
            }
        }
    }
}
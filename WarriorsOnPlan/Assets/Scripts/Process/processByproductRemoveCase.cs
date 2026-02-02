using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;
using System.Text;

namespace Processes {
    public class processByproductRemoveCase : processByproductAbst {
        private ICaseContainerContainer source;
        private caseBase caseTBR;   // case To Be Removed

        public processByproductRemoveCase(ICaseContainerContainer parSource, caseBase parCB) {
            source = parSource;
            caseTBR = parCB;
        }

        protected override void actualDO() {            
            source.removeCase(caseTBR);
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (caseTBR.caseType == enumCaseType.tool && source is Thing tempSource) {
                gameManager.GM.PC.popupRemoveCaseBase(tempSource.gameObject.getCanvasMainLocalPosition() + new Vector2(0, gameManager.GM.option.stick), caseTBR.caseImage);
            }
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(caseTBR?.ToString());
            parSB.Append(" removed from ");
            parSB.Append(source?.ToString());
        }
        #endregion test
    }
}
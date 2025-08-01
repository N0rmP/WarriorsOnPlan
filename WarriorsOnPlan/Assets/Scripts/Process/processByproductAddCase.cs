using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

namespace Processes {
    public class processByproductAddCase : processByproductAbst {
        private Thing source;
        // caseAddedPrimitive is the first caseBase passed as creator-parameter
        // mementoCaseAdded is the memento of caseAddedPrimitive just when this process is created, actual adding case is done with mementoCaseAdded
        private caseBase caseAddedPrimitive;
        private mementoIParametable mementoCaseAdded;
        

        public processByproductAddCase(Thing parSource, caseBase parCaseAdded) {
            source = parSource;
            mementoCaseAdded = parCaseAdded.getMementoIParametable();
            caseAddedPrimitive = parCaseAdded;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // onInterferableAddCase
            isInterfered = source.observeInterferable<ICaseInterferableAddCase>(new object[2] { source, caseAddedPrimitive });
            if (isInterfered) {
                return;
            }

            // onBeforeAddCase
            source.observeVoid<ICaseBeforeAddCase>(new object[2] { source, caseAddedPrimitive });
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAddedThis
            if (caseAddedPrimitive is ICaseAddedThis tempCaseAdded) {
                tempCaseAdded.caseFunc(source);
            }

            // onAfterAddCase
            source.observeVoid<ICaseAfterAddCase>(new object[2] { source, caseAddedPrimitive });            
        }

        protected override void actualDO() {
            source.addCase(mementoCaseAdded.getRestoredIt<caseBase>());
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(
                    source.transform.position, 
                    gameManager.GM.DHouC.bookWords.strAdd + " " +
                    caseAddedPrimitive.caseType switch { 
                        enumCaseType.tool => gameManager.GM.DHouC.bookWords.strTool,
                        enumCaseType.effect => gameManager.GM.DHouC.bookWords.strEffect,
                        _ => ""
                    } + " " +
                    gameManager.GM.DHouC.bookWords.strInterfere, 
                    false
                );
            }

            switch (caseAddedPrimitive.caseType) {
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
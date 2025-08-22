using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

namespace Processes {
    public class processByproductAddCase : processByproductAbst {
        // AddCase and RemoveCase can have ICaseContainerContainer as source, they need unique script about this
        private ICaseContainerContainer source;
        // caseAddedPrimitive is the first caseBase passed as creator-parameter
        // mementoCaseAdded is the memento of caseAddedPrimitive just when this process is created, actual adding case is done with mementoCaseAdded
        private caseBase caseAddedPrimitive;
        private mementoIParametable mementoCaseAdded;
        

        public processByproductAddCase(ICaseContainerContainer parSource, caseBase parCaseAdded) {
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

            bool tempIsSourceThing = source is Thing;
            Thing tempSource = tempIsSourceThing ? (source as Thing) : null;

            if (isInterfered) {
                if (tempIsSourceThing) {
                    gameManager.GM.PC.popupBasicAlert(
                        tempSource.gameObject.getCanvasMainLocalPosition() + new Vector2(0f, gameManager.GM.option.stickDegreed),
                        caseAddedPrimitive.caseType switch {
                            enumCaseType.tool => gameManager.GM.DHouC.bookWords.strTool,
                            enumCaseType.effect => gameManager.GM.DHouC.bookWords.strEffect,
                            _ => ""
                        } + " " +
                        gameManager.GM.DHouC.bookWords.strAdd + " " +
                        gameManager.GM.DHouC.bookWords.strInterfere
                    );
                }
                return;
            }

            if (caseAddedPrimitive.caseType == enumCaseType.tool && tempIsSourceThing) {
                gameManager.GM.PC.popupAddCaseBase(tempSource.gameObject.getCanvasMainLocalPosition() + new Vector2(0f, gameManager.GM.option.stickDegreed), caseAddedPrimitive.caseImage);
                gameManager.GM.AC.playSE(
                    SwissArmyStaticMethod.selectRandom<AudioClip>(
                        gameManager.GM.AHouC.arrClipToolEquip
                    )
                );
            }
        }
    }
}
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Processes {
    public class processByproductActionSkill : processByproductActionAbst {
        private Thing target;

        public processByproductActionSkill(Thing parSource, Thing parTarget, bool parIsSHOW = true) : base(parSource, parIsSHOW) {
            target = parTarget;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // if target is necessary but null, invalidate all execution as processActionSkill
            if (target is null && source.thisSkill.isTargetNeeded) {
                return;
            }

            // onInterferableUseSkill
            isInterfered = source.observeInterferable<ICaseInterferableUseSkill>(new object[2] { source, target });
            if (isInterfered) {
                return;
            }

            // onBeforeUseSkill
            source.observeVoid<ICaseBeforeUseSkill>(new object[2] { source, target });
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // if target is necessary but null, invalidate all execution as processActionSkill
            if (target is null && source.thisSkill.isTargetNeeded) {
                return;
            }

            // onAfterUseSkill
            source.observeVoid<ICaseAfterUseSkill>(new object[2] { source, target });
        }

        protected override void actualDO() {
            base.actualDO();

            // if target is necessary but null, invalidate all execution as processActionSkill
            if (target is null && source.thisSkill.isTargetNeeded) {
                return;
            }

            source.thisSkill.useSkill(source, target);
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(source.gameObject.getCanvasMainLocalPosition() + new Vector2(0f, gameManager.GM.option.stickDegreed), gameManager.GM.DHouC.bookWords.strSkill + " " + gameManager.GM.DHouC.bookWords.strInterfere);
            }

            if (target is null && source.thisSkill.isTargetNeeded) {
                gameManager.GM.PC.popupBasicAlert(source.gameObject.getCanvasMainLocalPosition() + new Vector2(0f, gameManager.GM.option.stickDegreed), gameManager.GM.DHouC.bookPopupAlert.strAlertNoSkillTarget);
                return;
            }

            source.thisSkill.SHOW(source, target);
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(source?.ToString());
            parSB.Append(" used skill to ");
            parSB.Append(target?.ToString());
        }
        #endregion test
    }
}
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
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
                gameManager.GM.PC.popupBasicAlert(source.transform.position, gameManager.GM.DHouC.bookWords.strSkill + " " + gameManager.GM.DHouC.bookWords.strInterfere, false);
            }

            if (target is null && source.thisSkill.isTargetNeeded) {
                // ★ 데이터 가져와서 문장 바꾸게 만들기
                gameManager.GM.PC.popupBasicAlert(source.transform.position + new Vector3(0f, 0f, 1f), "no skill target", false);
                return;
            }

            source.thisSkill.SHOW(source, target);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processActionSkill : processActionAbst {
        private Thing target;

        public processActionSkill(Thing parSource, Thing parTarget, bool parIsSHOW = true) : base(parIsSHOW) {
            source = parSource;
            target = parTarget;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // if target is necessary but null, invalidate all execution as processActionSkill
            if (target is null && source.thisSkill.isTargetNeeded) {
                return;
            }

            // onBeforeUseSkill
            foreach (ICaseBeforeUseSkill cb in source.getCaseList<ICaseBeforeUseSkill>()) {
                cb.onBeforeUseSkill(source, target);
            }
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // if target is necessary but null, invalidate all execution as processActionSkill
            if (target is null && source.thisSkill.isTargetNeeded) {
                return;
            }

            // onAfterUseSkill
            foreach (ICaseAfterUseSkill cb in source.getCaseList<ICaseAfterUseSkill>()) {
                cb.onAfterUseSkill(source, target);
            }
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

            if (target is null && source.thisSkill.isTargetNeeded) {
                // ★ 데이터 가져와서 문장 바꾸게 만들기
                gameManager.GM.PC.popupBasicAlert(source.transform.position + new Vector3(0f, 0f, 1f), "no skill target", false);
                return;
            }

            source.thisSkill.SHOW(source, target);
        }
    }
}
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

            // onBeforeUseSkill
            foreach (ICaseBeforeUseSkill cb in source.getCaseList<ICaseBeforeUseSkill>()) {
                cb.onBeforeUseSkill(source, target);
            }
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterUseSkill
            foreach (ICaseAfterUseSkill cb in source.getCaseList<ICaseAfterUseSkill>()) {
                cb.onAfterUseSkill(source, target);
            }
        }

        protected override void actualDO() {
            base.actualDO();

            source.thisSkill.useSkill(source, target);
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            source.thisSkill.SHOW(source, target);
        }
    }
}
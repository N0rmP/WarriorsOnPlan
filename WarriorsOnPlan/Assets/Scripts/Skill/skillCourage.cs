using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public class skillCourage : skillAbst {
        public override bool isReady {
            get {
                return false;
            }
        }

        public skillCourage() : base("Image/Case/Skill/Image_skillCourage") {
            code = 2002;
            isTimerNeeded = false;
            isRangeNeeded = false;
            isTargetNeeded = false;
        }

        protected override void actualUseSkill(Thing source, Thing target) { }
    }
}
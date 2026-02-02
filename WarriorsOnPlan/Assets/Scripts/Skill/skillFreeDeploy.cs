using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;
using Placablers;

public class skillFreeDeploy : skillAbst, ICaseSystemicAdded, ICaseSystemicRemoved {
    private IPlacabler thisPlacabler;

    public override bool isReady => false;

    public skillFreeDeploy() : base("Image/Case/Skill/Image_skillFreeDeploy") {
        code = 2001;
        isTimerNeeded = false;
        isRangeNeeded = false;
        isTargetNeeded = false;

        thisPlacabler = new placablerRowCol();
    }

    protected override void actualUseSkill(Thing source, Thing target) {
        thisPlacabler.restoreParameters(new List<int> { 0, combatManager.CM.GC.size0, 0, combatManager.CM.GC.size1 }.GetEnumerator());
        source.setPlacabler(thisPlacabler);
    }

    #region ICase
    void ICaseSystemicAdded.caseFunc(ICaseContainerContainer source) {
        if (source is Thing tempThing) {
            actualUseSkill(tempThing, null);
        }
    }

    // just in case...
    void ICaseSystemicRemoved.caseFunc(ICaseContainerContainer source) {
        if (source is Thing tempThing) {
            tempThing.setPlacabler(null);
        }
    }
    #endregion ICase
}

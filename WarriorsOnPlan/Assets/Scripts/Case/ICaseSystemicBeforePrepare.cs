using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ICaseSystemicBeforePrepare works right after level-systemInitiating done
    ICaseSystemicBeforePrepare is valid only when it's skillAbst, because it can't be added to Thing before preparing begins unless it is
    ICaseSystemicBeforePrepare can also be upgradeAbst, but it's better to use upgradeAbst.actualActivate for consistency
    in results ICaseSystemicBeforePrepare exists only for Thing who wants to revise before-prepare combat situation
*/
public interface ICaseSystemicBeforePrepare {
    public void caseFunc();
}

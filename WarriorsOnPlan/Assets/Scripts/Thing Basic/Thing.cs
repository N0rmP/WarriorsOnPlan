using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using System.Text;

using Cases;
using Processes;
using Placablers;
using UnityEditor.SceneManagement;

public enum enumStateWarrior {
    dead = 0,
    //deadRecently = 1,
    controlled = 10,
    focussingEnd = 15,
    focussing = 20,
    skill = 30,
    move = 40,
    idleAttack = 50,
    none = 9999
}

public abstract class Thing : MonoBehaviour, ICaseContainerContainer, IPlacableOccupier {
    #region field
    private int curHp_;
    private (int coor0, int coor1) curCoor;
    protected int codeSkill = 92001;

    protected IWeaponGrabber thisWeaponGrabber = new grabberBasic();
    public Transform[] containerHand;
    public Transform containerBack;
    protected int codeWeaponDefault = 3001;
    protected int[] parameterWeaponDefault = new int[4] { 1, 1, 1, 1 };
    private toolWeapon weaponDefault_;

    protected int codeThing = 0;
    protected string portraitPath = "Image/Portrait/Portrait_tester";

    protected canvasPersonal thisCanvasPersonal;
    protected cursor thisCursor;

    protected caseContainer thisCaseContainer;

    private bool isStatusDirty;
    protected structWarriorStatus thisStatus_;
    protected int damageDealt_;
    protected int damageTaken_;

    private ITransparency thisITransparency;
    private enablerOUTLINE thisEnablerOUTLINE;

    protected circuitHub thisCircuitHub;

    #region property
    public organAnimation thisOrganAnimation { get; protected set; }
    public animationTracker thisAnimationTracker { get; protected set; }
    public enumStateWarrior stateCur { get; private set; }
    public enumSide thisSide { get; protected set; }
    protected int maxHpOriginal { get; set; }
    public int maxHp { get; protected set; }
    public int curHp {
        get {
            return curHp_;
        }
        private set {
            curHp_ = Math.Clamp(value, 0, maxHp);
        }
    }    

    public IPlacabler thisPlacabler { get; protected set; } = null;
    public structWarriorStatus thisStatus {
        get {
            if (isStatusDirty) {
                recalculateStatus();
                isStatusDirty = false;
            }

            return thisStatus_;
        }
    }
    public skillAbst thisSkill { get; protected set; }
    public toolWeapon weaponDefault {
        get {
            weaponDefault_.restoreParameters((parameterWeaponDefault as IEnumerable<int>).GetEnumerator());
            return weaponDefault_;
        }
    }
    public Thing whatToAttack { get; private set; }
    public Thing whatToUseSkill { get; private set; }
    public string nameThing { get; private set; }
    public Sprite portrait { get; protected set; }
    public node curPosition {
        get {
            if (curCoor == (-1, -1)) {
                return null;
            } else {
                return combatManager.CM.GC[curCoor.coor0, curCoor.coor1];
            }
        }
        set {
            curCoor = value?.getCoor() ?? (-1, -1);

        }
    }
    public int thisActionOrder {
        get {
            return combatManager.CM.HouC.getPersonalActionOrder(this);
        }
    }
    public int damageDealt {
        get {
            return damageDealt_;
        }
        set {
            damageDealt_ = Math.Max(damageDealt_, value);
            if (combatManager.CM.CUM.CStatus.thisThing == this) {
                combatManager.CM.CUM.CStatus.updateNumber();
            }
        }
    }
    public int damageTaken {
        get {
            return damageTaken_;
        }
        set {
            damageTaken_ = Math.Max(damageTaken_, value);
            if (combatManager.CM.CUM.CStatus.thisThing == this) {
                combatManager.CM.CUM.CStatus.updateNumber();
            }
        }
    }
    #endregion property
    #endregion field

    #region callbacks
    public void Update() {
        // outline
        if (thisCanvasPersonal.gameObject.checkHoveredWorld() || combatManager.CM.CUM.CStatus.thisThing == this) {
            thisEnablerOUTLINE.enableOUTLINE();
        } else {
            thisEnablerOUTLINE.disableOUTLINE();
        }
    }
    #endregion callbacks

    #region initiation_N_restoration
    public virtual void init(enumSide parSide, int parMaxHp, int[] parSkillParameters) {
        thisOrganAnimation = new organAnimation(GetComponent<Animator>());
        thisAnimationTracker = gameObject.AddComponent<animationTracker>();

        thisCaseContainer = new caseContainer();

        thisSide = parSide;
        stateCur = enumStateWarrior.idleAttack;
        thisStatus_ = new structWarriorStatus(0);
        damageDealt_ = 0;
        damageTaken_ = 0;

        nameThing = gameManager.GM.DHouC.bookThingName.getThingName(codeThing);
        portrait = Resources.Load<Sprite>(portraitPath) ??
                   Resources.Load<Sprite>("Image/Portrait/Portrait_tester");

        GameObject tempObj;

        maxHpOriginal = parMaxHp;
        setMaxHp(maxHpOriginal);
        setCurHp(maxHp, false);
        

        // initiate canvasPersonal
        tempObj = Instantiate(Resources.Load<GameObject>("Prefab/UI/canvasPersonal"));
        tempObj.transform.SetParent(transform);
        tempObj.GetComponent<Canvas>().worldCamera = Camera.main;
        thisCanvasPersonal = tempObj.GetComponent<canvasPersonal>();
        thisCanvasPersonal.updateHpText(curHp);
        if (thisSide == enumSide.player) {
            thisCanvasPersonal.transform.Find("SwissArmyObject").gameObject.AddComponent<releasablePersonal>().init(this);
            thisCanvasPersonal.transform.Find("SwissArmyObject").gameObject.AddComponent<dragablePersonal>().init(this);
        }

        tempObj = Instantiate(Resources.Load<GameObject>("Prefab/Cursor"));
        transform.SetParent(tempObj.transform);
        thisCursor = tempObj.GetComponent<cursor>();
        thisCursor.setColorOriginal(thisSide);
        thisCursor.setDelEndRun(() => GetComponent<Animator>().SetBool("isRun", false));

        // weaponDefault making
        weaponDefault_ = gameManager.GM.MC.makeCodableObject<toolWeapon>(codeWeaponDefault, parSkillParameters, null);

        // skill making
        thisSkill = null;
        try {
            addCase(gameManager.GM.MC.makeCodableObject<skillAbst>(codeSkill, parSkillParameters, null));
        } catch (Exception e) {
            Debug.Log(GetType() + ".init error : failed to addCase skill of code " + codeSkill + "\n" + e.ToString());
            addCase(gameManager.GM.MC.makeCodableObject<skillAbst>(92001, new int[5] { 3, 3, 1, 5, 1 }, null));
        }

        // thisWeaponGrabber
        thisWeaponGrabber.init(containerHand, containerBack);

        // initiate canvasPersonal with skill
        thisCanvasPersonal.setSkill(thisSkill);
        updatePanelSkillTimer();

        // prepare circuits, be aware that this is the only creation of circuitHub in total script of Thing class
        thisCircuitHub = new circuitHub(parSide, thisSkill.targetGroupDefault);
        addCase(thisCircuitHub);

        // shader
        // ★ 만약 다른 shader를 사용하는 Thing이 존재한다면 아래 내용을 변경해야 함
        thisITransparency = gameObject.AddComponent<transparencyStripple>();
        thisITransparency.init();
        thisITransparency.fadeStrict(1f);
        thisEnablerOUTLINE = gameObject.AddComponent<enablerOUTLINE>();
        thisEnablerOUTLINE.setColor(SwissArmyStaticMethod.getSideColor(thisSide));
    }

    public void restore(mementoThing parMementoThing) {
        // ★ Thing이 사망하여 제거된 상태였을 경우, 원래대로 되돌리기

        setMaxHp(parMementoThing.maxHp);
        setCurHp(parMementoThing.curHp, false);

        if (curHp <= 0) {
            destroied();
            return;
        }
        // you don't need any code when reviving thing, state will be idleAttack / position will be set / houseComponent setting will be done there

        thisSide = parMementoThing.side;

        // graph is vacated in houseComponent.restore
        stopMoving();
        combatManager.CM.systemPlace(this, parMementoThing.coordinates);

        foreach (caseBase cb in thisCaseContainer) {
            if (cb is skillAbst || cb is circuitHub) {
                continue;
            }
            removeCase(cb);
        }

        thisCircuitHub.restore(parMementoThing.mCircuitHub);

        stateCur = enumStateWarrior.idleAttack;

        thisStatus_.reset();
        isStatusDirty = true;
        damageDealt_ = parMementoThing.damageDealt;
        damageTaken_ = parMementoThing.damageTaken;

        thisSkill.restore(parMementoThing.mSkill);
        foreach (mementoIParametable mc in parMementoThing.listCase) {
            addCase(mc.getRestoredIt<caseBase>());
        }

        thisCanvasPersonal.updateHpText(curHp, true);
        updatePanelSkillTimer();
        updatePanelImageEff();

        thisOrganAnimation.resetAnimator();
        combatManager.CM.GC[parMementoThing.coordinates.c0, parMementoThing.coordinates.c1].placeHere(this);
        Look(transform.position +
            thisSide switch {
                enumSide.player => new Vector3(0f, 0f, 1f),
                enumSide.enemy => new Vector3(0f, 0f, -1f),
                enumSide.neutral => new Vector3(1f, 0f, 0f),
                _ => new Vector3(0f, 0f, 0f)
            });
        thisITransparency.fadeStrict(stateCur > enumStateWarrior.dead ? 1f : 0f);
    }

    // protected abstract skillAbst makeSkill(int[] parSkillParameters);
    #endregion initiation_N_restoration

    #region Move
    // Thing is child of cursor, it's meaningless to set position of Thing only and you should call these methods to move Thing by moving cursor
    public void setPosition(Vector3 parPosition) {
        thisCursor.transform.position = parPosition;
    }

    public void stopMoving() {
        thisCursor.GetComponent<movableObject>().stop();
    }

    public void moveLinear(Vector3 parDestination) {
        thisCursor.GetComponent<movableObject>().startLinearMove(parDestination, 1f / (float)combatManager.CM.combatSpeed);
    }

    public void moveParabola(Vector3 parDestination) {
        thisCursor.GetComponent<movableObject>().startParabolaMove(parDestination, 1f / (float)combatManager.CM.combatSpeed);
    }
    #endregion Move

    #region panel
    public void updatePanelTotal() {
        updatePanelSkillTimer();
        updatePanelHp();
        updatePanelImageEff();
    }

    public void updatePanelSkillTimer() {
        if (thisSkill.isTimerNeeded) {
            thisCanvasPersonal.updateSkillTimer(thisSkill.timerCur, thisSkill.timerMax);
        } else {
            thisCanvasPersonal.openSkillTimer();
        }
    }

    public void updatePanelHp() {
        thisCanvasPersonal.updateHpText(curHp);
    }

    public void updatePanelImageEff() {
        thisCanvasPersonal.clearImgEffect();
        foreach (caseBase cb in getCaseList(enumCaseType.effect)) {
            if (cb.isVisible) {
                thisCanvasPersonal.addImgEffect(cb);
            }
        }
    }

    public void updatePanelActionOrder() {
        thisCanvasPersonal.updateActionOrder(combatManager.CM.HouC.getPersonalActionOrder(this), thisSide);
    }

    public void updatePanelDragableReleasable() {
        thisCanvasPersonal.updateDragableReleasable();
    }
    #endregion panel

    #region affecting
    public virtual void updateTargets() {
        whatToAttack = thisCircuitHub.selectAttackTarget(this);
        whatToUseSkill = thisCircuitHub.selectSkillTarget(this);
    }

    public void updateState() {
        if (stateCur <= enumStateWarrior.dead) {
            return;
        }

        (ICaseUpdateState updater, enumStateWarrior ESW) tempBuffer = (null, enumStateWarrior.idleAttack);
        foreach ((ICaseUpdateState updater, enumStateWarrior ESW) cur in observeReturnEnumerate<ICaseUpdateState, (ICaseUpdateState, enumStateWarrior)>(new object[1] { this })) {
            if (cur.ESW < tempBuffer.ESW) {
                tempBuffer.updater?.onInterfered(this);
                tempBuffer = cur;
            }
        }

        stateCur = tempBuffer.ESW;
    }

    // isPlus ain't asking is value positive or negative, it's asking is newly-setting curHp or adding value to the origial curHp
    public int setCurHp(int parValue, bool isPlus = true) {
        int tempResultChange = 0;
        if (isPlus) {
            if (curHp + parValue < 0) {
                tempResultChange = -curHp;
                curHp = 0;
            } else if (curHp + parValue > maxHp) {
                tempResultChange = maxHp - curHp;
                curHp = maxHp;
            } else {
                tempResultChange = parValue;
                curHp += parValue;
            }
        } else {
            parValue = Math.Clamp(parValue, 0, maxHp);
            tempResultChange = (parValue > curHp) ? (parValue - curHp) : (curHp - parValue);
            curHp = parValue;
        }

        return tempResultChange;
    }

    public void setMaxHp(int parValue) {
        maxHp = Math.Max(parValue, 1);
    }

    /*
    public void useSkill() {
        thisSkill.useSkill(this, whatToUseSkill);
    }
    */

    public virtual void destroied() {
        stateCur = enumStateWarrior.dead;
        curPosition?.expelHere(false);
        combatManager.CM.HouC.killThing(this);
    }

    public void destroiedSystemically() {
        // ★ 시스템적 삭제
    }
    #endregion affecting

    #region circuit
    public void setCircuit(
        int parCodeNavigatorIdle, int[] ppNavigatorIdle,
        int parCodeSensorForMove, int[] ppSensorForMove,
        int parCodeNavigatorPrioritized, int[] ppNavigatorPrioritized,
        int parCodeSensorForSkill, int[] ppSensorForSkill,
        int parCodeSelecterForSkill, int[] ppSelecterForSkill,
        int parCodeSelecterForAttack, int[] ppSelecterForAttack) {

        thisCircuitHub.setCircuitHub(
        thisSide,
        parCodeNavigatorIdle, ppNavigatorIdle,
        parCodeSensorForMove, ppSensorForMove,
        parCodeNavigatorPrioritized, ppNavigatorPrioritized,
        parCodeSensorForSkill, ppSensorForSkill,
        parCodeSelecterForSkill, ppSelecterForSkill,
        parCodeSelecterForAttack, ppSelecterForAttack
            );
    }

    public string[] getCircuitInfo() {
        return thisCircuitHub.getInfoTotal();
    }

    public string getCircuitInfo(int parNum) {
        return thisCircuitHub.getInfoSingle(parNum);
    }

    public int getCircuitCode(int parNum) {
        return thisCircuitHub.getCodeSingle(parNum);
    }

    public int[] getCircuitParameter(int parNum) {
        return thisCircuitHub.getParameterSingle(parNum) ?? new int[0];
    }

    public int getSelecterForSkillTargetGroup() {
        return thisCircuitHub.getSelecterForSkillTargetGroup();
    }

    public int getSelecterForAttackTargetGroup() {
        return thisCircuitHub.getSelecterForAttackTargetGroup();
    }
    #endregion circuit

    #region case
    public virtual void addCase(caseBase parCase) {
        thisCaseContainer.addCase(parCase);

        // if parCase is skill, set is as thisSkill
        if (parCase.caseType == enumCaseType.skill && thisSkill == null) {
            thisSkill = (skillAbst)parCase;
        }

        // if ICaseCalculateStatus is added or removed, prepare to recalculate status
        if (parCase is ICaseSystemicCalculateStatus) {
            isStatusDirty = true;
        }

        // ICaseSystemicAdded be executed in any situation
        if (parCase is ICaseSystemicAdded tempCSA) {
            tempCSA.caseFunc(this);
        }

        updateCaseResult(parCase.caseType);
    }

    public virtual void removeCase(caseBase parCase) {
        thisCaseContainer.removeCase(parCase);

        // if parCase is skill, remove thisSkill
        if (parCase.caseType == enumCaseType.skill) {
            thisSkill = null;
        }

        // if ICaseCalculateStatus is added or removed, prepare to recalculate status
        if (parCase is ICaseSystemicCalculateStatus) {
            isStatusDirty = true;
        }

        // ICaseSystemicRemoved be executed in any situation
        if (parCase is ICaseSystemicRemoved tempCSR) {
            tempCSR.caseFunc(this);
        }

        updateCaseResult(parCase.caseType);
    }

    public void updateCaseResult(enumCaseType parCaseType) {
        switch (parCaseType) {
            case enumCaseType.skill:
                updatePanelSkillTimer();
                break;
            case enumCaseType.effect:
                updatePanelImageEff();
                break;
        }

        if (combatManager.CM.CUM.CStatus.thisThing == this) {
            combatManager.CM.CUM.CStatus.updateTotal();
        }
    }

    public List<toolWeapon> getListAvailableWeapon(Thing parTarget = null) {
        List<toolWeapon> tempResult = new List<toolWeapon>();

        if (parTarget != null) {    // if parTarget is null, all weapons are useless
            int tempDistanceToTarget = node.getDistance(curPosition, parTarget.curPosition);
            foreach (toolWeapon tw in getCaseList<toolWeapon>().ToArray()) {
                // skip when target is out of the weapon's range
                if (tempDistanceToTarget > tw.rangeMax || tempDistanceToTarget < tw.rangeMin || !tw.isReady) {
                    continue;
                }

                tempResult.Add(tw);
            }
        }

        return tempResult;
    }

    #region relay_caseConatiner
    public IEnumerable<T> getCaseList<T>() {
        return thisCaseContainer.getCaseList<T>();
    }

    public IEnumerable<caseBase> getCaseList(enumCaseType parCaseType) {
        return thisCaseContainer.getCaseList(parCaseType);
    }

    public int getCaseCount<T>() {
        return thisCaseContainer.getCaseCount<T>();
    }
        
    public int getCaseCount(enumCaseType parCaseType) {
        return thisCaseContainer.getCaseCount(parCaseType);
    }

    public bool checkContainCaseType(caseBase parCase) {
        return thisCaseContainer.checkContainCaseType(parCase);
    }

    public bool checkContainConcreteCase(caseBase parCase) {
        return thisCaseContainer.checkContainCaseConcrete(parCase);
    }

    public void observeVoid<A>(object[] parParameters) {
        combatManager.CM.HouC.transcendentTotal.observeVoid<A>(parParameters);
        combatManager.CM.HouC.getTranscendent(thisSide).observeVoid<A>(parParameters);
        thisCaseContainer.observeVoid<A>(parParameters);
    }

    public IEnumerable<B> observeReturnEnumerate<A, B>(object[] parParameters) {
        List<B> tempListResult = new List<B>();
        tempListResult.AddRange(combatManager.CM.HouC.transcendentTotal.observeReturnEnumerate<A, B>(parParameters));
        tempListResult.AddRange(combatManager.CM.HouC.getTranscendent(thisSide).observeReturnEnumerate<A, B>(parParameters));
        tempListResult.AddRange(thisCaseContainer.observeReturnEnumerate<A, B>(parParameters));
        return tempListResult;
    }

    public bool observeInterferable<A>(object[] parParameters) {
        return combatManager.CM.HouC.transcendentTotal.observeInterferable<A>(parParameters) ||
               combatManager.CM.HouC.getTranscendent(thisSide).observeInterferable<A>(parParameters) ||
               thisCaseContainer.observeInterferable<A>(parParameters);
    }
    #endregion relay_caseConatiner
    #endregion case

    #region show
    public void Look(Vector3 parLookDestination) {
        if (parLookDestination != null) {
            transform.rotation = Quaternion.LookRotation(parLookDestination - transform.position);
        }
    }

    public void fadeIn(float parTimer = 1f, float parValue = 1f) {
        thisITransparency.fadeIn(parTimer, parValue);
    }

    public void fadeOut(float parTimer = 1f, float parValue = 0f) {
        thisITransparency.fadeOut(parTimer, parValue);
    }

    public void grabWeapon(Transform parObj) {
        thisWeaponGrabber.grabWeapon(parObj, containerHand, containerBack);
    }

    public void dropWeapon(Transform parObj) {
        thisWeaponGrabber.dropWeapon(parObj, containerHand, containerBack);
    }
    #endregion show

    #region cursor
    /*
    public void setCursorChosen(bool par) {
        thisCursor.setIsChosen(par);
    }

    public void setCursorHovered(bool par) {
        thisCursor.setIsHovered(par);
    }
    */
    #endregion cursor

    #region memento
    public mementoThing freezeDry() {
        List<mementoIParametable> tempList = new List<mementoIParametable>();
        foreach (caseBase c in thisCaseContainer) {
            if (c is not circuitHub and not skillAbst) {
                tempList.Add(c.getMementoIParametable());
            }
        }
        
        return new mementoThing(
            this,
            maxHp,
            curHp,
            stateCur != enumStateWarrior.dead ? curPosition.getCoor() : (0, 0),
            thisSkill.getMementoIParametable(),
            tempList,
            thisCircuitHub.getMementoIParametable(),
            damageDealt_,
            damageTaken_
        );
    }
    #endregion memento

    #region processMaking
    public processByproductActionAbst makeAction() {
        return stateCur switch {
            enumStateWarrior.controlled => makeActionSkip(),
            enumStateWarrior.focussingEnd => makeActionFocussEnd(),
            enumStateWarrior.focussing => makeActionFocuss(),
            enumStateWarrior.skill => makeActionSkill(),
            enumStateWarrior.move => makeActionMove(),
            enumStateWarrior.idleAttack => makeActionAttack(),
            _ => null   // state can't be below idelAttack, it means something's wrong that processAction has null as thisPBA
        };
    }

    private processByproductActionSkip makeActionSkip() {
        return new processByproductActionSkip(this);
    }

    private processByproductActionFocussEnd makeActionFocussEnd() {
        if (thisCaseContainer.thisCaseFocussing != null) {
            return new processByproductActionFocussEnd(this, thisCaseContainer.thisCaseFocussing);
        } else {
            Debug.Log(GetType().Name + " failed to makeActionFocussEnd because thisCaseContainer.thisCaseFocussing is null");
            return null;
        }
    }

    private processByproductActionFocuss makeActionFocuss() {
        return new processByproductActionFocuss(this);
    }

    private processByproductActionSkill makeActionSkill() {
        return new processByproductActionSkill(this, whatToUseSkill);
    }

    private processByproductActionMove makeActionMove() {
        return new processByproductActionMove(this, thisCircuitHub.getNextRoute(this));
    }

    private processByproductActionAttack makeActionAttack() {
        return new processByproductActionAttack(this);
    }
    #endregion processMaking

    #region others
    private void recalculateStatus() {
        thisStatus_.reset();

        // calculating status exceptionally ain't as observing, because it should work normally on total situation not only combat
        thisCaseContainer.observeVoid<ICaseSystemicCalculateStatus>(new object[1] { thisStatus_ }, true);
    }

    public void OnDestroy() {
        Destroy(thisCursor.gameObject);
        Destroy(thisCanvasPersonal.gameObject);
    }

    public void setPlacabler(IPlacabler parPlacabler) {
        thisPlacabler = parPlacabler;
    }
    #endregion others

    #region test
    public void testAllTools() {
        string temp = "TOOL CHECK " + this + " : ";
        foreach (caseBase ta in getCaseList(enumCaseType.tool)) {
            temp += ta + ", ";
        }
        Debug.Log(temp);
    }

    public void testStatus() {
        StringBuilder tempSB = new StringBuilder(this.ToString());

        tempSB.Append("\nSide : ");
        tempSB.Append(thisSide);

        tempSB.Append("\nHP : ");
        tempSB.Append(curHp);
        tempSB.Append(" / ");
        tempSB.Append(maxHp);

        tempSB.Append("\nweaponAmplifierAdd : ");   tempSB.Append(thisStatus.weaponAmplifierAdd);
        tempSB.Append("\nweaponamplifierMultiply : "); tempSB.Append(thisStatus.weaponAmplifierMultiply);
        tempSB.Append("\nskillAmplifierAdd : "); tempSB.Append(thisStatus.skillAmplifierAdd);
        tempSB.Append("\nskillAmplifierMultiply : "); tempSB.Append(thisStatus.skillAmplifierMultiply);
        tempSB.Append("\narmorAdd : "); tempSB.Append(thisStatus.armorAdd);
        tempSB.Append("\narmorMultiply : "); tempSB.Append(thisStatus.armorMultiply);
        tempSB.Append("\ndamageDealt : "); tempSB.Append(damageDealt);
        tempSB.Append("\ndamageTotalTaken : "); tempSB.Append(damageTaken);


        tempSB.Append("\nPosition : ");
        tempSB.Append(curPosition.coor0);
        tempSB.Append(" , ");
        tempSB.Append(curPosition.coor1);

        tempSB.Append("\nState : ");
        tempSB.Append(stateCur);

        tempSB.Append("\nSkill : ");
        tempSB.Append(thisSkill.ToString());
        tempSB.Append("   cooltime ");
        tempSB.Append(thisSkill.timerCur);
        tempSB.Append(" / ");
        tempSB.Append(thisSkill.timerMax);

        tempSB.Append("\nTools : ");
        foreach (caseBase cb in getCaseList(enumCaseType.tool)) {
            tempSB.Append(cb.ToString());
            tempSB.Append(", ");
        }

        tempSB.Append("\nEffects : ");
        foreach (caseBase cb in getCaseList(enumCaseType.effect)){
            tempSB.Append(cb.ToString());
            tempSB.Append(", ");
        }

        Debug.Log(tempSB.ToString());
        thisCircuitHub.testAllCircuits();
    }

    public override string ToString() {
        return GetType().Name;
    }
    #endregion test
}
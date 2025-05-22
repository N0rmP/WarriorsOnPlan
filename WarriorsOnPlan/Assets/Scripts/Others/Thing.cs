using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Text;

using Cases;
using Processes;
using static Unity.VisualScripting.Member;
public enum enumStateWarrior {
    dead = 0,
    //deadRecently = 1,
    controlled = 10,
    focussing = 20,
    skill = 30,
    move = 40,
    idleAttack = 50,
    none = 9999
}

public abstract class Thing : MonoBehaviour {
    #region variable
    private int curHp_;

    private (int coor0, int coor1) curCoor;

    protected int codeSkill = 92001;

    private int actionOrder_ = 999;

    private ICaseUpdateState semaphoreState;

    protected canvasPersonal thisCanvasPersonal;
    protected cursor thisCursor;

    protected List<caseBase> listCaseBaseAll;
    protected List<toolWeapon> listToolWeapon;

    private SortedSet<string> setAttackTriggerName;
    protected Animator thisAnimController;
    private ITransparency thisITransparency;

    protected circuitHub thisCircuitHub;

    #region property
    public enumStateWarrior stateCur { get; private set; }
    public enumSide thisSide { get; protected set; }
    public Vector3 vecMeshCenter { get; protected set; }
    public int actionOrder { 
        get {
            return actionOrder_;
        }
        set {
            if (combatManager.CM.combatState != enumCombatState.preparing) {
                return;
            }
            actionOrder_ = value;
        }
    }
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
    public int weaponAmplifierAdd { get; private set; }
    public int weaponAmplifierMultiply { get; private set; }
    public int skillAmplifierAdd { get; private set; }
    public int skillAmplifierMultiply { get; private set; }
    public int armorAdd { get; private set; }
    public int armorMultiply { get; private set; }
    public int damageTotalDealt { get; private set; }
    public int damageTotalTaken { get; private set; }
    public skillAbst thisSkill { get; protected set; }
    public Thing whatToAttack { get; private set; }
    public Thing whatToUseSkill { get; private set; }
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
    public toolWeapon[] copyWeapons {
        get { return listToolWeapon.ToArray(); }
    }

    public Sprite portrait { get; protected set; }
    #endregion property
    #endregion variable

    #region callbacks
    public void Update() {
        // ★ 메터리얼 페이드 인아웃 구현
    }
    #endregion callbacks

    #region initiation
    public virtual void init(enumSide parSide, int parMaxHp, int[] parSkillParameters) {
        vecMeshCenter = gameObject.getTotalBounds().center;
        thisAnimController = gameObject.GetComponent<Animator>();

        listCaseBaseAll = new List<caseBase>();
        listToolWeapon = new List<toolWeapon>();
        setAttackTriggerName = new SortedSet<string>();

        thisSide = parSide;
        semaphoreState = null;
        stateCur = enumStateWarrior.idleAttack;
        weaponAmplifierAdd = 0;
        weaponAmplifierMultiply = 0;
        skillAmplifierAdd = 0;
        skillAmplifierMultiply = 0;
        armorAdd = 0;
        armorMultiply = 0;
        damageTotalDealt = 0;
        damageTotalTaken = 0;

        portrait = Resources.Load<Sprite>("Image/Portrait/Portrait_" + GetType()) ??
                   Resources.Load<Sprite>("Image/Portrait/Portrait_tester");

        GameObject tempObj;

        maxHpOriginal = parMaxHp;
        setMaxHp(maxHpOriginal, false);
        setCurHp(maxHp, false);

        // initiate canvasPersonal
        tempObj = Instantiate(Resources.Load<GameObject>("Prefab/UI/canvasPersonal"));
        tempObj.transform.SetParent(transform);
        // tempObj.transform.localPosition = vecMeshCenter;
        tempObj.GetComponent<Canvas>().worldCamera = Camera.main;
        thisCanvasPersonal = tempObj.GetComponent<canvasPersonal>();
        thisCanvasPersonal.updateHpText(curHp);
        thisCanvasPersonal.transform.Find("SwissArmyObject").AddComponent<releasablePersonal>().init(this);
        if (thisSide == enumSide.player) {
            thisCanvasPersonal.transform.Find("SwissArmyObject").AddComponent<dragablePersonal>().init(this);
        }

        tempObj = Instantiate(Resources.Load<GameObject>("Prefab/Cursor"));
        transform.SetParent(tempObj.transform);
        thisCursor = tempObj.GetComponent<cursor>();
        thisCursor.setDelEndRun(() => thisAnimController.SetBool("isRun", false));

        // skill making
        thisSkill = null;
        try {
            addCase(gameManager.GM.MC.makeCodableObject<skillAbst>(codeSkill, parSkillParameters));
        } catch (Exception e) {
            string temp = GetType() + " results in a error while making skill with code " + codeSkill + " / parameters ";
            foreach (int i in from n in parSkillParameters select n) {
                temp += i.ToString() + ", ";
            }
            Debug.Log(temp + " ((" + e.Message);
            addCase(gameManager.GM.MC.makeCodableObject<skillAbst>(92001, new int[5] { 3, 3, 1, 5, 1 }));
        }

        // initiate canvasPersonal with skill
        thisCanvasPersonal.setSkill(thisSkill);
        updatePanelSkillTimer();

        // prepare circuits, be aware that this is the only creation of circuitHub in total script of Thing class
        thisCircuitHub = new circuitHub(new int[2] { (int)parSide, 0b010});
        addCase(thisCircuitHub);

        // ★ 만약 다른 shader를 사용하는 Thing이 존재한다면 아래 내용을 변경해야 함
        thisITransparency = gameObject.AddComponent<transparencyStripple>();
        thisITransparency.init();
        thisITransparency.fadeStrict(1f);
    }

    public void restore(mementoThing parMementoThing) {
        // ★ Thing이 사망하여 제거된 상태였을 경우, 원래대로 되돌리기

        setMaxHp(parMementoThing.maxHp, false);
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

        foreach (caseBase cb in listCaseBaseAll.ToArray()) {
            if (cb is skillAbst || cb is circuitHub) {
                continue;
            }
            removeCase(cb); 
        }
        foreach (toolWeapon tw in listToolWeapon.ToArray()) {
            removeCase(tw);
        }
        thisCircuitHub.restore(parMementoThing.mCircuitHub);

        semaphoreState = null;
        stateCur = enumStateWarrior.idleAttack;

        weaponAmplifierAdd = 0;
        weaponAmplifierMultiply = 0;
        skillAmplifierAdd = 0;
        skillAmplifierMultiply = 0;
        armorAdd = 0;
        armorMultiply = 0;
        damageTotalDealt = 0;
        damageTotalTaken = 0;

        thisSkill.restore(parMementoThing.mSkill);
        foreach (mementoIParametable mc in parMementoThing.listCase) {
            addCase(mc.getRestoredIt<caseBase>());
        }

        thisCanvasPersonal.updateHpText(curHp);
        updatePanelSkillTimer();
        updatePanelImageEff();

        resetAnimator();
        combatManager.CM.GC[parMementoThing.coordinates.c0, parMementoThing.coordinates.c1].placeThing(this);
        Look(transform.position -
            thisSide switch {
                enumSide.player => new Vector3(0f, 0f, 1f),
                enumSide.enemy => new Vector3(0f, 0f, -1f),
                enumSide.neutral => new Vector3(1f, 0f, 0f),
                _ => new Vector3(0f, 0f, 0f)
            });
        thisITransparency.fadeStrict(stateCur > enumStateWarrior.dead ? 1f : 0f);        
    }

    // protected abstract skillAbst makeSkill(int[] parSkillParameters);
    #endregion initiation

    #region Move
    // Thing is child of cursor, it's meaningless to set position of Thing only and you should call these methods to move Thing by moving cursor
    public void setPosition(Vector3 parPosition) {
        thisCursor.transform.position = parPosition;
    }

    public void stopMoving() {
        thisCursor.stop();
    }

    public void moveLinear(Vector3 parDestination) {
        thisCursor.GetComponent<cursor>().startLinearMove(parDestination, 1f / (float)combatManager.CM.combatSpeed);
    }

    public void moveParabola(Vector3 parDestination) {
        thisCursor.GetComponent<cursor>().startParabolaMove(parDestination, 1f / (float)combatManager.CM.combatSpeed);
    }
    #endregion Move

    #region affecting
    public virtual void updateTargets() {
        whatToAttack = thisCircuitHub.selectAttackTarget(this);
        whatToUseSkill = thisCircuitHub.selectSkillTarget(this);
    }

    public void updatePanelTotal() {
        updatePanelSkillTimer();
        updatePanelHp();
        updatePanelImageEff();
    }

    public void updatePanelSkillTimer() {
        if (thisSkill.isCoolTimeNeeded) {
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
        foreach (caseBase cb in getCaseList(enumCaseType.effect, false)) {
            if (cb.isVisible) {
                thisCanvasPersonal.addImgEffect(cb);
            }
        }
    }

    public void addPanelImgEffect(caseBase parCB) {
        // if parCB is not visible or Effect, skip it
        if (!parCB.isVisible || parCB.caseType != enumCaseType.effect) {
            return;
        }

        thisCanvasPersonal.addImgEffect(parCB);
    }

    public void removePanelImgEffect(caseBase parCB) {
        thisCanvasPersonal.removeImgEffect(parCB);
    }    

    public void updateState() {
        if (stateCur <= enumStateWarrior.dead) { return; }
        /*
        although technically updateState could be processed with onBeforeAction, it's separated due to algorithm below
        
        change of stateCur require to check all ICaseUpdateState, so it can't be done in only one method
        1. change to lower-value-state by any updater : change instantly, update semaphoreState
        2. change to higher-value-state by semaphoreState : change instantly
        3. change to higher-value-state by none-semaphoreState : save it temporarily and use it when proper
        
        main problem is when case 2 and 3 occur at the same time, case 3 should be applied although case 2 holds the semaphore and blocks it
        so the saved data of case 3 could be applied if it has lower-value-state than stateCur after all onUpdateState is called
        */

        (ICaseUpdateState updater, enumStateWarrior ESW) tempMemory = (null, enumStateWarrior.idleAttack);
        (ICaseUpdateState updater, enumStateWarrior ESW) tempBuffer;
        foreach (ICaseUpdateState cb in getCaseList<ICaseUpdateState>()) {
            tempBuffer = cb.onUpdateState(this);
            if (tempBuffer.ESW < stateCur) {
                semaphoreState?.onIntefered(this);
                stateCur = tempBuffer.ESW;
                semaphoreState = tempBuffer.updater;
            } else if (semaphoreState == tempBuffer.updater) {
                stateCur = tempBuffer.ESW;
            } else if (tempBuffer.ESW < tempMemory.ESW) {
                tempMemory = tempBuffer;
            }
        }

        if (tempMemory.ESW < stateCur) {
            stateCur = tempMemory.ESW;
            semaphoreState = tempMemory.updater;
        }
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

    public void setMaxHp(int parValue, bool isPlus = true) {
        maxHp = Math.Max(isPlus ? maxHp + parValue : parValue, 1);
    }

    /*
    public void useSkill() {
        thisSkill.useSkill(this, whatToUseSkill);
    }
    */

    public virtual void destroied() {
        stateCur = enumStateWarrior.dead;
        curPosition?.expelThing(false);
        combatManager.CM.HouC.killThing(this);
    }

    public void destroiedSystemically() { 
        // ★ 시스템적 삭제
    }
    #endregion affecting

    #region circuit
    public void setCircuit(
        int parCodeSensorForMove, int[] ppSensorForMove,
        int parCodeNavigatorPrioritized, int[] ppNavigatorPrioritized,
        int parCodeNavigatorIdle, int[] ppNavigatorIdle,
        int parCodeSensorForSkill, int[] ppSensorForSkill,
        int parCodeSelecterForSkill, int[] ppSelecterForSkill,
        int parCodeSelecterForAttack, int[] ppSelecterForAttack) {

        thisCircuitHub.setCircuitHub(
        thisSide,
        parCodeSensorForMove, ppSensorForMove,
        parCodeNavigatorPrioritized, ppNavigatorPrioritized,
        parCodeNavigatorIdle, ppNavigatorIdle,
        parCodeSensorForSkill, ppSensorForSkill,
        parCodeSelecterForSkill, ppSelecterForSkill,
        parCodeSelecterForAttack, ppSelecterForAttack
            );    
    }

    public string[] getCircuitInfo() {
        return thisCircuitHub.getTotalInfo();
    }

    public int[] getCircuitParameters(int parCircuitType) {
        return thisCircuitHub.getSingleParameter(parCircuitType);
    }
    #endregion circuit

    #region case
    public virtual void addCase(caseBase parCase) {
        if (parCase == null) {
            return;
        }

        listCaseBaseAll.Add(parCase);
        switch (parCase.caseType) {
            case enumCaseType.tool:
                if (parCase is toolWeapon tempToolWeapon) {
                    listToolWeapon.Add(tempToolWeapon);
                }
                break;
            case enumCaseType.circuit:
                break;
            case enumCaseType.effect:
                break;
            case enumCaseType.skill:
                if (thisSkill == null) {
                    thisSkill = (skillAbst)parCase;
                }
                break;
            default:
                break;
        }

        updateCaseResult();
    }

    public virtual void removeCase(caseBase parCase) {
        listCaseBaseAll.Remove(parCase);
        switch (parCase.caseType) {
            case enumCaseType.tool:
                if (parCase is toolWeapon tempToolWeapon) {
                    listToolWeapon.Remove(tempToolWeapon);
                }
                break;
            case enumCaseType.circuit:
                break;
            case enumCaseType.effect:
                thisCanvasPersonal.removeImgEffect(parCase);
                break;
            case enumCaseType.skill:
                thisSkill = null;
                break;
            default:
                break;
        }

        updateCaseResult();
    }

    public void updateCaseResult() {
        // ★ 각 능력치 재계산
        if (combatUIManager.CUM.CStatus.thisThing == this) {
            combatUIManager.CUM.CStatus.updateTotal();
        }
    }

    public List<T> getCaseList<T>(bool parIsForObserving = true) {
        // to prevent on~ methods to be called during prearing step getCaseList doesn't work by returning only empty list
        // you can use it anyway by setting parIsForObserving to false
        if (parIsForObserving && combatManager.CM.combatState != enumCombatState.combat) {
            return new List<T> { };
        }

        List<T> tempResult = null;

        foreach (caseBase cb in listCaseBaseAll) {
            if (tempResult == null) {
                tempResult = new List<T>();
            }
            if (cb is T tempCA)
                tempResult.Add(tempCA);
        }

        return tempResult;
    }

    public List<caseBase> getCaseList(enumCaseType parCaseType, bool parIsForObserving = true) {
        // to prevent on~ methods to be called during prearing step getCaseList doesn't work by returning only empty list
        // you can use it anyway by setting parIsForObserving to false
        if (parIsForObserving && combatManager.CM.combatState == enumCombatState.combat) {
            return new List<caseBase> { };
        }

        List<caseBase> tempResult = null;

        foreach (caseBase cb in listCaseBaseAll) {
            if (tempResult == null) {
                tempResult = new List<caseBase>();
            }
            if (cb.caseType == parCaseType) {
                tempResult.Add(cb);
            }
        }

        return tempResult;
    }

    public bool checkContainCase(caseBase parCase) {
        foreach (caseBase cb in listCaseBaseAll) {
            if (cb.GetType() == parCase.GetType()) {
                return true;
            }
        }
        return false;
    }

    public bool checkContainConcreteCase(caseBase parCase) {
        foreach (caseBase cb in listCaseBaseAll) {
            if (cb == parCase) {
                return true;
            }
        }
        return false;
    }

    public List<toolWeapon> getListAvailableWeapon(Thing parTarget = null) {
        int tempDistanceToTarget = node.getDistance(curPosition, (parTarget ?? whatToAttack).curPosition);
        List<toolWeapon> tempResult = new List<toolWeapon>();
        foreach (toolWeapon tw in getCaseList<toolWeapon>(false).ToArray()) {
            // skip when target is out of the weapon's range
            if (tempDistanceToTarget > tw.rangeMax || tempDistanceToTarget < tw.rangeMin || tw.isReady) {
                continue;
            }

            tempResult.Add(tw);
        }

        return tempResult;
    }
    #endregion case

    #region animation
    public void Look(Vector3 parLookDestination) {
        if (parLookDestination != null) {
            transform.rotation = Quaternion.LookRotation(parLookDestination - transform.position);
        }
    }

    private void setAnimationSpeed() {
        thisAnimController.SetFloat("multiplierTotal", combatManager.CM.combatSpeed);
        thisAnimController.SetFloat("multiplierAttack", Math.Max(1, combatManager.CM.combatSpeed * setAttackTriggerName.Count));
    }

    public void clearAttackAnimation() {
        setAttackTriggerName.Clear();
    }

    public void addAttackAnimation(enumAttackAnimation parEnumAttackAnimation) {
        setAttackTriggerName.Add(parEnumAttackAnimation.ToString());
    }

    public void addAttackAnimation(IEnumerable parEnumAttackAnimation) {
        foreach (enumAttackAnimation eaa in parEnumAttackAnimation) {
            addAttackAnimation(eaa);
        }
    }

    public int getAttackAnimationCount() {
        return setAttackTriggerName.Count;
    }

    public void animateMove() {
        setAnimationSpeed();
        thisAnimController.SetBool("isRun", true);
    }

    public void animateAttack(bool parIsProjectile = true) {
        int tempProjectileCount = 0;
        setAnimationSpeed();
        foreach (string trigName in setAttackTriggerName) {
            thisAnimController.SetTrigger(trigName);
            if (trigName == "trigAttackBrandish" || trigName == "trigAttackBow" || trigName == "trigAttackCase") {
                tempProjectileCount++;
            }
        }
        thisAnimController.SetTrigger("trigAttackStart");
    }

    public void animateUseSkill() {
        setAnimationSpeed();
        thisAnimController.SetTrigger("trigUseSkill");
    }

    public void animateDead() {
        setAnimationSpeed();
        thisAnimController.SetTrigger("trigDead");
    }

    /*
    public virtual void animate() {
        thisAnimController.SetBool("isFocussing", false);
        thisAnimController.SetBool("isControlled", false);
        switch (stateCur) {
            case enumStateWarrior.deadRecently:
                thisAnimController.SetTrigger("trigDead");
                //★ 페이드 아웃
                break;
            case enumStateWarrior.controlled:
                thisAnimController.SetBool("isControlled", true);
                break;
            case enumStateWarrior.focussing:
                thisAnimController.SetBool("isFocussing", true);
                break;
            case enumStateWarrior.move:                
                break;
            case enumStateWarrior.idleAttack:
                animateAttack();
                break;
            case enumStateWarrior.skill:
                thisAnimController.SetTrigger("trigUseSkill");
                break;
            default:
                break;
        }

        현재 state가 업데이트되고 있지 않은 것으로 추정됨, 또한 이동 애니메이션을 processByproductMove가 담당하고 있어 이에 대한 정리정돈이 요구됨
    }

    public void animate(Vector3 parLookDirection) {
        Look(parLookDirection);
        animate();
    }
    */

    // reset all parameters, and play the idle animation state
    public void resetAnimator() {
        foreach (AnimatorControllerParameter ACP in thisAnimController.parameters) {
            switch (ACP.type) {
                case AnimatorControllerParameterType.Int:
                    thisAnimController.SetInteger(ACP.name, 0);
                    break;
                case AnimatorControllerParameterType.Float:
                    thisAnimController.SetFloat(ACP.name, (ACP.name.Substring(0, 10) == "multiplier") ? 1f : 0f);
                    break;
                case AnimatorControllerParameterType.Bool:
                    thisAnimController.SetBool(ACP.name, false);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    thisAnimController.ResetTrigger(ACP.name);
                    break;
            }
        }
        thisAnimController.Play("Idle", 0);
    }

    public void fadeIn(float parTimer = 1f, float parValue = 1f) {
        thisITransparency.fadeIn(parTimer, parValue);
    }

    public void fadeOut(float parTimer = 1f, float parValue = 0f) {
        thisITransparency.fadeOut(parTimer, parValue);
    }
    #endregion animation

    #region cursor
    public void setCursorChosen(bool par) {
        thisCursor.setIsChosen(par);
    }

    public void setCursorHovered(bool par) {
        thisCursor.setIsHovered(par);
    }
    #endregion cursor

    #region numberManagement
    // most numbers can be negative, damageTotal-Delat & Taken can't be negative
    public void addWeaponAmplifierAdd(int par) {
        weaponAmplifierAdd += par;
    }

    public void addWeaponAmplifierMultiply(int par) {
        weaponAmplifierMultiply += par;
    }

    public void addSkillAmplifierAdd(int par) {
        skillAmplifierAdd += par;
    }

    public void addSkillAmplifierMultiply(int par) {
        skillAmplifierMultiply += par;
    }

    public void addArmorAdd(int par) {
        armorAdd += par;
    }

    public void addArmorMultiply(int par) {
        armorMultiply += par;
    }

    public void addDamageTotalDealt(int par) {
        if (par > 0) {
            damageTotalDealt += par;
        }
    }

    public void addDamageTotalTaken(int par) {
        if (par > 0) {
            damageTotalTaken += par;
        }
    }
    #endregion numberManagement

    #region memento
    public mementoThing freezeDry() {
        List<mementoIParametable> tempList = new List<mementoIParametable>();
        foreach (caseBase c in listCaseBaseAll) {
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
            thisCircuitHub.getMementoIParametable()
            );
    }    
    #endregion memento

    #region processMaking
    public processAbst makeAction() {
        testStatus();
        switch (stateCur) {
            // ★ 각각의 warrior 행동 시작 시 효과 발동
            case enumStateWarrior.controlled:
                return makeActionSkip();
            case enumStateWarrior.focussing:
                return makeActionFocuss();
            case enumStateWarrior.skill:
                return makeActionSkill();
            case enumStateWarrior.move:
                return makeActionMove();
            case enumStateWarrior.idleAttack:
                return makeActionAttack();
            default:
                // thing's state can't have lower priority below idleAttack, so this part shouldn't be executed
                Debug.Log(this.GetType() + " has inproper state now : " + stateCur);
                return makeActionAttack();
        }
    }

    private processActionSkip makeActionSkip() {
        return new processActionSkip();
    }

    private processAbst makeActionFocuss() {
        return new processActionFocuss(this);
    }

    private processActionSkill makeActionSkill() {
        return new processActionSkill(this, whatToUseSkill);
    }

    private processActionMove makeActionMove() {
        return new processActionMove(this, thisCircuitHub.getNextRoute(this));
    }

    private processActionAttack makeActionAttack() {
        return new processActionAttack(this);
    }
    #endregion processMaking

    #region test
    public void testAllTools() {
        string temp = "TOOL CHECK " + this + " : ";
        foreach (caseBase ta in getCaseList(enumCaseType.tool, false)) {
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

        tempSB.Append("\nweaponAmplifierAdd : ");   tempSB.Append(weaponAmplifierAdd);
        tempSB.Append("\nweaponamplifierMultiply : "); tempSB.Append(weaponAmplifierMultiply);
        tempSB.Append("\nskillAmplifierAdd : "); tempSB.Append(skillAmplifierAdd);
        tempSB.Append("\nskillAmplifierMultiply : "); tempSB.Append(skillAmplifierMultiply);
        tempSB.Append("\narmorAdd : "); tempSB.Append(armorAdd);
        tempSB.Append("\narmorMultiply : "); tempSB.Append(armorMultiply);
        tempSB.Append("\ndamageTotalDealt : "); tempSB.Append(damageTotalDealt);
        tempSB.Append("\ndamageTotalTaken : "); tempSB.Append(damageTotalTaken);


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
        foreach (caseBase cb in getCaseList(enumCaseType.tool, false)) {
            tempSB.Append(cb.ToString());
            tempSB.Append(", ");
        }

        tempSB.Append("\nEffects : ");
        foreach (caseBase cb in getCaseList(enumCaseType.effect, false)){
            tempSB.Append(cb.ToString());
            tempSB.Append(", ");
        }

        Debug.Log(tempSB.ToString());
        thisCircuitHub.testAllCircuits();
    }

    public override string ToString() {
        return GetType().ToString();
    }
    #endregion test
}
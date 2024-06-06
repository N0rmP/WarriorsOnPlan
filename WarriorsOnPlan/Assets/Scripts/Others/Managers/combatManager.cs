using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class combatManager : MonoBehaviour
{
    public static combatManager CM;
    public graphComponent GC { get; private set; }
    public fxComponent FC { get; private set; }

    // interval time between each action
    //★ intervalTime에 비례해 애니메이션 속도를 빠르게 할 수 있나 확인... 근데 그냥 게임 자체에 배속을 걸 수 있는지 찾는 게 빠를 것도 같다.
    private float intervalTime = 1f;

    //warriors array's indices represent different team, 0 = player's / 1 = computer's
    private List<warriorAbst>[] warriorsHpSorted_;
    private List<warriorAbst>[] warriorsDamageDealtSorted_;
    private List<warriorAbst>[] warriorsActionOrder_;
    private List<warriorAbst>[] warriorsDead_;

    #region properties
    //warriors in combatManager can't be copies because they consist of list & array
    public List<warriorAbst>[] warriorsHpSorted {
        get {
            return warriorsHpSorted_;
        }
    }
    public List<warriorAbst>[] warriorsDamageDealtSorted {
        get {
            return warriorsDamageDealtSorted_;
        }
    }
    public List<warriorAbst>[] warriorsActionOrder {
        get {
            return warriorsActionOrder_;
        }
    }
    public List<warriorAbst>[] warriorsDead {
        get {
            return warriorsDead_;
        }
    }
    #endregion properties

    private comparerHp comparerHpInstance;
    private comparerDamageDealt comparerDamageDealtInstance;

    #region callbacks
    public void Awake() {
        //combatManager is also a singleton manager, but it only exists during one scene
        if (CM == null) {
            CM = this;
        } else {
            Destroy(this);
        }

        GC = new graphComponent(7, 7);
        FC = new fxComponent();

        //comparers
        comparerHpInstance = new comparerHp();
        comparerDamageDealtInstance = new comparerDamageDealt();        

        //list initiate
        warriorsHpSorted_ = new List<warriorAbst>[2]{
            new List<warriorAbst>(),
            new List<warriorAbst>()
        };
        warriorsDamageDealtSorted_ = new List<warriorAbst>[2] {
            new List<warriorAbst>(),
            new List<warriorAbst>()
        };
        warriorsActionOrder_ = new List<warriorAbst>[2] {
            new List<warriorAbst>(),
            new List<warriorAbst>()
        };
        warriorsDead_ = new List<warriorAbst>[2]{
            new List<warriorAbst>(),
            new List<warriorAbst>()
        };
    }

    public void Start() {
        //★ test
        GameObject w1 = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/tester"));
        w1.GetComponent<warriorAbst>().init(true, 6, 6, 5);
        GameObject w2 = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/tester"));
        w2.GetComponent<warriorAbst>().init(false, 0, 0, 5);

        Coroutine c = StartCoroutine(combatLoop());
    }
    #endregion callbacks

    public IEnumerator combatLoop() {
        bool overCheck() {
            return (warriorsHpSorted_[0].Count <= 0 || warriorsHpSorted_[1].Count <= 0);
        }

        bool isPlrTurn = true;
        List<warriorAbst> tempListActors;

        //before combat starts, activate all onEngage from warriors
        foreach (List<warriorAbst> lis in warriorsActionOrder_) {
            foreach (warriorAbst wa in lis) {
                foreach (ICaseEngage ca in wa.getCaseList<ICaseEngage>()) {
                    ca.onEngage(wa);
                }
            }
        }

        while (true) {
            //★ 플레이어의 warrior는 사전에 지정한 순서대로, 상대방은 따로 지정되지 않았다면 남은 체력 내림차순으로
            //★ 상대방 warriorsActionsOrder_[1]을 warriorsHpSorted[1]에 참조시키면 위 기능을 간단히 해결할 수 있다.
            tempListActors = warriorsActionOrder_[isPlrTurn ? 0 : 1];

            //★ 턴 시작 시 효과 발동
            foreach (warriorAbst wa in tempListActors) {
                //process before action
                warriorsHpSorted_[0].Sort(comparerHpInstance);
                warriorsHpSorted_[1].Sort(comparerHpInstance);
                warriorsDamageDealtSorted_[0].Sort(comparerDamageDealtInstance);
                warriorsDamageDealtSorted_[1].Sort(comparerDamageDealtInstance);
                wa.updateTargets();
                wa.updateState();
                foreach (ICaseBeforeAction cb in wa.getCaseList<ICaseBeforeAction>()) {
                    cb.onBeforeAction(wa);
                }

                // decide what action this warrior does this time, priority is (Being Controlled) > UseSkill > Attack > Move
                switch (wa.stateCur) {
                    // ★ 각각의 warrior 행동 시작 시 효과 발동
                    case enumStateWarrior.controlled:
                        break;
                    case enumStateWarrior.focussing:
                        break;
                    case enumStateWarrior.skill:
                        //★ processUseSkill 호출
                        break;
                    case enumStateWarrior.move:
                        processMove(wa, wa.navigator.getNextEDirection());
                        break;
                    case enumStateWarrior.idleAttack:
                        wa.clearAttackAnimation();
                        foreach (toolWeapon tw in wa.copyWeapons) {
                            if (tw.timerCur <= 0) {
                                wa.addAttackAnimation(tw.animationType.ToString());
                                processAttack(wa, wa.whatToAttack, tw.getDamageInfo());
                            }
                        }
                        break;
                    default:
                        break;
                }
                wa.animate();

                // ★ 각각의 warrior 행동 종료 시 효과 발동
                yield return new WaitForSeconds(intervalTime);

                //dead warriors
                foreach (List<warriorAbst> lis in warriorsDead_) {
                    foreach (warriorAbst dwa in lis) {
                        if (dwa.stateCur == enumStateWarrior.deadRecently) {
                            dwa.animate();
                            dwa.stateCur = enumStateWarrior.dead;
                        }
                    }
                }

                //check if this combat is over after each action
                if (overCheck()) {
                    //★ 1초 정지 (사망 애니메이션을 보여서 가독성 강화)
                    //★게임 종료 처리
                    //return (warriorsHpSorted_[1].Count <= 0);
                }
            }
            //turn end processes
            foreach (warriorAbst wa in tempListActors) {
                //update weapon timer
                foreach (toolWeapon tw in wa.copyWeapons) {
                    tw.updateTimer();
                }
                //onTurnEnd
                foreach (ICaseTurnEnd cb in wa.getCaseList<ICaseTurnEnd>()) {
                    cb.onTurnEnd(wa);
                }
            }

            isPlrTurn = !isPlrTurn;
        }
    }

    #region processors
    public void processAttack(Thing source, Thing target, damageInfo DInfo) {
        //before attack
        foreach (ICaseBeforeAttack cb in source.getCaseList<ICaseBeforeAttack>()) {
            cb.onBeforeAttack(source, target, DInfo);
        }
        //before damaged
        foreach (ICaseBeforeDamaged cb in target.getCaseList<ICaseBeforeDamaged>()) {
            cb.onBeforeDamaged(source, target, DInfo);
        }
        //attck now
        DInfo.ATTACK(target);
        if (source is warriorAbst tempSource) {
            tempSource.addDamageTotalDealt(DInfo.damage);
        }
        //after damaged
        foreach (ICaseAfterDamaged cb in target.getCaseList<ICaseAfterDamaged>()) {
            cb.onAfterDamaged(source, target, DInfo);
        }
        //after attack
        foreach (ICaseAfterAttack cb in source.getCaseList<ICaseAfterAttack>()) {
            cb.onAfterAttack(source, target, DInfo);
        }
        //after the caseAll attack
        (DInfo.sourceCaseAll as toolWeapon)?.resetTimer();
    }

    //processMove with EDirection parameter makes a warrior walk a node to the parameter-direction
    public void processMove(warriorAbst source, EDirection parEDir) {
        //sendThing method will check if it's valid movement
        source.curPosition.sendThing(parEDir);
        Vector3 tempDestination = source.curPosition.getVector3();
        source.transform.rotation = Quaternion.LookRotation(tempDestination - source.transform.position);
        source.startLinearMove(tempDestination);
    }

    //processMove with two int parameters makes a warrior teleport to the destination
    public void processMove(warriorAbst source, int parCoor0, int parCoor1) {

    }

    //processUseSkill makes a warrior use his skill, this method is used for consistency and ICase calls
    public void processUseSkill(warriorAbst source, int parCoor0, int parCoor1) {

    }

    //processPlace 
    public void processPlace(Thing source, int parCoor0, int parCoor1) {
        //check if the position is out of boundary, or already containing another thing
        if (parCoor0 < 0 || parCoor0 > GC.size0 ||
           parCoor1 < 0 || parCoor1 > GC.size1 ||
           GC[parCoor0, parCoor1].thingHere != null) {
            return;
        }

        GC[parCoor0, parCoor1].placeThing(source);
        if (source is warriorAbst) {
            warriorAbst tempWarrior = (warriorAbst)source;
            int tempPlrSideNum = tempWarrior.isPlrSide ? 0 : 1;
            warriorsHpSorted_[tempPlrSideNum].Add(tempWarrior);
            warriorsDamageDealtSorted_[tempPlrSideNum].Add(tempWarrior);
            warriorsActionOrder_[tempPlrSideNum].Add(tempWarrior);
        }
    }
    #endregion

    #region utility
    public void addWarrior(warriorAbst parWarrior, bool isSortAfterAdd = false) {
        int tempSide = parWarrior.isPlrSide ? 0 : 1;
        warriorsHpSorted_[tempSide].Add(parWarrior);
        warriorsDamageDealtSorted_[tempSide].Add(parWarrior);

        if (isSortAfterAdd) {
            updateWarriors();
        }
    }

    public void removeWarrior(warriorAbst parWarrior) {
        int tempSide = parWarrior.isPlrSide ? 0 : 1;
        warriorsHpSorted_[tempSide].Remove(parWarrior);
        warriorsDamageDealtSorted_[tempSide].Remove(parWarrior);
    }

    public void addDeadWarrior(warriorAbst parWarrior) {
        int tempSide = parWarrior.isPlrSide ? 0 : 1;
        warriorsDead_[tempSide].Add(parWarrior);
    }

    //sort warriors Lists, careful not to call it frequently due to overhead of sorting 4 times (★might call it every certain period)
    public void updateWarriors() {
        warriorsHpSorted_[0].Sort(comparerHpInstance);
        warriorsHpSorted_[1].Sort(comparerHpInstance);
        warriorsDamageDealtSorted_[0].Sort(comparerDamageDealtInstance);
        warriorsDamageDealtSorted_[1].Sort(comparerDamageDealtInstance);
    }
    #endregion utility

    #region internalClasses
    private class comparerHp : IComparer<warriorAbst> {
        public int Compare(warriorAbst w1, warriorAbst w2) {
            return (w1.curHp - w2.curHp);
        }
    }

    private class comparerDamageDealt : IComparer<warriorAbst> {
        public int Compare(warriorAbst w1, warriorAbst w2) {
            return (w1.damageTotalDealt - w2.damageTotalDealt);
        }
    }
    #endregion internalClasses
}
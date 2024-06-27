using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class combatManager : MonoBehaviour {
    public static combatManager CM;
    public graphComponent GC { get; private set; }
    public fxComponent FC { get; private set; }

    private bool isCombatLooping = false;

    // interval time between each action
    //★ intervalTime에 비례해 애니메이션 속도를 빠르게 할 수 있나 확인... 근데 그냥 게임 자체에 배속을 걸 수 있는지 찾는 게 빠를 것도 같다.
    private float intervalTime = 0.5f;

    //warriors array's indices represent different team, 0 = player's / 1 = computer's
    private List<Thing>[] thingsHpSorted_;
    private List<Thing>[] thingsDamageDealtSorted_;
    private List<Thing>[] thingsActionOrder_;
    private List<Thing>[] thingsDead_;

    #region properties
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
        thingsHpSorted_ = new List<Thing>[3]{
            new List<Thing>(),
            new List<Thing>(),
            new List<Thing>()
        };
        thingsDamageDealtSorted_ = new List<Thing>[3] {
            new List<Thing>(),
            new List<Thing>(),
            new List<Thing>()
        };
        thingsActionOrder_ = new List<Thing>[3] {
            new List<Thing>(),
            new List<Thing>(),
            new List<Thing>()
        };
        thingsDead_ = new List<Thing>[3]{
            new List<Thing>(),
            new List<Thing>(),
            new List<Thing>()
        };
    }

    public void Start() {
        //★ test
        UI 사용해서 아군 warrior circuit 설정하기
        processSpawn("tester", enumSide.player, (6, 6));
        processSpawn("tester", enumSide.enemy, (0, 0));

        Coroutine c = StartCoroutine(combatLoop());
    }
    #endregion callbacks

    public IEnumerator combatLoop() {
        bool overCheck() {
            return (thingsHpSorted_[0].Count <= 0 || thingsHpSorted_[1].Count <= 0);
        }

        int codeTurn = 0;
        List<Thing> tempListActors;
        Vector3 tempLookDirection;

        //before combat starts, activate all onEngage from warriors
        tempListActors = new List<Thing>();
        tempListActors.AddRange(thingsActionOrder_[0]);
        tempListActors.AddRange(thingsActionOrder_[1]);
        tempListActors.AddRange(thingsActionOrder_[2]);
        foreach (Thing th in tempListActors) {
            foreach (ICaseEngage ca in th.getCaseList<ICaseEngage>()) {
                ca.onEngage(th);
            }
        }

        isCombatLooping = true;

        while (true) {
            //nuetral side's turn precedes enemy's turn to assure that player can make use of the nuetral side perfectly
            tempListActors = codeTurn switch { 
                0 => thingsActionOrder_[0],
                1 => thingsActionOrder_[2],
                2 => thingsActionOrder_[1],
                _ => new List<Thing>()  //prevent error
            };

            foreach (Thing th in tempListActors) {
                //update timers
                foreach (caseTimerHostileTurn ct in th.getCaseList<caseTimerHostileTurn>()) {
                    ct.updateOnTurnStart(th);
                }
                //onTurnStart
                foreach (ICaseTurnStart cb in th.getCaseList<ICaseTurnStart>()) {
                    cb.onTurnStart(th);
                }
            }

            foreach (Thing th in tempListActors) {
                //process before action
                thingsHpSorted_[0].Sort(comparerHpInstance);
                thingsHpSorted_[1].Sort(comparerHpInstance);
                thingsHpSorted_[2].Sort(comparerHpInstance);
                thingsDamageDealtSorted_[0].Sort(comparerDamageDealtInstance);
                thingsDamageDealtSorted_[1].Sort(comparerDamageDealtInstance);
                thingsDamageDealtSorted_[2].Sort(comparerDamageDealtInstance);
                th.updateTargets();
                foreach (caseTimerSelfishTurn ct in th.getCaseList<caseTimerSelfishTurn>()) {
                    ct.updateOnActionStart(th);
                }
                foreach (ICaseBeforeAction cb in th.getCaseList<ICaseBeforeAction>()) {
                    cb.onBeforeAction(th);
                }
                th.updateState();

                tempLookDirection = Vector3.negativeInfinity;
                switch (th.stateCur) {
                    // ★ 각각의 warrior 행동 시작 시 효과 발동
                    case enumStateWarrior.controlled:
                        break;
                    case enumStateWarrior.focussing:
                        break;
                    case enumStateWarrior.skill:
                        tempLookDirection = processUseSkill(th);
                        break;
                    case enumStateWarrior.move:
                        tempLookDirection = processMove(th);
                        break;
                    case enumStateWarrior.idleAttack:
                        tempLookDirection = processAttack(th, th.whatToAttack);
                        break;
                    default:
                        break;
                }
                th.animate(tempLookDirection);

                // ★ 각각의 warrior 행동 종료 시 효과 발동
                foreach (ICaseAfterAction cb in th.getCaseList<ICaseAfterAction>()) {
                    cb.onAfterAction(th);
                }
                foreach (caseTimerSelfishTurn ct in th.getCaseList<caseTimerSelfishTurn>()) {
                    ct.updateOnActionEnd(th);
                    if ((ct.timerCur <= 0) && (ct is skillAbst tempSkill)) {
                        foreach (ICaseSkillReady cb in th.getCaseList<ICaseSkillReady>()) {
                            cb.onSkillReady(th);
                        }
                    }
                }

                //interval between things
                yield return new WaitForSeconds(intervalTime);

                //dead warriors
                // ★ 중립 세력 추가시키기, 개선
                foreach (List<Thing> lis in thingsDead_) {
                    foreach (Thing dwa in lis) {
                        if (dwa.stateCur == enumStateWarrior.deadRecently) {
                            dwa.animate(tempLookDirection);
                            dwa.destroiedTotally();
                        }
                    }
                }

                //check if this combat is over after each action
                if (overCheck()) {
                    isCombatLooping = false;
                    //★ 1초 정지 (사망 애니메이션을 보여서 가독성 강화)
                    //★ 게임 종료 처리
                    //return (warriorsHpSorted_[1].Count <= 0);
                }
            }

            //turn end processes
            foreach (Thing th in tempListActors) {
                //onTurnEnd
                foreach (ICaseTurnEnd cb in th.getCaseList<ICaseTurnEnd>()) {
                    cb.onTurnEnd(th);
                }
                foreach (caseTimerHostileTurn ct in th.getCaseList<caseTimerHostileTurn>()) {
                    ct.updateOnTurnEnd(th);
                }
                foreach (caseTimerFriendlyTurn ct in th.getCaseList<caseTimerFriendlyTurn>()) {
                    ct.updateOnTurnEnd(th);
                }
            }

            //interval between turns
            yield return new WaitForSeconds(intervalTime);

            codeTurn = (++codeTurn >= 3 ? 0 : codeTurn);
        }
    }

    #region processors
    //★ 무기를 사용한 공격 행동 / 피해를 주는 과정을 따로 만들어볼 것
    public void processDealDamage(Thing source, Thing target, damageInfo DInfo) {
        //before attack
        foreach (ICaseBeforeDealDamage cb in source.getCaseList<ICaseBeforeDealDamage>()) {
            cb.onBeforeDealDamage(source, target, DInfo);
        }
        //before damaged
        foreach (ICaseBeforeDamaged cb in target.getCaseList<ICaseBeforeDamaged>()) {
            cb.onBeforeDamaged(source, target, DInfo);
        }
        //attck now
        int tempDamage = DInfo.DEAL(target);
        source.addDamageTotalDealt(DInfo.damage);
        //after damaged
        foreach (ICaseAfterDamaged cb in target.getCaseList<ICaseAfterDamaged>()) {
            cb.onAfterDamaged(source, target, DInfo);
        }
        //after attack
        foreach (ICaseAfterDealDamage cb in source.getCaseList<ICaseAfterDealDamage>()) {
            cb.onAfterDealDamage(source, target, DInfo);
        }
    }

    public Vector3 processAttack(Thing source, Thing target) {
        damageInfo tempDInfo;
        List<damageInfo> tempListDInfo = new List<damageInfo>();

        source.clearAttackAnimation();
        foreach (ICaseBeforeAttack cb in source.getCaseList<ICaseBeforeAttack>()) {
            cb.onBeforeAttack(source, target);
        }
        foreach (toolWeapon tw in source.copyWeapons) {
            Debug.Log(tw.timerCur);
            if (tw.isReady) {
                source.addAttackAnimation(tw.animationType.ToString());
                tempDInfo = tw.attack();
                processDealDamage(source, source.whatToAttack, tempDInfo);
                tempListDInfo.Add(tempDInfo);
                if (tw is ICaseThisWeaponUsed tempCB) {
                    tempCB.onThisWeaponUsed(source, target, tempDInfo);
                }
            }
        }
        foreach (ICaseAfterAttack cb in source.getCaseList<ICaseAfterAttack>()) {
            cb.onAfterAttack(source, target, tempListDInfo);
        }

        return target.transform.position;
    }

    //processMove with EDirection parameter makes a warrior walk a node to the parameter-direction
    public Vector3 processMove(Thing source) {
        node tempDestination = source.getNextRoute();
        
        source.curPosition.sendThing(tempDestination);

        Vector3 tempDestinationVector = source.curPosition.getVector3();
        source.transform.rotation = Quaternion.LookRotation(tempDestinationVector - source.transform.position);
        source.startLinearMove(tempDestinationVector);

        return source.curPosition.getVector3();
    }

    //processMove with two int parameters makes a warrior teleport to the destination
    public Vector3 processMove(Thing source, int parCoor0, int parCoor1) {
        return Vector3.negativeInfinity;
    }

    //processUseSkill makes a warrior use his skill, this method is used for consistency and ICase calls
    public Vector3 processUseSkill(Thing source) {
        foreach (ICaseBeforeUseSkill cb in source.getCaseList<ICaseBeforeUseSkill>()) {
            cb.onBeforeUseSkill(source);
        }
        source.useSkill();
        foreach (ICaseAfterUseSkill cb in source.getCaseList<ICaseAfterUseSkill>()) {
            cb.onAfterUseSkill(source);
        }

        return (source.whatToUseSkill == null) ? Vector3.negativeInfinity : source.whatToUseSkill.transform.position;
    }

    public void processSpawn(string sourceName, enumSide parSide, (int c0, int c1) parCoor) {
        GameObject w1 = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/" + sourceName));

        Thing tempThing = w1.GetComponent<Thing>();
        tempThing.init(parSide, 5);
        addThing(tempThing);

        processPlace(tempThing, parCoor);
    }

    //processPlace 
    public void processPlace(Thing source, (int c0, int c1) parCoor) {
        //check if the position is out of boundary, or already containing another thing
        if (parCoor.c0 < 0 || parCoor.c0 > GC.size0 ||
           parCoor.c1 < 0 || parCoor.c1 > GC.size1 ||
           GC[parCoor.c0, parCoor.c1].thingHere != null) {
            return;
        }

        GC[parCoor.c0, parCoor.c1].placeThing(source);
    }
    #endregion

    #region utility
    public Thing[] copyWarriorsActionOrder(int parIndex) {
        return thingsActionOrder_[parIndex].ToArray();
    }

    public void addThing(Thing parThing, bool isSortAfterAdd = false) {
        int tempIndex = (int)parThing.thisSide;
        thingsHpSorted_[tempIndex].Add(parThing);
        thingsDamageDealtSorted_[tempIndex].Add(parThing);

        //★ 추후 행동 순서 결정 시스템이 만들어지면 조정할 것
        thingsActionOrder_[tempIndex].Add(parThing);

        if (isSortAfterAdd) {
            updateTotal();
        }
    }

    public void removeThing(Thing parThing) {
        int tempIndex = (int)parThing.thisSide;
        thingsHpSorted_[tempIndex].Remove(parThing);
        thingsDamageDealtSorted_[tempIndex].Remove(parThing);
    }

    public void addDeadThing(Thing parThing) {
        int tempSide = (int)parThing.thisSide;
        thingsDead_[tempSide].Add(parThing);
    }

    //sort warriors Lists, careful not to call it frequently due to overhead of sorting 4 times (★might call it every certain period)
    public void updateTotal() {
        thingsHpSorted_[0].Sort(comparerHpInstance);
        thingsHpSorted_[1].Sort(comparerHpInstance);
        thingsHpSorted_[2].Sort(comparerHpInstance);
        thingsDamageDealtSorted_[0].Sort(comparerDamageDealtInstance);
        thingsDamageDealtSorted_[1].Sort(comparerDamageDealtInstance);
        thingsDamageDealtSorted_[2].Sort(comparerDamageDealtInstance);
    }
    #endregion utility

    #region internalClasses
    private class comparerHp : IComparer<Thing> {
        public int Compare(Thing w1, Thing w2) {
            return (w1.curHp - w2.curHp);
        }
    }

    private class comparerDamageDealt : IComparer<Thing> {
        public int Compare(Thing w1, Thing w2) {
            return (w1.damageTotalDealt - w2.damageTotalDealt);
        }
    }
    #endregion internalClasses
}
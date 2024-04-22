using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class combatManager : MonoBehaviour
{
    public static combatManager CM;
    public graphComponent graphCur;

    // interval time between each action
    //★ intervalTime에 비례해 애니메이션 속도를 빠르게 할 수 있나 확인... 근데 그냥 게임 자체에 배속을 걸 수 있는지 찾는 게 빠를 것도 같다.
    private float intervalTime = 1f;

    //warriors array's indices represent different team, 0 = player's / 1 = computer's
    private List<warriorAbst>[] warriorsHpSorted_;
    private List<warriorAbst>[] warriorsDamageDealtSorted_;
    private List<warriorAbst>[] warriorsActionOrder_;

    // if tupMove.timer > 0f, it means move animation is being played
    // if tupMove.timer <= 0f, it's set -2f
    // if tupMove.timer < -1f, it means no move animation is being played
    private (GameObject mover, Vector3 movementPerFrame, float timer) tupMove_;

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
    // when setting tupMove you should input the destination's coordinates vector3 in movementPerFrame, property will calculate it on itself
    public (GameObject mover, Vector3 movementPerFrame, float timer) tupMove {
        get {
            return tupMove_;
        }
        set {
            value.mover.transform.rotation = Quaternion.LookRotation(value.movementPerFrame - value.mover.transform.position);
            tupMove_ = (value.mover, (value.movementPerFrame - value.mover.transform.position) * Time.deltaTime, 1.0f);
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

        comparerHpInstance = new comparerHp();
        comparerDamageDealtInstance = new comparerDamageDealt();
        tupMove_ = (null, new Vector3(0f, 0f, 0f), -2f);

        //graph initiate
        graphCur = new graphComponent(7, 7);

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

        //★ test
        GameObject w1 = Instantiate<GameObject>(
                Resources.Load<GameObject>("Prefabs/tester")
            );
        w1.GetComponent<warriorAbst>().init(true, 6, 6, 100);
        GameObject w2 = Instantiate<GameObject>(
                Resources.Load<GameObject>("Prefabs/tester")
            );
        w2.GetComponent<warriorAbst>().init(false, 0, 0, 100);
        Coroutine c = StartCoroutine(combatLoop());
    }

    public void Update() {
        //warriorAbst Move, only one warrior will move at a time
        if (tupMove_.timer > -1.0f) {
            if (tupMove_.timer > 0f) {
                tupMove_.mover.transform.position += tupMove_.movementPerFrame;
                tupMove_.timer -= Time.deltaTime;
            } else {
                tupMove_.timer = -2f;
                //★ 이동 애니메이션 정지
            }
        }
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
                foreach (caseAll ca in wa.copyCaseAllAll) {
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
                foreach (caseAll ca in wa.copyCaseAllAll) {
                    ca.onBeforeAction(wa);
                }

                // decide what action this warrior does this time, priority is (Being Controlled) > UseSkill > Attack > Move
                switch (wa.stateCur) {
                    // ★ 각각의 warrior 행동 시작 시 효과 발동
                    case enumStateWarrior.controlled:
                        break;
                    case enumStateWarrior.focussing:
                        break;
                    case enumStateWarrior.skill:
                        break;
                    case enumStateWarrior.move:
                        processMove(wa, wa.navigator.getNextEDirection());
                        break;
                    case enumStateWarrior.attack:
                        break;
                    default:
                        break;
                }
                // ★ 각각의 warrior 행동 종료 시 효과 발동
                yield return new WaitForSeconds(intervalTime);

                //check if this combat is over after each action
                if (overCheck()) {
                    //★게임 종료 처리
                    //return (warriorsHpSorted_[1].Count <= 0);
                }
            }
            //★ 턴 종료 시 효과 발동

            isPlrTurn = !isPlrTurn;
        }
    }

    #region processors
    public void processAttack() {

    }

    //processMove with EDirection parameter makes a warrior walk a node to the parameter-direction
    public void processMove(warriorAbst source, EDirection parEDir) {
        //sendThing method will check if it's valid movement
        source.curPosition.sendThing(parEDir);
        (int c0, int c1) tempPosition = source.curPosition.getPosition();
        //★ 이동 애니메이션 시작
        tupMove = (source.gameObject, new Vector3(tempPosition.c0, 0f, tempPosition.c1), 0f);
        
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
        if (parCoor0 < 0 || parCoor0 > graphCur.size0 ||
           parCoor1 < 0 || parCoor1 > graphCur.size1 ||
           graphCur[parCoor0, parCoor1].thingHere != null) {
            return;
        }

        graphCur[parCoor0, parCoor1].placeThing(source);
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
    public void addWarrior(warriorAbst warrior, int warriorTeam, bool isSortAfterAdd = false) {
        warriorsHpSorted_[warriorTeam].Add(warrior);
        warriorsDamageDealtSorted_[warriorTeam].Add(warrior);

        if (isSortAfterAdd) {
            updateWarriors();
        }
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
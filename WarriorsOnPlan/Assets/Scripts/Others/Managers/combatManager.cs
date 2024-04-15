using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class combatManager : MonoBehaviour
{
    public static combatManager CM;
    public graphComponent graphCur;

    //�� ������, �ٸ� �������� ���������Ӵ��� ���� CMState�� �ڸ����� 1�ڸ��θ�ŭ �ʿ伺�� ����
    //CMState represents what work shoud CM do now, it consists of bits and 1 of each bit means CM should do the corresponding work
    //each bit represents (move animation play)
    //private int CMState;

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
            return tupMove;
        }
        set {
            value.mover.transform.rotation = Quaternion.Euler(value.movementPerFrame - value.mover.transform.position);
            tupMove = (value.mover, (value.movementPerFrame - value.mover.transform.position) * Time.deltaTime, 1.0f);
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
        // �� ������ Ȥ�� Awake���� warriros List���� �ʱ�ȭ�ϰ� ������ ��
    }

    public void Update() {
        //warriorAbst Move, only one warrior will move at a time
        if (tupMove_.timer > -1.0f) {
            if (tupMove_.timer > 0f) {
                tupMove_.mover.transform.position += tupMove_.movementPerFrame;
                tupMove_.timer -= Time.deltaTime;
            } else {
                tupMove_.timer = -2f;
                //�� �̵� �ִϸ��̼� ����
            }
        }
    }
    #endregion callbacks

    public bool combatLoop(bool isInstant = false) {

        while (warriorsHpSorted_[0].Count <= 0 || warriorsHpSorted_[1].Count <= 0) {
            bool isPlrTurn = true;
            List<warriorAbst> tempListActors;
            //�� �÷��̾��� warrior�� ������ ������ �������, ������ ���� �������� �ʾҴٸ� ���� ü�� ������������
            //�� ���� warriorsActionsOrder_[1]�� warriorsHpSorted[1]�� ������Ű�� �� ����� ������ �ذ��� �� �ִ�.
            
            while((warriorsHpSorted_[0].Count > 0) && (warriorsHpSorted_[1].Count > 0)){
                tempListActors = warriorsActionOrder_[isPlrTurn ? 0 : 1];
                foreach (warriorAbst wa in tempListActors) {
                    //�� �� ���� �� ȿ�� �ߵ�
                    switch (wa.stateCur) {
                        case stateWarrior.controlled:
                            break;
                        case stateWarrior.focussing:
                            break;
                        case stateWarrior.skill:
                            break;
                        case stateWarrior.attack:
                            break;
                        case stateWarrior.move:
                            processMove(wa, wa.navigator.getNextEDirection());
                            break;
                        default:
                            break;
                    }
                    //�� �� ���� �� ȿ�� �ߵ�
                    /*
                    decide what action this warrior does this time, priority is (Being Controlled) > UseSkill > Attack > Move
                        if UseSkill or Attack, predict the whole process and save it as an instance in another queue(we name it 'actionReserved')
                    if Move, move instantly(unlike other actions move is instant preventing inversion of position, and the mover can be ambushed just after move)
                    position in code will be moved in this part, but the prefab keeps its position
                    �� if position is changed due to other warrior's skill, follow the position changed by skill
                    */
                }
            }

            isPlrTurn = !isPlrTurn;

            //after all warriors act, update it with animation (this method will sleep for a second)
            //  moving warriors move
            //      (timerMoveAnimation = 1f, make the warriors in dictMoverNDestination rotate directing the destination)
            //  other warriors will do their actions referencing 'actionReserved' queue

            //after all updating, warriors with curHp below zero die (warriors' death is delayed for animation play for players)

            //��...�׳� ���ŵʰ� �ִϸ��̼��� �����صΰ�, actionReserved ���� �ٷιٷ� �ൿ�ص� �� �� ������??
            //�� ������ �� ��Ĵ�� �ϸ� ĳ������ �ൿ ������ �ʹ� ���������̴�, ���� �ൿ ������ ���� ���� ���� �����ϰ� ���� �� ������?
        }
        return true;
    }

    #region processors
    public void processAttack() {

    }

    //processMove with EDirection parameter makes a warrior walk a node to the parameter-direction
    public void processMove(warriorAbst source, EDirection parEDir) {
        //sendThing method will check if it's valid movement
        source.curPosition.sendThing(parEDir);
        (int c0, int c1) tempPosition = source.curPosition.getPosition();
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

    //sort warriors Lists, careful not to call it frequently due to overhead of sorting 4 times (��might call it every certain period)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatManager : MonoBehaviour
{
    public static combatManager CM;

    public graphComponent graphCur;

    //warriors array's indices represent different team, 0 = player's / 1 = computer's
    private List<warriorAbst>[] warriorsHpSorted_;
    private List<warriorAbst>[] warriorsDamageDealtSorted_;

    #region properties
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
    #endregion properties

    private comparerHp comparerHpInstance;
    private comparerDamageDealt comparerDamageDealtInstance;

    public void Awake() {
        if (CM == null) {
            CM = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        comparerHpInstance = new comparerHp();
        comparerDamageDealtInstance = new comparerDamageDealt();
    }

    public bool combatLoop(bool isInstant = false) {
        while (warriorsHpSorted_[0].Count <= 0 || warriorsHpSorted_[1].Count <= 0) {
            //makw a full warriors Queue, sort it by curHp descending
            //warriors loop (a warrior with highest curHp would act first of course)
            //  decide what action this warrior does this time, priority is UseSkill > Attack > Move
            //      if UseSkill or Attack, predict the whole process and save it as an instance in another queue (we name it 'actionReferved')
            //      if Move, move instantly (unlike above move is done instantly for preventing inversion of position and being ambushed just after move)
            //      position in code will be moved in this part, but the prefab keeps its position
            //      ¡Ú if position is changed due to other warrior's skill, follow the position changed by skill

            //after all warriors act, update it with animation (this method will sleep for a second)
            //  moving warriors move
            //  other warriors will do their actions referencing 'actionReserved' queue



            //      Move find the destination through (warriorAbst).howToMove.navigate()
            //          navigate() can skip this if there's nothing on its previous route
            //          navigate() might call whatToUseSkill() or whatToAttack() due to find Skill or Attack target
        }
    }

    #region processors
    public void actionAttack() {

    }

    //actionMove with EDirection parameter makes a warrior walk a node to the parameter-direction
    public void actionMove(warriorAbst source, EDirection parEDir) { 
        
    }

    //actionMove with two int parameters makes a warrior teleport to the destination
    public void actionMove(warriorAbst source, int parCoor0, int parCoor1) {

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

    //sort warriors Lists, careful not to call it frequently due to overhead of sorting 4 times (¡Úmight call it every certain period)
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
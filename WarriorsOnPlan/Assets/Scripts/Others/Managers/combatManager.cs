using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatManager : MonoBehaviour
{
    public static combatManager CM;

    public graphComponent graphComponentCur;

    //warriors array's indices represent different team, 0 = player's / 1 = computer's
    private List<warriorAbst>[] warriorsHpSorted_;
    private List<warriorAbst>[] warriorsDamageSorted_;
    public List<warriorAbst>[] warriorsHpSorted {
        get;
    }

    private comparerHp comparerHpInstance;
    private comparerDamage comparerDamageInstance;

    public void Awake() {
        if (CM == null) {
            CM = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        comparerHpInstance = new comparerHp();
        comparerDamageInstance = new comparerDamage();
    }

    public void actionAttack() {
    }

    #region utility
    public void addWarrior(warriorAbst warrior, int warriorTeam, bool isSortAfterAdd = false) {
        warriorsHpSorted_[warriorTeam].Add(warrior);
        warriorsDamageSorted_[warriorTeam].Add(warrior);

        if (isSortAfterAdd) {
            updateWarriors();
        }
    }

    //sort warriors Lists, careful not to call it frequently due to overhead of sorting 4 times (¡Úmight call it every certain period)
    public void updateWarriors() {
        warriorsHpSorted_[0].Sort(comparerHpInstance);
        warriorsHpSorted_[1].Sort(comparerHpInstance);
        warriorsDamageSorted_[0].Sort(comparerDamageInstance);
        warriorsDamageSorted_[1].Sort(comparerDamageInstance);
    }
    #endregion utility

    #region internalClasses
    private class comparerHp : IComparer<warriorAbst> {
        public int Compare(warriorAbst w1, warriorAbst w2) {
            return (w1.curHp - w2.curHp);
        }
    }

    private class comparerDamage : IComparer<warriorAbst> {
        public int Compare(warriorAbst w1, warriorAbst w2) {
            return (w1.damageTotalGiven - w2.damageTotalGiven);
        }
    }
    #endregion internalClasses
}
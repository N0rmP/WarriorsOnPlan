using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{
    protected int maxHp_;
    protected int curHp_;
    protected node curPosition_;
    public int maxHp { get; }
    public int curHp { get; }
    public node curPosition { get; set; }

    public void init(int parMaxHp) {
        maxHp_ = parMaxHp;
    }

    public int setCurHp(int parValue, bool isPlus = false) {
        int tempResultChange = 0;
        if (isPlus) {
            if (curHp_ + parValue < 0) {
                tempResultChange = -curHp_;
                curHp_ = 0;
            } else if (curHp_ + parValue > maxHp_) {
                tempResultChange = maxHp_ - curHp_;
                curHp_ = maxHp_;
            } else {
                tempResultChange = parValue;
                curHp_ += parValue;
            }
        } else {
            tempResultChange = (parValue > curHp_) ? (parValue - curHp_) : (curHp_ - parValue);
            curHp_ = parValue;
        }
        return tempResultChange;
    }

    protected void setInitialMaxHp(int parValue) {
        maxHp_ = parValue;
        curHp_ = parValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : movableObject
{
    protected int maxHp_;
    protected int curHp_;
    protected node curPosition_;
    public int maxHp { get { return maxHp_; } }
    public int curHp { get { return curHp_; } }
    public node curPosition { get; set; }

    public void init(int parMaxHp) {
        maxHp_ = parMaxHp;
        curHp_ = maxHp_;
    }

    public int setCurHp(int parValue, warriorAbst source, bool isPlus = false) {
        int tempResultChange = 0;
        //★ 체력 증감 이전 효과 발동
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
        //★ 체력 증감 이후 효과 발동

        //if curHp_ is below zero, warrior dies
        if (curHp_ <= 0) {
            destroied(source);
        }

        return tempResultChange;
    }

    public virtual void destroied(warriorAbst source) { }
}

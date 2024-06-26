using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caseTimer : caseBase
{
    private bool isAutoReset;
    public int timerMax { get; protected set; }
    public int timerCur { get; protected set; }

    public caseTimer(int parTimerMax, enumCaseType parEnumCaseType, bool parIsTimerMax = true, bool parIsAutoReset = true) : base(parEnumCaseType) {
        timerMax = parTimerMax;
        timerCur = parIsTimerMax ? timerMax : 0;
        isAutoReset = parIsAutoReset;
     }

    protected virtual void updateTimer(Thing source) {
        if (timerCur > 0) {
            timerCur--;
        } else {
            doOnAlarmed(source);
            if (isAutoReset) {
                resetTimer();
            }
        }
        //�� effect ���� �Ͻ����� caseBase�� ��� ������ ��
    }

    public void resetTimer() {
        timerCur = timerMax;
    }

    protected virtual void doOnAlarmed(Thing source) { }
}

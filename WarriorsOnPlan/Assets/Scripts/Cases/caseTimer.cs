using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caseTimer : caseBase
{
    public int timerMax { get; protected set; }
    public int timerCur { get; protected set; }

    public caseTimer(int parTimerMax, enumCaseType parEnumCaseType, bool parIsTimerMax = true) : base(parEnumCaseType) {
        timerMax = parTimerMax;
        timerCur = parIsTimerMax ? timerMax : 0;
    }

    protected virtual void updateTimer() {
        if (timerCur > 0) {
            timerCur--;
        }
        //�� effect ���� �Ͻ����� caseBase�� ��� ������ ��
    }

    public void resetTimer() {
        timerCur = timerMax;
    }
}

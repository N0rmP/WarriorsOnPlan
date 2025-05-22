// mementoes... mentos... 히히
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;
using Processes;
using Circuits;
using System.Text;

// mementoCombat represents the time right after its field processPrev execution is done
public class mementoCombat {
    public int countAction { get; private set; }
    public enumSide turn { get; private set; }
    public mementoHouse house { get; private set; }
    public processAbst processLast { get; private set; }
    private processAbst processNext_;
    public processAbst processNext {
        get {
            return processNext_;
        }
        set {
            if (processNext_ != null) {
                return;
            }

            processNext_ = value;
        }
    }
    public caseBase[] toolsProvided { get; private set; }

    public mementoCombat(int parCountAction, enumSide parTurn, mementoHouse parHouse, processAbst parLast, caseBase[] parToolsProvided) {
        countAction = parCountAction;
        turn = parTurn;
        house = parHouse;
        processLast = parLast;
        processNext_ = null;
        toolsProvided = parToolsProvided;
    }

    #region test
    public void testMementoCombat() {
        StringBuilder tempSB = new StringBuilder("- - - - - mementoCombat test - - - - - \ncountAction : ");
        tempSB.Append(countAction);

        tempSB.Append("\nturn : ");
        tempSB.Append(turn);

        tempSB.Append("\nprocessLast : ");
        tempSB.Append(processLast);

        tempSB.Append("\nprocessNext : ");
        tempSB.Append(processNext);

        tempSB.Append("\ntools provided : ");
        foreach (caseBase c in toolsProvided) {
            tempSB.Append(c);
            tempSB.Append(" , ");
        }

        tempSB.Append("\nhouse test is called individually");

        Debug.Log(tempSB.ToString());

        combatManager.CM.HouC.testAll();
    }
    #endregion test
}

public class mementoHouse {
    private Queue<mementoThing> queTotalAlive;

    private Queue<mementoThing> quePlayerDead;
    private Queue<mementoThing> queEnemyDead;
    private Queue<mementoThing> queNeutralDead;

    public mementoHouse(
        Queue<mementoThing> parQueTotalAlive,
        Queue<mementoThing> parQuePlayerDead,
        Queue<mementoThing> parQueEnemyDead,
        Queue<mementoThing> parQueNeutralDead
        ) {
        queTotalAlive = parQueTotalAlive;
        quePlayerDead = parQuePlayerDead;
        queEnemyDead = parQueEnemyDead;
        queNeutralDead = parQueNeutralDead;
    }

    public IEnumerable<mementoThing> getTotalAlive() {
        return queTotalAlive;
    }

    public IEnumerable<mementoThing> getPlayerDead() {
        return quePlayerDead;
    }

    public IEnumerable<mementoThing> getEnemyDead() {
        return queEnemyDead;
    }

    public IEnumerable<mementoThing> getNeutralDead() {
        return queNeutralDead;
    }
}

public class mementoThing {
    public Thing me { get; private set; }
    public int maxHp { get; private set; }
    public int curHp { get; private set; }
    public readonly enumSide side;
    public readonly(int c0, int c1) coordinates;
    public mementoIParametable mSkill { get; private set; }
    public List<mementoIParametable> listCase;
    public mementoIParametable mCircuitHub { get; private set; }

    // most circuits don't require to be saved but few using timer need to be (maybe only two of them is contained here)
    // ★ 전투 시작 n번째 턴부터 M 동안 이동 circuit
    // ★ 전투 시작 N턴 후 스킬 사용 circuit

    public mementoThing(
        Thing parMe,
        int parMaxHp,
        int parCurHp,
        (int, int) parCoordinates,
        mementoIParametable parMSkill,
        List<mementoIParametable> parListCase,
        mementoIParametable parMCircuitHub
        ) {
        me = parMe;
        maxHp = parMaxHp;
        curHp = parCurHp;
        side = me.thisSide;
        coordinates = parCoordinates;
        mSkill = parMSkill;
        listCase = parListCase;
        mCircuitHub = parMCircuitHub;
    }

    public Thing getRestoredMe() {
        me.restore(this);
        return me;
    }

    public IEnumerable<mementoIParametable> getCases() {
        return listCase;
    }
}

public class mementoIParametable {
    public IParametable it;
    public Dictionary<string, int[]> dicParameter;
    public List<object> listReference;

    public mementoIParametable(IParametable parIt, Dictionary<string, int[]> parListParameter, List<object> parListReference) {
        it = parIt;
        dicParameter = parListParameter;
        listReference = parListReference;
    }

    public T getRestoredIt<T>() where T : IParametable {
        if (it is not T) {
            throw new System.Exception("mementoIParametable is asked to return different type (" + typeof(T) + ") to it ((" + it.GetType());
        }

        it.restore(this);
        return (T)it;
    }
}
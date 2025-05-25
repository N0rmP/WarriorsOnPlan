using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// DD means DamageDealt
public class houseComponent {
    #region variable
    private comparerHp thisComparerHp;
    private comparerDamageDealt thisComparerDD;
    private comparerActionOrder thisComparerAO;

    private bool isTotalDirty;
    private List<Thing> listTotalAlive;

    private List<Thing> listPlayerAlive;
    private List<Thing> listEnemyAlive;
    private List<Thing> listNeutralAlive;

    private List<Thing> listPlayerHpSorted;
    private List<Thing> listEnemyHpSorted;
    private List<Thing> listNeutralHpSorted;

    private List<Thing> listPlayerDDSorted;
    private List<Thing> listEnemyDDSorted;
    private List<Thing> listNeutralDDSorted;

    private Queue<Thing> queDeadRecently;
    private List<Thing> listPlayerDead;
    private List<Thing> listEnemyDead;
    private List<Thing> listNeutralDead;

    #region property
    public Thing[] arrTotalAlive {
        get {
            if (isTotalDirty) {
                sumThing();
            }
            return listTotalAlive.ToArray();
        }
    }

    public Thing[] arrPlayerAlive { get { return listPlayerAlive.ToArray(); } }
    public Thing[] arrEnemyAlive { get { return listEnemyAlive.ToArray(); } }
    public Thing[] arrNeutralAlive { get { return listNeutralAlive.ToArray(); } }

    public Thing[] arrPlayerHpSorted { get { return listPlayerHpSorted.ToArray(); } }
    public Thing[] arrEnemyHpSorted { get { return listEnemyHpSorted.ToArray(); } }
    public Thing[] arrNeutralHpSorted { get { return listNeutralHpSorted.ToArray(); } }

    public Thing[] arrPlayerDDSorted { get { return listPlayerDDSorted.ToArray(); } }
    public Thing[] arrEnemyDDSorted { get { return listEnemyDDSorted.ToArray(); } }
    public Thing[] arrNeutralDDSorted { get { return listNeutralDDSorted.ToArray(); } }

    public Thing[] arrPlayerDead { get { return listPlayerDead.ToArray(); } }
    public Thing[] arrEnemyDead { get { return listEnemyDead.ToArray(); } }
    public Thing[] arrNeutralDead { get { return listNeutralDead.ToArray(); } }
    #endregion property
    #endregion variable

    public houseComponent() {
        thisComparerHp = new comparerHp();
        thisComparerDD = new comparerDamageDealt();
        thisComparerAO = new comparerActionOrder();

        isTotalDirty = false;
        listTotalAlive = new List<Thing>();

        listPlayerAlive = new List<Thing>();
        listEnemyAlive = new List<Thing>();
        listNeutralAlive = new List<Thing>();

        listPlayerHpSorted = new List<Thing>();
        listEnemyHpSorted = new List<Thing>();
        listNeutralHpSorted = new List<Thing>();

        listPlayerDDSorted = new List<Thing>();
        listEnemyDDSorted = new List<Thing>();
        listNeutralDDSorted = new List<Thing>();

        queDeadRecently = new Queue<Thing>();
        listPlayerDead = new List<Thing>();
        listEnemyDead = new List<Thing>();
        listNeutralDead = new List<Thing>();
    }

    public mementoHouse makeMementoHouse() {
        Queue<mementoThing> tempQueTotalAlive = new Queue<mementoThing>();
        Queue<mementoThing> tempQuePlayerDead = new Queue<mementoThing>();
        Queue<mementoThing> tempQueEnemyDead = new Queue<mementoThing>();
        Queue<mementoThing> tempQueNeutralDead = new Queue<mementoThing>();

        sumThing();
        foreach (Thing t in listTotalAlive) {
            tempQueTotalAlive.Enqueue(t.freezeDry());
        }
        foreach (Thing t in listPlayerDead) {
            tempQuePlayerDead.Enqueue(t.freezeDry());
        }
        foreach (Thing t in listEnemyDead) {
            tempQueEnemyDead.Enqueue(t.freezeDry());
        }
        foreach (Thing t in listNeutralDead) {
            tempQueNeutralDead.Enqueue(t.freezeDry());
        }

        return new mementoHouse(tempQueTotalAlive, tempQuePlayerDead, tempQueEnemyDead, tempQueNeutralDead);
    }

    public void restore(mementoHouse parMemento) {
        // remove all Thing first
        listTotalAlive.Clear();
        listPlayerAlive.Clear();
        listEnemyAlive.Clear();
        listPlayerHpSorted.Clear();
        listEnemyHpSorted.Clear();
        listPlayerDDSorted.Clear();
        listEnemyDDSorted.Clear();
        listPlayerDead.Clear();
        listEnemyDead.Clear();

        // Thing's restore method include placing the Thing on the concurrent position, all nodes need to be vacant for it
        combatManager.CM.GC.vacateGraph();

        // add all Thing back with mementoThing-restore
        void func(IEnumerable<mementoThing> parEMT) {
            Thing tempThing;
            foreach (mementoThing mt in parEMT) {
                tempThing = mt.getRestoredMe();
                if (tempThing.curHp <= 0) {
                    addDeadThing(tempThing);
                } else {
                    addThing(tempThing);
                }
            }
        }

        func(parMemento.getTotalAlive());
        func(parMemento.getPlayerDead());
        func(parMemento.getEnemyDead());
        func(parMemento.getNeutralDead());

        sortAll();
    }

    #region listManagement
    // sum up all things in three listActionOrder and update listTotal with the result
    public void sumThing() {
        listTotalAlive.Clear();
        listTotalAlive.AddRange(listPlayerAlive);
        listTotalAlive.AddRange(listEnemyAlive);
        listTotalAlive.AddRange(listNeutralAlive);
    }

    public void sortByHp() {
        listPlayerHpSorted.Sort(thisComparerHp);
        listEnemyHpSorted.Sort(thisComparerHp);
        listNeutralHpSorted.Sort(thisComparerHp);
    }

    // DD is Damage Dealt
    public void sortByDD() {
        listPlayerDDSorted.Sort(thisComparerDD);
        listEnemyDDSorted.Sort(thisComparerDD);
        listNeutralDDSorted.Sort(thisComparerDD);
    }

    // AO is Action Order
    public void sortByAO(enumSide parSide) {
        (parSide switch {
            enumSide.player => listPlayerAlive,
            enumSide.enemy => listEnemyAlive,
            enumSide.neutral => listNeutralAlive,
            _ => listPlayerAlive
        }).Sort(thisComparerAO);
    }

    public void sortByAO() {
        listPlayerAlive.Sort(thisComparerAO);
        listEnemyAlive.Sort(thisComparerAO);
        listNeutralAlive.Sort(thisComparerAO);
    }

    public void sortAll() {
        sortByHp();
        sortByDD();
        sortByAO();
    }
    #endregion listManagement

    #region add&Remove
    public void addThing(Thing parThing) {
        if (parThing == null) { return; }
        if (parThing.curHp <= 0) {
            Debug.Log("you are trying to add Thing with Hp 0 or lower, please check it again : added Thing is " + parThing);
        }

        switch (parThing.thisSide) {
            case enumSide.player:
                listPlayerAlive.Add(parThing);
                listPlayerHpSorted.Add(parThing);
                listPlayerDDSorted.Add(parThing);
                break;
            case enumSide.enemy:
                listEnemyAlive.Add(parThing);
                listEnemyHpSorted.Add(parThing);
                listEnemyDDSorted.Add(parThing);
                break;
            case enumSide.neutral:
                listNeutralAlive.Add(parThing);
                listNeutralHpSorted.Add(parThing);
                listNeutralDDSorted.Add(parThing);
                break;
            default:
                return;
        }

        isTotalDirty = true;
    }

    public void addDeadThing(Thing parThing) {
        if (parThing == null) { return; }
        if (parThing.curHp > 0) {
            Debug.Log("you are trying to add alive Thing as dead, please check it again : added Thing is " + parThing);
        }

        switch (parThing.thisSide) {
            case enumSide.player:
                listPlayerDead.Add(parThing);
                break;
            case enumSide.enemy:
                listEnemyDead.Add(parThing);
                break;
            case enumSide.neutral:
                listNeutralDead.Add(parThing);
                break;
            default:
                return;
        }
    }

    public void killThing(Thing parThing) {
        if (parThing == null) { return; }

        switch (parThing.thisSide) {
            case enumSide.player:
                listPlayerAlive.Remove(parThing);
                listPlayerHpSorted.Remove(parThing);
                listPlayerDDSorted.Remove(parThing);
                listPlayerDead.Add(parThing);
                break;
            case enumSide.enemy:
                listEnemyAlive.Remove(parThing);
                listEnemyHpSorted.Remove(parThing);
                listEnemyDDSorted.Remove(parThing);
                listEnemyDead.Add(parThing);
                break;
            case enumSide.neutral:
                listNeutralAlive.Remove(parThing);
                listNeutralHpSorted.Remove(parThing);
                listNeutralDDSorted.Remove(parThing);
                listNeutralDead.Add(parThing);
                break;
            default:
                return;
        }

        isTotalDirty = true;
    }

    public void reviveThing(Thing parThing) {
        if (parThing == null) { return; }

        listTotalAlive.Add(parThing);

        switch (parThing.thisSide) {
            case enumSide.player:
                listPlayerDead.Remove(parThing);
                listPlayerHpSorted.Add(parThing);
                listPlayerDDSorted.Add(parThing);
                listPlayerAlive.Add(parThing);
                break;
            case enumSide.enemy:
                listEnemyDead.Remove(parThing);
                listEnemyHpSorted.Add(parThing);
                listEnemyDDSorted.Add(parThing);
                listEnemyAlive.Add(parThing);
                break;
            case enumSide.neutral:
                listNeutralDead.Remove(parThing);
                listNeutralHpSorted.Add(parThing);
                listNeutralDDSorted.Add(parThing);
                listNeutralAlive.Add(parThing);
                break;
            default:
                return;
        }

        isTotalDirty = true;
    }

    // removeThing remove a not-dead thing from the game dryly, it doesn't do any post-process and may be rarely called
    public void removeThing(Thing parThing) {
        if (parThing == null) { return; }

        switch (parThing.thisSide) {
            case enumSide.player:
                listPlayerAlive.Remove(parThing);
                listPlayerHpSorted.Remove(parThing);
                listPlayerDDSorted.Remove(parThing);
                listPlayerDead.Remove(parThing);
                break;
            case enumSide.enemy:
                listEnemyAlive.Remove(parThing);
                listEnemyHpSorted.Remove(parThing);
                listEnemyDDSorted.Remove(parThing);
                listEnemyDead.Remove(parThing);
                break;
            case enumSide.neutral:
                listNeutralAlive.Remove(parThing);
                listNeutralHpSorted.Remove(parThing);
                listNeutralDDSorted.Remove(parThing);
                listNeutralDead.Remove(parThing);
                break;
            default:
                return;
        }

        isTotalDirty = true;
    }
    #endregion add&Remove

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

    private class comparerActionOrder : IComparer<Thing> {
        public int Compare(Thing w1, Thing w2) {
            return (w1.actionOrder - w2.actionOrder);
        }
    }
    #endregion internalClasses

    #region test
    public void testAll() {
        StringBuilder tempSB = new StringBuilder("-----houseComponent testAll-----\nCAUTION : this method doesn't call sorting manually");

        tempSB.Append("\n\narrTotalAlive : ");
        foreach (Thing t in arrTotalAlive) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }

        tempSB.Append("\n\narrPlayerAlive : ");
        foreach (Thing t in arrPlayerAlive) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrEnemyAlive : ");
        foreach (Thing t in arrEnemyAlive) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrNeutralAlive : ");
        foreach (Thing t in arrNeutralAlive) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }

        tempSB.Append("\n\narrPlayerHpSorted : ");
        foreach (Thing t in arrPlayerHpSorted) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrEnemyHpSorted : ");
        foreach (Thing t in arrEnemyHpSorted) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrNeutralHpSorted : ");
        foreach (Thing t in arrNeutralHpSorted) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }

        tempSB.Append("\n\narrPlayerDDSorted : ");
        foreach (Thing t in arrPlayerDDSorted) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrEnemyDDSorted : ");
        foreach (Thing t in arrEnemyDDSorted) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrNeutralDDSorted : ");
        foreach (Thing t in arrNeutralDDSorted) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }

        tempSB.Append("\n\narrPlayerDead : ");
        foreach (Thing t in arrPlayerDead) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrEnemyDead : ");
        foreach (Thing t in arrEnemyDead) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrNeutralDead : ");
        foreach (Thing t in arrNeutralDead) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }

        Debug.Log(tempSB.ToString());
    }

    public void testPlayerAlive() {
        foreach (Thing t in arrPlayerAlive) {
            t.testStatus();
        }
    }

    public void testEnemyAlive() {
        foreach (Thing t in arrEnemyAlive) {
            t.testStatus();
        }
    }

    public void testNeutralAlive() {
        foreach (Thing t in arrNeutralAlive) {
            t.testStatus();
        }
    }
    #endregion test
}

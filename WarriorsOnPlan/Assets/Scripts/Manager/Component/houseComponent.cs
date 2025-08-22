using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

#region comparers    
public class comparerHp : IComparer<Thing> {
    public int Compare(Thing w1, Thing w2) {
        return (w1.curHp - w2.curHp);
    }
}

public class comparerDamageDealt : IComparer<Thing> {
    public int Compare(Thing w1, Thing w2) {
        return (w1.damageDealt - w2.damageDealt);
    }
}
#endregion comparers

public class houseComponent {
    #region variable
    public readonly static comparerHp instComparerHp = new comparerHp();
    public readonly static comparerDamageDealt instComparerDD = new comparerDamageDealt();
    private readonly static comparerActionOrder instComparerAO = new comparerActionOrder();
    
    // transcendent's caseBase is regared as caseBase added to all adequate Thing, please check transcendent.cs for more information
    public transcendent transcendentTotal { get; private set; }
    private transcendent transcendentPlayer;
    private transcendent transcendentEnemy;
    private transcendent transcendentNeutral;

    private bool isTotalDirty;
    private List<Thing> listTotalAlive;

    private List<Thing> listPlayerAlive;
    private List<Thing> listEnemyAlive;
    private List<Thing> listNeutralAlive;

    private Queue<Thing> queDeadRecently;
    private List<Thing> listPlayerDead;
    private List<Thing> listEnemyDead;
    private List<Thing> listNeutralDead;

    private List<pairThingActionOrder> listPlayerActionOrder;
    private List<pairThingActionOrder> listEnemyActionOrder;
    private List<pairThingActionOrder> listNeutralActionOrder;

    #region property
    public Thing[] arrTotalAlive {
        get {
            if (isTotalDirty) {
                sumThing();
            }
            return listTotalAlive.ToArray();
        }
    }
    #endregion property
    #endregion variable

    public houseComponent() {
        transcendentTotal = new transcendent();
        transcendentPlayer = new transcendent();
        transcendentEnemy = new transcendent();
        transcendentNeutral = new transcendent();

        isTotalDirty = false;
        listTotalAlive = new List<Thing>();

        listPlayerAlive = new List<Thing>();
        listEnemyAlive = new List<Thing>();
        listNeutralAlive = new List<Thing>();

        queDeadRecently = new Queue<Thing>();
        listPlayerDead = new List<Thing>();
        listEnemyDead = new List<Thing>();
        listNeutralDead = new List<Thing>();

        listPlayerActionOrder = new List<pairThingActionOrder>();
        listEnemyActionOrder = new List<pairThingActionOrder>();
        listNeutralActionOrder = new List<pairThingActionOrder>();
    }

    #region ActionOrderRelated
    public int getPersonalActionOrder(Thing parThing) {
        foreach (pairThingActionOrder PRAO in getArrPairThingActionOrder(parThing.thisSide)) {
            if (PRAO.thisThing == parThing) {
                return PRAO.thisActionOrder;
            }
        }
        return -1;
    }

    public void setActionOrderNumber(enumSide parSide) {
        foreach (Thing th in getArrActionOrder(parSide)) {
            th.updatePanelActionOrder();
        }
    }

    public void changeActionOrder(Thing parThing, int parActionOrder, bool parIsShow = true) {
        List<pairThingActionOrder> tempListPTAO = parThing.thisSide switch {
            enumSide.player => listPlayerActionOrder,
            enumSide.enemy => listEnemyActionOrder,
            enumSide.neutral => listNeutralActionOrder,
            _ => null
        };
        int tempIndexToBeChanged = int.MinValue;
        for (int i = 0; i < tempListPTAO.Count; i++) {
            if (parThing == tempListPTAO[i].thisThing) {
                tempIndexToBeChanged = i;
                break;
            }
        }

        // if corresponding thing doesn't exist, return
        // if parThing already had the same ActionOrder, return        
        if (tempIndexToBeChanged == int.MinValue || tempListPTAO[tempIndexToBeChanged].thisActionOrder == parActionOrder) {
            return;
        }

        parActionOrder = Math.Max(0, parActionOrder);
        tempListPTAO[tempIndexToBeChanged] = new pairThingActionOrder(parThing, parActionOrder);
        tempListPTAO.Sort(instComparerAO);

        if (parIsShow) {
            setActionOrderNumber(parThing.thisSide);
        }
    }
    #endregion ActionOrderRelated

    #region memento
    public mementoHouse makeMementoHouse() {
        // ★ 내 생각엔 죽은 Thing들도 일괄 저장했다가 꺼내면서 thisSide따라 나누는 게 좋아보임, ActionOrder 정리정돈 끝나면 이거 좀 만지자
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
        // Thing's restore method include placing the Thing on the concurrent position, all nodes need to be vacant for it
        combatManager.CM.GC.vacateGraph();

        // add all Thing back with mementoThing-restore
        Thing tempThing;
        void addAllBack(IEnumerable<mementoThing> parEMT) {
            foreach (mementoThing mt in parEMT) {
                tempThing = mt.getRestoredMe();
                if (tempThing.curHp <= 0) {
                    addDeadThing(tempThing);
                } else {
                    addAliveThing(tempThing, mt.actionOrder);
                }
            }
        }

        clearTotal(false);
        addAllBack(parMemento.getTotalAlive());
        addAllBack(parMemento.getPlayerDead());
        addAllBack(parMemento.getEnemyDead());
        addAllBack(parMemento.getNeutralDead());

        sortByAO();
    }
    #endregion memento

    #region get
    public Thing[] getArrAlive(enumSide parSide) {
        return (parSide switch {
            enumSide.player => listPlayerAlive,
            enumSide.enemy => listEnemyAlive,
            enumSide.neutral => listNeutralAlive,
            _ => new List<Thing>()
        }).ToArray();
    }

    public Thing[] getArrDead(enumSide parSide) {
        return (parSide switch {
            enumSide.player => listPlayerDead,
            enumSide.enemy => listEnemyDead,
            enumSide.neutral => listNeutralDead,
            _ => new List<Thing>()
        }).ToArray();
    }

    public Thing[] getArrTotal(enumSide parSide) {
        return (parSide switch {
            enumSide.player => listPlayerAlive.getListSum<Thing>(listPlayerDead),
            enumSide.enemy => listEnemyAlive.getListSum<Thing>(listEnemyDead),
            enumSide.neutral => listNeutralAlive.getListSum<Thing>(listNeutralDead),
            _ => new List<Thing>()
        }).ToArray();
    }

    public Thing[] getArrTotalTotal() {
        List<Thing> tempResult = new List<Thing>();
        tempResult.AddRange(getArrTotal(enumSide.player));
        tempResult.AddRange(getArrTotal(enumSide.enemy));
        tempResult.AddRange(getArrTotal(enumSide.neutral));
        return tempResult.ToArray();
    }

    private pairThingActionOrder[] getArrPairThingActionOrder(enumSide parSide) {
        return (parSide switch {
            enumSide.player => listPlayerActionOrder,
            enumSide.enemy => listEnemyActionOrder,
            enumSide.neutral => listNeutralActionOrder,
            _ => new List<pairThingActionOrder>()
        }).ToArray();
    }

    public Thing[] getArrActionOrder(enumSide parSide) {
        pairThingActionOrder[] tempList = getArrPairThingActionOrder(parSide);
        Thing[] tempResult = new Thing[tempList.Length];
        for (int i = 0; i < tempList.Length; i++) {
            tempResult[i] = tempList[i].thisThing;
        }
        return tempResult;
    }

    public transcendent getTranscendent(enumSide parSide) {
        return parSide switch {
            enumSide.player => combatManager.CM.HouC.transcendentPlayer,
            enumSide.enemy => combatManager.CM.HouC.transcendentEnemy,
            enumSide.neutral => combatManager.CM.HouC.transcendentNeutral,
            _ => new transcendent()
        };
    }
    #endregion get

    #region sum_sort
    // sum up all things in three listActionOrder and update listTotal with the result
    public void sumThing() {
        listTotalAlive.Clear();
        listTotalAlive.AddRange(listPlayerAlive);
        listTotalAlive.AddRange(listEnemyAlive);
        listTotalAlive.AddRange(listNeutralAlive);
    }

    // AO is Action Order, sortByAO is not inteded to be called frequently, each list should be sorted individually when becoming dirty
    public void sortByAO() {
        listPlayerActionOrder.Sort(instComparerAO);
        listEnemyActionOrder.Sort(instComparerAO);
        listNeutralActionOrder.Sort(instComparerAO);
    }
    #endregion sum_sort

    #region add_Remove_clear
    // parActionOrderWanted is not index, it starts from 1 not 0
    public void addAliveThing(Thing parThing, int parActionOrderWanted = -1) {
        if (parThing == null) { return; }
        if (parThing.curHp <= 0) {
            Debug.Log("you are trying to add Thing with Hp 0 or lower, please check it again : added Thing is " + parThing);
        }

        switch (parThing.thisSide) {
            case enumSide.player:
                listPlayerAlive.Add(parThing);
                break;
            case enumSide.enemy:
                listEnemyAlive.Add(parThing);
                break;
            case enumSide.neutral:
                listNeutralAlive.Add(parThing);
                break;
            default:
                return;
        }

        addActionOrder(parThing, parActionOrderWanted);

        isTotalDirty = true;
    }

    // addDeadThing add a new thing to listDead, you should use killThing to remove a thing from listAlive and add it to listDead instead
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
                listPlayerDead.Add(parThing);
                break;
            case enumSide.enemy:
                listEnemyAlive.Remove(parThing);
                listEnemyDead.Add(parThing);
                break;
            case enumSide.neutral:
                listNeutralAlive.Remove(parThing);
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
                listPlayerAlive.Add(parThing);
                break;
            case enumSide.enemy:
                listEnemyDead.Remove(parThing);
                listEnemyAlive.Add(parThing);
                break;
            case enumSide.neutral:
                listNeutralDead.Remove(parThing);
                listNeutralAlive.Add(parThing);
                break;
            default:
                return;
        }
        
        isTotalDirty = true;
    }

    // removeThing remove a thing from the game dryly, it doesn't do any post-process and may be rarely called
    public void removeThing(Thing parThing) {
        if (parThing == null) { return; }

        switch (parThing.thisSide) {
            case enumSide.player:
                listPlayerAlive.Remove(parThing);
                listPlayerDead.Remove(parThing);
                foreach (pairThingActionOrder PTAO in listPlayerActionOrder.ToArray()) {
                    if (PTAO.thisThing == parThing) {
                        listPlayerActionOrder.Remove(PTAO);
                    }
                }
                break;
            case enumSide.enemy:
                listEnemyAlive.Remove(parThing);
                listEnemyDead.Remove(parThing);
                foreach (pairThingActionOrder PTAO in listEnemyActionOrder.ToArray()) {
                    if (PTAO.thisThing == parThing) {
                        listEnemyActionOrder.Remove(PTAO);
                    }
                }
                break;
            case enumSide.neutral:
                listNeutralAlive.Remove(parThing);
                listNeutralDead.Remove(parThing);
                foreach (pairThingActionOrder PTAO in listNeutralActionOrder.ToArray()) {
                    if (PTAO.thisThing == parThing) {
                        listNeutralActionOrder.Remove(PTAO);
                    }
                }
                break;
            default:
                return;
        }

        isTotalDirty = true;
    }

    // caution : clearTotal includes destroying thing-gameobject
    public void clearTotal(bool parIsDestroy = false) {
        foreach (Thing t in getArrTotalTotal()) {
            removeThing(t);
            if (parIsDestroy) {
                GameObject.Destroy(t.gameObject);
            }
        }

        // remove all Thing manually, just in case...
        listTotalAlive.Clear();
        listPlayerAlive.Clear();
        listEnemyAlive.Clear();
        listNeutralAlive.Clear();
        listPlayerDead.Clear();
        listEnemyDead.Clear();
        listNeutralDead.Clear();
        listPlayerActionOrder.Clear();
        listEnemyActionOrder.Clear();
        listNeutralActionOrder.Clear();
        isTotalDirty = true;    // jjust in case...
    }

    // listActionOrder 
    #region ActionOrder
    private void addActionOrder(Thing parThing, int parActionOrderWanted = -1, bool parIsShow = true) {
        List<pairThingActionOrder> tempList = parThing.thisSide switch {
            enumSide.player => listPlayerActionOrder,
            enumSide.enemy => listEnemyActionOrder,
            enumSide.neutral => listNeutralActionOrder,
            _ => null
        };

        // ActionOrder can't be below zero, and it's basically above zero
        if (parActionOrderWanted < 0) {
            parActionOrderWanted = tempList.Count + 1;
        } else {
            parActionOrderWanted = Math.Max(parActionOrderWanted, 1);
        }

        // insert pairThingActionOrder into the index of closest ActionOrder number
        pairThingActionOrder tempPTAOAdded = new pairThingActionOrder(parThing, parActionOrderWanted);
        int tempIndexToBeAdded = Math.Abs(tempList.BinarySearch(tempPTAOAdded, instComparerAO));
        if (tempList.Count > 0 && tempIndexToBeAdded < tempList.Count) {
            tempList.Insert(tempIndexToBeAdded, tempPTAOAdded);
        } else {
            tempList.Add(tempPTAOAdded);
        }

        // if two pairThingActionOrders have same ActionOrder, incrementing ActionOrders of the post ones, maybe the one added first
        pairThingActionOrder tempPTAOIncrement;
        for (int i = tempIndexToBeAdded; i < tempList.Count - 1; i++) {
            if (tempList[i].thisActionOrder == tempList[i + 1].thisActionOrder) {
                tempPTAOIncrement = tempList[i];
                tempPTAOIncrement.thisActionOrder++;
            }
        }

        if (parIsShow) {
            setActionOrderNumber(parThing.thisSide);
        }
    }

    // neutral & enemy warriors' ActionOrder is set by developer already, only player's warriors can change his ActionOrder
    // only exception is when new neutral & enemy warriors is spawned and his ActionOrder precedes other friendly warriors'
    public void rearrangePlayerActionOrder(IEnumerable<Thing> parThingCollection) {
        listPlayerActionOrder.Clear();
        foreach (Thing t in parThingCollection) {
            addActionOrder(t, parIsShow: false);
            t.updatePanelActionOrder();
        }
        setActionOrderNumber(enumSide.player);
    }
    #endregion ActionOrder

    #endregion add_Remove

    #region internal
    private record pairThingActionOrder {
        public Thing thisThing;
        public int thisActionOrder;

        public pairThingActionOrder(Thing parThing, int parActionOrder) {
            thisThing = parThing;
            thisActionOrder = parActionOrder;
        }
    }

    private class comparerActionOrder : IComparer<pairThingActionOrder> {
        public int Compare(pairThingActionOrder r1, pairThingActionOrder r2) {
            return (r1.thisActionOrder - r2.thisActionOrder);
        }
    }
    #endregion internal

    #region test
    public void testAll() {
        StringBuilder tempSB = new StringBuilder("-----houseComponent testAll-----\nCAUTION : this method doesn't call sorting manually");

        tempSB.Append("\n\narrTotalAlive : ");
        foreach (Thing t in arrTotalAlive) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }

        tempSB.Append("\n\narrPlayerAlive : ");
        foreach (Thing t in getArrAlive(enumSide.player)) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrEnemyAlive : ");
        foreach (Thing t in getArrAlive(enumSide.enemy)) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrNeutralAlive : ");
        foreach (Thing t in getArrAlive(enumSide.neutral)) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }

        tempSB.Append("\n\narrPlayerDead : ");
        foreach (Thing t in getArrDead(enumSide.player)) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrEnemyDead : ");
        foreach (Thing t in getArrDead(enumSide.enemy)) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }
        tempSB.Append("\n\narrNeutralDead : ");
        foreach (Thing t in getArrDead(enumSide.neutral)) {
            tempSB.Append(t.ToString());
            tempSB.Append(" , ");
        }

        tempSB.Append("\n\narrPlayerActionOrder : ");
        foreach (pairThingActionOrder p in getArrPairThingActionOrder(enumSide.player)) {
            tempSB.Append(p.thisActionOrder);
            tempSB.Append(" - ");
            tempSB.Append(p.thisThing.ToString());
            tempSB.Append(", ");
        }
        tempSB.Append("\n\narrEnemyActionOrder : ");
        foreach (pairThingActionOrder p in getArrPairThingActionOrder(enumSide.enemy)) {
            tempSB.Append(p.thisActionOrder);
            tempSB.Append(" - ");
            tempSB.Append(p.thisThing.ToString());
            tempSB.Append(", ");
        }
        tempSB.Append("\n\narrNeutralActionOrder : ");
        foreach (pairThingActionOrder p in getArrPairThingActionOrder(enumSide.neutral)) {
            tempSB.Append(p.thisActionOrder);
            tempSB.Append(" - ");
            tempSB.Append(p.thisThing.ToString());
            tempSB.Append(", ");
        }

        Debug.Log(tempSB.ToString());
    }

    public void testPlayerAlive() {
        Debug.Log("- - - houseComponent.testPlayerAlive - - -");
        foreach (Thing t in getArrAlive(enumSide.player)) {
            t.testStatus();
        }
    }

    public void testEnemyAlive() {
        Debug.Log("- - - houseComponent.testEnemyAlive - - -");
        foreach (Thing t in getArrAlive(enumSide.enemy)) {
            t.testStatus();
        }
    }

    public void testNeutralAlive() {
        Debug.Log("- - - houseComponent.testNeutralAlive - - -");
        foreach (Thing t in getArrAlive(enumSide.neutral)) {
            t.testStatus();
        }
    }
    #endregion test
}

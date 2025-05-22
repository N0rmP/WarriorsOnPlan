using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class historyComponent : IEnumerable<mementoCombat> {
    private List<mementoCombat> listMementoCombat;

    // mementoInitial represents when player first opened the level, not the state when combat started
    public mementoCombat mementoInitial { get; private set; }
    public mementoCombat mementoCombatStart { get; private set; }

    public historyComponent() {
        listMementoCombat = new List<mementoCombat>();
    }

    public void resetHistory() {
        listMementoCombat.Clear();
        mementoCombatStart = null;
    }

    public void setMementoInitial(mementoCombat parMementoInitial) {
        mementoInitial = parMementoInitial;
    }

    public void setMementoCombatStart(mementoCombat parMementoCombatStart) {
        mementoCombatStart = parMementoCombatStart;
    }

    public void addMemento(mementoCombat parMemento) {
        listMementoCombat.Add(parMemento);

        if (parMemento == null) {
            Debug.Log("historyComponent.addMemento malfunction : parMemento is null");
        }
        if (listMementoCombat.Count == 0) {
            Debug.Log("historyComponent.addMemento malfunction : listMementoCombat's count is still zero");
        }
    }

    

    #region inder
    public mementoCombat this[int i] {
        get {
            Math.Clamp(i, 0, listMementoCombat.Count - 1);
            return listMementoCombat[i];
        }
    }

    public IEnumerator<mementoCombat> GetEnumerator() {
        foreach (mementoCombat mc in listMementoCombat) {
            yield return mc;
        }
    }


    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
    #endregion indexer
}

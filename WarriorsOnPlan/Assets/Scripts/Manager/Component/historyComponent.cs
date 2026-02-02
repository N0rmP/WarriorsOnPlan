using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class historyComponent : IEnumerable<mementoCombat> {
    private List<mementoCombat> listMementoCombat;

    // mementoInitial represents when player first opened the level, not the state when combat started
    private mementoCombat mementoInitial_;
    private mementoCombat mementoPrepareDone_;
    public mementoCombat mementoInitial {
        get {
            return mementoInitial_;
        }
        set {
            if (mementoInitial_ != null) {
                return;
            }
            mementoInitial_ = value;
        }
    }
    public mementoCombat mementoPrepareDone {
        get {
            return mementoPrepareDone_;
        }
        set {
            if (mementoPrepareDone_ != null) {
                return;
            }
            mementoPrepareDone_ = value;
        }
    }

    public historyComponent() {
        listMementoCombat = new List<mementoCombat>();
    }

    // reset history for restarting combat
    public void resetHistory() {
        listMementoCombat.Clear();

        mementoPrepareDone_ = null;
    }

    // reset history for reloading level
    public void resetHistoryTotal() {
        resetHistory();
        mementoInitial_ = null;
    }

    public void addMemento(mementoCombat parMemento) {
        if (parMemento == null) {
            Debug.Log("historyComponent.addMemento malfunction : parMemento is null");
        }

        listMementoCombat.Add(parMemento);
        
        if (listMementoCombat.Count == 0) {
            Debug.Log("historyComponent.addMemento malfunction : listMementoCombat's count is still zero");
        }
    }    

    #region Seqeunce
    public mementoCombat this[int i] {
        get {
            Math.Clamp(i, 0, listMementoCombat.Count - 1);
            return listMementoCombat[i];
        }
    }

    public IEnumerator<mementoCombat> GetEnumerator() {
        return listMementoCombat.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
    #endregion Sequence
}

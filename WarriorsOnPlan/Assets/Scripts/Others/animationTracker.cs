using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  animationTracker tracks current animation-state's normalized time
//  programmer can enqueue time(time) - delegate pair into PriorityQueue, and execute it when the time comes
public class animationTracker : MonoBehaviour {
    #region field
    #region constant    
    public const float timeTestTest = 0.7f;
    public const float timeAttackBrandish = 0.14f;
    public const float timeAttackPunch = 0.4f;

    public static readonly cashStateHash cshAttackBow = new cashStateHash("Base Layer.Attack.Attack Bow");
    public static readonly cashStateHash cshAttackBrandish = new cashStateHash("Base Layer.Attack.Attack Bandish");
    public static readonly cashStateHash cshAttackCast = new cashStateHash("Base Layer.Attack.Attack Cast");
    public static readonly cashStateHash cshAttackCrossbow = new cashStateHash("Base Layer.Attack.Attack Crossbow");
    public static readonly cashStateHash cshAttackStab = new cashStateHash("Base Layer.Attack.Attack Stab");
    public static readonly cashStateHash cshAttackPunch = new cashStateHash("Base Layer.Attack.Attack Punch");
    public static readonly cashStateHash cshAttackIdle = new cashStateHash("Base Layer.Attack.Attack Idle");
    public static readonly cashStateHash cshControlled = new cashStateHash("Base Layer.Controlled");
    public static readonly cashStateHash cshDamaged = new cashStateHash("Base Layer.Damaged");
    public static readonly cashStateHash cshDead = new cashStateHash("Base Layer.Dead");
    public static readonly cashStateHash cshFocussing = new cashStateHash("Base Layer.UseSkill");
    public static readonly cashStateHash cshMove = new cashStateHash("Base Layer.Move");
    public static readonly cashStateHash cshIdle = new cashStateHash("Base Layer.Idle");
    #endregion constant

    private static Dictionary<enumAttackAnimation, (cashStateHash, float)> dictEaaTrackerInformation_;
    public static IReadOnlyDictionary<enumAttackAnimation, (cashStateHash csh, float time)> dictEaaTrackerInformation => dictEaaTrackerInformation_;

    private Animator thisAnimator;

    //  key is hash value of the state name
    //  value is PriorityQueue of tuple (time when Action executed, Action is Action)
    private Dictionary<int, PriorityQueue<(float time, Action del)>> dictStatePQ;
    private (float time, Action del) next;
    private int curStateHash;
    #endregion field

    #region callbacks
    void Awake() {
        if (!TryGetComponent<Animator>(out thisAnimator)) {
            Debug.Log(gameObject + ".animationTracker.Awake error : no Animator Component found");
            thisAnimator = gameObject.AddComponent<Animator>();
            return;
        }

        dictEaaTrackerInformation_ = new Dictionary<enumAttackAnimation, (cashStateHash, float)>() {
            { enumAttackAnimation.trigAttackBow, (cshAttackIdle, 0.5f) },
            { enumAttackAnimation.trigAttackBrandish, (cshAttackBrandish, timeAttackBrandish) },            
            { enumAttackAnimation.trigAttackCast, (cshAttackIdle, 0.5f)},
            { enumAttackAnimation.trigAttackCrossbow, (cshAttackIdle, 0.5f)},
            { enumAttackAnimation.trigAttackPunch, (cshAttackPunch, timeAttackPunch)},
            { enumAttackAnimation.trigAttackStab, (cshAttackIdle, 0.5f)}
        };

        dictStatePQ = new Dictionary<int, PriorityQueue<(float, Action)>>();
        next = (2f, () => { });
        curStateHash = Animator.StringToHash("none");
        Clear();
    }

    void Update() {
        // reset this on state-transition
        if (curStateHash != thisAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash) {
            OnTransition();
            return;
        }

        // ★ 만약 동일한 time에 대해 2개 이상의 tuple이 Enqueue된 경우, sfx 혹은 vfx 등이 겹칠 수 있다. 큰 문제는 아닐 거 같은데, 추후 문제되면 변경 필요
        if (thisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f >= next.time) {
            if (next.del != null) {
                next.del();
            }
            next = (dictStatePQ.ContainsKey(curStateHash) && dictStatePQ[curStateHash].Count > 0) ? 
                dictStatePQ[curStateHash].Dequeue() : 
                (2f, () => { });
        }
    }
    #endregion callbacks

    #region methods
    //  Enqueue can reserve delegate to be executed at the time of certain state & normalizedTime
    //  before Enqueue, cur can changes
    //  1. if cur is not set (cur.time > 1f), cur = parTup and parTup ain't be Enqueued
    //  2. if parTup.time < cur.time, cur = parTup and cur is Enqueued instead of parTup
    //  3. else, just Enqueue parTup
    public void Enqueue(cashStateHash parStateHash, (float time, Action del) parTup) {
        if (parTup.time < 0f && parTup.time > 1f) {
            return;
        }

        if (!dictStatePQ.ContainsKey(parStateHash.hashState)) {
            dictStatePQ.Add(parStateHash.hashState, new PriorityQueue<(float time, Action del)>((x, y) => (x.time).CompareTo(y.time)));
        }

        dictStatePQ[parStateHash.hashState].Enqueue(parTup);
    }

    public void Clear() {
        foreach (int i in dictStatePQ.Keys) {
            dictStatePQ[i].Clear();
        }
        curStateHash = Animator.StringToHash("none");
        next = (2f, () => { });
    }

    private void OnTransition() {
        if (dictStatePQ.ContainsKey(curStateHash)) {
            dictStatePQ[curStateHash].Clear();
        }
        curStateHash = thisAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        next = dictStatePQ.ContainsKey(curStateHash) ? dictStatePQ[curStateHash].Dequeue() : (2f, () => { });
    }
    #endregion methods

    #region test
    // testHalfHashToString convert certain hash-value included in the constant field of animationTracker class back to the original string
    public static string testHalfHashToString(int parHash) {
        if (parHash == cshIdle.hashState) { return cshIdle.stringState; }
        if (parHash == cshAttackIdle.hashState) { return cshAttackIdle.stringState; }
        if (parHash == cshAttackBrandish.hashState) { return cshAttackBrandish.stringState; }
        if (parHash == cshAttackPunch.hashState) { return cshAttackPunch.stringState; }
        return "Nothing Corresponding";
}
    #endregion test
}

//  cashStateHash stores hashed value of each animation-state-name, it optimizes and utilizes animationTracker
public struct cashStateHash {
    private readonly string stringState_;
    private readonly int hashState_;
    public int hashState => hashState_;
    public string stringState => stringState_;
    public cashStateHash(string parString) {
        stringState_ = parString;
        hashState_ = Animator.StringToHash(parString);
    }
}
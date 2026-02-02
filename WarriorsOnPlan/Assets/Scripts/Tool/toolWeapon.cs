using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumAttackAnimation { 
    trigAttackBrandish,
    trigAttackStab,
    trigAttackBow,
    trigAttackCrossbow,
    trigAttackCast,
    trigAttackPunch,
    max
}

public enum enumAttackAudio { 
    audioSword,
    audioHammer,
    audioBow,
    audioMagic
}

namespace Cases {
    public abstract class toolWeapon : caseTimerSelfishTurn, ICaseSystemicAdded, ICaseSystemicRemoved {
        // delay between each showEffect when same attack-animation folded
        protected const float intervalFolded = 0.05f;

        //range of toolWeapon consists of two int nums. each index represents minimum range and maximum range
        //min range can't be below 1, max range can't be below min range
        private int rangeMin_ = 1;
        private int rangeMax_ = 1;
        public int rangeMin {
            get {
                return rangeMin_;
            }
            protected set {
                rangeMin_ = Math.Max(1, value);
            }
        }
        public int rangeMax {
            get {
                return rangeMax_;
            }
            protected set {
                rangeMax_ = Math.Max(rangeMin, value);
            }
        }
        public int damageOriginal { get; protected set; }
        public virtual bool isReady {
            get {
                return timerCur <= 0;
            }
        }
        public enumDamageType damageType { get; protected set; } = enumDamageType.basic;
        public enumAttackAnimation attackAnimation { get; protected set; }

        protected string thisObjPath = null;
        private GameObject thisObj_ = null;
        private GameObject thisObj {
            get {
                if (thisObjPath == null) {
                    return null;
                }
                if (thisObj_ == null) {
                    GameObject tempToBeInstantiated = Resources.Load<GameObject>(thisObjPath);
                    if (tempToBeInstantiated == null) {
                        thisObjPath = null;
                        return null;
                    }
                    thisObj_ = GameObject.Instantiate(tempToBeInstantiated);
                    thisObj_.transform.position = new Vector3(-20f, 0f, -20f);
                }
                return thisObj_;
            }
        }

        public toolWeapon(string parImagePath) : base(parImagePath, enumCaseType.tool, parIsVisible: true) { }

        #region memento
        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();
            tempResult["toolWeapon"] = new int[3] { rangeMin, rangeMax, damageOriginal };
            return tempResult;
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            rangeMin = parParameters.MoveNext() ? parParameters.Current : 1;
            rangeMax = parParameters.MoveNext() ? parParameters.Current : 1;
            damageOriginal = parParameters.MoveNext() ? parParameters.Current : 1;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            rangeMin = parParameters["toolWeapon"][0];
            rangeMax = parParameters["toolWeapon"][1];
            damageOriginal = parParameters["toolWeapon"][2];
        }
        #endregion memento

        public override object[] getDescriptionArgument() {
            return new object[1] { damageOriginal };
        }

        public virtual IEnumerable<damageInfo> attack(Thing parOwner) {
            doBeforeAttack();
            damageInfo tempDI = new damageInfo(parOwner, this, damageOriginal, damageType);
            tempDI.addDamage(parOwner.thisStatus.weaponAmplifierAdd);
            tempDI.mulitplyDamage(parOwner.thisStatus.weaponAmplifierMultiply - 100);
            yield return tempDI;
        }

        protected void doBeforeAttack() {
            resetTimer();
        }

        // weapon's showEffect basically animates body animation & sound according to enumAttackType
        public virtual void showEffect(Thing source, Thing parTarget, int parCountFolded) {
            (cashStateHash csh, float time) tempTup = animationTracker.dictEaaTrackerInformation[attackAnimation];
            source.thisAnimationTracker.Enqueue(tempTup.csh, (tempTup.time + intervalFolded * parCountFolded,
                () => gameManager.GM.AC.playSE(gameManager.GM.AHouC.selectAttackSound(attackAnimation))
            ));
        }

        void ICaseSystemicAdded.caseFunc(ICaseContainerContainer source) {
            if (thisObj != null && source is Thing tempThing) {
                tempThing.grabWeapon(thisObj.transform);
            }
        }

        void ICaseSystemicRemoved.caseFunc(ICaseContainerContainer source) {
            if (thisObj != null && source is Thing tempThing) {
                tempThing.dropWeapon(thisObj.transform);
            }
        }
    }
}
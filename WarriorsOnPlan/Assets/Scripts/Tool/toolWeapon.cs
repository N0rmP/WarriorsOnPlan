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
    trigAttackPunch
}

namespace Cases {
    public abstract class toolWeapon : caseTimerSelfishTurn {
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
        // ★ damageCur 방식 개선, 가능하면 Thing에 case changed를 bool 변수로 나타내게 해서 계산 빈도가 낮도록 메서드 1개 설계
        public int damageCur { get; set; }
        public virtual bool isReady {
            get {
                return timerCur <= 0;
            }
        }
        public enumDamageType damageType { get; protected set; } = enumDamageType.basic;
        public enumAttackAnimation attackAnimation { get; protected set; }

        public toolWeapon(int[] parWeaponParameters) : base(parWeaponParameters, enumCaseType.tool, parIsVisible: true) { }

        public override void restore(mementoIParametable parMementoCase) {
            base.restore(parMementoCase);
        }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();
            tempResult["toolWeapon"] = new int[3] { rangeMin, rangeMax, damageCur };
            return tempResult;
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            rangeMin = parParameters.MoveNext() ? parParameters.Current : 1;
            rangeMax = parParameters.MoveNext() ? parParameters.Current : 1;
            damageCur = parParameters.MoveNext() ? parParameters.Current : 1;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            rangeMin = parParameters["toolWeapon"][0];
            rangeMax = parParameters["toolWeapon"][1];
            damageCur = parParameters["toolWeapon"][2];
        }

        public virtual IEnumerable<damageInfo> attack(Thing parOwner) {
            doBeforeAttack();
            damageInfo tempDI = new damageInfo(parOwner, this, damageCur, damageType);
            tempDI.addDamage(parOwner.weaponAmplifierAdd);
            tempDI.mulitplyDamage(parOwner.weaponAmplifierMultiply);
            yield return tempDI;
        }

        protected void doBeforeAttack() {
            resetTimer();
        }

        public abstract void showEffect(Thing source, Thing parTarget);

        #region basic_animation
        protected void showBasicProjectile(Vector3 parDeparture, Vector3 parDestination) {
            combatManager.CM.FC.callVFX(
                        enumVFX.projectile_simple,
                        combatManager.CM.FC.getRetrieverMoveStop(),
                        parDeparture,
                        parDestination,
                        enumMoveType.linear,
                        null,
                        0.5f
                    );
        }
        #endregion basic_animation
    }
}
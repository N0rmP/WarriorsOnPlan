using Cases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public abstract class upgradeAbst : caseBase {
        public int starRequired { get; protected set; }
        public bool isUpgraded { get; private set; }

        public upgradeAbst(string parImagePath) : base(parImagePath, enumCaseType.upgrade, true) {
            isUpgraded = false;
        }

        #region combat
        // actualActivate executed right after level-systemmInitiating (before the mementoInitial created)
        public abstract void actualActivate();

        // public virtual void reset() { }
        #endregion combat

        #region IParametable
        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();
            tempResult["upgradeAbst"] = new int[2] { starRequired, isUpgraded.ToInteger() };

            return tempResult;
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            starRequired = parParameters.MoveNext() ? parParameters.Current : 99;
            isUpgraded = parParameters.MoveNext() ? (parParameters.Current).ToBoolean() : false;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            starRequired = parParameters["upgradeAbst"][0];
            isUpgraded = parParameters["upgradeAbst"][1].ToBoolean();
        }
        #endregion IParametable
    }
}
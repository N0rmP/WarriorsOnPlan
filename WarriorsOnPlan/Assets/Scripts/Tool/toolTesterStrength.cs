using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public class toolTesterStrength : toolAbst, ICaseSystemicCalculateStatus {
        private int value;

        #region InfoImplementation
        public override object[] getDescriptionArgument() {
            return new object[1] { value };
        }
        #endregion InfoImplementation

        public toolTesterStrength() : base("Image/Case/Tool/Image_toolTesterStrength") {
            code = 93002;
        }

        #region memento
        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();

            tempResult["concrete"] = new int[1] { value };

            return tempResult;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            value = parParameters["concrete"][0];
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            value = parParameters.MoveNext() ? parParameters.Current : 0;
        }
        #endregion memento        

        #region ICase
        void ICaseSystemicCalculateStatus.caseFunc(structWarriorStatus parStatus) {
            parStatus.weaponAmplifierAdd += value;
        }
        #endregion ICase
    }
}
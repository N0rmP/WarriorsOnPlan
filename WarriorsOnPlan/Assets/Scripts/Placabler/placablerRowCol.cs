using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Placablers {
    public class placablerRowCol : IPlacabler {
        private int minCoor0 = 0;
        private int maxCoor0 = 6;
        private int minCoor1 = 0;
        private int maxCoor1 = 2;

        public bool checkPlacable(node parNode) {
            return
                parNode.coor0 >= minCoor0 &&
                parNode.coor0 <= maxCoor0 &&
                parNode.coor1 >= minCoor1 &&
                parNode.coor1 <= maxCoor1;
        }

        #region IParametable
        public Dictionary<string, int[]> getParameters() {
            return new Dictionary<string, int[]>() { { "concrete", new int[4] { minCoor0, maxCoor0, minCoor1, maxCoor1 } } };
        }

        public List<object> getReferences() {
            return new List<object>();
        }

        public void restore(mementoIParametable parmementoIParametable) {
            restoreParameters(parmementoIParametable.dicParameter);
        }

        public void restoreParameters(Dictionary<string, int[]> parParameters) {
            restoreParameters(parParameters["concrete"].GetEnumerator() as IEnumerator<int>);
        }

        public void restoreParameters(IEnumerator<int> parParameters) {
            minCoor0 = parParameters.MoveNext() ? parParameters.Current : 0;
            maxCoor0 = parParameters.MoveNext() ? parParameters.Current : 6;
            minCoor1 = parParameters.MoveNext() ? parParameters.Current : 0;
            maxCoor1 = parParameters.MoveNext() ? parParameters.Current : 2;
        }

        public void restoreReferences(List<object> parListReference) { }
        #endregion IParametable
    }
}
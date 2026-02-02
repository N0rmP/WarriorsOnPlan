using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Differentiaters {
    public abstract class differentiaterBase {
        private bool isEnabled = false;

        private bool isSequentialValid;
        private int indexSequential;
        protected Func<bool>[] arrDelCheckSequential;
        protected Action[] arrDelDoSequential;

        private bool isSpontaneousValid;
        private bool[] arrIsSpontaneousExceptions;
        protected Func<bool>[] arrDelCheckSpontaneous;
        // delegate in listDelDoSpontaneous returns true when the executed index should be removed from arrDelCheckSpontaneous & listDelDoSpontaneous
        protected Func<bool>[] arrDelDoSpontaneous;

        public void init() {
            isEnabled = true;

            isSequentialValid = arrDelCheckSequential != null && arrDelDoSequential != null && arrDelCheckSequential.Length == arrDelDoSequential.Length;
            indexSequential = 0;

            isSpontaneousValid = arrDelCheckSpontaneous != null && arrDelDoSpontaneous != null && arrDelCheckSpontaneous.Length == arrDelDoSpontaneous.Length;
            arrIsSpontaneousExceptions = new bool[arrDelCheckSpontaneous == null ? 0 : arrDelCheckSpontaneous.Length];
            Array.Fill(arrIsSpontaneousExceptions, false);
            
            actualInit();
        }

        public void checkAndDo() {
            if (!isEnabled) {
                return;
            }

            // sequential
            if (isSequentialValid && arrDelCheckSequential[indexSequential]()) {
                arrDelDoSequential[indexSequential]();
                indexSequential++;
                if (indexSequential >= arrDelCheckSequential.Length) {
                    isSequentialValid = false;
                }
            }

            // spontaneous
            int i = 0;
            while (isSpontaneousValid && i < arrDelCheckSpontaneous.Length) {
                if (arrIsSpontaneousExceptions[i]) {
                    i++;
                    continue;
                }

                if (arrDelCheckSpontaneous[i]()) {
                    if (arrDelDoSpontaneous[i]()) {
                        arrIsSpontaneousExceptions[i] = true;
                    }
                }
                i++;
            }
        }

        // actualInit prepares any object's unordinary state (ex : hide UI for first tutorial)
        protected abstract void actualInit();

        public virtual void restoreWhenLeave() {
            isEnabled = false;
        }
    }
}
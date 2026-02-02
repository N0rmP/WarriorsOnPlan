using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace System.Collections.Generic {
    public class PriorityQueue<T> : IEnumerable<T> {
        private const int ratioGrowth = 2;

        // arr is heap, index 0 is not usef for comfortable calculation
        private T[] arr;
        public int Count { get; private set; }
        private Comparison<T> thisComparison;

        #region creator
        public PriorityQueue() {
            arr = new T[2];
            thisComparison = Comparer<T>.Default.Compare;
        }

        public PriorityQueue(Comparer<T> parComparer) {
            arr = new T[2];
            thisComparison = parComparer.Compare;
        }

        public PriorityQueue(Comparison<T> parComparison) {
            arr = new T[2];
            thisComparison = parComparison;
        }
        #endregion creator

        // heap enqueue
        public void Enqueue(T par) {
            // exponential growth
            while (Count + 2 > arr.Length) {
                Array.Resize(ref arr, arr.Length * ratioGrowth);
            }

            arr[++Count] = par;
            int tempIndexParent = Count / 2;
            int tempIndexChild = Count;
            while (thisComparison(arr[tempIndexParent], arr[tempIndexChild]) > 0 && tempIndexChild > 1) {
                Swap(tempIndexParent, tempIndexChild);
                tempIndexChild = tempIndexParent;
                tempIndexParent = tempIndexChild / 2;
            }
        }

        public T Dequeue() {
            if (Count == 0) {
                Debug.Log("PriorityQueue.Dequeue error : tried Dequeue while Count == 0");
                return default(T);
            }

            T tempResult = arr[1];

            arr[1] = arr[Count--];
            int tempIndexCurrent = 1;
            int tempIndexChildSmaller;
            // loop until index-child is out of boundary
            while (tempIndexCurrent * 2 < Count + 1) {
                if (tempIndexCurrent * 2 + 1 < Count + 1) {
                    tempIndexChildSmaller = thisComparison(arr[tempIndexCurrent * 2], arr[tempIndexCurrent * 2 + 1]) <= 0
                        ? (tempIndexCurrent * 2)
                        : (tempIndexCurrent * 2 + 1);
                } else {
                    tempIndexChildSmaller = tempIndexCurrent * 2;
                }

                // if Current is larger than ChildSmaller, Swap
                if (thisComparison(arr[tempIndexCurrent], arr[tempIndexChildSmaller]) > 0) {
                    Swap(tempIndexCurrent, tempIndexChildSmaller);
                }
                tempIndexCurrent = tempIndexChildSmaller;
            }

            // exponential decay
            // exponential decay done when used-space is less than 1/4 of arr, to prevent too frequent resize
            while (Count + 1 < arr.Length / 4 && arr.Length > 2) {
                Array.Resize(ref arr, arr.Length / ratioGrowth);
            }

            return tempResult;
        }

        public bool Contains(T parTarget) {
            return arr.Contains(parTarget);
        }

        public bool Contains(Func<T, bool> parPredicate) {
            for (int i =0; i< Count; i++) {
                if (parPredicate(arr[i])) {
                    return true;
                }
            }
            return false;
        }

        public void Clear() {
            Count = 0;
            Array.Resize(ref arr, 2);
        }

        private void Swap(int parIndex1, int parIndex2) {
            if (parIndex1 > arr.Length - 1 || parIndex2 > arr.Length - 1) {
                return;
            }

            T tempBuffer = arr[parIndex1];
            arr[parIndex1] = arr[parIndex2];
            arr[parIndex2] = tempBuffer;
        }

        #region IEnumerable
        public IEnumerator<T> GetEnumerator() {
            foreach (T t in arr) {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return arr.GetEnumerator();
        }
        #endregion IEnumerable

        #region test
        public void testPriorityQueue() {
            StringBuilder tempSB = new StringBuilder("test PriorityQueue\n");

            int tempIndex = 1;
            int tempSquared = 2;
            int tempTotalLayer = (int)Math.Log(arr.Length, 2);
            while (tempIndex <= Count) {
                tempSB.Append(arr[tempIndex]);

                if (tempIndex < tempSquared - 1) {
                    if (tempIndex % 2 == 0) {
                        tempSB.Append(", ");
                    } else {
                        tempSB.Append(",");
                        tempSB.Append('\t', tempTotalLayer - (int)Math.Log(tempSquared, 2) + 1);
                    }
                } else {
                    tempSB.Append("\n");
                    tempSquared *= 2;
                }

                tempIndex++;
            }

            Debug.Log(tempSB.ToString());
        }
        #endregion test
    }

    public static class exIEnumerable {
        public static T selectRandom<T>(this IEnumerable<T> parIEnumerable) {
            int tempCount = parIEnumerable.Count();

            int tempRandom = UnityEngine.Random.Range(0, tempCount);
            IEnumerator<T> tempIEnumerator = parIEnumerable.GetEnumerator();
            tempIEnumerator.MoveNext();
            while (tempRandom-- > 0) {
                tempIEnumerator.MoveNext();
            }
            return tempIEnumerator.Current;
        }
    }
}
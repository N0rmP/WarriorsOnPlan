using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// carrierCrude supports similar-object-pooling
// when it's too small to use object-pooling, but it's necessary to deactivate unused pooled-object... this is the thing!
public class carrierCrude {
    /*
        updateCarrierCrude uses only required amount of elements and throw away the rest

        parDelActivate : delegate to set elements to be activated
        parDelDeactivate : delegate to retrieve(deactivate) elements that runs out of their use
    */
    public static void updateCarrierCrude<A, B>(A[] parContainerA, IEnumerable<B> parContainerB, Func<A> parDelCreate, Action<A, B> parDelActivate, Action<A> parDelDeactivate) {
        int tempIndex = 0;
        
        /* 
            activate
            B is prioritized
            A is created until the number of A is same as B
        */
        foreach (B binst in parContainerB) {
            if (tempIndex >= parContainerA.Length) {
                parDelActivate(parDelCreate(), binst);
            } else {
                parDelActivate(parContainerA[tempIndex], binst);
            }
            tempIndex++;
        }

        /*
            deactivate
            unused A in parContainerA is deactivated
        */
        for (; tempIndex < parContainerA.Length; tempIndex++) {
            parDelDeactivate(parContainerA[tempIndex]);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Placablers;

public class placablerComponent {
    public IPlacabler curPlacabler { get; private set; }

    private IPlacabler[] arrPlacabler;
    // plablerDefaut is placablerRowCol in arrPlacabler
    private IPlacabler placablerDefault;

    public placablerComponent() {
        IEnumerable<Type> tempColPlacabler = typeof(IPlacabler).Assembly.GetTypes().Where(
            (x) => typeof(IPlacabler).IsAssignableFrom(x) && !x.IsInterface
        );
        List<IPlacabler> tempListPlacabler = new List<IPlacabler>();
        foreach (Type t in tempColPlacabler) {
            tempListPlacabler.Add((IPlacabler)Activator.CreateInstance(t));
            if (t == typeof(placablerRowCol)) {
                placablerDefault = tempListPlacabler.Last();
            }
        }
        arrPlacabler = tempListPlacabler.ToArray();
    }

    public void setPlacabler(string parPlacablerName, int[] parParameter) {
        void preparePlcablerDefault() {
            curPlacabler = placablerDefault;
            curPlacabler.restoreParameters(new List<int> { 0, 6, 0, 2 }.GetEnumerator());
        }

        if (parParameter == null) {
            parParameter = new int[0];
        }

        if (parPlacablerName == null || parParameter.Length == 0) {
            preparePlcablerDefault();
            return;
        }
        

        foreach (IPlacabler pblr in arrPlacabler) {
            if (pblr.GetType().Name.Contains(parPlacablerName)) {
                curPlacabler = pblr;
                curPlacabler.restoreParameters(((IEnumerable<int>)parParameter).GetEnumerator());
                return;
            }
        }

        preparePlcablerDefault();
    }

    /*
    // IPlacabler has no any mutable field / member or methods that can result in unexpected execution, so it'll be fine to pass itself
    public bool checkPlacabler(node parNode) {
        if (curPlacabler == null) {
            Debug.Log("placablerComponent.curPlacabler is null");
            return placablerDefault.checkPlacable(parNode);
        }

        return curPlacabler.checkPlacable(parNode);
    }
    */
}

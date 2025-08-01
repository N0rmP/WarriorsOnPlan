using Cases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    transcendent's caseBase is regared as caseBase added to all adequate Thing
    there are three transcendents for each enumSide, and one transcendent for all Thing
    so if one ICase is observed for player-Thing, observing goes in order of transcendentTotal >>> transcendentPlayer >>> each player-Thing
*/
public class transcendent : ICaseContainerContainer {
    private caseContainer thisCaseContainer;

    public transcendent() {
        thisCaseContainer = new caseContainer();
    }

    public void addCase(caseBase parCase) {
        thisCaseContainer.addCase(parCase);
    }

    public void removeCase(caseBase parCase) {
        thisCaseContainer.removeCase(parCase);
    }

    public bool checkContainCaseType(caseBase parCase) {
        return thisCaseContainer.checkContainCaseType(parCase);
    }

    public bool checkContainConcreteCase(caseBase parCase) {
        return thisCaseContainer.checkContainCaseConcrete(parCase);
    }

    public List<T> getCaseList<T>() {
        return thisCaseContainer.getCaseList<T>();
    }

    public List<caseBase> getCaseList(enumCaseType parCaseType) {
        return thisCaseContainer.getCaseList(parCaseType);
    }

    public bool observeInterferable<A>(object[] parParameters) {
        return thisCaseContainer.observeInterferable<A>(parParameters);
    }

    public IEnumerable<B> observeReturnEnumerate<A, B>(object[] parParameters) {
        return thisCaseContainer.observeReturnEnumerate<A, B>(parParameters);
    }

    public void observeVoid<A>(object[] parParameters) {
        thisCaseContainer.observeVoid<A>(parParameters);
    }

    
}

using Cases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ICaseContainerContainer is expected to have caseContainer as field, please see Thing.cs to understand it clean
public interface ICaseContainerContainer {
    public void addCase(caseBase parCase);

    public void removeCase(caseBase parCase);

    public List<T> getCaseList<T>();

    public List<caseBase> getCaseList(enumCaseType parCaseType);

    public bool checkContainCaseType(caseBase parCase);

    public bool checkContainConcreteCase(caseBase parCase);

    public void observeVoid<A>(object[] parParameters);

    public IEnumerable<B> observeReturnEnumerate<A, B>(object[] parParameters);

    public bool observeInterferable<A>(object[] parParameters);
}

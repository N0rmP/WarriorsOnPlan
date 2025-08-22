using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

public interface ICaseAfterAddCase {
    public void caseFunc(ICaseContainerContainer source, caseBase caseAdded);
}

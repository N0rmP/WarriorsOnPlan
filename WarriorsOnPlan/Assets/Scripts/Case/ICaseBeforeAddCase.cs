using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public interface ICaseBeforeAddCase {
        public bool onBeforeAddCase(Thing source, caseBase parCaseAdded); 
    }
}
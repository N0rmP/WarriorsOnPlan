using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ★ ...이거 필요한가?? 그냥 각 tool에서 enumCaseType.tool만 재깍재깍 넣어주면 될 거 같긴 한데... tool에 뭘 추가할지 알 수 없으니 일단 유지

namespace Cases {
    public abstract class toolAbst : caseBase {

        public toolAbst(int[] parToolParameters) : base(parToolParameters, enumCaseType.tool, true) { }
    }
}

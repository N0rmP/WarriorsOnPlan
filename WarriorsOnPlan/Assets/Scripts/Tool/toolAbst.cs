using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ...�̰� �ʿ��Ѱ�?? �׳� �� tool���� enumCaseType.tool�� ������ �־��ָ� �� �� ���� �ѵ�... tool�� �� �߰����� �� �� ������ �ϴ� ����

namespace Cases {
    public abstract class toolAbst : caseBase {

        public toolAbst(int[] parToolParameters) : base(parToolParameters, enumCaseType.tool, true) { }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseThisSkillUsed
{
    // �� ��ų ���ο��� processDealDamage�� ȣ���� ������ �װ��� �ν��� �� ������?
    public void onThisSkillUsed(Thing source, Thing target, damageInfo DInfo);
}

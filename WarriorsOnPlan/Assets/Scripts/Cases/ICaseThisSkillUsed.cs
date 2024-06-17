using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseThisSkillUsed
{
    // ★ 스킬 내부에서 processDealDamage를 호출할 때마다 그것을 인식할 수 있을까?
    public void onThisSkillUsed(Thing source, Thing target, damageInfo DInfo);
}

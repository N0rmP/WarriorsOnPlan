using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class navigatorAbst : caseAll
{
    protected node destination_;
    // route will be recalculated just before every movement, but can remain only when whole nodes in route have nothing on them
    protected Stack<EDirection> route;

    public node destination {
        set { destination_ = value; }
    }

    /*
    1. 이동 방식은 사실 모두 동일하다. 다만 목적지 / 이동을 시작하는 조건이 모두 달라진다.
    2. 이동은 경로를 저장해두지 않는다. 그냥 1초에 1번 턴을 가질 때마다 새로이 경로를 계산해야 할테니까
    3. 목적지 선택은 몇 초 동안 임시로 변경했다가 원상복구시킬 수 있어야 한다.
        (moverAbst와 ICaseTimed를 모두 상속받게 한 뒤, 원본 mover를 정해진 타이머만큼 보관해두었다가 다시 둘을 교환해서 원상복구)
     */

    public navigatorAbst() {
        destination_ = null;
        route = new Stack<EDirection>();
    }

    public abstract EDirection getNextEDirection();
}

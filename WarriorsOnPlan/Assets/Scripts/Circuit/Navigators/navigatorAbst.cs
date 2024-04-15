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
    1. �̵� ����� ��� ��� �����ϴ�. �ٸ� ������ / �̵��� �����ϴ� ������ ��� �޶�����.
    2. �̵��� ��θ� �����ص��� �ʴ´�. �׳� 1�ʿ� 1�� ���� ���� ������ ������ ��θ� ����ؾ� ���״ϱ�
    3. ������ ������ �� �� ���� �ӽ÷� �����ߴٰ� ���󺹱���ų �� �־�� �Ѵ�.
        (moverAbst�� ICaseTimed�� ��� ��ӹް� �� ��, ���� mover�� ������ Ÿ�̸Ӹ�ŭ �����صξ��ٰ� �ٽ� ���� ��ȯ�ؼ� ���󺹱�)
     */

    public navigatorAbst() {
        destination_ = null;
        route = new Stack<EDirection>();
    }

    public abstract EDirection getNextEDirection();
}

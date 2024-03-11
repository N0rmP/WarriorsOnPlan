using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class moverAbst
{
    protected node destination_;
    public node destination {
        set { destination_ = value; }
    }

    /*
    1. �̵� ����� ��� ��� �����ϴ�. �ٸ� ������ / �̵��� �����ϴ� ������ ��� �޶�����.
    2. �̵��� ��θ� �����ص��� �ʴ´�. �׳� 1�ʿ� 1�� ���� ���� ������ ������ ��θ� ����ؾ� ���״ϱ�
    3. ������ ������ �� �� ���� �ӽ÷� �����ߴٰ� ���󺹱���ų �� �־�� �Ѵ�.
        (moverAbst�� ICaseTimed�� ��� ��ӹް� �� ��, ���� mover�� ������ Ÿ�̸Ӹ�ŭ �����صξ��ٰ� �ٽ� ���� ��ȯ�ؼ� ���󺹱�)
     */

    public virtual void move() { 
        // destination���� �̵� 1ĭ, ���� destination�� ������ �����ϴٸ� route�� �����ص� ���� ������?
    }
}

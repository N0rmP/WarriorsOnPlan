using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motherboard
{
    /* ��
    2024.05.31 ���� ����
    1.  ��� circuit Ŭ������ ���̻� caseAll�� ��ӹ��� �ʴ´�.
        ü���� ���� �� �� ���� ���̴� ���� ȿ���� �ش� ������ ��� �۵��ؾ� ������
        circuit�� warriorAbst�� �ൿ�� �����ϴ¸�ŭ updateState ������ �۵��ص� �� ������ �� �ϱ� ����.
    2.  ��� circuit Ŭ������ ������ �޼����� �Ű������� object[]�� ������ �Ѵ�.
        circuit ���� �� �پ��� ���� / ������ �Ű������� �߻��ϱ� ����.
        ��ǥ������ '���� ���� N�� �� M�� ���� �̵�'�� 2���� int �Ű������� �䱸�Ǹ�,
        '�÷��̾ ������ ������ ���� ������� ����'�� 1���� warriorAbst �Ű������� �䱸�ȴ�.
    3.  motherboard ��ü ������ ���� factory ������ ����ϴ� ���� �ٶ������� �ʴ�.
        ������ �ѵ� ���� ���� ������ ���ø��� ������ �� �ƴ϶�,
        ������ circuit ��ü�� ��� ���� �ٸ� �� �ֱ� ������ factory method�� ����� �ص��� ��ƴ�.
        �׳� motherboardMaker ������ AISetter Ŭ�������� �÷��̾ �����ϴ� ���� �� ���������� �����ص״ٰ�,
        ���� �Ϸ� ��ư�� ������ �� �Ѳ����� motherboard ���ο� �����ϴ� ���°� �ξ� ���ƺ��δ�.
        '���߿� ���ο� circuit Ŭ������ ����� �����?'��� ������ ������ ����� �� ������ ���ø��� �����Ǵ� ���°� �ƴϱ� ������ �ذ��� �� ����.
    4.  wigwagger�� ������ ������ �̵� / ��ų�� ��� ����� �� �ְ� ���� ��.
        �̵��� ��ų ���� ��� 'Ư�� ��Ȳ�� �̵� or ��ų�� ����Ͻʽÿ�'�� ����Ű�� ������ ������ �� ���� ������ ���δ�.
        ������ ��� �Ϻ� ������ �� ��쿡 ���������� ����� �� �ִ�.
    5.  warriorAbst.updateState�� ������ ���۵Ǹ� motherboard.refreshBoard ����
        >> refreshBoard�� 2���� wigwagger�� ���� Ȯ���Ͽ� navigator Ȥ�� 
    */

    public navigatorAbst navigator { get; private set; }    //ICaseUpdateState ���
    //publlic skillAlarmerAbst skillAlarmer { get; set; }   //ICaseUpdateState ���
    public wigwaggerAbst wigwagger { get; private set; }    //caseAll ���
    public selecterAbst selecterForAttack { get; private set; } //�ƹ� �͵� ��ӹ��� ����
    public selecterAbst selecterForSkill { get; private set; }  //�ƹ� �͵� ��ӹ��� ����
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motherboard
{
    /* ★
    2024.05.31 생각 정리
    1.  모든 circuit 클래스는 더이상 caseAll을 상속받지 않는다.
        체력을 잃을 때 그 값을 줄이는 등의 효과는 해당 순간에 즉시 작동해야 하지만
        circuit은 warriorAbst의 행동을 결정하는만큼 updateState 때에만 작동해도 제 역할을 다 하기 때문.
    2.  모든 circuit 클래스는 생성자 메서드의 매개변수에 object[]를 가져야 한다.
        circuit 설정 시 다양한 개수 / 종류의 매개변수가 발생하기 때문.
        대표적으로 '전투 시작 N턴 후 M턴 동안 이동'은 2개의 int 매개변수가 요구되며,
        '플레이어가 지정한 적군을 공격 대상으로 지정'은 1개의 warriorAbst 매개변수가 요구된다.
    3.  motherboard 객체 생성을 위해 factory 패턴을 사용하는 것은 바람직하지 않다.
        설정값 한두 개에 따라 동일한 템플릿이 나오는 게 아니라,
        각각의 circuit 객체가 모두 서로 다를 수 있기 때문에 factory method를 만들기 극도로 어렵다.
        그냥 motherboardMaker 내지는 AISetter 클래스에서 플레이어가 설정하는 동안 그 설정값들을 저장해뒀다가,
        설정 완료 버튼을 눌렀을 때 한꺼번에 motherboard 내부에 저장하는 형태가 훨씬 나아보인다.
        '나중에 새로운 circuit 클래스가 생기면 어떡하지?'라는 생각이 들지만 상술했 듯 일정한 템플릿이 결정되는 형태가 아니기 때문에 해결할 수 없다.
    4.  wigwagger의 구조를 개편해 이동 / 스킬에 모두 사용할 수 있게 만들어볼 것.
        이동과 스킬 조건 모두 '특정 상황에 이동 or 스킬을 사용하십시오'를 가리키기 때문에 통합할 수 있을 것으로 보인다.
        통합할 경우 일부 조건은 두 경우에 공통적으로 사용할 수 있다.
    5.  warriorAbst.updateState는 실행이 시작되면 motherboard.refreshBoard 실행
        >> refreshBoard는 2개의 wigwagger를 각각 확인하여 navigator 혹은 
    */

    public navigatorAbst navigator { get; private set; }    //ICaseUpdateState 상속
    //publlic skillAlarmerAbst skillAlarmer { get; set; }   //ICaseUpdateState 상속
    public wigwaggerAbst wigwagger { get; private set; }    //caseAll 상속
    public selecterAbst selecterForAttack { get; private set; } //아무 것도 상속받지 않음
    public selecterAbst selecterForSkill { get; private set; }  //아무 것도 상속받지 않음
}

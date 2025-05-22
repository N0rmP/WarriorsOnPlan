using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxRetrieveOnStop : MonoBehaviour {
    /*
    vfxMovable는 movableObject를 상속받고 있어 vfx로 통합 상속시키기 어렵다
        통합 상속이 필요한가? 필요하다면 해결책은 있는가?

   fxComponent에서의 원활한 관리를 위해 통합 상속은 필요하다. + 통합 상속에 더해 fxComponent에서 vfxMovable의 return 조건을 지정할 수 있어야 한다.
        vfxMovable을 일종의 vfxContainer로 사용, return하는 조건을 따로 유사 strategy 패턴으로 빼내어 구현 후 fxComponent에서 지정
    */


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

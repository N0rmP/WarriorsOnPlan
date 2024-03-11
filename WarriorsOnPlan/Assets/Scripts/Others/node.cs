using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node
{
    public readonly int coor0;
    public readonly int coor1;
    private readonly int[] directionToCoor = new int[4]{ -1, 0, 1, 0 };

    private node[] link;
    private Thing thingHere_;
    public Thing thingHere {
        get { return thingHere_; }
        set { thingHere_ = value; }
    }

    public node(int parCoor0, int parCoor1) {
        coor0 = parCoor0;
        coor1 = parCoor1;
    }

    public void setLink(node parNode, EDirection parDir) {
        link[(int)parDir] = parNode;
    }

    public void placeThing(Thing parThing) {
        if (thingHere != null) { return; /*★사용자 정의 exception 만들어서 "node에 뭐 있는데요?" UI라도 띄우기*/ }

        this.thingHere_ = parThing;
        parThing.gameObject.transform.position = new Vector3((float)(coor0 * 2), 0f, (float)(coor1 * 2));
    }

    public void sendThing(EDirection parDir) {
        //check if its not boundary, and if there is something on destination
        if ((link[(int)parDir] == null) && (link[(int)parDir].thingHere_ != null)) {
            return;
        }
        link[(int)parDir].thingHere = thingHere_;
        this.thingHere_ = null;
    }
}

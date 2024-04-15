using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection {
    //direction     = index //required calculation for direction-to-coordinates conversion (directionConverter)
    //you can find that z calculation is two indice latter than x calculation
    forward         = 0,    //-1    0
    forward_right   = 1,    //-1    -1
    right           = 2,    //0     -1
    backward_right  = 3,    //1     -1
    backward        = 4,    //1     0
    backward_left   = 5,    //1     1
    left            = 6,    //0     1
    forward_left    = 7,    //-1    1
    none            = 8     // this EDirection represents no direction, used for initiation of EDirectionToDeparture when graph search
}

public class node
{
    public readonly int coor0;
    public readonly int coor1;
    private static readonly int[] directionConverter = new int[8]{ -1, -1, 0, 1, 1, 1, 0, -1};

    private node[] link_;
    private Thing thingHere_;
    public node[] link {
        get { return link_; }
    }
    public Thing thingHere {
        get { return thingHere_; }
        set { thingHere_ = value; }
    }

    // variables below is used for various searches, it's declared public being used only by external instances
    public bool swissArmyVisited;
    public EDirection swissArmyEDirection;

    public node(int parCoor0, int parCoor1) {
        coor0 = parCoor0;
        coor1 = parCoor1;
        swissArmyVisited = false;
        link_ = new node[8];
    }

    public (int c0, int c1) getPosition() {
        return (coor0 * 2, coor1 * 2);
    }

    public (int c0, int c1) getlink_Coordinates(EDirection parEDir) {
        return (
            coor0 + directionConverter[(int)parEDir],
            coor1 + directionConverter[((int)parEDir + 6) % 8]
        );
    }

    // setLink set not only this node's link_ but also the target node's link_
    public void setLink(node parNode, EDirection parDir) {
        link_[(int)parDir] = parNode;
        parNode.link_[((int)parDir + 4) % 8] = this;
    }

    public void placeThing(Thing parThing) {
        if (thingHere != null) { return; /*★사용자 정의 exception 만들어서 "node에 뭐 있는데요?" UI라도 띄우기*/ }

        parThing.curPosition = this;
        this.thingHere_ = parThing;
        parThing.gameObject.transform.position = new Vector3((float)(coor0 * 2), 0f, (float)(coor1 * 2));
    }

    public bool sendThing(EDirection parDir) {
        //check if its not boundary, and if there is something on destination
        if ((link_[(int)parDir] == null) && (link_[(int)parDir].thingHere_ != null)) {
            return false;
        }
        thingHere_.curPosition = link_[(int)parDir];
        link_[(int)parDir].thingHere = thingHere_;
        this.thingHere_ = null;
        return true;
    }
}

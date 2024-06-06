using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseDestroy
{
    //source of onDestroy is owner, target is the destroied warrior
    public void onDestroy(Thing source, Thing target);
}

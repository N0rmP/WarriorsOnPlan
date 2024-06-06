using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseDestroied
{
    //source of onDestroied is the destroyer warrior, target is owner
    public virtual void onDestroied(Thing source, Thing target) { }
}

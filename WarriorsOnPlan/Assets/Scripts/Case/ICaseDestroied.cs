using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseDestroied {
    public virtual void onDestroied(Thing dead, Thing destroyer) { }
}

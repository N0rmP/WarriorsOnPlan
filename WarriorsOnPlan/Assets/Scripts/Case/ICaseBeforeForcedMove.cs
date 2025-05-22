using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeForcedMove {
    public void onBeforeForcedMove(Thing source, node destination);
}

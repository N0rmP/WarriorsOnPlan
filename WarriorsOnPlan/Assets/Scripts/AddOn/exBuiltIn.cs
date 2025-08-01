using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class exBoolean {
    public static int ToInteger(this bool parBool) {
        return parBool ? 1 : 0;
    }
}

public static class exInteger {
    public static bool ToBoolean(this int parInt) { 
        return parInt != 0 ? true : false;
    }
}

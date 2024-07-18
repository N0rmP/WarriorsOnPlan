using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolMaker
{
    public static caseBase makeTool(int parCode, int[] pp) { 
        return parCode switch { 
            -1 => new weaponTester(pp),
            _ => null
        };
    }
}

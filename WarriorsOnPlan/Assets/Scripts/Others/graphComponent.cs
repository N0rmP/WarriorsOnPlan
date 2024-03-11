using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;

public enum EDirection { up = 0, right = 1, down = 2, left = 3 }

public class graphComponent
{
    readonly int size0;
    readonly int size1;

    private node[,] graph;

    public graphComponent(int parSize0, int parSize1) {
        size0 = parSize0;
        size1 = parSize1;
        graph = new node[size0, size1];
    }

    public node this[int ind0, int ind1] {
        get { return graph[ind0, ind1]; }
    }
}

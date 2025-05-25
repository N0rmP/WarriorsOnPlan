using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graphComponent {
    public readonly int size0;
    public readonly int size1;

    private GameObject prefabNode;

    private node[,] graph;
    
    public graphComponent(int parSize0, int parSize1) {
        size0 = parSize0;
        size1 = parSize1;
        prefabNode = Resources.Load<GameObject>("Prefab/Node");
        graph = new node[size0, size1];

        // create nodes and connect each other
        for (int i = 0; i < size0; i++) {
            for (int j = 0; j < size1; j++) {
                graph[i, j] = GameObject.Instantiate(prefabNode).GetComponent<node>().init(i, j);
                graph[i, j].transform.SetParent(combatManager.CM.transform);

                if (i > 0) {
                    if (j > 0) {
                        graph[i, j].setLink(graph[i - 1, j - 1], EDirection.backward_left);
                    }
                    graph[i, j].setLink(graph[i - 1, j], EDirection.left);
                    if (j < size1 - 1) {
                        graph[i, j].setLink(graph[i - 1, j + 1], EDirection.forward_left);
                    }
                }

                if (j > 0) {
                    graph[i, j].setLink(graph[i, j - 1], EDirection.backward);
                }
                /*
                ★ 희대의 멍청이 해삼 말미잘 등신 머저리의 상징으로써 이 주석을 남겨놓습니다. 과거의 최정훈은 두고두고 반성하세요 이 돌대가리 빡통아
                if (i > 0) {
                    if (j > 0) {
                        graph[i, j].setLink(graph[i - 1, j - 1], EDirection.forward_right);
                    }
                    graph[i, j].setLink(graph[i - 1, j], EDirection.forward);
                    if (j < size1 - 1) {
                        graph[i, j].setLink(graph[i - 1, j + 1], EDirection.forward_left);
                    }
                }

                if (j > 0) {
                    graph[i, j].setLink(graph[i, j - 1], EDirection.right);
                }
                */
            }
        }

        resetNodeSearchVariables();
    }

    public void vacateGraph() {
        for (int i = 0; i < size0; i++) {
            for (int j = 0; j < size1; j++) {
                if (graph[i, j].thingHere != null) {
                    graph[i, j].expelThing();
                }
            }
        }
    }

    public void resetNodeSearchVariables() {
        for (int i=0; i<graph.GetLength(0); i++) {
            for (int j = 0; j < graph.GetLength(1); j++) {
                graph[i, j].swissArmyVisited = false;
                graph[i, j].swissArmyEDirection = EDirection.none;
            }
        }
    }

    public void BFS(node parDeparture, Func<node, bool> delGoalCheck, Stack<EDirection> parRoute, Vector2 parVectorToDestination) {
        if (parVectorToDestination == null) {
            parVectorToDestination = Vector2.zero;
        }

        // in this method we call tuple (node cur, EDirection EDirFromdeparture, int distFromDeparture) 'search'
        Queue<(node curNode, EDirection EDirFromDeparture, int distFromdeparture)> tempSearchQueue = new Queue<(node cur, EDirection EDirFromDeparture, int distFromDeparture)>();
        tempSearchQueue.Enqueue((parDeparture, EDirection.none, 0));
        (node curNode, EDirection EDirFromDeparture, int distFromDeparture) tempCurSearch;
        (node curNode, EDirection EDirFromDeparture, int distFromDeparture) tempMinDistanceGoal = (null, EDirection.none, int.MaxValue);
        node tempNode;

        // calculate the order of directions in which nodes be added while BFS searching
        IEnumerable<EDirection> tempFirstDirection = node.getDirectionClosestSorted(parVectorToDestination);
        int tempLinkIndex;

        // reset variables for search
        resetNodeSearchVariables();

        parDeparture.swissArmyVisited = true;
        // parDeparture.swissArmyEDirection = EDirection.none;
        while (tempSearchQueue.Count > 0){
            tempCurSearch = tempSearchQueue.Dequeue();            
            tempNode = tempCurSearch.curNode;

            // if tempCurSearch includes Goal Node and Lower distFromDeparture, update tempMinDistanceGoal
            if (delGoalCheck(tempNode) && (tempCurSearch.distFromDeparture < tempMinDistanceGoal.distFromDeparture)) {
                tempMinDistanceGoal = tempCurSearch;
            }

            // enqueue non-visited nodes around
            foreach (EDirection edir in tempFirstDirection) {
                tempLinkIndex = (int)edir;
                if (tempNode.link[tempLinkIndex] == null || tempNode.link[tempLinkIndex].swissArmyVisited || tempNode.link[tempLinkIndex].thingHere != null) {
                    continue; 
                }
                tempNode.link[tempLinkIndex].swissArmyVisited = true;
                tempNode.link[tempLinkIndex].swissArmyEDirection = edir;
                tempSearchQueue.Enqueue((tempNode.link[tempLinkIndex], edir, tempCurSearch.distFromDeparture + 1));
            }
        }

        // after BFS search, make route from tempMinDistanceGoal.curNode to departure using swissArmyEDirection
        tempNode = tempMinDistanceGoal.curNode;
        while (tempNode != parDeparture) {
            parRoute.Push(tempNode.swissArmyEDirection);
            tempNode = tempNode.link[((int)tempNode.swissArmyEDirection + 4) % 8];
        }
    }


    #region collection
    public node this[int ind0, int ind1] {
        get {
            if (ind0 < 0 || ind0 >= size0 || ind1 < 0 || ind1 >= size1) {
                Debug.Log("required coordinates is out of boundary : " + ind0 + " , " + ind1);
                return null;
            }
            return graph[ind0, ind1]; 
        }
    }

    public void doOnAllNode(Action<node> parDel) {
        for (int i = 0; i < size0; i++) {
            for (int j = 0; j < size1; j++) {
                parDel(graph[i, j]);
            }
        }
    }
    #endregion collection
}

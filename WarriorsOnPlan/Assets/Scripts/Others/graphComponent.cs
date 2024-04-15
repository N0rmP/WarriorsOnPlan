using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;

public class graphComponent
{
    public readonly int size0;
    public readonly int size1;

    private node[,] graph;

    public graphComponent(int parSize0, int parSize1) {
        size0 = parSize0;
        size1 = parSize1;
        graph = new node[size0, size1];

        // create nodes and connect each other
        for (int i = 0; i < size0; i++) {
            for (int j = 0; j < size1; j++) {
                graph[i, j] = new node(i, j);

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
            }
        }

        resetNodeSearchVariables();
    }

    public void resetNodeSearchVariables() {
        for (int i=0; i<graph.GetLength(0); i++) {
            for (int j = 0; j < graph.GetLength(1); j++) {
                graph[i, j].swissArmyVisited = false;
                graph[i, j].swissArmyEDirection = EDirection.none;
            }
        }
    }

    public void BFS(node parDeparture, Func<node, bool> delGoalCheck, ref Stack<EDirection> parRoute) {
        // ★ 탐색하면서 delGoalCheck을 통해 목표 지점인지 확인
        // ★ 최단거리 찾기
        // ★ 방향 저장하기
        // in this method we call tuple (node cur, EDirection EDirFromdeparture, int distFromDeparture) 'search'
        Queue<(node curNode, EDirection EDirFromDeparture, int distFromdeparture)> tempSearchQueue = new Queue<(node cur, EDirection EDirFromDeparture, int distFromDeparture)>();
        tempSearchQueue.Enqueue((parDeparture, EDirection.none, 0));
        (node curNode, EDirection EDirFromDeparture, int distFromDeparture) tempCurSearch;
        (node curNode, EDirection EDirFromDeparture, int distFromDeparture) tempMinDistanceGoal = (null, EDirection.none, int.MaxValue);
        node tempNode;

        //reset variables for search
        resetNodeSearchVariables();

        while (tempSearchQueue.Count > 0){
            tempCurSearch = tempSearchQueue.Dequeue();
            tempNode = tempCurSearch.curNode;
            tempNode.swissArmyVisited = true;
            tempNode.swissArmyEDirection = tempCurSearch.EDirFromDeparture;

            // if tempCurSearch includes Goal Node and Lower distFromDeparture, update tempMinDistanceGoal
            if (delGoalCheck(tempNode) && (tempCurSearch.distFromDeparture < tempMinDistanceGoal.distFromDeparture)) {
                tempMinDistanceGoal = tempCurSearch;
            }

            // enqueue non-visited nodes around
            for (int i=0; i<8; i++) {
                
                if (tempNode.swissArmyVisited || tempNode.thingHere != null) { 
                    continue; 
                }
                tempSearchQueue.Enqueue((tempCurSearch.curNode, (EDirection)i, tempCurSearch.distFromDeparture + 1));
            }
        }

        // after BFS search, make route from tempMinDistanceGoal.curNode to departure using swissArmyEDirection
        tempNode = tempMinDistanceGoal.curNode;
        while (tempNode != parDeparture) {
            parRoute.Push(tempNode.swissArmyEDirection);
            tempNode = tempNode.link[((int)tempNode.swissArmyEDirection + 4) % 8];
        }
    }

    public node this[int ind0, int ind1] {
        get { 
            return graph[ind0, ind1]; 
        }
    }
}

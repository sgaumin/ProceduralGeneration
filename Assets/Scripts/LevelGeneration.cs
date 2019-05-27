using System;
using System.Collections;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    private const int XMax = 100;
    private const int YMax = 80;

    private int _xMinDivision;
    private int _yMinDivision;
    
    private void Start()
    {
        // Init Parameters
        _xMinDivision = XMax / 4;
        _yMinDivision = YMax / 4;
        
        StartCoroutine(LevelGenerator());
    }

    private static IEnumerator LevelGenerator()
    {
//        1: start with the entire dungeon area (root node of the BSP tree)
        while (true)
        {
            
            
            break;
        }
//        2: divide the area along a horizontal or vertical line

//        3: select one of the two new partition cells

//        4: if this cell is bigger than the minimal acceptable size:
//        5: go to step 2 (using this cell as the area to be divided)
//        6: select the other partition cell, and go to step 4
//        7: for every partition cell:
//        8: create a room within the cell by randomly
//            choosing two points (top left and bottom right) within its boundaries
//        9: starting from the lowest layers, draw corridors to connect
//            rooms corresponding to children of the same parent in the BSP tree
//        10:repeat 9 until the children of the root node are connected
        
        yield break;
    }
}
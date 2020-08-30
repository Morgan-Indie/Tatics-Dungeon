using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FloorTile
    {
        public int index = -1;
        public enum Pos { Single, Upper, Lower, UpperRight, UpperLeft, LowerRight, LowerLeft };
        public Pos pos = Pos.Single;
        //Up, Right, Down, Left
        public bool[] connections = { false, false, false, false };
        public Color color = Color.white;
        public Vector3[] connectionMults =
        {
        new Vector3(0, 1, 1),
        new Vector3(1, 0, 1),
        new Vector3(0, -1, 1),
        new Vector3(-1, 0, 1)
    };

        public int TotalNeighbors() { int i = 0; foreach (bool n in connections) { if (n) { i++; } } return i; }
    }

}

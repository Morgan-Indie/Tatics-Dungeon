using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public static class GridMetrics
    {
        public const float squareSize = 1.5f;
        public const float cornerDist = (squareSize / 2);
        public const int sectionsPerSquare = 1;
        public const float heightIncrement = squareSize;

        public static Vector3[] corners = {
        new Vector3(cornerDist, 0f, cornerDist),
        new Vector3(-cornerDist, 0f, cornerDist),
        new Vector3(-cornerDist, 0f, -cornerDist),
        new Vector3(cornerDist, 0f, -cornerDist)
    };
        public static int totalTriangulateCalls = 0;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    [System.Serializable]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GridMesh : MonoBehaviour
    {
        Mesh gridMesh;
        List<Vector3> vertices;
        List<int> triangles;
        List<Color> colors;
        List<GameObject> lines;
        MeshCollider meshCollider;
        Grid grid;
        public GameObject LinePrefab;
        [HideInInspector]
        public GridCell[,] cells;

        static Color color1 = new Color(1f, 0f, 0f);
        static Color color2 = new Color(0f, 1f, 0f);
        static Color color3 = new Color(0f, 0f, 1f);

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = gridMesh = new Mesh();
            grid = GetComponentInParent<Grid>();
            meshCollider = gameObject.AddComponent<MeshCollider>();
            gridMesh.name = "Grid Mesh";
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color>();
            lines = new List<GameObject>();
            gameObject.tag = "GridMesh";
        }

        public void Triangulate()
        {
            gridMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();
            ClearLines();
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    Trianglulate(cells[x, y]);
                    if (x + 1 < cells.GetLength(0))
                    {
                        DrawHorizontalCellInteraction(cells[x, y], cells[x + 1, y]);
                    }
                    if (y + 1 < cells.GetLength(1))
                    {
                        DrawVerticalCellInteraction(cells[x, y], cells[x, y + 1]);
                    }
                }
            }
        }

        public void ApplyTriangulation()
        {
            gridMesh.vertices = vertices.ToArray();
            gridMesh.colors = colors.ToArray();
            gridMesh.triangles = triangles.ToArray();
            gridMesh.RecalculateNormals();
            meshCollider.sharedMesh = gridMesh;
        }

        void Trianglulate(GridCell cell)
        {
            if (grid == null) { grid = GetComponentInParent<Grid>(); }

            Vector3 center = cell.transform.position;
            center.y += cell.height * GridMetrics.heightIncrement;
            float sectionSize = GridMetrics.squareSize / GridMetrics.sectionsPerSquare;
            Vector3 topLeft = center + GridMetrics.corners[2];
            Vector3[] t1 = {
            topLeft,
            new Vector3(topLeft.x, topLeft.y, topLeft.z + sectionSize),
            new Vector3(topLeft.x + sectionSize, topLeft.y, topLeft.z + sectionSize)
        };
            Vector3[] t2 = {
            new Vector3(topLeft.x + sectionSize, topLeft.y, topLeft.z + sectionSize),
            new Vector3(topLeft.x + sectionSize, topLeft.y, topLeft.z),
            topLeft
        };
            for (int i = 0; i < GridMetrics.sectionsPerSquare; i++)
            {
                for (int n = 0; n < GridMetrics.sectionsPerSquare; n++)
                {
                    //  Vector3[] heightOffsets = NeighborCompensatedHeightCalc(cell, new IntVector2(n, i)); 
                    Vector3 increment = new Vector3(sectionSize * n, 0, sectionSize * i);
                    AddTriangle(t1[0] + increment, t1[1] + increment, t1[2] + increment);
                    AddTriangleColor(cell.GetColor());
                    AddTriangle(t2[0] + increment, t2[1] + increment, t2[2] + increment);
                    AddTriangleColor(cell.GetColor());
                    /*
                    AddTriangle(t1[0] + increment + heightOffsets[0], t1[1] + increment + heightOffsets[1], t1[2] + increment + heightOffsets[2]);
                    AddTriangleColor(cell.GetColor());
                    AddTriangle(t2[0] + increment + heightOffsets[2], t2[1] + increment + heightOffsets[3], t2[2] + increment + heightOffsets[0]);
                    AddTriangleColor(cell.GetColor());
                    */
                }
            }

            //AddTriangle(center + GridMetrics.corners[2], center + GridMetrics.corners[1], center + GridMetrics.corners[0]);
            //AddTriangleColor(cell.GetColor());
            //AddTriangle(center + GridMetrics.corners[0], center + GridMetrics.corners[3], center + GridMetrics.corners[2]);
            //AddTriangleColor(cell.GetColor());
        }

        public void RenderLines()
        {
            ClearLines();
            foreach (GridCell cell in cells)
            {
                Vector3 center = cell.transform.position;
                center.y += cell.height * GridMetrics.heightIncrement;
                Vector3 lineOrigin = center - new Vector3(GridMetrics.squareSize / 2, -0.2f, GridMetrics.squareSize / 2) + transform.position;
                GameObject newLine = Instantiate(LinePrefab, lineOrigin, Quaternion.identity);
                lines.Add(newLine);
                List<Vector3> linePositions = new List<Vector3> {
            Vector3.zero,
            new Vector3(0, 0, GridMetrics.squareSize),
            new Vector3(GridMetrics.squareSize, 0, GridMetrics.squareSize),
            new Vector3(GridMetrics.squareSize, 0, 0),
            Vector3.zero
        };
                newLine.GetComponent<LineRenderer>().positionCount = linePositions.Count;
                newLine.GetComponent<LineRenderer>().SetPositions(linePositions.ToArray());
                newLine.transform.SetParent(transform);
            }
        }

        Vector3[] NeighborCompensatedHeightCalc(GridCell cell, IntVector2 section)
        {
            //   GridCell[] neighbors = grid.GetNeighbors(cell);

            Vector3[] results = new Vector3[4];
            results[0] = SinglePointNeighborCalc(cell, section);
            results[1] = SinglePointNeighborCalc(cell, new IntVector2(section.x, section.y + 1));
            results[2] = SinglePointNeighborCalc(cell, new IntVector2(section.x + 1, section.y + 1));
            results[3] = SinglePointNeighborCalc(cell, new IntVector2(section.x + 1, section.y));

            return results;
        }

        Vector3 SinglePointNeighborCalc(GridCell cell, IntVector2 indices)
        {
            GridCell[,] neighbors = grid.GetNeighbors(cell);

            Vector3 result = Vector3.zero;

            float halfSquareSections = (float)GridMetrics.sectionsPerSquare / 2f;
            float xMult, yMult, diaMult;
            int diaX, diaY;
            float xHeight = 0; float yHeight = 0;
            if (indices.x < halfSquareSections)
            {
                if (neighbors[0, 1] != null)
                {
                    xMult = (halfSquareSections - indices.x) / halfSquareSections;
                    xHeight = neighbors[0, 1].height;
                    diaX = 0;
                }
                else
                {
                    xMult = 1;
                    diaX = -1;
                }
            }
            else
            {
                if (neighbors[2, 1] != null)
                {
                    xMult = (indices.x - halfSquareSections) / halfSquareSections;
                    xHeight = neighbors[2, 1].height;
                    diaX = 2;
                }
                else
                {
                    xMult = 1;
                    diaX = -1;
                }
            }


            if (indices.y < halfSquareSections)
            {
                if (neighbors[1, 0] != null)
                {
                    yMult = (halfSquareSections - indices.y) / halfSquareSections;
                    yHeight = neighbors[1, 0].height;
                    diaY = 0;
                }
                else
                {
                    yMult = 1;
                    diaY = -1;
                }
            }
            else
            {
                if (neighbors[1, 2] != null)
                {
                    yMult = (indices.y - halfSquareSections) / halfSquareSections;
                    yHeight = neighbors[1, 2].height;
                    diaY = 2;
                }
                else
                {
                    yMult = 1;
                    diaY = -1;
                }
            }

            result.y += ((xMult * (xHeight - cell.height)) / 2f) * (1 - yMult);
            result.y += ((yMult * (yHeight - cell.height)) / 2f) * (1 - xMult);
            diaMult = Mathf.Sqrt(xMult * xMult + yMult * yMult) / 1.414f;
            float diaHeight = diaY != -1 && diaX != -1 ? neighbors[diaX, diaY].height : cell.height;
            diaHeight = (diaHeight + yHeight + xHeight) / 3f;
            result.y += (xMult) * (yMult) * (diaHeight - cell.height) * diaMult;

            if (diaX >= 0 && diaY >= 0)
            {

                //  result.y += ((neighbors[diaX, diaY].height - cell.height) * diaMult);
            }
            else diaMult = 1;

            // result.y *= 1 - xMult; result.y *= 1 - yMult; result.y *= 1 - diaMult;

            //result.y += diaMult * Mathf.Sqrt((yHeight - cell.height) * (yHeight - cell.height) + (xHeight - cell.height) * (xHeight - cell.height));

            return result;
        }

        float LinerPointInterpolation(float value, float b)
        {
            return value * b;
        }

        public void DrawHorizontalCellInteraction(GridCell left, GridCell right)
        {
            if (left.height == right.height) { return; }
            float leftHeight = (float)left.height * GridMetrics.heightIncrement;
            float rightHeight = (float)right.height * GridMetrics.heightIncrement;
            Vector3 center = left.transform.position;
            Vector3 v1 = center + GridMetrics.corners[0] + new Vector3(0, leftHeight, 0);
            Vector3 v2 = center + GridMetrics.corners[3] + new Vector3(0, leftHeight, 0);
            center = right.transform.position;
            Vector3 v3 = center + GridMetrics.corners[1] + new Vector3(0, rightHeight, 0);
            Vector3 v4 = center + GridMetrics.corners[2] + new Vector3(0, rightHeight, 0);
            AddTriangle(v3, v2, v1);
            AddTriangle(v3, v4, v2);
            AddTriangleColor(right.GetColor(), left.GetColor(), left.GetColor());
            AddTriangleColor(right.GetColor(), right.GetColor(), left.GetColor());
        }

        public void DrawVerticalCellInteraction(GridCell up, GridCell down)
        {
            if (up.height == down.height) { return; }
            float upHeight = (float)up.height * GridMetrics.heightIncrement;
            float downHeight = (float)down.height * GridMetrics.heightIncrement;
            Vector3 center = up.transform.position;
            Vector3 v1 = center + GridMetrics.corners[0] + new Vector3(0, upHeight, 0);
            Vector3 v2 = center + GridMetrics.corners[1] + new Vector3(0, upHeight, 0);
            center = down.transform.position;
            Vector3 v3 = center + GridMetrics.corners[3] + new Vector3(0, downHeight, 0);
            Vector3 v4 = center + GridMetrics.corners[2] + new Vector3(0, downHeight, 0);
            AddTriangle(v1, v2, v3);
            AddTriangle(v2, v4, v3);
            AddTriangleColor(up.GetColor(), up.GetColor(), down.GetColor());
            AddTriangleColor(up.GetColor(), down.GetColor(), down.GetColor());
        }

        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }

        void AddTriangleColor(Color color)
        {
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
        }

        void AddTriangleColor(Color c1, Color c2, Color c3)
        {
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c3);
        }

        void ClearLines()
        {
            foreach (GameObject line in lines)
            {
                Destroy(line);
            }
            lines.Clear();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    public class EditorScript : MonoBehaviour
    {
        public GridMapAdapter grid;

        [Header("SaveData Storage")]
        public GridMap gridMap;

        [Header("Cells Panel")]
        public CellTemplate[] cellTemplates;
        public GameObject templateButton;
        int currentTemplate = 0;

        [Header("Height Panel")]
        int currentHeight = 0;

        [Header("All Panels")]
        public GameObject[] panels;

        [Header("BrushSize")]
        public Text brushSizeText;
        public Slider brushSizeSlider;
        int brushSize = 1;

        public enum EditMode { Color, Height, Generate };
        //  EditMode currentMode = EditMode.Color;

        bool[] editModes = new bool[2];
        GridCell highlightedCell;

        private void Start()
        {
            editModes[0] = true;
            brushSizeSlider.onValueChanged.AddListener(delegate { ChangeBrushSize(); });
            brushSizeText.text = "Brush Size: " + brushSizeSlider.value.ToString();

            for (int i = 0; i < cellTemplates.Length; i++)
            {
                GameObject button = Instantiate(templateButton);
                button.transform.SetParent(panels[0].transform);
                button.GetComponentInChildren<Text>().text = cellTemplates[i].cellName;
                Button b = button.GetComponent<Button>();
                b.image.color = cellTemplates[i].tint;
                int x = i;
                b.onClick.AddListener(delegate { SetTemplate(x); });
            }

            FormatDisplay();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0)) { HandleInput(); }
        }

        void HandleInput()
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                EditSquare(grid.transform.InverseTransformPoint(hit.point));
            }
        }

        public void EditSquare(Vector3 point)
        {
            GridCell[] cells = grid.GetCellsByPosAndRange(point, brushSize - 1);

            SetCells(cells);
            grid.UpdateSpecificCells(point, brushSize);
        }

        public void FormatDisplay()
        {
            for (int i = 0; i < editModes.Length; i++)
            {
                panels[i].SetActive(editModes[i]);
            }
        }

        public void SetCells(GridCell[] cells)
        {
            foreach (GridCell cell in cells)
            {
                cell.cellTemplate = cellTemplates[currentTemplate];
                if (editModes[0])
                    cell.color = cellTemplates[currentTemplate].tint;
                if (editModes[1])
                    cell.height = currentHeight;
            }
        }

        public void ToggleEditMode(int mode)
        {
            editModes[mode] = !editModes[mode];
            FormatDisplay();
        }

        public void SetTemplate(int t)
        {
            currentTemplate = t;
        }

        public void SetHeight(float h)
        {
            currentHeight = (int)h;
        }

        void ChangeBrushSize()
        {
            brushSizeText.text = "Brush Size: " + brushSizeSlider.value.ToString();
            brushSize = (int)brushSizeSlider.value;
        }

        public void SaveMap()
        {
            gridMap.width = grid.gridMap.width;
            gridMap.height = grid.gridMap.height;
            gridMap.maxMeshSize = grid.gridMap.maxMeshSize;

            GridMesh[,] meshes = grid.GetMeshesArray();
            int x = 0; int y = 0;
            for (int i = 0; i < meshes.GetLength(0); i++) { x += meshes[i, 0].cells.GetLength(0); }
            for (int i = 0; i < meshes.GetLength(1); i++) { y += meshes[0, i].cells.GetLength(1); }
            GridCell[,] cells = new GridCell[x, y];

            for (int ys = 0; ys < meshes.GetLength(1); ys++)
            {
                for (int xs = 0; xs < meshes.GetLength(0); xs++)
                {
                    for (int yx = 0; yx < meshes[xs, ys].cells.GetLength(1); yx++)
                    {
                        for (int xx = 0; xx < meshes[xs, ys].cells.GetLength(0); xx++)
                        {
                            cells[xx + xs * grid.gridMap.maxMeshSize, yx + ys * grid.gridMap.maxMeshSize] = meshes[xs, ys].cells[xx, yx];
                        }
                    }
                }
            }
            gridMap.ProcessCells(cells);
        }
    }

}

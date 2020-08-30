using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FloorManager : MonoBehaviour
    {
        public static FloorManager Instance { get; private set; }

        public FloorMatrix floorMatrix;
        public FloorMapUI floorMapUI;

        bool inited = false;

        public GridMap[] floor1Maps;

        public IntVector2 currentRoom = new IntVector2(5, 5);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Update()
        {
            if (!inited)
            {
                if (floorMatrix != null && floorMapUI != null)
                {
                    inited = true;
                    FormatMapUI();
                }
            }
        }

        public void SetMatrix(FloorMatrix f) { floorMatrix = f; }
        public void SetMapUI(FloorMapUI f) { floorMapUI = f; }
        public GridMap GetCurrentMap() { return floor1Maps[floorMatrix.GetMatrix()[currentRoom.x, currentRoom.y].index]; }
        public FloorTile GetCurrentFloorTile() { return floorMatrix.GetMatrix()[currentRoom.x, currentRoom.y]; }
        public void FormatMapUI() { floorMapUI.ReformatMap(floorMatrix.GetMatrix()); }
        public void MovePlayerMarker() { floorMapUI.DrawPlayerMarker(floorMatrix.GetMatrix()); }

        public int GetRandomFloor1MapIndex()
        {
            return Random.Range(0, floor1Maps.Length);
        }

        public void AddToRoomIndex(int x, int y)
        {
            currentRoom.x += x; currentRoom.y += y;
        }
    }

}

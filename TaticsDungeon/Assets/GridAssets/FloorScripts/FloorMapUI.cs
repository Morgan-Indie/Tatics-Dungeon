using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class FloorMapUI : MonoBehaviour
    {
        public GameObject roomGraphic;
        public GameObject roomJointGraphic;

        List<GameObject> currentUIObjects;

        float roomSpacing;
        float roomJointSpacing;

        public Color[] colors;

        public GameObject playerMarkerPrefab;
        GameObject playerMarker;

        private void Start()
        {
            FloorManager.Instance.SetMapUI(this);

            currentUIObjects = new List<GameObject>();
            roomSpacing = roomGraphic.GetComponent<RectTransform>().sizeDelta.x / 2f;
            roomJointSpacing = roomJointGraphic.GetComponent<RectTransform>().sizeDelta.x / 2f;
        }

        public void ReformatMap(FloorTile[,] matrix)
        {
            foreach (GameObject uiOb in currentUIObjects) { Destroy(uiOb); }
            float offsetIncrement = 2 * roomSpacing + 2 * roomJointSpacing;

            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                for (int x = 1; x < matrix.GetLength(0); x++)
                {
                    if (matrix[x, y] != null)
                    {
                        Vector3 pos = new Vector3(-offsetIncrement * (matrix.GetLength(0) - x), -offsetIncrement * (matrix.GetLength(1) - y), 0);
                        AddRoom(pos, matrix[x, y], x, y);
                    }
                }
            }
            DrawPlayerMarker(matrix);
        }

        public void AddRoom(Vector3 pos, FloorTile tile, int x, int y)
        {
            GameObject ob;
            ob = Instantiate(roomGraphic, transform.position, transform.rotation);
            ob.transform.SetParent(transform);
            ob.GetComponent<RectTransform>().localPosition = pos;
            currentUIObjects.Add(ob);
            ob.GetComponent<Image>().color = tile.color;
            ob.GetComponentInChildren<Text>().text = x.ToString() + "," + y.ToString() + "\n" + tile.index;
            float offset = roomSpacing + roomJointSpacing;
            Vector3 offsetVector = new Vector3(offset, offset, 0);

            for (int i = 0; i < tile.connections.Length; i++)
            {
                if (tile.connections[i])
                {
                    ob = Instantiate(roomJointGraphic, transform.position, transform.rotation);
                    ob.transform.SetParent(transform);
                    ob.GetComponent<RectTransform>().localPosition = pos + Vector3.Scale(offsetVector, tile.connectionMults[i]);
                }
            }
        }

        public void DrawPlayerMarker(FloorTile[,] matrix)
        {
            if (playerMarker == null)
                playerMarker = Instantiate(playerMarkerPrefab);
            float offsetIncrement = 2 * roomSpacing + 2 * roomJointSpacing;
            IntVector2 i = FloorManager.Instance.currentRoom;
            Vector3 pos = new Vector3(-offsetIncrement * (matrix.GetLength(0) - i.x), -offsetIncrement * (matrix.GetLength(1) - i.y), 0);
            playerMarker.transform.SetParent(transform);
            playerMarker.transform.localPosition = pos;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TownController : MonoBehaviour
    {
        public static TownController Instance { get; private set; }

        public LayerMask buildingLayer;
        public TownCameraHandler townCameraHandler;

        private GameObject highlightedBuilding;
        private Camera mainCamera;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (townCameraHandler.IsViewingTown() && !townCameraHandler.traveling)
            {
                BuildingHighlightRaycast();
            }

            if (Input.GetMouseButtonDown(0) && highlightedBuilding != null)
            {
                townCameraHandler.Transition(highlightedBuilding.GetComponent<BuildingAbstract>().cameraTransition);
            }

            if (Input.GetKeyDown("escape"))
            {
                townCameraHandler.TransitionToTownView();
            }
        }

        private void BuildingHighlightRaycast()
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayer))
            {
                if (hit.collider.gameObject != highlightedBuilding)
                {
                    hit.collider.gameObject.GetComponent<BuildingAbstract>().Highlight();
                    if (highlightedBuilding != null)
                        highlightedBuilding.GetComponent<BuildingAbstract>().RemoveHighlight();
                    highlightedBuilding = hit.collider.gameObject;
                }
            }
            else
            {
                if (highlightedBuilding != null)
                {
                    highlightedBuilding.GetComponent<BuildingAbstract>().RemoveHighlight();
                    highlightedBuilding = null;
                }
            }
        }
    }
}
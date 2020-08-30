using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace PrototypeGame
{
    public class EquipmentButtonMethod : UIButtonScript
    {
        [Header("Required")]
        public GameObject equipmentMenu, inventoryMenu;
        public GameObject equipmentTabButton, inventoryTabButton;
        public Camera UIcam;
        Light UIcamLight; 

        public void Start()
        {
            equipmentMenu.SetActive(false);
            UIcam = Camera.main.GetComponentsInChildren<Camera>()[1];
            UIcamLight = UIcam.GetComponentInChildren<Light>();
            UIcam.enabled = false;
            UIcamLight.enabled = false;
        }

        public void SwithToEquipmentTab()
        {
            inventoryMenu.SetActive(false);
            equipmentMenu.SetActive(true);
            UIcam.enabled = true;
            UIcamLight.enabled = true;
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // set new selected object
            EventSystem.current.SetSelectedGameObject(inventoryTabButton);
        }
    }
}


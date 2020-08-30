using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace PrototypeGame
{
    public class InventoryButtonMethods : UIButtonScript
    {
        [Header("Required")]
        public GameObject equipmentMenu,inventoryMenu;
        public GameObject equipmentTabButton,inventoryTabButton;
        public Camera UIcam;
        Light UIcamLight;

        public void Start()
        {
            UIcam = Camera.main.GetComponentsInChildren<Camera>()[1];
            UIcamLight = UIcam.GetComponentInChildren<Light>();

        }

        public void SwithToInventoryTab()
        {
            equipmentMenu.SetActive(false);
            inventoryMenu.SetActive(true);
            UIcam.enabled = false;
            UIcamLight.enabled = false;            
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // set new selected object
            EventSystem.current.SetSelectedGameObject(equipmentTabButton);
        }        
    }
}


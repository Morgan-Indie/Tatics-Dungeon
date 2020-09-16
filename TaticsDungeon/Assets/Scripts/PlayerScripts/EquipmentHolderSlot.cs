using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EquipmentHolderSlot : MonoBehaviour
    {
        [Header("Required")]
        public Transform parentOverride;
        [Header("Not Required")]
        public GameObject currentModel;
        public AnimationHandler animationHandler;

        public SlotType slotType;

        public void Start()
        {
            animationHandler = GetComponentInParent<AnimationHandler>();
        }

        public void UnloadEquipment()
        {
            if (currentModel != null)
                currentModel.SetActive(false);
            animationHandler.animator.SetBool("Equiped", false);
        }

        public void UnloadEquipmentAndDestroy()
        {
            if (currentModel != null)
                Destroy(currentModel);
            animationHandler.animator.SetBool("Equiped", false);
        }
        
        public void LoadEquipmentModel(EquipableItem item)
        {
            if (item == null)
                return;

            GameObject equipmentModel = Instantiate(item.modelPrefab) as GameObject;
            if (equipmentModel != null)
            {
                if (parentOverride!=null)
                    equipmentModel.transform.parent = parentOverride;
                else
                    equipmentModel.transform.parent = transform;

                foreach (Transform t in equipmentModel.GetComponentsInChildren<Transform>())
                    t.gameObject.layer=equipmentModel.transform.parent.gameObject.layer;

                equipmentModel.transform.localPosition = Vector3.zero;
                equipmentModel.transform.localRotation = Quaternion.identity;
                equipmentModel.transform.localScale = Vector3.one;

                if (animationHandler.tag == "Player")
                    equipmentModel.transform.GetChild(0).GetChild(0).localScale = Vector3.one*100f;

            }
            currentModel = equipmentModel;
            animationHandler.animator.SetBool("Equiped", true);
        }
    }
}

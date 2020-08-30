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

        public SlotType slotType;

        public void UnloadEquipment()
        {
            if (currentModel != null)
                currentModel.SetActive(false);
        }

        public void UnloadEquipmentAndDestroy()
        {
            if (currentModel != null)
                Destroy(currentModel);
        }
        
        public void LoadEquipmentModel(dynamic item)
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
            }
            currentModel = equipmentModel;
        }
    }
}

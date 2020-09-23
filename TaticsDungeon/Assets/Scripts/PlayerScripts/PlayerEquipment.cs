using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum meshLocation
    {
        HeadFull, HeadPartial, LeftHand, RightHand, Chest, LeftKnee, RightKnee,
        LeftElbow, RightElbow, HipAttachment, Back, Torso,
        LeftShoulder, RightShoulder,UpperRightArm, LowerRightArm,
        UpperLeftArm, LowerLeftArm, LeftLeg,RightLeg,Hips,
    }

    public enum Gender
    {
        Male,Female
    }

    public class PlayerEquipment : MonoBehaviour
    {
        public Gender gender;
        EquipmentSlotManager equipmentSlotManager;
        public GameObject characterEquipment;
        public Dictionary<string, Mesh> baseMeshDict = new Dictionary<string, Mesh>();
        public Dictionary<string, SkinnedMeshRenderer> equipmentsMeshDict = new Dictionary<string, SkinnedMeshRenderer>();
        public GameObject facialFeatures;

        private void Awake()
        {
            equipmentSlotManager = GetComponentInChildren<EquipmentSlotManager>();
            foreach (InventorySlot itemSlot in equipmentSlotManager.equipedItems)
            {
                if (itemSlot.item != null)
                    equipmentSlotManager.LoadEquipmentOnSlot(itemSlot.item, itemSlot.item.slotType);
            }
            foreach (SkinnedMeshRenderer skinRenderer in characterEquipment.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                baseMeshDict.Add(skinRenderer.gameObject.name, skinRenderer.sharedMesh);
                equipmentsMeshDict.Add(skinRenderer.gameObject.name, skinRenderer);
            }
        }

        public void LoadEquipmentMesh(Mesh mesh, meshLocation loc)
        {
            if (loc == meshLocation.HeadFull)
            {
                facialFeatures.SetActive(false);
                characterEquipment.transform.Find("HeadPartial").GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }

            else if (loc == meshLocation.HeadPartial)
            {
                facialFeatures.SetActive(true);
                characterEquipment.transform.Find("HeadFull").GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }
            characterEquipment.transform.Find(loc.ToString()).GetComponent<SkinnedMeshRenderer>().sharedMesh=mesh;
        }

        public void ResetToBaseEquipment(meshLocation loc)
        {
            if (loc == meshLocation.HeadFull)
                facialFeatures.SetActive(true);
            equipmentsMeshDict[loc.ToString()].sharedMesh = baseMeshDict[loc.ToString()];
        }
    }
}


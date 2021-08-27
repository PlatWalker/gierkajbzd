using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Created by Kumdzio
/// Class responsible for changing in-game 3D equipment
/// </summary>
namespace jbzdy.Items
{
    public class ItemEquipper : MonoBehaviour
    {
        private Dictionary<int, Transform> _playerBonesDictionary;
        private Transform[] _bonesTransforms = new Transform[61];

        [SerializeField] private List<GameObject> headBodyParts;
        [SerializeField] private List<GameObject> chestBodyParts;
        [SerializeField] private List<GameObject> legsBodyParts;
        [SerializeField] private List<GameObject> bootsBodyParts;
        private Transform _boots;
        private Transform _legs;
        private Transform _chest;
        private Transform _helmet;
        private Transform _offHand;
        private Transform _mainHand;

        [SerializeField] private Transform WeaponPlaceholder;
        [SerializeField] private Transform OffHandPlaceholder;



        private void Start()
        {
            _playerBonesDictionary = new Dictionary<int, Transform>();
            traverseHierarchy(gameObject.transform);
        }

        public void EquipWeaponOrTrinket(Item item)
        {
            switch (item.itemType)
            {
                case Enums.ItemTypes.Weapon:
                    _mainHand = spawnHandItem(item,WeaponPlaceholder);
                    break;
                case Enums.ItemTypes.Trinket:
                    _offHand = spawnHandItem(item,OffHandPlaceholder);
                    break;
            }
        }
        public void UnequipWeaponOrTrinket(Item item)
        {
            switch (item.itemType)
            {
                case Enums.ItemTypes.Weapon:
                    if (_mainHand) Destroy(_mainHand.gameObject);
                    break;
                case Enums.ItemTypes.Trinket:
                    if (_offHand) Destroy(_offHand.gameObject);
                    break;
            }
        }
        public void EquipArmor(ArmorItem item)
        {
            switch (item.armorType)
            {
                case Enums.ArmorTypes.Head:
                    _helmet = equip(item);
                    headBodyParts.ForEach(disactive);
                    break;
                case Enums.ArmorTypes.Chest:
                    _chest = equip(item);
                    chestBodyParts.ForEach(disactive);
                    break;
                case Enums.ArmorTypes.Boots:
                    _boots = equip(item);
                    bootsBodyParts.ForEach(disactive);
                    break;
                case Enums.ArmorTypes.Legs:
                    _legs = equip(item);
                    legsBodyParts.ForEach(disactive);
                    break;
            }
        }
        public void UnequipArmor(ArmorItem item)
        {
            switch (item.armorType)
            {
                case Enums.ArmorTypes.Head:
                    if (_helmet)
                    {
                        Destroy(_helmet.gameObject);
                        headBodyParts.ForEach(activate);
                    }
                        break;
                case Enums.ArmorTypes.Chest:
                    if (_chest)
                    {
                        Destroy(_chest.gameObject);
                        chestBodyParts.ForEach(activate);
                    }
                    break;
                case Enums.ArmorTypes.Boots:
                    if (_boots)
                    {
                        Destroy(_boots.gameObject);
                        bootsBodyParts.ForEach(activate);
                    }
                    break;
                case Enums.ArmorTypes.Legs:
                    if (_legs)
                    {
                        Destroy(_legs.gameObject);
                        legsBodyParts.ForEach(activate);
                    }
                    break;
            }
            
        }

        private Transform equip(Item item)
        {
            return addLimb(item.gameObject);
        }

       

        private Transform addLimb(GameObject item)
        {
            Transform limb = ProcessEquippingItem(item.GetComponentInChildren<SkinnedMeshRenderer>());
            limb.SetParent(gameObject.transform);
            return limb;
        }

        private Transform ProcessEquippingItem(SkinnedMeshRenderer renderer)
        {
            Transform bonedObject = new GameObject().transform;

            SkinnedMeshRenderer meshRenderer = bonedObject.gameObject.AddComponent<SkinnedMeshRenderer>();

            Transform[] bones = renderer.bones;

            for (int i = 0; i < bones.Length; i++)
            {
                _bonesTransforms[i] = _playerBonesDictionary[bones[i].name.GetHashCode()];
            }
            meshRenderer.bones = _bonesTransforms;
            meshRenderer.sharedMesh = renderer.sharedMesh;
            meshRenderer.materials = renderer.sharedMaterials;
            return bonedObject;
        }

        private void traverseHierarchy(Transform transform)
        {
            foreach (Transform child in transform)
            {
                _playerBonesDictionary.Add(child.name.GetHashCode(), child);
                traverseHierarchy(child);
            }
        }
        private void disactive(GameObject g)
        {
            g.SetActive(false);
        }
        private void activate(GameObject g)
        {
            g.SetActive(true);
        }

        private Transform spawnHandItem(Item item,Transform parent)
        {
            GameObject spawned = (GameObject)Instantiate(item.gameObject,Vector3.zero,Quaternion.identity, parent);
            spawned.GetComponent<Actions.Interaction.ItemPickup>().enabled = false;
            spawned.SetActive(true);
            spawned.transform.localPosition = Vector3.zero;
            spawned.transform.localRotation = Quaternion.identity;
            return spawned.transform;
        }
    }
}
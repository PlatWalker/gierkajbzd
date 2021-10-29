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

        [SerializeField] private List<GameObject> _headBodyParts;
        [SerializeField] private List<GameObject> _chestBodyParts;
        [SerializeField] private List<GameObject> _legsBodyParts;
        [SerializeField] private List<GameObject> _bootsBodyParts;

        private Transform _boots;
        private Transform _legs;
        private Transform _chest;
        private Transform _helmet;
        private Transform _offHand;
        private Transform _mainHand;

        [SerializeField] private Transform _weaponPlaceholder;
        [SerializeField] private Transform _offHandPlaceholder;



        private void Start()
        {
            _playerBonesDictionary = new Dictionary<int, Transform>();
            TraverseHierarchy(gameObject.transform);
        }

        public void EquipWeaponOrTrinket(Item item)
        {
            switch (item.itemType)
            {
                case Enums.ItemTypes.Weapon:
                    _mainHand = SpawnHandItem(item,_weaponPlaceholder);
                    break;
                case Enums.ItemTypes.Trinket:
                    _offHand = SpawnHandItem(item,_offHandPlaceholder);
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
                    _helmet = Equip(item);
                    _headBodyParts.ForEach(Disactive);
                    break;
                case Enums.ArmorTypes.Chest:
                    _chest = Equip(item);
                    _chestBodyParts.ForEach(Disactive);
                    break;
                case Enums.ArmorTypes.Boots:
                    _boots = Equip(item);
                    _bootsBodyParts.ForEach(Disactive);
                    break;
                case Enums.ArmorTypes.Legs:
                    _legs = Equip(item);
                    _legsBodyParts.ForEach(Disactive);
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
                        _headBodyParts.ForEach(Activate);
                    }
                        break;
                case Enums.ArmorTypes.Chest:
                    if (_chest)
                    {
                        Destroy(_chest.gameObject);
                        _chestBodyParts.ForEach(Activate);
                    }
                    break;
                case Enums.ArmorTypes.Boots:
                    if (_boots)
                    {
                        Destroy(_boots.gameObject);
                        _bootsBodyParts.ForEach(Activate);
                    }
                    break;
                case Enums.ArmorTypes.Legs:
                    if (_legs)
                    {
                        Destroy(_legs.gameObject);
                        _legsBodyParts.ForEach(Activate);
                    }
                    break;
            }
            
        }

        private Transform Equip(Item item)
        {
            return AddLimb(item.gameObject);
        }

        private Transform AddLimb(GameObject item)
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

        private void TraverseHierarchy(Transform transform)
        {
            foreach (Transform child in transform)
            {
                _playerBonesDictionary.Add(child.name.GetHashCode(), child);
                TraverseHierarchy(child);
            }
        }

        private void Disactive(GameObject g)
        {
            g.SetActive(false);
        }

        private void Activate(GameObject g)
        {
            g.SetActive(true);
        }

        private Transform SpawnHandItem(Item item,Transform parent)
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
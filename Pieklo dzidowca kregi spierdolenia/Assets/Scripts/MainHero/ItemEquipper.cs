using System;
using System.Collections.Generic;
using jbzd.Items;
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

        public void EquipItem(ItemSO item)
        {
            switch (item.ItemType)
            {
                case ItemTypes.HeadArmor:
                    _helmet = Equip(item.ItemPrefab);
                    _headBodyParts.ForEach(Inactive);
                    break;
                case ItemTypes.ChestArmor:
                    _chest = Equip(item.ItemPrefab);
                    _chestBodyParts.ForEach(Inactive);
                    break;
                case ItemTypes.LegArmor:
                    _legs = Equip(item.ItemPrefab);
                    _legsBodyParts.ForEach(Inactive);
                    break;
                case ItemTypes.BootsArmor:
                    _boots = Equip(item.ItemPrefab);
                    _bootsBodyParts.ForEach(Inactive);
                    break;
                case ItemTypes.Weapon:
                    _mainHand = SpawnHandItem(item.ItemPrefab,_weaponPlaceholder);
                    break;
                case ItemTypes.OffHandItem:
                    _offHand = SpawnHandItem(item.ItemPrefab,_offHandPlaceholder);
                    break;
                case ItemTypes.Consumable:
                    //nie zakladamy tego na postac na ten moment.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void UnEquipItem(ItemSO item)
        {
            switch (item.ItemType)
            {
                case ItemTypes.HeadArmor:
                    if (_helmet)
                    {
                        Destroy(_helmet.gameObject);
                        _headBodyParts.ForEach(Activate);
                    }
                    break;
                case ItemTypes.ChestArmor:
                    if (_chest)
                    {
                        Destroy(_chest.gameObject);
                        _chestBodyParts.ForEach(Activate);
                    }
                    break;
                case ItemTypes.LegArmor:
                    if (_legs)
                    {
                        Destroy(_legs.gameObject);
                        _legsBodyParts.ForEach(Activate);
                    }
                    break;
                case ItemTypes.BootsArmor:
                    if (_boots)
                    {
                        Destroy(_boots.gameObject);
                        _bootsBodyParts.ForEach(Activate);
                    }
                    break;
                case ItemTypes.Weapon:
                    if (_mainHand) Destroy(_mainHand.gameObject);
                    break;
                case ItemTypes.OffHandItem:
                    if (_offHand) Destroy(_offHand.gameObject);
                    break;
                case ItemTypes.Consumable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Transform Equip(GameObject item)
        {
            return AddLimb(item);
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

        private void Inactive(GameObject g) => g.SetActive(false);

        private void Activate(GameObject g) => g.SetActive(true);

        private Transform SpawnHandItem(GameObject item,Transform parent)
        {
            var spawned = Instantiate(item, parent, false);
            spawned.GetComponent<Actions.Interaction.ItemPickup>().enabled = false;
            spawned.SetActive(true);
            spawned.transform.localPosition = Vector3.zero;
            spawned.transform.localRotation = Quaternion.identity;
            return spawned.transform;
        }
    }
}
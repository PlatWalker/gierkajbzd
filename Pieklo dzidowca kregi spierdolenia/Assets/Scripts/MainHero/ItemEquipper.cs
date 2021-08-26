using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Created by Kumdzio
/// Class responsible for changing in-game 3D equipment
/// </summary>
namespace jbzdy.Items.Equipable
{
    public class ItemEquipper : MonoBehaviour
    {
        private Dictionary<int, Transform> _playerBonesDictionary;
        private Transform[] _bonesTransforms = new Transform[61];

        [SerializeField] private GameObject toEquip;
        private Transform _boots;
        private Transform _chest;
        private Transform _hemlet;
        private Transform _offHand;
        private Transform _mainHand;

        private void Start()
        {
            _playerBonesDictionary = new Dictionary<int, Transform>();
            traverseHierarchy(gameObject.transform);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Equip();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Unequip();
            }
        }

        public void Equip(/*tutaj coś co sharashino przekaże*/)
        {
            _chest = addLimb(toEquip);
        }

        public void Unequip()
        {
            if(_chest) Destroy(_chest.gameObject);
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
    }
}
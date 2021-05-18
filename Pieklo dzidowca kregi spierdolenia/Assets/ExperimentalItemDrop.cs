using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jbzdy.Items.Drop
{
    public class ExperimentalItemDrop : MonoBehaviour
    {
        [SerializeField] private ItemDropBase xd;
        
        public ItemDropBase Xd
        {
            get => xd; set => xd = value;
        }
    }
}
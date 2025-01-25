using System;
using UnityEngine;

namespace jbzd.Common
{
    public class MaintainWorldPosition: MonoBehaviour
    {
        public void Awake()
        {
            transform.SetParent(transform.parent, false);
        }
    }
}
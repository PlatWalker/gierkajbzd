using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Common
{
    public static class FreezeTime
    {
        public static void Freeze()
        {
            Time.timeScale = 0;
        }
        public static void Unfreeze()
        {
            Time.timeScale = 1;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jbzdy.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerObject;

        #region Singleton

        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        #endregion

        public GameObject PlayerObject1 { get => playerObject; set => playerObject = value; }
    }
}

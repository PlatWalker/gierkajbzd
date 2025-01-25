using UnityEngine;

namespace jbzd.MinorSystems
{
    public class ConanInput : MonoBehaviour
    {
        public GameObject ConanImage;
        private string _conanProgress = "";

        void Start()
        {
            if (ConanImage == null) Debug.LogError("ConanImage is null");
        }
        
        void Update()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        string keyString = key.ToString().ToLower();
                        if (keyString.Length == 1 && char.IsLetter(keyString[0]))
                        {
                            char pressedChar = keyString[0];

                            _conanProgress += pressedChar;

                            if ("conan".StartsWith(_conanProgress))
                            {
                                if (_conanProgress == "conan")
                                {
                                    Debug.Log("Conan wpisany");
                                    ConanImage.SetActive(!ConanImage.activeSelf);
                                    _conanProgress = "";
                                }
                            }
                            else
                            {
                                _conanProgress = "";
                            }
                        }
                    }
                }
            }
        }
    }
}

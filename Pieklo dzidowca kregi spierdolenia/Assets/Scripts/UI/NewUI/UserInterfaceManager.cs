using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using jbzd.Common;
using UnityEngine;

namespace jbzd.UI.NewUI
{
    public class UserInterfaceManager : MonoBehaviour
    {
        #region UI elements references

        [SerializeField] private UserInterfaceController hudUIController;

        [SerializeField] private UserInterfaceController questUIController;

        [SerializeField] private UserInterfaceController playerMenuUIController;
        
        #endregion
        
        [SerializeReference]
        [JbzdReadOnly]
        private List<UserInterfaceController> elements;
        
        public void Awake()
        {
            FillListWithVariables();
            CheckIfAllChildrenAreAdded();
        }

        private void CheckIfAllChildrenAreAdded()
        {
            if (elements.Count != transform.childCount)
            {
                Debug.LogError("Nie wszystkie referencje sa dodane do UserInterfaceController'a!");
            }
        }

        private void FillListWithVariables()
        {
            const BindingFlags bindingFlags = BindingFlags.Instance |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Public;
            var fieldValues = GetType()
                .GetFields(bindingFlags)
                .Where(field => field.FieldType == typeof(UserInterfaceController))
                .Select(field => field.GetValue(this))
                .ToList();

            if (fieldValues.Any(field => field is null))
            {
                Debug.LogError("UserInterfaceController nie posiada wszystkich referencji do ui elementow!");
            }

            foreach (UserInterfaceController fieldValue in fieldValues)
            {
                elements.Add(fieldValue);
            }
        }
        
        public T GetUIController<T>() where T : UserInterfaceController
        {
            var uiController = (T) elements.Find(ui => ui.GetType() == typeof(T));
            
            if (uiController != null) return uiController;

            Debug.LogWarning("There is no such user interface class!");
            return default;
        }
    }
}
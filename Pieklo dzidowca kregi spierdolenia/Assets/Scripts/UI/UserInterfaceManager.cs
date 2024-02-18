using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using jbzd.Common;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace jbzd.UI
{
    public class UserInterfaceManager : MonoBehaviour
    {
        #region UI elements references

        [UsedImplicitly][SerializeField] private UserInterfaceController hudUIController;

        [UsedImplicitly][SerializeField] private UserInterfaceController questUIController;

        [UsedImplicitly][SerializeField] private UserInterfaceController inGameMenuUIController;
        
        [UsedImplicitly][SerializeField] private UserInterfaceController dialogueUIController;
        [UsedImplicitly][SerializeField] private UserInterfaceController inventoryUIController;
        
        [UsedImplicitly][SerializeField] private UserInterfaceController escapeMenuController;
        
        #endregion
        
        [SerializeReference]
        [JbzdReadOnly]
        private List<UserInterfaceController> userInterfaceControllers;

        private UserInterfaceInput _input;
        
        [Inject]
        public void Construct(InputManager inputManager)
        {
            _input = inputManager.GetInput<UserInterfaceInput>();
        }

        private void Awake()
        {
            FillListWithVariables();
            CheckIfAllChildrenAreAdded();
            InitializeUserInterfaceControllers();
        }

        private void InitializeUserInterfaceControllers()
        {
            foreach (var userInterfaceController in userInterfaceControllers)
            {
                userInterfaceController.ConnectInputToHandler(_input);
                userInterfaceController.gameObject.SetActive(userInterfaceController.InitialActivationState());
            }
        }

        private void CheckIfAllChildrenAreAdded()
        {
            if (userInterfaceControllers.Count != transform.childCount - 1) // -1 bo event system dodatkowo jest
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

            foreach (var fieldValue in fieldValues)
            {
                userInterfaceControllers.Add(fieldValue as UserInterfaceController);
            }
        }
        
        /// <summary>
        /// Method that give access to UI controllers. 
        /// </summary>
        /// <typeparam name="T">Required UI controller</typeparam>
        /// <returns>UI controller that was required</returns>
        public T GetUIController<T>() where T : UserInterfaceController
        {
            var uiController = (T) userInterfaceControllers.Find(ui => ui.GetType() == typeof(T));
            
            if (uiController != null) return uiController;

            Debug.LogWarning("There is no such user interface class!");
            return default;
        }
    }
}
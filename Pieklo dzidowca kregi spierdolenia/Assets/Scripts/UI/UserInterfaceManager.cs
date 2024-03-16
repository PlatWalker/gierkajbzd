using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using jbzd.Common;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using jbzd.UI.Dialogues;
using jbzd.UI.EscapeMenu;
using jbzd.UI.Hud;
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
        
        [UsedImplicitly][SerializeField] private UserInterfaceController escapeMenuController;
        
        [UsedImplicitly][SerializeField] private UserInterfaceController inventoryUIController;
        
        #endregion
        
        [SerializeReference]
        [JbzdReadOnly]
        private List<UserInterfaceController> userInterfaceControllers;

        private UserInterfaceInput _input;
        private PlayerManager _playerManager;
        
        [Inject]
        public void Construct(InputManager inputManager, PlayerManager playerManager)
        {
            _input = inputManager.GetInput<UserInterfaceInput>();
            _playerManager = playerManager;
        }

        private void Awake()
        {
            FillListWithVariables();
            CheckIfAllChildrenAreAdded();
            InitializeUserInterfaceControllers();
            SetupUserInterfaceClickHandler();
        }

        private void InitializeUserInterfaceControllers()
        {
            foreach (var userInterfaceController in userInterfaceControllers)
            {
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

        #region Logic of input handling

        private JbzdInput.ClickNotify _onEscapeClickHandler;
        private JbzdInput.ClickNotify _onInventoryOpenedHandler;
        private JbzdInput.ClickNotify _onInGameMenuOpenedHandler;
        private JbzdInput.ClickNotify _onQuestLogOpenedHandler;
        
        private void SetupUserInterfaceClickHandler()
        {
            _onEscapeClickHandler = () =>
            {
                if (inventoryUIController.gameObject.activeSelf || questUIController.gameObject.activeSelf)
                {
                    inventoryUIController.gameObject.SetActive(false);
                    questUIController.gameObject.SetActive(false);
                }
                else
                {
                    escapeMenuController.gameObject.SetActive(!escapeMenuController.gameObject.activeSelf);
                    UserInterfaceClickHandler(escapeMenuController);
                }
            };
            
            _onInventoryOpenedHandler = () =>
            {
                inventoryUIController.gameObject.SetActive(!inventoryUIController.gameObject.activeSelf);
                UserInterfaceClickHandler(inventoryUIController);
            };

            _onInGameMenuOpenedHandler = () =>
            {
                inGameMenuUIController.gameObject.SetActive(!inGameMenuUIController.gameObject.activeSelf);
                UserInterfaceClickHandler(inGameMenuUIController);
            };
            
            _onQuestLogOpenedHandler = () =>
            {
                questUIController.gameObject.SetActive(!questUIController.gameObject.activeSelf);
                UserInterfaceClickHandler(questUIController);
            };
            
            _input.OnEscapeClick += _onEscapeClickHandler;
            _input.OnInventoryOpened += _onInventoryOpenedHandler;
            _input.OnInGameMenuOpened += _onInGameMenuOpenedHandler;
            _input.OnQuestLogOpened += _onQuestLogOpenedHandler;
        }

        private void UserInterfaceClickHandler(UserInterfaceController clickedUserInterfaceController)
        {
            var activeUserInterfaces = userInterfaceControllers.Where(controller => controller.gameObject.activeSelf).ToList();
            
            if (activeUserInterfaces.Count(activeUserInterface => activeUserInterface is not HudUIController) != 0)
            { 
                FreezeTime.Freeze();
            }
            else
            {
                FreezeTime.Unfreeze();
            }

            if (clickedUserInterfaceController.gameObject.activeSelf)
            {
                foreach (var uui in activeUserInterfaces.Where(x => 
                             x.GetType() != clickedUserInterfaceController.GetType() &&
                             x is not HudUIController or DialogueUIController))
                {
                    uui.gameObject.SetActive(false);
                }
            }
            
        }

        public void OnDestroy()
        {
            _input.OnEscapeClick -= _onEscapeClickHandler;
            _input.OnInventoryOpened -= _onInventoryOpenedHandler;
            _input.OnInGameMenuOpened -= _onInGameMenuOpenedHandler;
            _input.OnQuestLogOpened -= _onQuestLogOpenedHandler;
        }

        #endregion
    }
}
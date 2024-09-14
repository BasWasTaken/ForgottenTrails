using Ink.Runtime;
using UnityEngine;
using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCInventoryMenuState : SCInGameMenuState
        {

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.InGameMenu.pages.inventoryPage.gameObject.SetActive(true);
                Controller.InterfaceBroker.InGameMenu.labels.inventoryPageLabel.color = Color.black;
                Controller.InterfaceBroker.inventory.FetchItems(Controller.Story.state.variablesState["Inventory"] as InkList);
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                foreach (var choice in Controller.Story.currentChoices)
                {
                    if (choice.text.Contains("{UNITY:CloseInventory}"))
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);
                        Controller.Story.ContinueMaximally();
                        break;
                    }
                }
                Controller.InterfaceBroker.InGameMenu.pages.inventoryPage.gameObject.SetActive(false);
                Controller.InterfaceBroker.InGameMenu.labels.inventoryPageLabel.color = Color.white;
            }

            #endregion Public Methods

        }

        #endregion Classes
    }
}
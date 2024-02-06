using Ink.Runtime;
using UnityEngine;
using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCInventoryState : SCBookMenuState
        {
            // Inspector Properties

            // Public Properties

            // Private Properties

            // Public Methods

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.book.pages.inventoryPage.SetAsLastSibling();
                Controller.InterfaceBroker.book.markers.inventoryMark.color = Color.clear;
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
                Controller.InterfaceBroker.book.markers.inventoryMark.color = Color.white;
            }

            #endregion Public Methods

            // Private Methods
        }

        #endregion Classes
    }
}
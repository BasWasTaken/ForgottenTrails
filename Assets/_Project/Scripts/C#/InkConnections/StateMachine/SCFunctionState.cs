using VVGames.Common;
using System;
using System.Collections.Generic;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public partial class TextProduction
        {
            #region Fields

            internal Queue<Action> PendingFunctions = new();

            #endregion Fields

            #region Classes

            public class SCFunctionState : SCProductionState
            {
                #region Public Methods

                public override void OnEnter()
                {
                    if (!DropCondition)
                    {
                    }
                }

                public override void OnUpdate()
                {
                    base.OnUpdate();
                    int safetyInt = 0;
                    while (Controller.TextProducer.PendingFunctions.TryDequeue(out Action function))
                    {
                        function();
                        safetyInt++;
                        if (safetyInt > 100)
                        {
                            throw new OverflowException();
                        }
                    }
                    if (Controller.TextProducer.PendingFunctions.Count == 0)
                    {
                        DropCondition = true;
                    }
                }

                public override void OnExit()
                {
                }

                #endregion Public Methods
            }

            #endregion Classes
        }

        #endregion Classes
    }
}
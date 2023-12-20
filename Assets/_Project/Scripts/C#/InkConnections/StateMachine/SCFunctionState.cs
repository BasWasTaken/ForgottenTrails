using Bas.Common;
using System;
using System.Collections.Generic;

namespace Bas.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        public partial class TextProduction
        {
            internal Queue<Action> PendingFunctions = new();
            public class SCFunctionState : SCProductionState
            {
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
            }
        }

    }
}

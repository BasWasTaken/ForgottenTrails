using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bas.Utility;
using DataService;
using Ink.Runtime;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;

namespace ForgottenTrails.InkFacilitation
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        public partial class TextProduction
        {
            internal Queue<Action> PendingFunctions;
            public class SCFunctionState : SCProductionState
            {
                public override void OnEnter()
                {

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
                        Machine.DropState(this);
                    }
                }
                public override void OnExit()
                {

                }
            }
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bas.Utility;


namespace ForgottenTrails.InkFacilitation
{
    /// <summary>
    /// <para>Base class for all machinestates for <see cref="TextProducer"/> object.</para>
    /// </summary>
    public class TextProducer_State : FSMState
    {
        // DEPRICATED!
        public class Entry : TextProducer_State
        {

        }
        public class Idle : TextProducer_State
        {

        }
        public class Production : TextProducer_State
        {

        }
    }
}
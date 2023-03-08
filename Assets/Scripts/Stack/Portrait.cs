using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class Portrait : MonoBehaviour
    {
        public bool ignoreLayout = false;

        private void LateUpdate()
        {
            if (!ignoreLayout)
            {
                //this.,transform=dummytransfrom.translated to screenposition
            }
        }
    }
}

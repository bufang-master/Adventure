using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Game.Fight
{
    public class DestorySelf:MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 7.5f);
        }
    }
}

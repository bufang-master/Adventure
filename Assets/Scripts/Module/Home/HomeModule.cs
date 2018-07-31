using System;
using SGF.Module.Framework;
using SGF.UI.Framework;
using UnityEngine;

namespace Scripts.Module
{
    public class HomeModule:BusinessModule
    {
        protected override void Show(object arg)
        {
            base.Show(arg);
            UIManager.Instance.OpenPage(UIDef.UIMainPage);
        }
    }
}

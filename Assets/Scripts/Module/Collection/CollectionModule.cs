using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGF.Module.Framework;
using SGF.UI.Framework;
using UnityEngine;

namespace Scripts.Module
{
    public class CollectionModule: BusinessModule
    {
        protected override void Show(object arg)
        {
            base.Show(arg);
            UIManager.Instance.OpenPage(UIDef.UICollectionPage);
        }
    }
}

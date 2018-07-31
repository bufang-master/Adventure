using System;
using SGF.Module.Framework;
using SGF.UI.Framework;

namespace Scripts.Module
{
    public class TaskModule : BusinessModule
    {
        protected override void Show(object arg)
        {
            base.Show(arg);
            UIManager.Instance.OpenPage(UIDef.UITaskPage);
        }
    }
}

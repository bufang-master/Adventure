using SGF.Module.Framework;
using SGF.UI.Framework;
using UnityEngine;

namespace Scripts.Module
{
    public class MainModule:BusinessModule
    {
        public void OpenModule(string name,object arg)
        {
            switch (name)
            {
                case ModuleDef.HomeModule:
                case ModuleDef.CollectionModule:
                case ModuleDef.StrengthenModule:
                    ModuleManager.Instance.ShowModule(name, arg);
                    break;
                default:
                    UIAPI.ShowMsgBox(name, "模块正在开发中..c.", "确定");
                    break;
            }
        }
        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
            switch (msg)
            {
                case "show": Show(args); break;
            }
        }
        protected override void Show(object arg)
        {
            base.Show(arg);
            UIManager.Instance.OpenPage(UIDef.UIMainPage);
        }
    }
}

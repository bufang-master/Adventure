using SGF.Module.Framework;
using SGF.UI.Framework;
using Scripts.Module;
using Scripts.Service.User;
using Scripts.Service.UserManager.Data;
using UnityEngine.UI;
using UnityEngine;

namespace Scripts.UI.Home
{
    public class HomePage : UIPage
    {

        protected override void OnOpen(object arg = null)
        {
            base.OnOpen(arg);
            UpdateUserInfo();
        }
        
        private void UpdateUserInfo()
        {

        }

    }
}

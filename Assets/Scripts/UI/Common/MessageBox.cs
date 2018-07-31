using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGF;
using Scripts.Service.UserManager.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SGF.UI.Framework;

namespace Scripts.UI.Common
{
    public class MessageBox : UIWindow
    {
        public class UIMsgBoxArg
        {
            public string title = "";
            public string content = "";
            public string btnText;//"确定|取消|关闭"
        }

        private UIMsgBoxArg m_arg;
        public Text txtContent;
        public UIBehaviour ctlTitle;
        public Button[] buttons;


        protected override void OnOpen(object arg = null)
        {
            base.OnOpen(arg);
            m_arg = arg as UIMsgBoxArg;
            txtContent.text = m_arg.content;
            string[] btnTexts = m_arg.btnText.Split('|');

            UIUtils.SetChildText(ctlTitle, m_arg.title);
            UIUtils.SetActive(ctlTitle, !string.IsNullOrEmpty(m_arg.title));

            int n = btnTexts.Length;
            if (n == 1)
            {
                UIUtils.SetButtonText(buttons[0], btnTexts[0]);
                UIUtils.SetActive(buttons[0],true);

                Vector3 pos = buttons[0].transform.localPosition;
                pos.x = 0;
                buttons[0].transform.localPosition =  pos;

                UIUtils.SetActive(buttons[1], false);
            }else if (n > 1)
            {
                for(int i = 0;i < n; i++)
                {
                    UIUtils.SetButtonText(buttons[i], btnTexts[i]);
                    UIUtils.SetActive(buttons[i], true);
                }
            }


        }

        public void OnBtnClick(int btnIndex)
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            this.Close(btnIndex);
        }
    }
}

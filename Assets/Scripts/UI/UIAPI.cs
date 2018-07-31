using Scripts.UI.Common;
using SGF.UI.Framework;
using UnityEngine;
using Scripts.UI.Collection;
using UnityEngine.UI;

namespace Scripts
{
    public static class UIAPI
    {
        /// <summary>
        /// 对MsgBox的调用封装
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="btnText">如果有多个按钮，用|分割，例如：确定|取消|关闭</param>
        /// <param name="onCloseEvent"></param>
        /// <returns></returns>
        public static UIWindow ShowMsgBox(string title, string content, string btnText, UIWindow.CloseEvent onCloseEvent = null)
        {
            MessageBox.UIMsgBoxArg arg = new MessageBox.UIMsgBoxArg();
            arg.content = content;
            arg.title = title;
            arg.btnText = btnText;
            UIWindow wnd = UIManager.Instance.OpenWindow(UIDef.MessageBox, arg);

            if (wnd != null && onCloseEvent != null)
            {
                wnd.onClose += closeArg =>
                {
                    onCloseEvent(closeArg);
                };
            }

            return wnd;
        }

        public static void ShowMessage(string info)
        {
            GameObject tips = GameObject.Find("Tips");
            if (tips != null)
            {
                foreach (Transform child in tips.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                GameObject go = Resources.Load(UIDef.UIMessage) as GameObject;
                if (go != null)
                {
                    GameObject message = GameObject.Instantiate(go);
                    message.transform.SetParent(tips.transform, false);
                    message.GetComponent<Text>().text = info;
                }
            }
        }

        public static UIWindow ShowCollectionInfoBox(Sprite sprite,string name,string info)
        {
            CollectionInfoBox.InfoBoxArgs arg = new CollectionInfoBox.InfoBoxArgs();
            arg.image = sprite;
            arg.name = name;
            arg.info = info;

            UIWindow wnd = UIManager.Instance.OpenWindow(UIDef.CollectionInfoBox, arg);

            return wnd;
        }
    }
}

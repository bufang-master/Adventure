using System;
using System.Collections;
using System.Collections.Generic;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Collection
{
    public class CollectionInfoBox : UIWindow
    {
        public class InfoBoxArgs
        {
            public Sprite image;
            public string name;
            public string info;
        }
        private InfoBoxArgs m_arg;

        public Image itemImage;
        public Text itemName;
        public Text itemInfo;

        public Scrollbar scrollView;
        protected override void OnOpen(object arg = null)
        {
            base.OnOpen(arg);
            m_arg = arg as InfoBoxArgs;
            itemImage.sprite = m_arg.image;
            itemName.text = m_arg.name;
            itemInfo.text = m_arg.info;
            StartCoroutine(SetValue());//若直接使scrollView.value=1，则会莫名变成0.5
        }
        public void DestroySelf()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            Destroy(gameObject);
        }
        private IEnumerator SetValue()
        {
            yield return new WaitForSeconds(0.1f);
            scrollView.value = 1;
        }
    }
}


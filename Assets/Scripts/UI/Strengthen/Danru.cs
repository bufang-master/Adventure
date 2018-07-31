using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI.Strengthen
{
    public class Danru : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public GameObject root;
        public bool isIn = false;
        private int spriteNum;

        public int SpriteNum { get { return spriteNum; } }
        public Sprite[] sprite;
       

        public void GetNum()
        {
            for(int i = 0;i < sprite.Length;i++)
            {
                if (sprite[i] == this.GetComponent<Image>().sprite)
                {
                    spriteNum = i;
                }
                //else
                //{
                //    spriteNum = -1;
                //}
            }
            root.GetComponent<StrengthenPage>().OnBtnChoose();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isIn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isIn = false;
        }
    }
}

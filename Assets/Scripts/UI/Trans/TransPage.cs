using System;
using Scripts.Guide;
using Scripts.Service.UserManager.Data;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI.Trans
{
    public class TransPage:MonoBehaviour
    {
        enum ButtonID { SKY,EARTH,HUMAN};
        private bool[] btn;
        private UserData ud;

        public GameObject circle;
        public float speed;

        private void Start()
        {
            speed = 20f;
            btn = new bool[3] { false, false, false };
            ud = AppConfig.Value.mainUserData;
        }
        private void Update()
        {
            circle.transform.Rotate(0, 0, -speed * Time.deltaTime, Space.Self);
        }

        public void OnBtnHelp(GameObject go)
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            go.gameObject.SetActive(!go.activeSelf);
        }

        public void OnBtnStone(int num)
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            var ud = AppConfig.Value.mainUserData;
            if (ud.stone[num])
            {
                string name = "";
                switch (num)
                {
                    case (int)ButtonID.SKY: name = "SkyImage"; break;
                    case (int)ButtonID.EARTH: name = "EarthImage"; break;
                    case (int)ButtonID.HUMAN: name = "HumanImage"; break;
                }
                btn[num] = !btn[num];
                GameObject.Find(name).GetComponent<Image>().enabled = btn[num];
            }
            else
            {
                UIAPI.ShowMsgBox("提示", "未收集该元石", "确定");
            }
        }
        public void OnBtnTrans()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            
            if (ud.food < ud.stat_num[4])
            {
                UIAPI.ShowMsgBox("食物不足", "背包未装满食物，请备满食物再前往冒险世界？", "确定", delegate(object arg) {
                    GuideAPI.BuyFoodFunc();
                });
            }else
            {
                if (btn[0])
                {
                    if (btn[1])
                    {
                        if (btn[2])
                        {
                            //天地人
                            ud.mapPath = "200|200";
                            ud.BOSSID = 15;
                        }
                        else
                        {
                            //天地
                            ud.mapPath = "150|150";
                            ud.BOSSID = 14;
                        }
                    }
                    else
                    {
                        if (btn[2])
                        {
                            //天人
                            ud.mapPath = "150|150";
                            ud.BOSSID = 14;
                        }
                        else
                        {
                            //天
                            ud.mapPath = "100|100";
                            ud.BOSSID = 13;
                        }
                    }
                }
                else
                {
                    if (btn[1])
                    {
                        if (btn[2])
                        {
                            //地人
                            ud.mapPath = "150|150";
                            ud.BOSSID = 14;
                        }
                        else
                        {
                            //地
                            ud.mapPath = "100|100";
                            ud.BOSSID = 13;
                        }
                    }
                    else
                    {
                        if (btn[2])
                        {
                            //人
                            ud.mapPath = "100|100";
                            ud.BOSSID = 13;
                        }
                        else
                        {
                            //无
                            ud.mapPath = "map/mapData/text";
                            ud.BOSSID = 12;
                        }
                    }
                }
                ud.food -= (uint)ud.stat_num[4];
                AppConfig.Value.mainUserData = ud;
                AppConfig.Save();
                UIManager.MainScene = "Adventure";
                SceneManager.LoadScene("Loading");
            }
        }
    }
}
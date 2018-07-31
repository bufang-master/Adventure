using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripts.Guide;
using Scripts.Service.User;
using SGF.Module.Framework;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Common
{
    public class NavControler: MonoBehaviour
    {
        public Text coins;
        public Text ocoins;
        public Text food;
        public Button[] nav;
        public GameObject info;
        public GameObject go;
        public Camera cam;

        public GameObject newCollection;
        public GameObject newTask;

        private void Start()
        {
            AudioControl.PlayBGMusic(GetComponent<AudioSource>());
            if(!AppConfig.Value.mainUserData.IsAdven)
                GuideAPI.FightEndFunc();
            CheckNewItem();
        }

        private void CheckNewItem()
        {
            if (CheckCollections())
            {
                newCollection.SetActive(true);
            }
            if (CheckTasks())
            {
                newTask.SetActive(true);
            }
        }

        private bool CheckCollections()
        {
            var col = AppConfig.Value.mainUserData.collection_data;
            foreach(var i in col)
            {
                if (i == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckTasks()
        {
            var dai = AppConfig.Value.mainUserData.dailyTask;
            foreach(var i in dai)
            {
                if (i.currentNum >= GameData.mission[i.ID].num[i.hardDegree]&&!i.isGain)
                {
                    return true;
                }
            }
            return false;
        }
        //public void OpenModule(string name, object arg=null)
        //{
        //    switch (name)
        //    {
        //        case ModuleDef.HomeModule:
        //        case ModuleDef.CollectionModule:
        //        case ModuleDef.StrengthenModule:
        //            ModuleManager.Instance.ShowModule(name, arg);
        //            break;
        //        default:
        //            UIAPI.ShowMsgBox(name, "模块正在开发中...", "确定");
        //            break;
        //    }
        //}
        private void OnGUI()
        {
            coins.text = AppConfig.Value.mainUserData.coins.ToString();
            ocoins.text = AppConfig.Value.mainUserData.ocoins.ToString();
            food.text = AppConfig.Value.mainUserData.food + " / " + AppConfig.Value.mainUserData.stat_num[5];
        }
        private bool AddModule(string path,object arg = null)
        {
            var uiModule = UIRes.LoadPrefab(path);
            if (uiModule != null)
            {
                CheckNewItem();
                for(int i = 0;i < go.transform.childCount; i++)
                {
                    GameObject child = go.transform.GetChild(i).gameObject;
                    Destroy(child);
                }
                var module = GameObject.Instantiate(uiModule);
                module.transform.SetParent(go.transform,false);
                //module.transform.parent = go.transform;
                //module.transform.localPosition = Vector3.zero;
                //module.GetComponent<RectTransform>().localPosition = Vector3.zero; 
                module.name = path.Split('/').Last();
                return true;
            }else
            {
                UIAPI.ShowMsgBox(path.Split('/').Last(), "模块开发中。。。", "确定");
                return false;
            }
        }

        private void BtnHighLight(int n)
        {
            for(int i = 0;i < 5; i++)
            {
                if(i != n)
                {
                    nav[i].GetComponent<Transform>().SetScaleXY(0.9f, 0.9f);
                }
                else
                {
                    nav[i].GetComponent<Transform>().SetScaleXY(1.0f, 1.0f);
                }
            }
        }

        public void OnBtnHome()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (AddModule(UIDef.UITransPage))
                BtnHighLight(0);
        }
        public void OnBtnStrengthen()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (AddModule(UIDef.UIStrengthenPage))
                BtnHighLight(1);
        }
        public void OnBtnCollection()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            //ModuleManager.Instance.ShowModule(ModuleDef.CollectionModule);
            if (AddModule(UIDef.UICollectionPage))
            {
                newCollection.SetActive(false);
                BtnHighLight(2);
            }
        }
        public void OnBtnTask()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (AddModule(UIDef.UITaskPage))
            {
                newTask.SetActive(false);
                BtnHighLight(3);
            }
        }
        public void OnBtnSetting()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (AddModule(UIDef.UISettingPage))
                BtnHighLight(4);
        }

        public void OnBtnBuy()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            UIAPI.ShowMsgBox("食物购买", "是否花费500金币购买100粮食？", "确定|取消",BuyFood);
        }

        private void BuyFood(object arg)
        {
            if ((int)arg == 0)
            {
                var ud = AppConfig.Value.mainUserData;
                if (ud.food == ud.stat_num[5])
                {
                    UIAPI.ShowMsgBox("提示", "仓库已满，无需购买", "确定");
                }
                else if (ud.coins >= 500)
                {
                    ud.coins -= 500;
                    ud.food += 100;
                    if (ud.food > ud.stat_num[5])
                    {
                        ud.food = (uint)ud.stat_num[5];
                    }
                    AppConfig.Value.mainUserData = ud;
                    AppConfig.Save();
                }
                else
                {
                    UIAPI.ShowMsgBox("提示", "金币不足", "确定");
                }
            }
        }

        public void OnBtnInfo()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            //显示个人信息
            GameObject i = Instantiate(info);
            i.transform.SetParent(go.transform, false);
        }
    }
}

using Scripts.Module;
using Scripts.Service.User;
using Scripts.Service.UserManager.Data;
using SGF.Module.Framework;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.UI;
using System;
using Scripts.Guide;

namespace Scripts.UI.Strengthen
{
    public class StrengthenPage:UIPage
    {
        public GameObject statObject;
        public GameObject skillObject;
        public Image backgroundImage;
        public Sprite bg_stat;
        public Sprite bg_skill;
        public Sprite defaultSprite;

        private uint costCoins;
        private uint costOCoins;
        private string strengthenItem;
        private int addPoint;
        private int sType;
        private int level;
        private Stat item;

        public Image target;
        public Text costCoinNum;
        public Text costOCoinNum;
        public Text strengthenInfo;
        public Text addInfo;

        private void Start()
        {
            //GuideAPI.StatEnhanceFunc();
        }

        private void OnGUI()
        {
            costCoinNum.text = costCoins.ToString();
            costOCoinNum.text = costOCoins.ToString();
            strengthenInfo.text = strengthenItem;
            if (addPoint <= 0)
            {
                addInfo.text = null;
            }else
            {
                addInfo.text = "+" + addPoint;
            }
        }

        public void OnBtnStat()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (skillObject != null)
            {
                skillObject.SetActive(false);
            }
            if (statObject != null)
            {
                statObject.SetActive(true);
            }
            backgroundImage.sprite = bg_stat;
        }
        public void OnBtnSkill()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (skillObject != null)
            {
                skillObject.SetActive(true);
            }
            if (statObject != null)
            {
                statObject.SetActive(false);
            }
            backgroundImage.sprite = bg_skill;
        }
        public void OnBtnChoose()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            sType = target.GetComponent<Danru>().SpriteNum;
            item = GameData.stat[sType];
            level = AppConfig.Value.mainUserData.stat_level[sType];
            if (level < item.maxLevel)
            {
                costCoins = (uint)item.costCoin[level - 1];
                costOCoins = (uint)item.costOCoin[level - 1];
                strengthenItem = item.statName + "(Lv" + level + ") " + item.addNum[level - 1];
                addPoint = item.addNum[level]-item.addNum[level-1];
            }
            else
            {
                costCoins = 0;
                costOCoins = 0;
                strengthenItem = item.statName + "(LvMax) " + item.addNum[level - 1];
                addPoint = 0;
            }
        }
        
        public void OnBtnSure()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (target.sprite != defaultSprite)
            {
                if (level >= item.maxLevel)
                {
                    UIAPI.ShowMsgBox("强化失败", "已达最大等级", "确定");
                }
                else
                {
                    UIAPI.ShowMsgBox("强化", "是否使用" + costCoins + "金币和" + costOCoins + "灵石升级" + item.statName, "确定|取消",OnChoose);
                    
                }
            }
            
        }

        private void OnChoose(object arg)
        {
            if ((int)arg == 0)
            {
                UserData ud = AppConfig.Value.mainUserData;
                if (ud.coins < costCoins || ud.ocoins < costOCoins)
                {
                    UIAPI.ShowMsgBox("强化失败", "金币或灵石不足", "确定");
                }
                else
                {
                    ud.coins -= costCoins;
                    ud.ocoins -= costOCoins;
                    AppConfig.Value.mainUserData.stat_level[sType]++;
                    AppConfig.Value.mainUserData.stat_num[sType] += addPoint;
                    
                    AppConfig.Value.mainUserData = ud;
                    AppConfig.Save();

                    OnBtnChoose();
                }
            }
        }
    }
}

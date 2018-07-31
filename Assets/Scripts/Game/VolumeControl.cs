using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class VolumeControl:MonoBehaviour
    {

        public Image HPVolume;
        public Image FoodVolume;
        public Image ATCVolume;
        public Image DEFVolume;

        public Text HPInfo;
        public Text FoodInfo;
        public Text ATCInfo;
        public Text DEFInfo;
        public Text CoinInfo;
        public Text OCoinInfo;

        public Button leave;
        public GameObject setAudio;

        private int lastHP;

        private void Start()
        {
            var gameData = GameControl.gameData;
            ATCInfo.text = gameData.ATC.ToString();
            DEFInfo.text = gameData.DEF.ToString();
            float per_atc = 1.0f * gameData.ATC / (gameData.ATC + gameData.DEF);
            float per_def = 1 - per_atc;
            ATCVolume.transform.SetScaleX(per_atc);
            DEFVolume.transform.SetScaleX(per_def);
            lastHP = GameControl.gameData.currentHP;

            UpdateVolume();
        }

        private void Update()
        {
            UpdateVolume();
            float per_hp = (float)Math.Round(1.0f * GameControl.gameData.currentHP / GameControl.gameData.maxHP, 2);
            if (HPVolume.transform.localScale.x != per_hp)
            {
                HPVolume.transform.SetScaleX(Mathf.Lerp(HPVolume.transform.localScale.x, per_hp, Time.deltaTime * 4));
                if (Mathf.Abs(per_hp - HPVolume.transform.localScale.x)<=0.01)
                {
                    HPVolume.transform.SetScaleX(per_hp);
                }
            }

            if (GameControl.gameData.currentHP != lastHP)
            {
                lastHP = GameControl.gameData.currentHP;
                CheckPlayerDie();
            }
            if (GameControl.gameData.BOSS_die)
            {
                GameDef.isFight = true;
                AudioControl.PlayEffect(GameDef.winEffect, 0.5f);
                UIAPI.ShowMsgBox("胜利", "成功击败BOSS，即将退出冒险世界", "确定", delegate(object arg) {
                    Settlement(2);
                });
                GameControl.gameData.BOSS_die = false;
            }
        }

        private void CheckPlayerDie()
        {
            if (lastHP <= 0)
            {
                GameDef.isFight = true;
                AudioControl.StopBGMusic();
                AudioControl.PlayEffect(GameDef.failEffect, 0.5f);
                GameControl.gameData.Statistics[GameData.Statist[(int)GameData.SID.DEATH]]++;
                UIAPI.ShowMsgBox("失败", "人物死亡，失去全部物品", "确定", Settlement);
            }
        }

        private void Settlement(object arg)
        {
            AudioControl.StopBGMusic();

            int coin = 0;
            int ocoin = 0;
            int food = 0;

            var ud = AppConfig.Value.mainUserData;
            if ((int)arg == 0)//人物死亡
            {
            }
            else if ((int)arg == 1)//未击败BOSS逃离世界
            {
                coin = GameControl.gameData.gainCoin / 2;
                ocoin = GameControl.gameData.gainOCoin / 2;
            }else if ((int)arg == 2)//击败BOSS离开世界
            {
                coin = GameControl.gameData.gainCoin;
                ocoin = GameControl.gameData.gainOCoin;
                food = GameControl.gameData.currentFood;
            }

            GameControl.gameData.Statistics[GameData.Statist[(int)GameData.SID.COIN]] += coin;
            GameControl.gameData.Statistics[GameData.Statist[(int)GameData.SID.OCOIN]] += ocoin;

            for (int i = 0;i < 3; i++)
            {
                ud.dailyTask[i].currentNum += GameControl.gameData.Statistics[GameData.mission[ud.dailyTask[i].ID].target];
            }
            for(int i = 0;i < GameControl.gameData.Statistics.Count(); i++)
            {
                ud.Statistics[GameData.Statist[i]] += GameControl.gameData.Statistics[GameData.Statist[i]];
            }
            ud.coins += (uint)coin;
            ud.ocoins += (uint)ocoin;
            ud.food += (uint)food;
            if (ud.food > ud.stat_num[5])
            {
                ud.food = (uint)ud.stat_num[5];
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<color=#FFD800>金币 * " + coin + "</color>");
            sb.Append("\n<color=#FB00D4>灵石 * " + ocoin + "</color>");
            sb.Append("\n<color=#00FF0C>食物 * " + food + "</color>");
            for(int i = 0;i < GameControl.gameData.colloctions.Count(); i++)
            {
                int n = GameControl.gameData.colloctions[i];
                if (ud.collection_data[n] == 0)
                {
                    ud.collection_data[n] = 1;
                }
                sb.Append("\n<color=red>" + GameData.collectItem[n].itemName + "</color>");
            }
            switch (GameControl.gameData.stone)
            {
                case 0: sb.Append("\n<color=#FA800A>天元石</color>"); break;
                case 1: sb.Append("\n<color=#FA800A>地元石</color>"); break;
                case 2: sb.Append("\n<color=#FA800A>人元石</color>"); break;
            }
            UIAPI.ShowMsgBox("奖励结算", sb.ToString(), "领取", delegate(object args) {
                AppConfig.Value.mainUserData.IsAdven = false;
                AppConfig.Value.mainUserData.dataControl = null;
                AppConfig.Save();
                UIManager.MainScene = "Main";
                SceneManager.LoadScene("Loading");
            });
            AppConfig.Value.mainUserData = ud;
        }

        private void UpdateVolume()
        {
            var gameData = GameControl.gameData;
            if (gameData.currentFood > gameData.BAG)
                gameData.currentFood = gameData.BAG;
            else if (gameData.currentFood <= 0)
                gameData.currentFood = 0;
            if (gameData.currentHP > gameData.maxHP)
                gameData.currentHP = gameData.maxHP;
            else if (gameData.maxHP <= 0)
                gameData.currentHP = 0;
            HPInfo.text = gameData.currentHP + " / " + gameData.maxHP;
            FoodInfo.text = gameData.currentFood + " / " + gameData.BAG;
            float per_food = 1.0f * gameData.currentFood / gameData.BAG;
            FoodVolume.transform.SetScaleX(per_food);

            CoinInfo.text = gameData.gainCoin.ToString();
            OCoinInfo.text = gameData.gainOCoin.ToString();
        }
        public void onSetBtn()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (true)//!GameDef.isFight)
            {
                if (GameObject.Find(UIDef.UISettingPanel.Split('/').Last())==null)
                {
                    GameObject go = UIRes.LoadPrefab(UIDef.UISettingPanel) ;
                    if (go != null)
                    {
                        GameObject panel = Instantiate(go);
                        panel.transform.parent = setAudio.transform;
                        panel.transform.localPosition = go.transform.position;
                        panel.transform.localScale = go.transform.localScale;
                    }
                    else
                    {
                        //Debug.Log("Q");
                    }
                }
            }
        }
        public void Leave()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (!GameDef.isFight)
            {
                GameDef.isFight = true;
                leave.GetComponent<Image>().enabled = false;
                UIAPI.ShowMsgBox("退出", "未击败BOSS，是否消耗一半的代币与全部的食物，直接退出当前冒险世界", "确定|取消", delegate(object arg) {
                    if ((int)arg == 0)
                    {
                        Settlement(1);
                        AudioControl.PlayEffect(GameDef.failEffect, 0.5f);
                    }
                    else
                    {
                        leave.GetComponent<Image>().enabled = true;
                        GameDef.isFight = false;
                    }
                });
            }
        }
    }
}

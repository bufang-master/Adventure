using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Task
{
    public class TaskItem:MonoBehaviour
    {
        
        public int taskType;//0:成就任务 1:日常任务
        public int taskSub;
        public Image gain;
        public Image currentP;
        public Sprite[] gainSprite;
        public Text taskContent;
        public Text infoP;
        public Text awardCoin;
        public Text awardOCoin;

        private bool btnEnable;

        private Color[] degree = new Color[] {new Color(0,0.5f,0), new Color(0, 0, 0.5f), new Color(0.5f, 0, 0) } /*{ new Color(8,167,2), new Color(8,2,167), new Color(195,14,14) }*/;

        private void Start()
        {
            var ud = AppConfig.Value.mainUserData;
            btnEnable = false;
            switch (taskType)
            {
                case 0:
                    SetGrowthTask();
                    break;
                case 1:
                    SetDailyTask();
                    break;
            }
        }
        private void SetGrowthTask()
        {
            var ud = AppConfig.Value.mainUserData;
            var g_mission = GameData.mission[taskSub];
            var sub = ud.growthTask[taskSub];
            taskContent.text = g_mission.content1 + g_mission.num[sub] + g_mission.content2;
            if (sub >= 2)
            {
                taskContent.color = degree[2];
                currentP.color = degree[2];
            }else
            {
                taskContent.color = degree[sub];
                currentP.color = degree[sub];
            }
            awardCoin.text = g_mission.c_award[sub].ToString();
            awardOCoin.text = g_mission.o_award[sub].ToString();
            float q = 1.0f * ud.stat_num[taskSub] / g_mission.num[sub];
            if (q >= 1)//任务完成
            {
                if(sub >= 3)
                {
                    gain.sprite = gainSprite[2];
                }else
                {
                    gain.sprite = gainSprite[1];
                    btnEnable = true;
                }
                currentP.transform.SetScaleX(1);
                infoP.text = ud.stat_num[taskSub] + " / " + g_mission.num[sub];
            }
            else
            {
                currentP.transform.SetScaleX(q);
                infoP.text = ud.stat_num[taskSub] + " / " + g_mission.num[sub];
                gain.sprite = gainSprite[0];
            }
        }
        private void SetDailyTask()
        {
            var ud = AppConfig.Value.mainUserData;
            var item = ud.dailyTask[taskSub];
            var d_mission = GameData.mission[item.ID];
            float p = 1.0f * item.currentNum / d_mission.num[item.hardDegree];
            if (p >= 1)//任务完成
            {
                currentP.transform.SetScaleX(1);
                infoP.text = d_mission.num[item.hardDegree] + " / " + d_mission.num[item.hardDegree];
                if (item.isGain)
                {
                    gain.sprite = gainSprite[2];
                }
                else
                {
                    gain.sprite = gainSprite[1];
                    btnEnable = true;
                }
            }
            else//任务未完成
            {
                currentP.transform.SetScaleX(p);
                infoP.text = item.currentNum + " / " + d_mission.num[item.hardDegree];
                gain.sprite = gainSprite[0];
            }
            awardCoin.text = d_mission.c_award[item.hardDegree].ToString();
            awardOCoin.text = d_mission.o_award[item.hardDegree].ToString();
            taskContent.text = d_mission.content1 + d_mission.num[item.hardDegree] + d_mission.content2;
            taskContent.color = degree[item.hardDegree];
            currentP.color = degree[item.hardDegree];
        }
        public void OnBtnGain()
        {
            if (btnEnable)
            {
                AudioControl.PlayEffect(GameDef.clickEffect);
                var ud = AppConfig.Value.mainUserData;
                ud.coins += uint.Parse(awardCoin.text);
                ud.ocoins += uint.Parse(awardOCoin.text);

                btnEnable = false;
                if(taskType == 0)
                {
                    if (++ud.growthTask[taskSub] == 3)//如果三阶成长任务完成，则显示已完成
                    {
                        gain.sprite = gainSprite[2];
                    }else
                    {
                        gain.sprite = gainSprite[0];
                        AppConfig.Value.mainUserData = ud;
                        SetGrowthTask();
                    }
                }else if(taskType == 1)
                {
                    ud.dailyTask[taskSub].isGain = true;
                    gain.sprite = gainSprite[2];
                }

                AppConfig.Value.mainUserData = ud;
                AppConfig.Save();
            }
        }
    }
}

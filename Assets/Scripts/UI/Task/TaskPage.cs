using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripts.Service.User;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Task
{
    public class TaskPage:UIPage
    {
        public GameObject dailyPanel;
        public GameObject growthPanel;
        public Text refreshTime;

        private void Start()
        {
            SetTime();
            for(int i = 0;i < 3; i++)
            {
                AddChild(dailyPanel, i, 1);
            }
            for(int i = 0;i < 6; i++)
            {
                AddChild(growthPanel, i, 0);
            }
        }
        private void Update()
        {
            SetTime();
        }
        private void AddChild(GameObject panel, int sub, int type)
        {
            GameObject child = UIRes.LoadPrefab(UIDef.UITaskItem);
            if (child != null)
            {
                GameObject task = GameObject.Instantiate(child);
                task.transform.SetParent(panel.transform, false);
                task.GetComponent<TaskItem>().taskSub = sub;
                task.GetComponent<TaskItem>().taskType = type;
            }
        }

        private void SetTime()
        {
            var ud = AppConfig.Value.mainUserData;
            var nowTime = DateTime.Now;
            var rTime = ud.freshTime.AddHours(-ud.freshTime.Hour).AddMinutes(-ud.freshTime.Minute).AddSeconds(-ud.freshTime.Second).AddDays(1);
            var rest = rTime - nowTime;
            refreshTime.text = (rest.Days*24 + rest.Hours).ToString().PadLeft(2, '0') + ":" + rest.Minutes.ToString().PadLeft(2, '0') + ":" + rest.Seconds.ToString().PadLeft(2, '0');
            if (rTime < nowTime)
            {
                RefreshTask();
            }
        }
        //更新每日任务
        private void RefreshTask()
        {
            var ud = AppConfig.Value.mainUserData;
            System.Random rom = new System.Random();
            for(int i = 0;i < 3; i++)
            {
                int id = rom.Next(6, GameData.mission.Count());
                ud.dailyTask[i].ID = id;

                int hard = rom.Next(0, 100);
                if (hard < 60)
                    ud.dailyTask[i].hardDegree = 0;
                else if (hard < 90)
                    ud.dailyTask[i].hardDegree = 1;
                else
                    ud.dailyTask[i].hardDegree = 2;

                ud.dailyTask[i].currentNum = 0;
                ud.dailyTask[i].isGain = false;
                //防止出现重复任务
                for (int j = 0; j < i; j++)
                {
                    if (id == ud.dailyTask[j].ID)
                    {
                        i--; break;
                    }
                }
            }
            ud.freshTime = DateTime.Now;
            AppConfig.Value.mainUserData = ud;
            AppConfig.Save();

            foreach(Transform child in dailyPanel.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < 3; i++)
            {
                AddChild(dailyPanel, i, 1);
            }
        }
        public void OnBtnHelp(GameObject go)
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            go.gameObject.SetActive(!go.activeSelf);
        }
    }
}

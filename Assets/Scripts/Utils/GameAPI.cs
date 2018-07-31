using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts
{
    public static class GameAPI
    {
        public static bool IsMeet(float bizarre)
        {
            System.Random rom = new System.Random();
            if (rom.Next(1000) > bizarre * 1000)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void CheckTime()
        {
            var ud = AppConfig.Value.mainUserData;
            DateTime nowTime = DateTime.Now;
            var waitTime = nowTime - ud.freshTime;
            float h = (float)Math.Round(waitTime.TotalHours,1);
            if (h > 8)
            {
                h = 8;
            }
            else if (h < 0)
            {
                h = 0;
            }
            UIAPI.ShowMsgBox("离线收益", "您上次登录游戏为" + h + "小时前，共为您收获" + h * 50 + "食物，已为您存放到仓库", "确定");
            ud.food += (uint)(h * 100);
            if (ud.food > ud.stat_num[5])
            {
                ud.food = (uint)ud.stat_num[5];
            }
            var rTime = ud.freshTime.AddHours(-ud.freshTime.Hour).AddMinutes(-ud.freshTime.Minute).AddSeconds(-ud.freshTime.Second).AddDays(1);
            if (rTime < nowTime)
            {
                System.Random rom = new System.Random();
                for (int i = 0; i < 3; i++)
                {
                    int id = rom.Next(6, GameData.mission.Count());
                    ud.dailyTask[i].ID = id;

                    int hard = rom.Next(0, 100);
                    if (hard < 65)
                        ud.dailyTask[i].hardDegree = 0;
                    else if (hard < 90)
                        ud.dailyTask[i].hardDegree = 1;
                    else
                        ud.dailyTask[i].hardDegree = 2;

                    ud.dailyTask[i].currentNum = 0;
                    ud.dailyTask[i].isGain = false;
                    for (int j = 0; j < i; j++)
                    {
                        if (id == ud.dailyTask[j].ID)
                        {
                            i--; break;
                        }
                    }
                }
            }
            if (nowTime > ud.freshTime)
                ud.freshTime = nowTime;
            AppConfig.Value.mainUserData = ud;
            AppConfig.Save();
        }
    }
}

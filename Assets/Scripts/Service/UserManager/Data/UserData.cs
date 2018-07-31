using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripts.Game;
using Scripts.Guide;

namespace Scripts.Service.UserManager.Data
{
    [Serializable]
    public struct DailyTask
    {
        public int ID;
        public int currentNum;
        public int hardDegree;//任务难度0：绿色；1：蓝色；2：红色
        public bool isGain;
        public DailyTask(int ID,int currentNum,int hardDegree,bool isGain)
        {
            this.ID = ID;
            this.currentNum = currentNum;
            this.hardDegree = hardDegree;
            this.isGain = isGain;
        }
    }

    [Serializable]
    public class UserData
    {
        public string name;//用户名字
        public uint coins = 1000;//主要货币数量
        public uint ocoins=10;//其他货币数量
        public uint food=200;//食物数量
        public int[] stat_level = new int[] { 1, 1, 1, 1, 1, 1 };//HP ATC DEF AGI BAG STO
        public int[] stat_num = new int[] { 200, 50, 25, 10, 100, 200 };//HP ATC DEF AGI BAG STO
        public int[] skill_level = new int[] { 1, 1, 1, 1, 1, 1 };//技能等级
        public Boolean[] stone = new Boolean[] { false, false, false };
        public int[] collection_data = new int[18] {
                                            0,0,0,0,0,0,0,0,0,
                                            0,0,0,0,0,0,0,0,0
                                        };//收集信息 0:未收集 1:已收集未点亮 2:已点亮
        
        public bool IsAdven = false;//是否在冒险途中
        public string mapPath = "map/mapData/text";//地图信息
        public int BOSSID = 12;
        public DataControl dataControl;

        public Dictionary<string, int> Statistics = new Dictionary<string, int> {
            { GameData.Statist[(int)GameData.SID.COIN],0 }, {GameData.Statist[(int)GameData.SID.OCOIN],0 }, {GameData.Statist[(int)GameData.SID.LCHEST] ,0 },
            { GameData.Statist[(int)GameData.SID.MCHEST] ,0 },{ GameData.Statist[(int)GameData.SID.HCHEST] ,0 }, {GameData.Statist[(int)GameData.SID.BOSS] ,0 },
            { GameData.Statist[(int)GameData.SID.MONSTER] ,0 },{ GameData.Statist[(int)GameData.SID.FOOD],0 }, { GameData.Statist[(int)GameData.SID.EXPLORE] ,0 },
            { GameData.Statist[(int)GameData.SID.DEATH] ,0 },{ GameData.Statist[(int)GameData.SID.ADVEN] ,0 }
        };

        public DailyTask[] dailyTask = new DailyTask[] { new DailyTask(6, 0, 0, false), new DailyTask(7, 0, 1, false), new DailyTask(8, 0, 2, false) };
        public DateTime freshTime = DateTime.Now;
        public int[] growthTask = new int[] { 0, 0, 0, 0, 0, 0};//成长任务等阶

        public Dictionary<string, bool> GuideFlag = new Dictionary<string, bool>
        {
            {GuideAPI.AdventureStart,false }, {GuideAPI.BuyFood,false },  {GuideAPI.EnterTransPage,false },
            {GuideAPI.FightStart,false }, {GuideAPI.PlayerInfo,false }, {GuideAPI.StatEnhance,false },
            {GuideAPI.FightEnd,false }
        };
    }
}

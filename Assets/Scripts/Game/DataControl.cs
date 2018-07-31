using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Game
{
    [Serializable]
    public class DataControl
    {
        public enum STAT { HP, ATC, DEF, AGI, BAG, STO };
        public int currentHP;//当前血量
        public int maxHP;//最大血量
        public int ATC;//攻击力
        public int DEF;//防御力
        public int AGI;//敏捷
        public int currentFood;//当前剩余食物
        public int BAG;//背包容量
        public int gainCoin;//获取金币
        public int gainOCoin;//获取灵石
        public string mapPath;
        public int level;
        public int bossX;
        public int bossY;
        public int playerX;
        public int playerY;
        public List<int> colloctions;
        public Dictionary<string, int> Statistics = new Dictionary<string, int> {
            { GameData.Statist[(int)GameData.SID.COIN],0 }, {GameData.Statist[(int)GameData.SID.OCOIN],0 }, {GameData.Statist[(int)GameData.SID.LCHEST] ,0 },
            { GameData.Statist[(int)GameData.SID.MCHEST] ,0 },{ GameData.Statist[(int)GameData.SID.HCHEST] ,0 }, {GameData.Statist[(int)GameData.SID.BOSS] ,0 },
            { GameData.Statist[(int)GameData.SID.MONSTER] ,0 },{ GameData.Statist[(int)GameData.SID.FOOD],0 }, { GameData.Statist[(int)GameData.SID.EXPLORE] ,0 },
            { GameData.Statist[(int)GameData.SID.DEATH] ,0 },{ GameData.Statist[(int)GameData.SID.ADVEN] ,0 }
        };

        public int BOSSID { get; internal set; }
        public bool BOSS_die;

        public List<List<int>> mapData;
        public int[,] mapInfo;
        public int stone;

        public void Init()
        {
            var ud = AppConfig.Value.mainUserData;
            currentHP = maxHP = ud.stat_num[(int)STAT.HP];
            currentFood = BAG = ud.stat_num[(int)STAT.BAG];
            ATC = ud.stat_num[(int)STAT.ATC];
            DEF = ud.stat_num[(int)STAT.DEF];
            AGI = ud.stat_num[(int)STAT.AGI];
            mapPath = ud.mapPath;
            gainCoin = gainOCoin = 0;
            BOSSID = ud.BOSSID;
            BOSS_die = false;

            level = 0;
            for(int i = 0;i < 4; i++)
            {
                level += ud.stat_level[i];
            }
            bossX = bossY = 0;
            colloctions = new List<int>();
            stone = -1;
            mapData = new List<List<int>>();
            
        }
    }
}

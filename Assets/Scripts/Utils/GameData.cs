using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts
{
    public struct CollectItem{
        public int itemID;
        public string itemName;
        public int pageID;
        public string itemInfo;
        public string path;

        public CollectItem(int itemID, string itemName, int pageID,string path,string info) : this()
        {
            this.itemID = itemID;
            this.itemName = itemName;
            this.pageID = pageID;
            this.itemInfo = info;
            this.path = path;
        }
    }
    public struct Stat
    {
        public int statID;
        public string statName;
        public int[] costCoin;
        public int[] costOCoin;
        public int[] addNum;
        public int maxLevel;

        public Stat(int id, string name, int[] num, int[] coin,int[] ocoin, int max)
        {
            this.statID = id;
            this.statName = name;
            this.costCoin = coin;
            this.costOCoin = ocoin;
            this.addNum = num;
            this.maxLevel = max;
        }
    }
    public struct Skill
    {
        public int skillID;
        public string skillName;
        public int[] num1;
        public int[] num2;
        public int[] costCoin;
        public int[] costOCoin;
        public int maxLevel;
        public int skillType;//1为主动技能，2为辅助技能，3为被动技能
        public string path;
        public string[] skillInfo;

        public Skill(int id, string name, int[] data1, int[] data2,int[] coin,int[] ocoin, int max, int type,string path, params string[] info)
        {
            this.skillID = id;
            this.skillName = name;
            this.num1 = data1;
            this.num2 = data2;
            this.costCoin = coin;
            this.costOCoin = ocoin;
            this.maxLevel = max;
            this.skillType = type;
            this.path = path;
            this.skillInfo = info;
        }
    }

    public struct Mission
    {
        public int ID;
        public string content1;//任务内容
        public string content2;
        public int[] num;//目标数值
        public int[] c_award;//金币奖励
        public int[] o_award;//灵石奖励
        public int type;//任务类型，1为日常任务，0为成长任务
        public string target;//任务目标

        public Mission(int ID,int[] num,int[] c_award,int[] o_award,int type,string target,string content1, string content2)
        {
            this.ID = ID;
            this.content1 = content1;
            this.content2 = content2;
            this.num = num;
            this.o_award = o_award;
            this.c_award = c_award;
            this.target = target;
            this.type = type;
        }
    }

    public struct Goods
    {
        public int ID;
        public string name;
        public string info;
        public bool canUse;

    }

    public struct Monsters
    {
        public int ID;
        public string name;
        public int[] m_HP;
        public int[] m_ATC;
        public int[] m_DEF;
        public int[] m_AGI;
        public int[] m_coin;
        public int[] m_ocoin;
        public string path;
        public string type;
        public Monsters(int ID,string name, int[] HP, int[] ATC, int[] DEF, int[] AGI,int[] coin,int[] ocoin,string path,string type)
        {
            this.ID = ID;
            this.name = name;
            this.m_HP = HP;
            this.m_ATC = ATC;
            this.m_DEF = DEF;
            this.m_AGI = AGI;
            this.m_coin = coin;
            this.m_ocoin = ocoin;
            this.path = path;
            this.type = type;
        }
    }

    public struct MapInfo
    {
        public int ID;
        public string name;
        //public bool isPass;//能否通过
        public string type;//能否探索
        public float bizarre;//遇怪率
        public int costFood;//通过该地图消耗的食物
        public int costHP;//通过该地图的血量变化，正数为回复血量，负数为消耗血量
        public string info;

        public MapInfo(int ID, string name, float bizarre, int costFood, int costHP, string type,string info)
        {
            this.ID = ID;
            this.name = name;
            //this.isPass = isPass;
            this.type = type;
            this.bizarre = bizarre;
            this.costFood = costFood;
            this.costHP = costHP;
            this.info = info;
        }
    }

    public static class GameData
    {
        public static List<CollectItem> collectItem = new List<CollectItem>();
        public static Dictionary<int, string> collectPage = new Dictionary<int, string>();
        public static List<Stat> stat = new List<Stat>();
        public static List<Skill> skill = new List<Skill>();
        public static List<Mission> mission = new List<Mission>();
        public static List<MapInfo> mapInfo = new List<MapInfo>();
        public static List<Monsters> monster = new List<Monsters>();

        public static List<Vector3> collectItemPos = new List<Vector3>();
        
        public static List<string> Statist = new List<string>() { "COIN", "OCOIN", "LCHEST", "MCHEST", "HCHEST", "BOSS", "MONSTER", "FOOD", "EXPLORE", "DEATH","ADVEN" };
        public enum SID { COIN , OCOIN , LCHEST , MCHEST , HCHEST , BOSS , MONSTER , FOOD , EXPLORE , DEATH ,ADVEN};

        public static void InitGameData()
        {
            stat.Add(new Stat(0, "血量", new int[] { 200, 240, 280, 320, 390, 460, 530, 630, 730, 900 }, new int[] { 100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600 }, new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512 }, 10));
            stat.Add(new Stat(1, "攻击", new int[] { 50, 60, 70, 80, 100, 120, 140, 170, 200, 240 }, new int[] { 100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600 }, new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512 }, 10));
            stat.Add(new Stat(2, "防御", new int[] { 25, 30, 35, 40, 50, 60, 70, 85, 100, 120 }, new int[] { 100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600 }, new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512 }, 10));
            stat.Add(new Stat(3, "敏捷", new int[] { 10, 11, 12, 13, 15, 17, 19, 22, 25, 30 }, new int[] { 100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600 }, new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512 }, 10));
            stat.Add(new Stat(4, "背包", new int[] { 100, 115, 130, 145, 165, 185, 205, 235, 265, 300 }, new int[] { 100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600 }, new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512 }, 10));
            stat.Add(new Stat(5, "仓库", new int[] { 200, 300, 400, 500, 650, 800, 950, 1150, 1350, 1600 }, new int[] { 100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600 }, new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512 }, 10));

            skill.Add(new Skill(0, "鼓舞", new int[] { 10, 15, 15, 20, 25 }, new int[] { 3, 3, 2, 2, 2 }, new int[] { 500, 2000, 8000, 30000 }, new int[] { 10, 20, 40, 100 }, 5, 2,"ui/Textures/Common/guwu", "没有队友，只能自己给自己打气，有点惨。辅助技能，提升攻击", "%，冷却", "回合"));
            skill.Add(new Skill(1, "坚守", new int[] { 20, 30, 30, 40, 50 }, new int[] { 2, 2, 1, 1, 1 }, new int[] { 500, 2000, 8000, 30000 }, new int[] { 10, 20, 40, 100 }, 5, 2, "ui/Textures/Common/jianshou", "在这物欲横流的世界，人心冷漠无情，只有这盾牌还有点温暖。辅助技能，提升防御", "%,冷却", "回合"));
            skill.Add(new Skill(2, "连击", new int[] { 2, 2, 2, 3, 3 }, new int[] { 4, 3, 2, 4, 3 }, new int[] { 500, 2000, 8000, 30000 }, new int[] { 10, 20, 40, 100 }, 5, 1, "ui/Textures/Common/lianji", "回合制战斗用连击会不会太过分了。攻击技能，连续攻击", "次，冷却", "回合"));
            skill.Add(new Skill(3, "舍命", new int[] { 15, 15, 10, 10, 5 }, new int[] { 3, 2, 2, 1, 1 }, new int[] { 500, 2000, 8000, 30000 }, new int[] { 10, 20, 40, 100 }, 5, 1, "ui/Textures/Common/sheming", "以伤换伤，惨烈异常，不与<color=#ff0000>连击</color>同时生效，消耗自身最大生命的", "%，对敌方造成无视防御的真实伤害，冷却", "回合，血量不足无法使用"));
            skill.Add(new Skill(4, "全力攻击", new int[] { 10, 20 }, new int[] { -1, -1 }, new int[] { 15000 }, new int[] { 70 }, 2, 3, "ui/Textures/Common/gong", "被动技能，进攻是最好的防御，放弃防御，十成攻击，增加攻击力", "%", ""));
            skill.Add(new Skill(5, "全力防守", new int[] { 50, 75 }, new int[] { -1, -1 }, new int[] { 10000 }, new int[] { 50 }, 2, 3, "ui/Textures/Common/fang", "被动技能，防御是最好的进攻，放弃攻击，十成防御，血量恢复敌方攻击力的", "%", ""));

            mapInfo.Add(new MapInfo(0, "海水", 0.2f, 1, -1, "none", ""));
            mapInfo.Add(new MapInfo(1, "熔岩", 0.3f, 1, -2, "none", ""));
            mapInfo.Add(new MapInfo(2, "山峰", 0.3f, 3, 1, "climb|1", ""));
            mapInfo.Add(new MapInfo(3, "草地", 0.1f, 1, 1, "none", ""));
            mapInfo.Add(new MapInfo(4, "沙地", 0.1f, 1, 1, "none", ""));
            mapInfo.Add(new MapInfo(5, "森林", 0.3f, 2, 2, "none", ""));
            mapInfo.Add(new MapInfo(6, "果林", 0.3f, 2, 2, "collect|15", ""));
            mapInfo.Add(new MapInfo(7, "盆地", 0.2f, 2, 2, "explore|35", ""));//偏移：[-15,15]
            mapInfo.Add(new MapInfo(8, "洞穴", 0.4f, 2, 1, "explore|55", ""));//无：[0,25) 普通：[25,50) 中等：[50,75) 高级：[75,100]

            mission.Add(new Mission(0, new int[] { 320, 530, 900 }, new int[] { 500, 2000, 8000 }, new int[] { 5, 20, 80 }, 0, "HP", "冒险者血量提升至", ""));
            mission.Add(new Mission(1, new int[] { 80, 140, 240 }, new int[] { 500, 2000, 8000 }, new int[] { 5, 20, 80 }, 0, "ATC", "冒险者攻击提升至", ""));
            mission.Add(new Mission(2, new int[] { 40, 70, 120 }, new int[] { 500, 2000, 8000 }, new int[] { 5, 20, 80 }, 0, "DEF", "冒险者防御提升至", ""));
            mission.Add(new Mission(3, new int[] { 13, 19, 30 }, new int[] { 500, 2000, 8000 }, new int[] { 5, 20, 80 }, 0, "AGI", "冒险者敏捷提升至", ""));
            mission.Add(new Mission(4, new int[] { 145, 205, 300 }, new int[] { 500, 2000, 8000 }, new int[] { 5, 20, 80 }, 0, "BAG", "冒险者背包容量提升至", ""));
            mission.Add(new Mission(5, new int[] { 500, 950, 1600 }, new int[] { 500, 2000, 8000 }, new int[] { 5, 20, 80 }, 0, "STO", "冒险者仓库容量提升至", ""));
            mission.Add(new Mission(6, new int[] { 4, 8, 15 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.EXPLORE], "探索", "个洞穴或盆地"));
            mission.Add(new Mission(7, new int[] { 15, 25, 50 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.MONSTER], "击败", "个任意怪物"));
            mission.Add(new Mission(8, new int[] { 1, 2, 5 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.BOSS], "击败", "次冒险世界BOSS"));
            mission.Add(new Mission(9, new int[] { 3, 5, 10 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.LCHEST], "收集", "个低级宝箱"));
            mission.Add(new Mission(10, new int[] { 2, 4, 9 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.MCHEST], "收集", "个中级宝箱"));
            mission.Add(new Mission(11, new int[] { 1, 2, 3 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.HCHEST], "收集", "个高级宝箱"));
            mission.Add(new Mission(12, new int[] { 1000, 2000, 5000 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.COIN], "在冒险世界中赚取", "金币"));
            mission.Add(new Mission(13, new int[] { 300, 500, 1000 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.FOOD], "在冒险世界中收获", "食物"));
            mission.Add(new Mission(14, new int[] { 3, 5, 10 }, new int[] { 200, 600, 2000 }, new int[] { 2, 4, 10 }, 1, GameData.Statist[(int)GameData.SID.ADVEN], "进行", "次冒险"));
            //                                          HP                                     ATC                                 DEF                               AGI                               coin                                 ocoin                                       
            monster.Add(new Monsters(0, "双头木偶", new int[] { 105, 140, 200, 280, 360 }, new int[] {35, 45, 70, 90, 125 }, new int[] { 19, 27, 34, 43, 57 }, new int[] { 8, 10, 12, 15, 20 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m12", "normal"));
            monster.Add(new Monsters(1, "铁巨人", new int[] { 125, 160, 240, 320, 420 }, new int[] {33, 43, 66, 86, 120 }, new int[] { 21, 29, 38, 47, 63 }, new int[] { 7, 9, 10, 13, 17 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m1", "normal"));
            monster.Add(new Monsters(2, "野人兄贵", new int[] { 105, 140, 200, 280, 360 }, new int[] {28, 38, 56, 76, 104 }, new int[] { 20, 28, 36, 45, 60 }, new int[] { 8, 10, 12, 15, 20 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m2", "normal"));
            monster.Add(new Monsters(3, "秃头蓝魔", new int[] { 105, 140, 200, 280, 360 }, new int[] {30, 40, 60, 80, 110 }, new int[] { 20, 28, 36, 45, 60 }, new int[] { 8, 10, 12, 15, 20 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m3", "normal"));
            monster.Add(new Monsters(4, "独角翼虫", new int[] { 105, 140, 200, 280, 360 }, new int[] {35, 45, 70, 90, 125 }, new int[] { 22, 30, 40, 49, 66 }, new int[] { 8, 10, 12, 15, 20 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m4", "normal"));
            monster.Add(new Monsters(5, "蝠翼狮", new int[] { 105, 140, 200, 280, 360 }, new int[] {40, 50, 75, 95, 130 }, new int[] { 20, 28, 36, 45, 60 }, new int[] { 8, 10, 12, 15, 20 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m5", "normal"));
            monster.Add(new Monsters(6, "舞魔", new int[] { 105, 140, 200, 280, 360 }, new int[] {28, 38, 56, 76, 104 }, new int[] { 20, 28, 36, 45, 60 }, new int[] { 12, 15, 19, 24, 30 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m6", "normal"));
            monster.Add(new Monsters(7, "灰巫师", new int[] { 90, 130, 180, 260, 330 }, new int[] {40, 50, 75, 95, 130 }, new int[] { 16, 24, 30, 40, 52 }, new int[] { 11, 13, 16, 20, 26 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m7", "normal"));
            monster.Add(new Monsters(8, "哥布花生", new int[] { 105, 140, 200, 280, 360 }, new int[] {30, 40, 60, 80, 110 }, new int[] { 24, 32, 42, 50, 68 }, new int[] { 8, 10, 12, 15, 20 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m8", "normal"));
            monster.Add(new Monsters(9, "独臂圣灵", new int[] { 110, 150, 220, 300, 400 }, new int[] {30, 40, 60, 80, 110 }, new int[] { 20, 28, 36, 45, 60 }, new int[] { 8, 10, 12, 15, 20 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m9", "normal"));
            monster.Add(new Monsters(10, "象牙山龟", new int[] { 200, 300, 400, 550, 700 }, new int[] { 15, 20, 30, 40, 55 }, new int[] { 40, 56, 72, 90, 120 }, new int[] { 6, 8, 10, 12, 15 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m10", "normal"));
            monster.Add(new Monsters(11, "丑灵", new int[] { 105, 140, 200, 280, 360 }, new int[] {32, 42, 64, 84, 116 }, new int[] { 20, 28, 36, 45, 60 }, new int[] { 11, 13, 16, 20, 26 }, new int[] { 20, 80, 300, 800, 1800 }, new int[] { 1, 3, 8, 15, 30 }, "Monsters/m11", "normal"));

            monster.Add(new Monsters(12, "金龙", new int[] { 200, 280, 390, 530, 730 }, new int[] {50, 70, 100, 140, 200 }, new int[] { 30, 40, 60, 85, 120 }, new int[] { 11, 13, 16, 20, 26 }, new int[] { 40, 160, 600, 800, 3600 }, new int[] { 2, 6, 16, 30, 60 }, "Monsters/boss1", "boss"));
            monster.Add(new Monsters(13, "红龙", new int[] { 200, 280, 390, 530, 730 }, new int[] {60, 80, 120, 170, 240 }, new int[] { 25, 35, 50, 70, 100 }, new int[] { 8, 10, 12, 15, 20 }, new int[] { 50, 200, 750, 1000, 4500 }, new int[] { 2, 6, 16, 30, 60 }, "Monsters/boss2", "boss"));
            monster.Add(new Monsters(14, "灵龙", new int[] { 180, 260, 370, 510, 710 }, new int[] {50, 70, 120, 140, 200 }, new int[] { 25, 35, 50, 70, 100 }, new int[] { 15, 18, 25, 30, 38 }, new int[] { 60, 240, 900, 1200, 5400 }, new int[] { 2, 6, 16, 30, 60 }, "Monsters/boss3", "boss"));
            monster.Add(new Monsters(15, "魔龙", new int[] { 180, 260, 370, 510, 710 }, new int[] {55, 75, 110, 155, 220 }, new int[] { 30, 40, 60, 85, 120 }, new int[] { 11, 13, 16, 20, 26 }, new int[] { 80, 320, 1200, 1600, 7200 }, new int[] { 2, 6, 16, 30, 60 }, "Monsters/boss4", "boss"));

            collectPage.Add(0, "天生九奇");
            collectPage.Add(1, "九曜仙兵");

            collectItem.Add(new CollectItem(0, "往生符", 0, "Collections/item9", "往生符，送人往生，无视实力差距，使用后损毁，经历万年方可再度塑形"));
            collectItem.Add(new CollectItem(1, "土灵珠", 0, "Collections/item10", "土灵珠，五灵珠之一，镇守北方方，由圣兽玄武守护"));
            collectItem.Add(new CollectItem(2, "死亡卷宗", 0, "Collections/item11", "死亡卷宗，断人生死，在死亡卷宗上写上对方生辰八字，并言明死法，则当死亡卷宗被毁时，对方会按照卷宗上的死法死去。传说。。。"));
            collectItem.Add(new CollectItem(3, "水灵珠", 0, "Collections/item12", "水灵珠，五灵珠之一，镇守东方，由圣兽青龙守护"));
            collectItem.Add(new CollectItem(4, "木灵珠", 0, "Collections/item13", "木灵珠，五灵珠之一，镇守中央，由圣兽麒麟守护"));
            collectItem.Add(new CollectItem(5, "雷灵珠", 0, "Collections/item14", "雷灵珠，五灵珠之一，镇守西方，由圣兽白虎守护"));
            collectItem.Add(new CollectItem(6, "回魂雪莲", 0, "Collections/item15", "回魂雪莲，起死回生，死人用之，可聚拢散去的三魂七魄，修复损毁的肉身，形成先天胚胎，在七七四十九天之内恢复到死前状态，并拥有先天之体。传说赤脚大仙等神仙下凡寻找张百忍担任昊天金阙无上至尊自然妙有弥罗至真玉皇上帝时，张百忍遭遇奸人毒害身死，赤脚大仙寻来回魂雪莲，使其起死回生，并成就先天之体，担任玉帝管理天界"));
            collectItem.Add(new CollectItem(7, "火灵珠", 0, "Collections/item16", "火灵珠，五灵珠之一，镇守南方，由圣兽朱雀守护"));
            collectItem.Add(new CollectItem(8, "离情箫", 0, "Collections/item17", "离情箫，箫响情离，一曲恩仇断，从此不相问。传说。。。"));
            collectItem.Add(new CollectItem(9, "日曜太阳璧", 1, "Collections/item0","日曜太阳上尊的专属仙兵，传说。。。"));
            collectItem.Add(new CollectItem(10, "月曜太阴玉", 1, "Collections/item1", "月曜太阴上尊的专属仙兵，传说。。。"));
            collectItem.Add(new CollectItem(11, "火曜荧惑戟", 1, "Collections/item2", "火曜荧惑上尊的专属仙兵，传说。。。"));
            collectItem.Add(new CollectItem(12, "水曜星辰裳", 1, "Collections/item3", "水曜星辰上尊的专属仙兵，传说。。。"));
            collectItem.Add(new CollectItem(13, "木曜太岁幡", 1, "Collections/item4", "木曜太岁上尊的专属仙兵，传说。。。"));
            collectItem.Add(new CollectItem(14, "金曜太白剑", 1, "Collections/item5", "金曜太白上尊的专属仙兵，传说。。。"));
            collectItem.Add(new CollectItem(15, "土曜镇魂葫", 1, "Collections/item6", "土曜镇魂上尊的专属仙兵，传说。。。"));
            collectItem.Add(new CollectItem(16, "暗曜罗睺刀", 1, "Collections/item7", "暗曜罗睺上尊的专属仙兵，传说。。。"));
            collectItem.Add(new CollectItem(17, "隐曜计都枪", 1, "Collections/item8", "隐曜计都上尊的专属仙兵，传说。。。"));

            collectItemPos.Add(new Vector3(-350.0f, 350f, 0f));
            collectItemPos.Add(new Vector3(0f, 350f, 0f));
            collectItemPos.Add(new Vector3(350f, 350f, 0f));
            collectItemPos.Add(new Vector3(-350f, 0f, 0f));
            collectItemPos.Add(new Vector3(0f, 0f, 0f));
            collectItemPos.Add(new Vector3(350f, 0f, 0f));
            collectItemPos.Add(new Vector3(-350f, -350f, 0f));
            collectItemPos.Add(new Vector3(0f, -350f, 0f));
            collectItemPos.Add(new Vector3(350f, -350f, 0f));
        }
    }
}

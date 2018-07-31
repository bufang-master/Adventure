using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripts.Guide;
using Scripts.Service.User;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Strengthen
{
    public class Ability: MonoBehaviour
    {
        public int skillType;

        private uint costCoin;
        private uint costOCoin;
        private Skill skill;
        private int level;

        private void Start()
        {
            var ud = AppConfig.Value.mainUserData;
            level = ud.skill_level[skillType];
            skill = GameData.skill[skillType];
            if (level < skill.maxLevel)
            {
                costCoin = (uint)skill.costCoin[level-1];
                costOCoin = (uint)skill.costOCoin[level-1];
            }
        }

        public void OnBtnUplevel()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            costCoin = (uint)skill.costCoin[level - 1];
            costOCoin = (uint)skill.costOCoin[level - 1];
            UIAPI.ShowMsgBox("技能升级", "是否花费" + costCoin + "金币和" + costOCoin + "灵石升级" + skill.skillName, "确定|取消", UpLevel);
        }

        private void UpLevel(object arg)
        {
            if ((int)arg == 0)
            {
                var ud = AppConfig.Value.mainUserData;
                if (ud.coins < costCoin || ud.ocoins < costOCoin)
                {
                    UIAPI.ShowMsgBox("升级失败", "金币或者灵石不足", "确定");
                }
                else
                {
                    level++;
                    ud.skill_level[skillType]++;
                    ud.coins -= costCoin;
                    ud.ocoins -= costOCoin;

                    AppConfig.Value.mainUserData = ud;
                    AppConfig.Save();
                }
            }
        }
        private void OnGUI()
        {
            SetSkill(skill.skillName + "(Lv" + level + ")", JointInfo(skillType,level));
        }
        private string JointInfo(int i, int level)
        {
            var s = GameData.skill[i];
            string info = null;
            if (level<skill.maxLevel)
            {
                int n1 = s.num1[level] - s.num1[level - 1];
                string s1 = n1.ToString();
                if (s.num1[level-1] < 0)
                    s1 = "";
                else if (n1 >= 0)
                    s1 = "+" + s1;

                int n2 = (s.num2[level] - s.num2[level - 1]);
                string s2 = n2.ToString();
                if (s.num2[level-1] < 0)
                    s2 = "";
                else if (n2 >= 0)
                    s2 = "+" + s2;

                info = s.skillInfo[0] + s.num1[level - 1]
                    + "<color=#ff0000>" + s1 + "</color>"
                    + s.skillInfo[1] + (s.num2[level - 1] > 0 ? s.num2[level - 1].ToString() : "") +
                    "<color=#ff0000>" + s2 + "</color>"
                    + s.skillInfo[2];
            }else
            {
                info = s.skillInfo[0] + s.num1[level - 1] + s.skillInfo[1] + (s.num2[level - 1] > 0 ? s.num2[level - 1].ToString() : "") + s.skillInfo[2];
            }
            return info;
        }
        private void SetSkill(string skillName, string skillInfo)
        {
            foreach (Transform child in this.transform)
            {
                if (child.name == "SkillName")
                {
                    child.GetComponent<Text>().text = skillName;
                }
                else if (child.name == "SkillInfo")
                {
                    child.GetComponent<Text>().text = skillInfo;
                }else if(child.name == "SkillUp")
                {
                    if (level >= skill.maxLevel)
                    {
                        child.GetComponentInChildren<Text>().text = "已满级";
                        child.GetComponent<Button>().enabled = false;
                    }
                }
            }
        }
    }
}

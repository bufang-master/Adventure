using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SGF.UI.Framework;
using UnityEngine.UI;


namespace Scripts.UI.Strengthen
{
    public class SkillPage:MonoBehaviour
    {
        public GameObject skillPanel;
        public Sprite[] skillImage;

        private GameObject skill;

        private void Start()
        {
            var ud = AppConfig.Value.mainUserData;
            var skillLevel = ud.skill_level;
            for (int i = 0; i < 6; i++)
            {
                if (skillLevel[i] > 0)
                {
                    AddChild("skill" + (i + 1), i);
                    foreach (Transform child in skill.transform)
                    {
                        if (child.name == "SkillImage")
                        {
                            child.GetComponent<Image>().sprite = skillImage[i];
                            break;
                        }
                    }
                }
            }
        }
        
        private void AddChild(string name,int type)
        {
            GameObject child = UIRes.LoadPrefab(UIDef.UISkill);
            if (child != null)
            {
                skill = GameObject.Instantiate(child);
                skill.name = name;
                skill.transform.SetParent(skillPanel.transform,false);
                
                skill.GetComponent<Ability>().skillType = type;
            }
        }
    }
}

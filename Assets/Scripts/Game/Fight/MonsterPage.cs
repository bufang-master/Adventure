using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Game.Fight
{
    public class MonsterPage:MonoBehaviour
    {
        public GameObject FightPanel;
        public void MonsterAttack()
        {
            FightPanel.GetComponent<FightPage>().p_defanse = true;
        }
        public void AddHP()
        {
            FightPanel.GetComponent<FightPage>().p_addHP = true;
        }
    }
}

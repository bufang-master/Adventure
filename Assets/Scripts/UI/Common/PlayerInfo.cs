using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace Scripts.UI.Common
{
    public class PlayerInfo:MonoBehaviour
    {
        public Text[] stat;

        private void Start()
        {
            ShowInfo();
        }
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Destroy(this.gameObject);
            }
        }
        private void ShowInfo()
        {
            var ud = AppConfig.Value.mainUserData;
            string colorStr = "white";
            for(int i = 0;i < stat.Count(); i++)
            {
                if (ud.stat_level[i] > 0 && ud.stat_level[i] <= 2)
                    colorStr = "white";
                else if (ud.stat_level[i] <= 4)
                    colorStr = "green";
                else if (ud.stat_level[i] <= 6)
                    colorStr = "blue";
                else if (ud.stat_level[i] <= 8)
                    colorStr = "red";
                else
                    colorStr = "purple";
                stat[i].text = ud.stat_num[i].ToString()+"（<color="+colorStr+">Lv"+ud.stat_level[i]+"</color>）";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Common
{
    public class HideConsole:MonoBehaviour
    {
        public InputField input;

        public void AlterData()
        {
            string text = input.text;
            if(text != null && text.Contains("|"))
            {
                string item = text.Split('|')[0];
                int data = int.Parse(text.Split('|')[1]);
                switch (item)
                {
                    case "coin":
                        AppConfig.Value.mainUserData.coins = (uint)data;
                        AppConfig.Save();
                        break;
                    case "ocoin":
                        AppConfig.Value.mainUserData.ocoins = (uint)data;
                        AppConfig.Save();
                        break;
                    case "adven":
                        AppConfig.Value.mainUserData.Statistics[GameData.Statist[(int)GameData.SID.ADVEN]] = data;
                        AppConfig.Save();
                        break;
                    case "stone":
                        if (data >= 0 && data <= 2)
                        {
                            AppConfig.Value.mainUserData.stone[data] = true;
                            AppConfig.Save();
                        }
                        break;
                    case "food":
                        AppConfig.Value.mainUserData.food = (uint)data;
                        AppConfig.Save();
                        break;
                    case "collection":
                        if (data >= 0 && data < GameData.collectItem.Count())
                        {
                            AppConfig.Value.mainUserData.collection_data[data] = 1;
                            AppConfig.Save();
                        }
                        break;
                 }
            }
            Destroy(this.gameObject);
        }
    }
}

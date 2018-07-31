using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class BottomBar:MonoBehaviour
    {
        public GameObject point;
        public Text info;
        public GameObject infoPanel;
        public Image mask;
        public Text bossPos;

        private bool fadeStart;
        private float fadeTime;
        private Color color_p;
        private Color color_i;
        private float coolTime;
        private float maxCoolTime;
        private DateTime lastCall;
        private void Start()
        {
            fadeTime = 0;
            fadeStart = false;
            color_i = info.color;
            color_p = infoPanel.GetComponent<Image>().color;
            coolTime = 0;
            maxCoolTime = 20.0f;
            bossPos.text = "";
        }
        private void Update()
        {
            if (fadeStart && infoPanel.activeSelf)
            {
                fadeTime += Time.deltaTime;
                infoPanel.GetComponent<Image>().color = new Color(83 / 255f, 83 / 255f, 83 / 255f, Mathf.Lerp(color_p.a, 0, fadeTime));
                info.color = new Color(1, 1, 1, Mathf.Lerp(color_i.a, 0, fadeTime));
            }else
            {
                fadeStart = false;
                fadeTime = 0;
            }
            if (!GameDef.isFight && coolTime!=0)
            {
                coolTime -= Time.deltaTime;
                if (coolTime <= 0)
                    coolTime = 0;
                mask.fillAmount = 1.0f * coolTime / maxCoolTime;
            }
        }

        public void ShowBOSSPos()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (coolTime == 0 && !GameDef.isFight)
            {
                coolTime = maxCoolTime;
                StartCoroutine(SearchBoss());
            }
        }

        private IEnumerator SearchBoss()
        {
            bossPos.text = "<size=50><color=white>正在搜寻BOSS</color></size>";
            yield return new WaitForSeconds(2f);
            AudioControl.PlayEffect(GameDef.findEffect);
            var data = GameControl.gameData;
            int bX = data.bossX;
            int bY = data.bossY;
            int pX = data.playerX;
            int pY = data.playerY;
            string pos;
            if (pY == bY)
            {
                if (pX > bX)
                {
                    pos = "<size=60><color=red>正北</color></size>";
                }
                else
                {
                    pos = "<size=60><color=red>正南</color></size>";
                }
            }
            else if (pX == bX)
            {
                if (pY > bY)
                {
                    pos = "<size=60><color=red>正西</color></size>";
                }
                else
                {
                    pos = "<size=60><color=red>正东</color></size>";
                }
            }
            else if (pY > bY)
            {
                if (pX > bX)
                {
                    pos = "<size=60><color=red>西北</color></size>";
                }
                else
                {
                    pos = "<size=60><color=red>西南</color></size>";
                }
            }
            else
            {
                if (pX > bX)
                {
                    pos = "<size=60><color=red>东北</color></size>";
                }
                else
                {
                    pos = "<size=60><color=red>东南</color></size>";
                }
            }
            bossPos.text = pos;
        }

        public void ShowMapInfo()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            if (!infoPanel.activeSelf && !GameDef.isFight)
            {
                string name = point.name;
                int pointX = int.Parse(name.Split(',')[0]);
                int pointY = int.Parse(name.Split(',')[1]);
                int mapID = GameControl.gameData.mapData[pointX][pointY];
                var map = GameData.mapInfo[mapID];
                StringBuilder sb = new StringBuilder();
                sb.Append("<size=60>" + map.name + "</size>\n");
                sb.Append("HP" + (map.costHP > 0 ? "<color=green>+" + map.costHP + "</color>" : "<color=red>" + map.costHP + "</color>") + "\t");
                sb.Append("食物" + "<color=red>-" + map.costFood + "</color>\n");
                sb.Append("遇怪率：" + (map.bizarre * 100).ToString("0.00") + "%\n");
                sb.Append("<color=red>特殊</color>：");
                switch (map.type.Split('|')[0])
                {
                    case "none":
                        sb.Append("无");
                        break;
                    case "climb":
                        sb.Append("可攀登，视野<color=green>+1</color>，<color=red>食物-1</color>");
                        break;
                    case "collect":
                        sb.Append("可采集，食物<color=green>+" + map.type.Split('|')[1] + "</color>");
                        break;
                    case "explore":
                        sb.Append("可探索，成功率" + map.type.Split('|')[1] + "%");
                        if (GameControl.gameData.mapInfo[pointX, pointY] != 0)
                        {
                            sb.Append("<color=red>(已探索)</color>");
                        }
                        break;
                }
                info.text = sb.ToString();
                infoPanel.SetActive(true);
                lastCall = DateTime.Now;
                StartCoroutine(Fade(lastCall));
            }else
            {
                infoPanel.SetActive(false);
            }
        }

        private IEnumerator Fade(DateTime last)
        {
            yield return new WaitForSeconds(4.0f);
            if(lastCall == last)
            {
                if (infoPanel.activeSelf)
                {
                    fadeStart = true;
                }
                yield return new WaitForSeconds(1f);
                infoPanel.SetActive(false);
                infoPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                info.color = new Color(1, 1, 1, 1);
            }
        }
    }
}

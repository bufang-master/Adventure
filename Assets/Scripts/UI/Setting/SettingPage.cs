using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Setting
{
    public class SettingPage: UIPage
    {
        public Image musicBtn;
        public Image effectBtn;
        public Sprite[] status;

        private int num;

        private void Start()
        {
            var ud = AppConfig.Value;
            if (ud.enableBgMusic)
            {
                musicBtn.sprite = status[1];
            }else
            {
                musicBtn.sprite = status[0];
            }
            if (ud.enableSoundEffect)
            {
                effectBtn.sprite = status[1];
            }else
            {
                effectBtn.sprite = status[0];
            }
            num = 0;
        }

        public void OnMusicBtn()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            AppConfig.Value.enableBgMusic = !AppConfig.Value.enableBgMusic;
            if (AppConfig.Value.enableBgMusic)
            {
                musicBtn.sprite = status[1];
                AudioControl.PlayBGMusic(AudioControl.audioSourceBG);
            }
            else
            {
                musicBtn.sprite = status[0];
                AudioControl.StopBGMusic();
            }

            AppConfig.Save();
        }

        public void OnEffectBtn()
        {
            AppConfig.Value.enableSoundEffect = !AppConfig.Value.enableSoundEffect;
            if (AppConfig.Value.enableSoundEffect)
            {
                effectBtn.sprite = status[1];
            }
            else
            {
                effectBtn.sprite = status[0];
            }
            AudioControl.PlayEffect(GameDef.clickEffect);
            AppConfig.Save();
        }
        
        public void OnResetBtn()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            UIAPI.ShowMsgBox("重置", "是否确定重置新手引导？", "确定|取消", delegate(object arg) {
                if ((int)arg == 0)
                {
                    AudioControl.PlayEffect(GameDef.findEffect);
                    var ud = AppConfig.Value.mainUserData;
                    List<string> key = new List<string>();
                    key.AddRange(ud.GuideFlag.Keys);
                    foreach(var i in key)
                    {
                        ud.GuideFlag[i] = false;
                    }
                    AppConfig.Value.mainUserData = ud;
                    AppConfig.Save();
                }
            });
        }

        public void OnCloseBtn()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            Destroy(gameObject);
        }

        public void OnHideBtn()
        {
            num++;
            if (num == 5)
            {
                GameObject go = Resources.Load(UIDef.UIHideConsole) as GameObject;
                if (go != null)
                {
                    GameObject obj = Instantiate(go);
                    GameObject root = GameObject.Find("UIRoot");
                    if (root == null || obj == null)
                    {
                        return;
                    }
                    obj.transform.SetParent(root.transform, false);
                }
            }
        }
    }
}

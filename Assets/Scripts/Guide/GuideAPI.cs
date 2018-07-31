using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripts.Game;
using Scripts.UI.Common;
using Scripts.UI.Trans;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Scripts.Guide
{
    public class GuideAPI
    {
        public const string EnterTransPage = "EnterTransPage";
        public const string PlayerInfo = "PlayerInfo";
        public const string AdventureStart = "AdventureStart";
        public const string BuyFood = "BuyFood";
        public const string StatEnhance = "StatEnhance";
        public const string FightStart = "FightStart";
        public const string FightEnd = "FightEnd";

        internal static void AddChild(GameObject child)
        {
            GameObject root = GameObject.Find("UIRoot");
            if (root == null || child == null)
            {
                return;
            }
            child.transform.SetParent(root.transform, false);
            return;
        }


        //internal static void StatEnhanceFunc()
        //{
        //    if (!AppConfig.Value.mainUserData.GuideFlag[StatEnhance])
        //    {
        //        GameObject go = Resources.Load(UIDef.UIStatEnhanceGuide) as GameObject;
        //        if (go != null)
        //        {
        //            GameObject obj = GameObject.Instantiate(go);
        //            AddChild(obj);
        //            Button onTargetClick = GameObject.Find("OnTargetClick").GetComponent<Button>();
        //            Debug.Log("go");
        //            onTargetClick.onClick.AddListener(delegate ()
        //            {
        //                Debug.Log("destory");
        //                GameObject.Destroy(obj);
        //                AppConfig.Value.mainUserData.GuideFlag[StatEnhance] = true;
        //                AppConfig.Save();
        //            });
        //        }
        //    }
        //}



        internal static void FightEndFunc()
        {
            if (!AppConfig.Value.mainUserData.GuideFlag[FightEnd] && AppConfig.Value.mainUserData.GuideFlag[FightStart])
            {
                GameObject go = Resources.Load(UIDef.UIFightEndGuide) as GameObject;
                if (go != null)
                {
                    GameObject obj = GameObject.Instantiate(go);
                    AddChild(obj);
                    Button onTargetClick = GameObject.Find("OnTargetClick").GetComponent<Button>();
                    onTargetClick.onClick.AddListener(() =>
                    {
                        GameObject.Find("Bar").GetComponent<NavControler>().OnBtnStrengthen();
                        GameObject.Destroy(obj);
                        AppConfig.Value.mainUserData.GuideFlag[FightEnd] = true;
                        AppConfig.Save();
                    });
                }
            }
        }

        internal static void EnterTransPageFunc()
        {
            UIAPI.ShowMsgBox("新手引导", "伟大的冒险者，欢迎来到<color=red>冒险！冒险！</color>,初次光临，请跟随引导熟悉这个世界", "确定", arg => {
                GameObject go = Resources.Load(UIDef.UIEnterAdventureGuide) as GameObject;
                if (go != null)
                {
                    GameObject obj = GameObject.Instantiate(go);
                    AddChild(obj);
                    GameObject.Find("OnTargetClick").GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        GameObject transPage = GameObject.Find("TransPage");
                        if (transPage != null)
                        {
                            transPage.GetComponent<TransPage>().OnBtnTrans();
                            GameObject.Destroy(obj);
                            AppConfig.Value.mainUserData.GuideFlag[EnterTransPage] = true;
                            AppConfig.Save();
                        }
                    });
                            //i.GetComponent<Button>().onClick.AddListener(delegate (){
                            //    GameObject bar = GameObject.Find("Bar");
                            //    if (bar != null)
                            //    {
                            //        bar.GetComponent<NavControler>().OnBtnInfo();
                            //        GameObject.Destroy(GameObject.Find("BootArrow"));
                            //        GameObject guideInfo = GameObject.Find("GuideInfo");
                            //        if (guideInfo != null)
                            //        {
                            //            //guideInfo.GetComponent<Text>().text = "<size=65>血量是生命的保证，攻击与防御决定伤害，</size>"
                            //        }
                            //    }
                            //});
                }else
                {
                    Debug.Log(UIDef.UIEnterAdventureGuide+" is not exist!");
                }
            });
        }

        internal static void BuyFoodFunc()
        {
            if (!AppConfig.Value.mainUserData.GuideFlag[BuyFood])
            {
                UIAPI.ShowMsgBox("新手引导","食物不足以支撑您的下次冒险，您可以等待一段时间（一小时收获50食物），或者点击食物旁的加号使用金币购买。","购买|等待",arg=> {
                    if ((int)arg == 0)
                    {
                        GameObject.Find("Bar").GetComponent<NavControler>().OnBtnBuy();
                    }
                });
                AppConfig.Value.mainUserData.GuideFlag[BuyFood] = true;
                AppConfig.Save();
            }
        }

        internal static void AdventureStartFunc()
        {
            if (!AppConfig.Value.mainUserData.GuideFlag[AdventureStart])
            {
                UIAPI.ShowMsgBox("新手引导", "欢迎进入冒险世界，跟随指引熟悉一下界面吧！", "确定",arg=> {
                GameObject go = Resources.Load(UIDef.UIAdventurePanelGuide) as GameObject;
                    if (go != null)
                    {
                        GameObject obj = GameObject.Instantiate(go);
                        AddChild(obj);
                        Text guideInfo = GameObject.Find("GuideInfo").GetComponent<Text>();
                        GameObject bootArrow = GameObject.Find("BootArrow");
                        Button onTargetClick = GameObject.Find("OnTargetClick").GetComponent<Button>();
                        bootArrow.transform.localPosition = new Vector3(200, 900, 0);
                        bootArrow.transform.SetScaleXYZ(2, 2, 2);
                        bootArrow.transform.localEulerAngles = new Vector3(0, 0, -90);
                        guideInfo.text = "人物血量，低于0，则丢失在当前世界获得的全部物品，返回主世界";
                        onTargetClick.onClick.AddListener(() =>
                        {
                            bootArrow.transform.localPosition = new Vector3(200, 800, 0);
                            guideInfo.text = "食物，人物行走消耗一定量的食物，当食物消耗完，则消耗两倍量的生命";
                            onTargetClick.onClick.RemoveAllListeners();
                            onTargetClick.onClick.AddListener(() =>
                            {
                                bootArrow.transform.localPosition = new Vector3(200, 700, 0);
                                guideInfo.text = "攻击和防御是决定伤害的两个属性，己方攻击减去敌方防御即为对敌方造成的伤害";
                                onTargetClick.onClick.RemoveAllListeners();
                                onTargetClick.onClick.AddListener(() =>
                                {
                                    bootArrow.transform.localPosition = new Vector3(-400, -660, 0);
                                    bootArrow.transform.SetScaleXYZ(3, 3, 3);
                                    bootArrow.transform.localEulerAngles = Vector3.zero;
                                    guideInfo.text = "单击选择一块地图，再点击左下角查询，可获取所选地图的信息，只能单击人物周围两格的地图哦！";
                                    onTargetClick.onClick.AddListener(() =>
                                    {
                                        guideInfo.text = "当人物行走在有特殊的地图块上时，点击按钮可进行相应的操作";
                                        bootArrow.transform.localPosition = new Vector3(0, -660, 0);
                                        onTargetClick.onClick.RemoveAllListeners();
                                        onTargetClick.onClick.AddListener(() =>
                                        {
                                            guideInfo.text = "点击右下角的搜索，可探寻BOSS相对于你的位置。人物行走通过在屏幕上<color=red>滑动</color>，向着BOSS进发吧，冒险者！";
                                            bootArrow.transform.localPosition = new Vector3(400, -660, 0);
                                            onTargetClick.onClick.RemoveAllListeners();
                                            onTargetClick.onClick.AddListener(() =>
                                            {
                                                GameObject.Destroy(obj);
                                                AppConfig.Value.mainUserData.GuideFlag[AdventureStart] = true;
                                                AppConfig.Save();
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    }
                });
            }
        }

        internal static void FightStartFunc()
        {
            if (!AppConfig.Value.mainUserData.GuideFlag[FightStart])
            {
                GameObject go = Resources.Load(UIDef.UIFightStartGuide) as GameObject;
                if (go != null)
                {
                    GameObject obj = GameObject.Instantiate(go);
                    AddChild(obj);
                    GameObject targetPoint = GameObject.Find("TargetPointGuide");
                    Text guideInfo = GameObject.Find("GuideInfo").GetComponent<Text>();
                    Button onTargetClick = GameObject.Find("OnTargetClick").GetComponent<Button>();
                    guideInfo.text = "遇上怪物了，跟随指引了解一下战斗方式吧！";
                    onTargetClick.onClick.AddListener(() =>
                    {
                        SetTargetPoint(targetPoint, "M_LevelGuide");
                        guideInfo.text = "怪物星级，冒险者越强，怪物星级越高，击败后获得的奖励越多。";
                        onTargetClick.onClick.AddListener(() =>
                        {
                            SetTargetPoint(targetPoint, "M_StatGuide");
                            guideInfo.text = "怪物的最大攻击和防御以及敏捷属性，初始攻防比为5:5，生命低于10%放弃防御，全力攻击。";
                            onTargetClick.onClick.AddListener(() =>
                            {
                                SetTargetPoint(targetPoint, "M_VolumeGuide","P_VolumeGuide");
                                guideInfo.text = "左右分别为敌人与己方的进度条，当一方进度条满，则轮到该方攻击，进度条增长速度由敏捷决定。";
                                onTargetClick.onClick.AddListener(() =>
                                {
                                    SetTargetPoint(targetPoint, "ATCInfoGuide", "DEFInfoGuide");
                                    guideInfo.text = "左右为己方攻防信息，点击加号调整攻击防御比，提升攻击的同时会降低防御，十成攻击和十成防御都会触发被动技能哦！";
                                    onTargetClick.onClick.AddListener(() =>
                                    {
                                        SetTargetPoint(targetPoint, "SkillInfoGuide");
                                        guideInfo.text = "框选为技能栏，长按可查看技能效果，技能有冷却时间，一次攻击可使用多个技能。";
                                        onTargetClick.onClick.AddListener(() =>
                                        {
                                            SetTargetPoint(targetPoint, "AttackGuide");
                                            guideInfo.text = "准备就绪后，点击<color=red>战</color>，即可攻击敌人。";
                                            onTargetClick.onClick.AddListener(() =>
                                            {
                                                GameObject.Destroy(obj);
                                                AppConfig.Value.mainUserData.GuideFlag[FightStart] = true;
                                                AppConfig.Save();
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                } else
                {
                    Debug.Log(UIDef.UIFightStartGuide + " is not exist!");
                }
            }
        }

        private static void SetTargetPoint(GameObject parent,params string[] name) {
            foreach(Transform child in parent.transform)
            {
                foreach(string i in name)
                {
                    if(child.name == i)
                    {
                        child.gameObject.SetActive(true);
                        child.DOShakePosition(3f, 5);
                        break;
                    }
                    child.gameObject.SetActive(false);
                }
            }
        }


    }
}

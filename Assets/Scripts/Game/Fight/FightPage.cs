using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Fight
{
    public class FightPage:UIPage
    {
        public GameObject monsterPanel;
        public GameObject playerPanel;
        public GameObject skillPanel;
        public Image mTime;
        public Image pTime;
        public Image mHP;
        public Text mHPInfo;
        public Text mATCInfo;
        public Text mDEFInfo;
        public Text mAGIInfo;
        public Text mName;
        public Image mLevel;
        public Sprite[] starLevel;
        public Image[] skillMask;
        public GameObject pFrame;
        public GameObject mFrame;

        public Text pATCPer;
        public Text pDEFPer;
        public Text pATC;
        public Text pDEF;

        public GameObject player;
        public Image mSprite;
        public GameObject mHurt;
        public GameObject pHurt;
        public Transform sword;
        public Transform shield;
        private new Animation animation;
        public Button btnAddATC;
        public Button btnAddDEF;

        //[HideInInspector]
        public bool isStart;//为true时，计时开始，为false时，暂停计时
        private float m_time;//怪物回合计时
        private float p_time;//主角回合计时
        private float target;//目标时间，当计时达到该目标时，则轮到该方攻击
        private bool p_attack;//播放主角攻击动画
        private bool m_attack;//播放怪物攻击动画
        private Monsters monster;//怪物实体
        private int m_level;//怪物等级
        private int m_c_hp;//怪物当前血量
        private int p_c_atc;//主角当前最大攻击力
        private int p_c_def;//主角当前最大防御力
        private int m_atcPer;
        private int m_defPer;
        private Skill[] p_skill;//人物技能
        private bool[] p_skill_enabel;
        private float[] cool_time;//冷却时间
        private float[] max_cool_time;

        private Animator hurt_anim;
        private bool isBoss;
        private bool fightWin;

        [HideInInspector]
        public bool p_defanse;//播放主角防御动画
        [HideInInspector]
        public bool p_addHP;
        [HideInInspector]
        public int jewData;

        private DateTime lastTouch;
        private bool isShowSkillInfo;

        private GameControl gameControl;

        private void Awake()
        {
            gameControl = GameObject.Find("GameControl").GetComponent<GameControl>();
            var ud = AppConfig.Value.mainUserData;
            target = 0.5f + GameControl.gameData.AGI / 10.0f;
            animation = player.GetComponent<Animation>();
            p_skill_enabel = new bool[] { false, false, false, false, false, false };
            cool_time = new float[4];
            max_cool_time = new float[4];
            for(int i = 0;i < max_cool_time.Length; i++)
            {
                max_cool_time[i] = (GameData.skill[i].num2[ud.skill_level[i]-1] + 1) * target;
            }

        }

        public void Init(int p)
        {
            AudioControl.StopBGMusic();
            AudioControl.PlayBGMusic(GetComponent<AudioSource>());

            GameDef.isFight = true;
            fightWin = false;
            isStart = true;
            ResetSkill();
            for(int i = 0; i < cool_time.Length; i++)
            {
                cool_time[i] = 0;
            }
            foreach(var i in skillMask)
            {
                i.fillAmount = 0;
            }
            jewData = p;
            SetMonster();
            SetPlayer();

            SetSkill();
        }

        //初始化主角
        private void SetPlayer()
        {
            p_time = target * 0.5f;
            p_attack = false;
            p_defanse = false;
            p_addHP = false;

            SetADNum(5);

            p_c_atc = GameControl.gameData.ATC;
            p_c_def = GameControl.gameData.DEF;

            btnAddATC.enabled = false;
            btnAddDEF.enabled = false;
        }
        //初始化怪物
        private void SetMonster()
        {
            m_time = 0f;
            m_defPer = 5;
            m_atcPer = 5;
            m_attack = false;
            //随机怪物类型与等级
            System.Random rom = new System.Random();
            if (jewData < 75)
            {
                isBoss = false;
                monster = GameData.monster[rom.Next(GameData.monster.Where(p => p.type == "normal").Count())];
                mName.text = monster.name;
            }
            else
            {
                isBoss = true;
                monster = GameData.monster[GameControl.gameData.BOSSID];
                mName.text =  "<color=red>" + monster.name + "</color>";
            }
            m_level = CalLevel(rom.Next(-3, 3));
            m_c_hp = monster.m_HP[m_level];
            mATCInfo.text = monster.m_ATC[m_level].ToString();
            mDEFInfo.text = monster.m_DEF[m_level].ToString();
            mAGIInfo.text = monster.m_AGI[m_level].ToString();
            mLevel.sprite = starLevel[m_level];
            mSprite.sprite = Resources.Load<Sprite>(monster.path);

            UpdataInfo();
        }

        private void SetSkill()
        {
            p_skill = new Skill[4];
            for(int i = 0;i < 4; i++)
            {
                p_skill[i] = GameData.skill[i];
            }
        }

        private int CalLevel(int n)
        {
            int l = GameControl.gameData.level + n;
            if (l < 11)
            {
                return 0;
            }else if (l < 18)
            {
                return 1;
            }else if (l < 25)
            {
                return 2;
            }else if (l < 32)
            {
                return 3;
            }else
            {
                return 4;
            }
        }

        private void OnGUI()
        {
            if(isStart)
            {
                mTime.transform.SetScaleX(m_time / target);
                pTime.transform.SetScaleX(p_time / target);
            }
            else
            {
                if (m_time == 0)
                {
                    mTime.transform.SetScaleX(1);
                }
                if (p_time == 0)
                {
                    pTime.transform.SetScaleX(1);
                }
            }
        }
        private void Update()
        {
            if (isStart&&fightWin == false)
            {
                p_time += Time.deltaTime * GameControl.gameData.AGI / 10.0f;
                if(p_time >= target)
                {
                    btnAddATC.enabled = true;
                    btnAddDEF.enabled = true;
                    p_attack = true;
                    isStart = false;
                    p_time = 0;
                    pFrame.SetActive(true);
                    playerPanel.transform.SetScaleXYZ(0.97f, 0.97f, 0.97f);
                }
                m_time += Time.deltaTime * monster.m_AGI[m_level] / 10.0f;
                if(m_time >= target)
                {
                    m_attack = true;
                    isStart = false;
                    m_time = 0;
                    mSprite.GetComponent<Animator>().SetBool("attack", true);
                    if (1.0f * m_c_hp / monster.m_HP[m_level] <= 0.1f)//怪物血量低于10%，放弃防御，全力攻击
                    {
                        m_atcPer = 10;
                        m_defPer = 0;
                    }
                    mFrame.SetActive(true);
                    monsterPanel.transform.SetScaleXYZ(0.97f, 0.97f, 0.97f);
                }
                for(int i = 0; i < cool_time.Length; i++)
                {
                    if (cool_time[i] > 0)
                    {
                        cool_time[i] -= Time.deltaTime;
                        skillMask[i].fillAmount = 1.0f * cool_time[i] / max_cool_time[i];
                    }
                }
            }
            else
            {
                if (p_defanse)
                {
                    Defanse();
                    p_defanse = false;
                    m_attack = false;
                    isStart = true;
                    mSprite.GetComponent<Animator>().SetBool("attack", m_attack);
                    CalHurt_M();
                    mFrame.SetActive(false);
                    monsterPanel.transform.SetScaleXYZ(0.95f, 0.95f, 0.95f);
                }
                if (p_addHP)
                {
                    if (p_skill_enabel[5])
                    {
                        var ud = AppConfig.Value.mainUserData;
                        int atc = Mathf.RoundToInt(monster.m_ATC[m_level] * 1.0f * m_atcPer / 10);
                        int addHP = Mathf.RoundToInt(1.0f * atc * GameData.skill[5].num1[ud.skill_level[5]-1] / 100);
                        GameControl.gameData.currentHP += addHP;
                        p_skill_enabel[5] = false;
                        SetHurtInfo(pHurt.transform, addHP.ToString(), Color.green);
                    }
                    p_addHP = false;
                }
            }
            if(mHP.transform.localScale.x != 1.0f * m_c_hp / monster.m_HP[m_level])
            {
                mHP.transform.SetScaleX(Mathf.Lerp(mHP.transform.localScale.x, 1.0f * m_c_hp / monster.m_HP[m_level], Time.deltaTime * 4));
                if (Mathf.Abs(1.0f * m_c_hp / monster.m_HP[m_level] - mHP.transform.localScale.x )<= 0.01)
                {
                    mHP.transform.SetScaleX(1.0f * m_c_hp / monster.m_HP[m_level]);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hitInfo = Physics.RaycastAll(ray);
                if (hitInfo.Count()>0)
                {
                    foreach (var hit in hitInfo)
                    {
                        if (hit.collider.gameObject.tag == "Skill")
                        {
                            lastTouch = DateTime.Now;
                            StartCoroutine(IsHoldEnough(lastTouch));
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerator IsHoldEnough(DateTime last)
        {
            yield return new WaitForSeconds(0.5f);
            if(Input.GetMouseButton(0) && last == lastTouch)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hitInfo = Physics.RaycastAll(ray);
                if (hitInfo.Count() > 0)
                {
                    foreach (var hit in hitInfo)
                    {
                        if (hit.collider.gameObject.tag == "Skill")
                        {
                            isShowSkillInfo = true;
                            ShowSkillInfo(int.Parse(hit.collider.name));
                            break;
                        }
                    }
                }
            }
        }

        private void ShowSkillInfo(int id)
        {
            var go = UIRes.LoadPrefab(UIDef.UISkillInfo);
            if (go != null)
            {
                var s = GameData.skill[id];
                int level = AppConfig.Value.mainUserData.skill_level[id];
                string info = "<color=red><size=45>" + s.skillName + "</size></color>\n" 
                    + s.skillInfo[0] + s.num1[level - 1] + s.skillInfo[1] 
                    + (s.num2[level - 1] > 0 ? s.num2[level - 1].ToString() : "") + s.skillInfo[2];
                var skillInfoPanel = Instantiate(go);
                skillInfoPanel.transform.parent = skillPanel.transform;
                Vector3 skillPos = new Vector3();
                Vector3 skillAngle = new Vector3();
                switch (id)
                {
                    case 0:
                        skillPos = new Vector3(-330, 280, 0);
                        skillAngle = new Vector3(0, 180, 180);
                        break;
                    case 1:
                        skillPos = new Vector3(-330, 280, 0);
                        skillAngle = new Vector3(0, 0, 180);
                        break;
                    case 2:
                        skillPos = new Vector3(330, 280, 0);
                        skillAngle = new Vector3(0, 180, 180);
                        break;
                    case 3:
                        skillPos = new Vector3(330, 280, 0);
                        skillAngle = new Vector3(0, 0, 180);
                        break;
                }
                skillInfoPanel.transform.localPosition = skillPos;
                skillInfoPanel.transform.localEulerAngles = skillAngle;
                skillInfoPanel.transform.localScale = go.transform.localScale;
                foreach(Transform i in skillInfoPanel.transform)
                {
                    i.localEulerAngles = skillAngle;
                    i.GetComponent<Text>().text = info;
                }
            }else
            {
                Debug.Log("none");
            }
        }

        private void UpdataInfo()
        {
            int hp = monster.m_HP[m_level];
            mHPInfo.text = m_c_hp + " / " + hp;
        }

        public void AddATC()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            int atc_per = int.Parse(pATCPer.text);
            if (++atc_per > 10)
            {
                atc_per = 0;
            }
            SetADNum(atc_per);
        }

        public void AddDEF()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            int def_per = int.Parse(pDEFPer.text);
            if (++def_per > 10)
            {
                def_per = 0;
            }
            SetADNum(10 - def_per);
        }

        public void OnBtnSkill(int tag)
        {
            if (isShowSkillInfo)
            {
                isShowSkillInfo = false;
                return;
            }
            if (p_attack)
            {
                var ud = AppConfig.Value.mainUserData;
                var skill = GameData.skill[tag];
                switch (tag)
                {
                    case 0:
                        if (p_skill_enabel[0])
                        {
                            AudioControl.PlayEffect(GameDef.skillCancleEffect);
                            p_skill_enabel[0] = false;
                            cool_time[0] = 0;
                            skillMask[tag].fillAmount = 0;
                            p_c_atc = Mathf.RoundToInt(1.0f * p_c_atc / (100 + skill.num1[ud.skill_level[tag] - 1]) * 100);
                            SetADNum(int.Parse(pATCPer.text));
                            return;
                        }
                        else if (cool_time[tag] <= 0)
                        {
                            AudioControl.PlayEffect(GameDef.skill1Effect);
                            p_skill_enabel[0] = true;
                            p_c_atc = Mathf.RoundToInt(1.0f * p_c_atc * (100 + skill.num1[ud.skill_level[tag] - 1]) / 100);
                        }
                        break;
                    case 1:
                        if(p_skill_enabel[1])
                        {
                            AudioControl.PlayEffect(GameDef.skillCancleEffect);
                            p_skill_enabel[1] = false;
                            cool_time[1] = 0;
                            skillMask[tag].fillAmount = 0;
                            p_c_def = Mathf.RoundToInt(1.0f * p_c_def / (100 + skill.num1[ud.skill_level[tag] - 1]) * 100);
                            SetADNum(int.Parse(pATCPer.text));
                            return;
                        }
                        else if (cool_time[tag] <= 0)
                        {
                            AudioControl.PlayEffect(GameDef.skill1Effect);
                            p_skill_enabel[1] = true;
                            p_c_def = Mathf.RoundToInt(1.0f * p_c_def * (100 + skill.num1[ud.skill_level[tag] - 1]) / 100);
                        }
                        break;
                    case 2:
                        if(p_skill_enabel[2])
                        {
                            AudioControl.PlayEffect(GameDef.skillCancleEffect);
                            p_skill_enabel[2] = false;
                            cool_time[2] = 0;
                            skillMask[tag].fillAmount = 0;
                        }
                        else if(cool_time[tag] <= 0)
                        {
                            if (p_skill_enabel[3])
                            {
                                UIAPI.ShowMessage("连击与舍命不可同时使用");
                                AudioControl.PlayEffect(GameDef.skillCancleEffect);
                                return;
                            }
                            AudioControl.PlayEffect(GameDef.skill1Effect);
                            p_skill_enabel[2] = true;
                        }
                        break;
                    case 3:
                        if (p_skill_enabel[3])
                        {
                            AudioControl.PlayEffect(GameDef.skillCancleEffect);
                            p_skill_enabel[3] = false;
                            cool_time[3] = 0;
                            skillMask[tag].fillAmount = 0;
                        }
                        else if (cool_time[tag] <= 0)
                        {
                            int harm = Mathf.RoundToInt(GameControl.gameData.maxHP * GameData.skill[3].num1[AppConfig.Value.mainUserData.skill_level[3] - 1] / 100f);
                            if (GameControl.gameData.currentHP < harm)
                            {
                                UIAPI.ShowMessage("血量不足，无法使用技能");
                                AudioControl.PlayEffect(GameDef.skillCancleEffect);
                                return;
                            }

                            if (p_skill_enabel[2])
                            {
                                UIAPI.ShowMessage("连击与舍命不可同时使用");
                                AudioControl.PlayEffect(GameDef.skillCancleEffect);
                                return;
                            }
                            AudioControl.PlayEffect(GameDef.skill2Effect);
                            p_skill_enabel[3] = true;
                        }
                        break;
                }

                if (p_skill_enabel[tag])
                {
                    SkillAnimationPlay(tag);

                    cool_time[tag] = max_cool_time[tag];
                    skillMask[tag].fillAmount = 1;
                }
                SetADNum(int.Parse(pATCPer.text));
            }
        }

        private void SetADNum(int a)
        {
            bool flag = false;
            int d = 10 - a;
            //攻防比变更，flag用于判断是否播放技能动画
            if (a != int.Parse(pATCPer.text))
            {
                flag = true;
            }
            pATCPer.text = a.ToString();
            pDEFPer.text = d.ToString();
            pATC.text = Mathf.RoundToInt(p_c_atc * a / 10.0f).ToString();//四舍六入五成双
            pDEF.text = Mathf.RoundToInt(p_c_def * d / 10.0f).ToString();
            if (a == 10)
            {
                var ud = AppConfig.Value.mainUserData;
                pATC.text = Mathf.RoundToInt(1.0f * p_c_atc * (100 + GameData.skill[4].num1[ud.skill_level[4]-1]) / 100).ToString();
                p_skill_enabel[4] = true;
                if (flag)
                {
                    AudioControl.PlayEffect(GameDef.skillANDEffect);
                    SkillAnimationPlay(4);
                }
            }
            if(d == 10)
            {
                p_skill_enabel[5] = true;
                if (flag)
                {
                    AudioControl.PlayEffect(GameDef.skillANDEffect);
                    SkillAnimationPlay(5);
                }
            }else
            {
                p_skill_enabel[5] = false;
            }
            
            float scale = 0.5f + a / 10.0f;
            sword.SetScaleXYZ(scale, scale, scale);
            shield.SetScaleXYZ(2 - scale, 2 - scale, 2 - scale);
        }

        private void SkillAnimationPlay(int tag)
        {
            GameObject go = UIRes.LoadPrefab(UIDef.UISkillAnima);
            if (go != null)
            {
                GameObject skillAnima = Instantiate(go);
                skillAnima.GetComponent<Image>().sprite = Resources.Load<Sprite>(GameData.skill[tag].path);
                skillAnima.transform.SetParent(skillPanel.transform,false);
                //skillAnima.transform.parent = skillPanel.transform;
                //skillAnima.transform.localPosition = go.transform.localPosition;
            }
        }
        /// <summary>
        /// 撤退
        /// </summary>
        public void FallBack()
        {
            if (p_attack && GameDef.isFight)
            {
                AudioControl.PlayEffect(GameDef.clickEffect);
                if (GameAPI.IsMeet(0.4f))
                {
                    UIAPI.ShowMessage("撤退成功");
                    StartCoroutine(EndGame(true));
                }
                else
                {
                    UIAPI.ShowMessage("撤退失败");
                    p_attack = false;
                    btnAddATC.enabled = false;
                    btnAddDEF.enabled = false;
                    isStart = true;
                    ResetSkill();
                    pFrame.SetActive(false);
                    playerPanel.transform.SetScaleXYZ(0.95f, 0.95f, 0.95f);
                }
            }
        }
        
        public void BallAttackBtn()
        {
            if(isStart == false && p_attack == true){
                p_attack = false;
                btnAddATC.enabled = false;
                btnAddDEF.enabled = false;
                if (p_skill_enabel[3])
                {
                    AudioControl.PlayEffect(GameDef.beheadEffect);
                    StartCoroutine(MonsDefanse(GameDef.AttackAnimationClip3));
                    if (p_skill_enabel[2])
                    {
                        cool_time[2] = 0;
                        skillMask[2].fillAmount = 0;
                    }
                }
                else
                {
                    StartCoroutine(MonsDefanse(GameDef.AttackAnimationClip2));
                }
                pFrame.SetActive(false);
                playerPanel.transform.SetScaleXYZ(0.95f,0.95f,0.95f);
            }
        }

        private IEnumerator MonsDefanse(string name)
        {
            ChangeClip(name);
            yield return new WaitForSeconds(0.5f);
            AudioControl.PlayEffect(GameDef.swooshEffect);
            mSprite.GetComponent<Animator>().SetBool("defense", true);
            CalHurt_P();
            yield return new WaitForSeconds(0.5f);
            mSprite.GetComponent<Animator>().SetBool("defense", false);

            isStart = true;
            ResetSkill();
        }

        private void ResetSkill()
        {
            p_c_atc = GameControl.gameData.ATC;
            if (!p_skill_enabel[1])
                p_c_def = GameControl.gameData.DEF;
            int atc = int.Parse(pATCPer.text);
            SetADNum(atc);
            for (int i = 0; i < 5; i++)
            {
                p_skill_enabel[i] = false;
            }
        }

        //计算主角对怪物造成的伤害
        private void CalHurt_P()
        {
            int hurt = 0;
            string hurtInfo = "";
            if (p_skill_enabel[3])
            {
                hurt = int.Parse(pATC.text);
                hurtInfo = hurt.ToString();
                var ud = AppConfig.Value.mainUserData;
                int harm = Mathf.RoundToInt(GameControl.gameData.maxHP * GameData.skill[3].num1[ud.skill_level[3] - 1] / 100f);
                SetHurtInfo(pHurt.transform, harm.ToString(), Color.red);
                if (harm < GameControl.gameData.currentHP)
                {
                    GameControl.gameData.currentHP -= harm;
                }
                else
                {
                    GameControl.gameData.currentHP = 0;
                    ChangeClip(GameDef.DieAnimationClip);
                    StartCoroutine(EndGame());
                }
            }
            else
            {
                hurt = int.Parse(pATC.text) - Mathf.RoundToInt(monster.m_DEF[m_level] * 1.0f * m_defPer / 10);
                if (hurt <= 0)
                {
                    hurt = 1;
                }
                if (m_c_hp / hurt < 4)
                {
                    if (m_defPer < 10)
                    {
                        m_defPer++;
                        m_atcPer--;
                    }
                }
                int n = 1;
                hurtInfo = hurt.ToString();
                if (p_skill_enabel[2])
                {
                    var ud = AppConfig.Value.mainUserData;
                    n = GameData.skill[2].num1[ud.skill_level[2] - 1];
                    hurtInfo = hurt + " X " + n;
                    hurt = hurt * n;
                }
            }
            if (hurt < m_c_hp)
            {
                m_c_hp -= hurt;
            }
            else
            {
                m_c_hp = 0;
                fightWin = true;
                mSprite.GetComponent<Animator>().SetBool("die", true);
                StartCoroutine(EndGame());
            }
            UpdataInfo();
            SetHurtInfo(mHurt.transform, hurtInfo, Color.red);
        }

        //计算怪物对主角造成的伤害
        private void CalHurt_M()
        {
            int hurt = Mathf.RoundToInt(monster.m_ATC[m_level] * 1.0f * m_atcPer / 10) - int.Parse(pDEF.text);
            if (hurt <= 0)
            {
                hurt = 1;//强制伤害1点
            }
            if (1.0f * GameControl.gameData.maxHP / hurt > 13)//对玩家伤害小于1/13最大血量，提升1成攻击
            {
                if (m_atcPer < 10)
                {
                    m_atcPer++;
                    m_defPer--;
                }
                if ((int.Parse(pATC.text) - monster.m_DEF[m_level] * m_defPer) > m_c_hp * 0.3f)//玩家伤害将大于剩余血量的30%，防御提升一成
                {
                    if (m_defPer < 10)
                    {
                        m_atcPer--;
                        m_defPer++;
                    }
                }
            }
            if (hurt < GameControl.gameData.currentHP)
            {
                GameControl.gameData.currentHP -= hurt;
            }else
            {
                GameControl.gameData.currentHP = 0;
                ChangeClip(GameDef.DieAnimationClip);
                StartCoroutine(EndGame());
            }
            SetHurtInfo(pHurt.transform, hurt.ToString(), Color.red);
        }

        private void SetHurtInfo(Transform parent,string hurtInfo,Color color)
        {
            GameObject prefab = UIRes.LoadPrefab(UIDef.UIHurtInfo);
            if (prefab != null)
            {
                GameObject hurt = Instantiate(prefab);
                hurt.transform.parent = parent;
                if (color == Color.green)
                    hurt.GetComponent<Text>().text = "<color=green>" + hurtInfo + "</color>";
                else
                    hurt.GetComponent<Text>().text = hurtInfo;
                hurt.transform.localPosition = Vector3.zero;
            }
        }

        private IEnumerator EndGame(bool flag = false)
        {
            if (!flag)
            {
                yield return new WaitForSeconds(1f);
            }
            this.GetComponent<Animator>().SetBool("end", true);
            AudioControl.StopBGMusic();
            if (fightWin)//战胜
            {
                AudioControl.PlayEffect(GameDef.winEffect,0.5f);
                if (isBoss)//敌方是BOSS
                {
                    GameControl.gameData.Statistics[GameData.Statist[(int)GameData.SID.BOSS]]++;
                }else//对方是普通怪物
                {
                    GameControl.gameData.Statistics[GameData.Statist[(int)GameData.SID.MONSTER]]++;
                }
                System.Random r = new System.Random();
                jewData += r.Next(-15, 15);
                if (jewData < 25)
                {
                    GameDef.isFight = false;
                }
                else if (jewData < 50)
                {
                    GameControl.gameData.Statistics[GameData.Statist[(int)GameData.SID.LCHEST]]++;
                    UIAPI.ShowMsgBox("宝箱", "发现一个<color=green><size=65>低级宝箱</size></color>，是否打开", "打开", OpenChestL);
                }
                else if (jewData < 75)
                {
                    GameControl.gameData.Statistics[GameData.Statist[(int)GameData.SID.MCHEST]]++;
                    UIAPI.ShowMsgBox("宝箱", "发现一个<color=blue><size=65>中级宝箱</size></color>，是否打开", "打开", OpenChestM);
                }
                else
                {
                    GameControl.gameData.Statistics[GameData.Statist[(int)GameData.SID.HCHEST]]++;
                    UIAPI.ShowMsgBox("宝箱", "发现一个<color=red><size=65>高级宝箱</size></color>，是否打开", "打开", OpenChestH);
                }
                UIAPI.ShowMsgBox("战斗胜利", "<color=#FFD800>金币 * " + monster.m_coin[m_level] + "</color>\n<color=#FB00D4>灵石 * " + monster.m_ocoin[m_level] + "</color>", "领取",AddCoin);
            }else{
		GameDef.isFight = false;
	    }
            yield return new WaitForSeconds(1f);
            AudioControl.PlayBGMusic(GameObject.Find("GameControl").GetComponent<AudioSource>());
            gameObject.SetActive(false);
        }

        private void OpenChestL(object arg)
        {
            System.Random r = new System.Random();
            int coin = r.Next(monster.m_coin[m_level], 3 * monster.m_coin[m_level]);
            int ocoin = r.Next(0, 3 * monster.m_ocoin[m_level]);
            UIAPI.ShowMsgBox("低级宝箱", "<color=#FFD800>金币 * " + coin + "</color>\n<color=#FB00D4>灵石 * " + ocoin + "</color>", "确定",(args)=> {
                GameDef.isFight = false;
            });
            GameControl.gameData.gainCoin += coin;
            GameControl.gameData.gainOCoin += ocoin;
        }

        private void OpenChestM(object arg)
        {
            System.Random r = new System.Random();
            int coin = r.Next(2 * monster.m_coin[m_level], 4 * monster.m_coin[m_level]);
            int ocoin = r.Next(1 * monster.m_ocoin[m_level], 4 * monster.m_ocoin[m_level]);
            int food = r.Next(10, 20);
            UIAPI.ShowMsgBox("中级宝箱", "<color=#FFD800>金币 * " + coin + "</color>\n<color=#FB00D4>灵石 * " + ocoin + "</color>\n<color=#00FF0C>食物 * " + food + "</color>", "确定", Quit);
            GameControl.gameData.gainCoin += coin;
            GameControl.gameData.gainOCoin += ocoin;
            GameControl.gameData.currentFood += food;
        }

        private void OpenChestH(object arg)
        {
            System.Random r = new System.Random();
            int coin = r.Next(3 * monster.m_coin[m_level], 5 * monster.m_coin[m_level]);
            int ocoin = r.Next(2 * monster.m_ocoin[m_level], 5 * monster.m_ocoin[m_level]);

            int collection = r.Next(0, (m_level + 1) * (GameData.collectItem.Count() / 5 + 1) > GameData.collectItem.Count() ? GameData.collectItem.Count() : (m_level + 1) * (GameData.collectItem.Count() / 5 + 1) + 1);
            GameControl.gameData.colloctions.Add(collection);
            //Debug.Log(collection);

            StringBuilder gain = new StringBuilder();
            gain.Append("<color=#FFD800>金币 * " + coin + "</color>\n<color=#FB00D4>灵石 * " + ocoin + "</color>\n<color=red>" + GameData.collectItem[collection].itemName + "</color>");
            var ud = AppConfig.Value.mainUserData;
            switch (GameControl.gameData.BOSSID)
            {
                case 12:
                    if (!ud.stone[2] && ud.Statistics[GameData.Statist[(int)GameData.SID.ADVEN]] > 5)
                    {
                        if (GameAPI.IsMeet(0.5f))
                        {
                            ud.stone[2] = true;
                            gain.Append("\n<color=#FA800A>人元石</color>");
                            GameControl.gameData.stone = 2;
                        }
                    }
                    break;
                case 13:
                    if (!ud.stone[1] && ud.Statistics[GameData.Statist[(int)GameData.SID.ADVEN]] > 10)
                    {
                        if (GameAPI.IsMeet(0.25f))
                        {
                            ud.stone[1] = true;
                            gain.Append("\n<color=#FA800A>地元石</color>");
                            GameControl.gameData.stone = 1;
                        }
                    }
                    break;
                case 14:
                    if (!ud.stone[0] && ud.Statistics[GameData.Statist[(int)GameData.SID.ADVEN]] > 15)
                    {
                        if (GameAPI.IsMeet(0.1f))
                        {
                            ud.stone[0] = true;
                            gain.Append("\n<color=#FA800A>天元石</color>");
                            GameControl.gameData.stone = 0;
                        }
                    }
                    break;
            }
            AppConfig.Value.mainUserData = ud;
            AppConfig.Save();
            UIAPI.ShowMsgBox("高级宝箱", gain.ToString(), "确定", Quit);
            GameControl.gameData.gainCoin += coin;
            GameControl.gameData.gainOCoin += ocoin;
        }

        private void AddCoin(object arg)
        {
            GameControl.gameData.gainCoin += monster.m_coin[m_level];
            GameControl.gameData.gainOCoin += monster.m_ocoin[m_level];
            AudioControl.PlayEffect(GameDef.coinEffect);
        }

        //private void OpenChest(object arg)
        //{
        //    System.Random r = new System.Random();
        //    int coin = 0;
        //    int ocoin = 0;
        //    int food = 0;
        //    switch (jewData / 25)
        //    {
        //        case 0:break;
        //        case 1:
        //            coin = r.Next(monster.m_coin[m_level], 2 * monster.m_coin[m_level]);
        //            ocoin = r.Next(2 * monster.m_ocoin[m_level], 3 * monster.m_ocoin[m_level]);
        //            UIAPI.ShowMsgBox("低级宝箱", "<color=#FFD800>金币 * " + coin + "</color>\n<color=#FB00D4>灵石 * " + ocoin + "</color>", "确定");
        //            break;
        //        case 2:
        //            coin = r.Next(3 * monster.m_coin[m_level], 5 * monster.m_coin[m_level]);
        //            ocoin = r.Next(4 * monster.m_ocoin[m_level], 6 * monster.m_ocoin[m_level]);
        //            food = r.Next(10, 20);
        //            UIAPI.ShowMsgBox("中级宝箱", "<color=#FFD800>金币 * " + coin + "</color>\n<color=#FB00D4>灵石 * " + ocoin + "</color>\n<color=#00FF0C>食物 * " + food + "</color>", "确定");
        //            break;
        //        case 3:
        //            coin = r.Next(4 * monster.m_coin[m_level], 5 * monster.m_coin[m_level]);
        //            ocoin = r.Next(5 * monster.m_ocoin[m_level], 6 * monster.m_ocoin[m_level]);
        //            int collection = r.Next(0, GameData.collectItem.Count());
        //            GameControl.gameData.colloctions.Add(collection);
        //            UIAPI.ShowMsgBox("高级宝箱", "<color=#FFD800>金币 * " + coin + "</color>\n<color=#FB00D4>灵石 * " + ocoin + "</color>\n<color=red>" + GameData.collectItem[collection].itemName + "</color>", "确定",Quit);
        //            break;
        //    }
        //    GameControl.gameData.gainCoin += coin;
        //    GameControl.gameData.gainOCoin += ocoin;
        //    GameControl.gameData.currentFood += food;
        //}

        private void Quit(object arg)
        {
            if (isBoss)
            {
                GameControl.gameData.BOSS_die = true;
            }
            GameDef.isFight = false;
        }

        private void Defanse()
        {
            AudioControl.PlayEffect(GameDef.impackEffect);
            ChangeClip(GameDef.DefanseAnimationClip);
        }

        private void ChangeClip(string name)
        {
            animation.clip = animation.GetClip(name);
            animation.Play();
            animation.clip = animation.GetClip(GameDef.IdleAnimationClip);
            animation.PlayQueued(GameDef.IdleAnimationClip);
        }
    }
}

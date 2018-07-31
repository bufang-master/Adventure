using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scripts.Game.Fight;
using Scripts.Guide;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Scripts.Game
{
    public class GameControl:MonoBehaviour
    {
        public Camera cam;
        public GameObject map;
        public GameObject player;
        public GameObject point;
        public GameObject boss;
        public GameObject fightPanel;
        public Button chooseBall;
        public Sprite[] ballSprite;
        private int ballID;

        public GameObject[] grass;
        public GameObject[] grit;
        public GameObject[] water;
        public GameObject[] lava;
        public GameObject[] cave;
        public GameObject[] basin;
        public GameObject[] forest;
        public GameObject[] mountain;
        public GameObject[] foodTree;

        private static DataControl dataControl;
        [HideInInspector]
        public static DataControl gameData { get { return dataControl; } }

        private bool IsPause;

        private enum BallType {Fight,None,Explore,Collect,Climb};
        private enum MoveDir { idle,left,right,up,down};
        private MoveDir moveDir;
        private float minDis;
        private string mapTemp;

        private int mapWidth;
        private int mapHeight;

        private Vector3 pPos;
        private Vector3 cPos;
        
        private Vector3 lastTouch;

        private void Awake()
        {
            if (AppConfig.Value.mainUserData.IsAdven)
            {
                //恢复游戏
                dataControl = AppConfig.Value.mainUserData.dataControl;
                mapWidth = dataControl.mapData[0].Count;
                mapHeight = dataControl.mapData.Count;
            }
            else
            {
                AppConfig.Value.mainUserData.IsAdven = true;
                System.Random r = new System.Random();
                dataControl = new DataControl();
                dataControl.Init();

                dataControl.Statistics[GameData.Statist[(int)GameData.SID.ADVEN]]++;
                if (dataControl.mapPath.Contains("/"))
                    ReadMap(dataControl.mapPath);
                else
                    CreateMap(dataControl.mapPath);
                dataControl.mapInfo = new int[mapHeight, mapWidth];
                SetSpecialMap();

                dataControl.bossX = r.Next(1, mapHeight - 2);
                dataControl.bossY = r.Next(1, mapWidth - 2);
                dataControl.playerX = dataControl.bossX;
                dataControl.playerY = dataControl.bossY;
                while (dataControl.bossX + 10 >= dataControl.playerX && dataControl.bossX - 10 <= dataControl.playerX && dataControl.bossY + 10 >= dataControl.playerY && dataControl.bossY - 10 <= dataControl.playerY)
                {
                    dataControl.playerX = r.Next(dataControl.bossX - 30 >= 5 ? dataControl.bossX - 30 : 5, dataControl.bossX + 30 <= mapHeight - 5 ? dataControl.bossX + 30 : mapHeight - 5);
                    dataControl.playerY = r.Next(dataControl.bossY - 30 >= 5 ? dataControl.bossY - 30 : 5, dataControl.bossY + 30 <= mapWidth - 5 ? dataControl.bossY + 30 : mapWidth - 5);
                }
                SaveGameData();
            }
            LoadMap();
        }

        private void CreateMap(string mapPath)
        {
            mapWidth = int.Parse(mapPath.Split('|')[0]);
            mapHeight = int.Parse(mapPath.Split('|')[1]);
            System.Random rom = new System.Random();
            for(int i = 0;i < mapHeight; i++)
            {
                List<int> temp = new List<int>();
                for(int j = 0;j < mapWidth; j++)
                {
                    temp.Add(rom.Next(0, 6));
                }
                List<int> t = new List<int>();
                foreach (var x in temp)
                {
                    t.Add(x);
                }
                dataControl.mapData.Add(t);
                temp.Clear();
            }
        }

        public static void SaveGameData()
        {
            AppConfig.Value.mainUserData.dataControl = dataControl;
            AppConfig.Save();
        }

        private void ReadMap(string path)
        {
            TextAsset file = Resources.Load<TextAsset>(path);
            if (file != null)
            {
                string _map = file.text;
                List<int> list = new List<int>();
                StringBuilder sb = new StringBuilder();
                foreach (char s in _map)
                {
                    if (s == '\n')
                    {
                        if (sb.Length != 0)
                        {
                            list.Add(int.Parse(sb.ToString()));
                            sb.Length = 0;
                        }
                        List<int> t = new List<int>();
                        foreach (var x in list)
                            t.Add(x);
                        dataControl.mapData.Add(t);
                        list.Clear();
                    }
                    if(s == '\r' || s == ' ')
                    {
                        if (sb.Length != 0)
                        {
                            list.Add(int.Parse(sb.ToString()));
                            sb.Length = 0;
                        }
                    }
                    else
                    {
                        sb.Append(s);
                    }
                }
                mapWidth = dataControl.mapData[0].Count;
                mapHeight = dataControl.mapData.Count;
            }
        }
        private void SetSpecialMap()
        {
            bool[] noSame = new bool[mapWidth * mapHeight];
            System.Random r = new System.Random();
            int sMapNum = mapWidth * mapHeight / 30;
            int nartul = 0;
            for (int i = 0;i < sMapNum; i++)
            {
                nartul = r.Next(0, mapHeight * mapWidth - 1);
                while (noSame[nartul]||Around(nartul))
                {
                    nartul = r.Next(0, mapHeight * mapWidth - 1);
                }
                noSame[nartul] = true;
                int h = nartul / mapWidth;
                int w = nartul % mapWidth;
                if (i < sMapNum * 0.7)
                {
                    dataControl.mapData[h][w] = 6;
                }else if (i < sMapNum * 0.9)
                {
                    dataControl.mapData[h][w] = 7;
                }else if (i < sMapNum)
                {
                    dataControl.mapData[h][w] = 8;
                }
            }

        }

        private bool Around(int nartul)
        {
            int x = nartul / mapWidth;
            int y = nartul % mapWidth;
            StringBuilder sb = new StringBuilder();
            sb.Append("_" + dataControl.mapData[x][y] + "_");
            if (x - 1 >= 0)
            {
                sb.Append("_" + dataControl.mapData[x - 1][y] + "_");
            }
            if (x + 1 < mapHeight)
            {
                sb.Append("_" + dataControl.mapData[x+1][y] + "_");
            }
            if (y - 1 >= 0)
            {
                sb.Append("_" + dataControl.mapData[x][y-1] + "_");
            }
            if (y + 1 < mapWidth)
            {
                sb.Append("_" + dataControl.mapData[x][y+1] + "_");
            }
            if (sb.ToString().Contains("_0_") || sb.ToString().Contains("_1_") || sb.ToString().Contains("_7_") || sb.ToString().Contains("_8_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LoadMap()
        {
            System.Random ran = new System.Random();
            GameObject go = new GameObject();
            Destroy(go);  
            for(int i = 0;i < mapHeight; i++)
            {
                for(int j = 0;j < mapWidth; j++)
                {  
                    switch (dataControl.mapData[i][j])
                    {
                        case 0:
                            go = Instantiate(water[ran.Next(water.Length)]);
                            break;
                        case 1:
                            go = Instantiate(lava[ran.Next(lava.Length)]);
                            break;
                        case 2:
                            go = Instantiate(mountain[ran.Next(mountain.Length)]);
                            break;
                        case 3:
                            go = Instantiate(grass[ran.Next(grass.Length)]);
                            break;
                        case 4:
                            go = Instantiate(grit[ran.Next(grit.Length)]);
                            break;
                        case 5:
                            go = Instantiate(forest[ran.Next(forest.Length)]);
                            break;
                        case 6:
                            int n = ran.Next(foodTree.Length);
                            go = Instantiate(foodTree[n]);
                            dataControl.mapInfo[i, j] = n;
                            break;
                        case 7:
                            go = Instantiate(basin[ran.Next(basin.Length)]);
                            break;
                        case 8:
                            go = Instantiate(cave[ran.Next(cave.Length)]);
                            break;
                    }
                    go.name = i + "," + j;
                    go.transform.parent = map.transform;
                    go.transform.localPosition = new Vector3((j - (mapWidth - 1) / 2) * 10.5f, ((mapHeight - 1) / 2 - i) * 10.5f, 0);
                }
            }
        }

        private void Start()
        {
            AudioControl.PlayBGMusic(GetComponent<AudioSource>());

            minDis = 10;
            Input.multiTouchEnabled = true;
            Input.simulateMouseWithTouches = true;

            SwitchBall(GameData.mapInfo[dataControl.mapData[dataControl.playerX][dataControl.playerY]].type);

            //设置boss和player的位置
            boss.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GameData.monster[dataControl.BOSSID].path);
            boss.transform.localPosition = new Vector3((dataControl.bossY - (mapWidth - 1) / 2) * 10.5f, ((mapHeight - 1) / 2 - dataControl.bossX) * 10.5f, 0);

            int cX = dataControl.playerX, cY = dataControl.playerY;
            if (dataControl.playerX < 3)
            {
                cX = 3;
            }
            if (dataControl.playerX >= mapHeight - 3)
            {
                cX = mapHeight - 4;
            }
            if (dataControl.playerY < 2)
            {
                cY = 2;
            }
            if (dataControl.playerY >= mapWidth - 2)
            {
                cY = mapWidth - 3;
            }
            pPos = new Vector3((dataControl.playerY - (mapWidth - 1) / 2) * 10.5f, ((mapHeight - 1) / 2 - dataControl.playerX) * 10.5f, 0);
            cPos = new Vector3((cY - (mapWidth - 1) / 2) * 10.5f, ((mapHeight - 1) / 2 - cX) * 10.5f, 0);
            player.transform.localPosition = pPos;
            cam.transform.position = cPos;
            point.name = dataControl.playerX + "," + dataControl.playerY;

            GuideAPI.AdventureStartFunc();

            IsPause = false;

            GameDef.isFight = false;
        }

        private void Update()
        {
            if (!GameDef.isFight&&!IsPause)
            {
                SwitchBall(GameData.mapInfo[dataControl.mapData[dataControl.playerX][dataControl.playerY]].type);
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began|| Input.GetMouseButtonDown(0))
                {
                    //lastTouch = Input.GetTouch(0).position;
                    lastTouch = Input.mousePosition;
                }
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Canceled|| Input.GetMouseButtonUp(0))
                {
                    if(Input.mousePosition == lastTouch)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
                        RaycastHit hitInfo;
                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            //Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
                            string name = hitInfo.collider.gameObject.name;
                            int x = int.Parse(name.Split(',')[0]) - dataControl.playerX;
                            int y = int.Parse(name.Split(',')[1]) - dataControl.playerY;
                            if (x <= 2 && x >= -2 && y <= 2 && y >= -2)
                            {
                                AudioControl.PlayEffect(GameDef.clickEffect);
                                point.transform.localPosition = new Vector3(2.35f * y, -2.35f * x, 0);
                                point.name = name;
                            }
                        }
                    }
                }
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (Input.GetTouch(0).deltaPosition.sqrMagnitude > minDis)
                    {
                        var deltaPos = Input.GetTouch(0).deltaPosition;
                        if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
                        {
                            moveDir = deltaPos.x > 0 ? MoveDir.right : MoveDir.left;
                        }
                        else
                        {
                            moveDir = deltaPos.y > 0 ? MoveDir.up : MoveDir.down;
                        }
                    }
                }
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    switch (moveDir)
                    {
                        case MoveDir.left:
                            Move(0, -1, -1, 0);
                            break;
                        case MoveDir.right:
                            Move(0, 1, 1, 0);
                            break;
                        case MoveDir.up:
                            Move(-1, 0, 0, 1);
                            break;
                        case MoveDir.down:
                            Move(1, 0, 0, -1);
                            break;
                    }
                    moveDir = MoveDir.idle;
                }
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    Move(-1, 0, 0, 1);
                }
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    Move(1, 0, 0, -1);
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    Move(0, -1, -1, 0);
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    Move(0, 1, 1, 0);
                }
            }
        }
        private bool OutSide(int x,int y,int posX,int posY)
        {
            if (posX < 3 || posX >= mapHeight - 3)
            {
                if (y == 0)
                    return true;
            }
            if(posY < 2 || posY >= mapWidth - 2)
            {
                if (x == 0)
                    return true;
            }
            return false;
        }
        private void Move(int x,int y,int m,int n)
        {
            int X = dataControl.playerX + x;
            int Y = dataControl.playerY + y;
            if (X < mapHeight && X >= 0 && Y >= 0 && Y < mapWidth)
            {
                AudioControl.PlayEffect(GameDef.walkEffect);
                var currentPos = GameData.mapInfo[dataControl.mapData[X][Y]];
                //判断人物位置是否偏离
                if (player.transform.localPosition != pPos)
                {
                    player.transform.localPosition = pPos;
                }
                if (cam.transform.position != cPos)
                {
                    cam.transform.position = cPos;
                }
                if(OutSide(x,y,dataControl.playerX,dataControl.playerY))
                {
                    if (dataControl.playerX < 3 || dataControl.playerX >= mapHeight - 3|| dataControl.playerY < 2 || dataControl.playerY >= mapWidth - 2)
                    {
                        pPos = player.transform.localPosition + new Vector3(10.5f * m, 10.5f * n, 0);
                        player.transform.DOLocalMove(pPos, 0.5f);
                        //player.transform.position += new Vector3(10.5f * m, 10.5f * n, 0);
                    }
                    else
                    {
                        pPos = cPos = new Vector3(10.5f * m + cam.transform.position.x, 10.5f * n + cam.transform.position.y, 0);
                        cam.transform.DOMove(cPos, 0.5f);
                        player.transform.DOLocalMove(pPos, 0.5f);
                        //cam.transform.position = new Vector3(10.5f * m + cam.transform.position.x, 10.5f * n + cam.transform.position.y, 0);
                        //player.transform.localPosition = cam.transform.position;
                    }
                }
                else
                {
                    if (OutSide(x, y, X, Y))
                    {
                        pPos = player.transform.localPosition + new Vector3(10.5f * m, 10.5f * n, 0);
                        player.transform.DOLocalMove(pPos, 0.5f);
                        //player.transform.position += new Vector3(10.5f * m, 10.5f * n, 0);
                    }
                    else
                    {
                        cPos = new Vector3(10.5f * m + cam.transform.position.x, 10.5f * n + cam.transform.position.y, 0);
                        cam.transform.DOMove(cPos,0.5f);
                        pPos = player.transform.localPosition + new Vector3(10.5f * m, 10.5f * n, 0);
                        player.transform.DOLocalMove(pPos, 0.5f);
                        //cam.transform.position = new Vector3(10.5f * m + cam.transform.position.x, 10.5f * n + cam.transform.position.y, 0);
                        //player.transform.localPosition += new Vector3(10.5f * m, 10.5f * n, 0);
                    }
                }
                dataControl.playerX = X;
                dataControl.playerY = Y;
                point.transform.localPosition = Vector3.zero;
                point.name = dataControl.playerX + "," + dataControl.playerY;

                if (dataControl.currentFood >= currentPos.costFood)//食物充足
                {
                    dataControl.currentFood -= currentPos.costFood;
                    dataControl.currentHP += currentPos.costHP;
                }else//食物不足，血量减去两倍食物
                {
                    dataControl.currentHP -= ((currentPos.costFood - dataControl.currentFood) * 2 - currentPos.costHP);
                    dataControl.currentFood = 0;
                }

                SwitchBall(currentPos.type);

                if (dataControl.bossX + 1 >= dataControl.playerX && dataControl.bossX - 1 <= dataControl.playerX && dataControl.bossY + 1 >= dataControl.playerY && dataControl.bossY - 1 <= dataControl.playerY)
                {
                    IsPause = true;
                    UIAPI.ShowMsgBox("BOSS", "发现BOSS，是否战斗，胜利后将直接返回主世界", "战斗|逃离", (arg) => {
                        if ((int)arg == 0)
                        {
                            fightPanel.SetActive(true);
                            fightPanel.GetComponent<FightPage>().Init(84);
                            SetChooseSprite(BallType.Fight);
                            GuideAPI.FightStartFunc();
                            //Debug.Log("1");
                        }
                        IsPause = false;
                    });
                }
                else if (!currentPos.type.Contains("explore") || dataControl.mapInfo[dataControl.playerX, dataControl.playerY] != 0)
                {
                    if (GameAPI.IsMeet(currentPos.bizarre))
                    {
                        IsPause = true;
                        UIAPI.ShowMsgBox("怪物", "发现怪物，是否战斗", "战斗|逃离", (arg) => {
                            if ((int)arg == 0)
                            {
                                fightPanel.SetActive(true);
                                fightPanel.GetComponent<FightPage>().Init(15);
                                SetChooseSprite(BallType.Fight);
                                GuideAPI.FightStartFunc();
                            }
                            IsPause = false;
                        });
                    }
                }
                SaveGameData();
            }
        }

        private void SwitchBall(string type)
        {
            string[] t = type.Split('|');
            switch (t[0])
            {
                case "none":
                    SetChooseSprite(BallType.None);
                    break;
                case "explore":
                    SetChooseSprite(BallType.Explore);
                    mapTemp = t[1];
                    break;
                case "collect":
                    SetChooseSprite(BallType.Collect);
                    mapTemp = t[1];
                    break;
                case "climb":
                    SetChooseSprite(BallType.Climb);
                    mapTemp = t[1];
                    break;
            }
        }
        private void SetChooseSprite(BallType id)
        {
            chooseBall.GetComponent<Image>().sprite = ballSprite[(int)id];
            for (int i = 0; i < chooseBall.onClick.GetPersistentEventCount(); i++)
            {
                chooseBall.onClick.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.Off);
            }
            chooseBall.onClick.SetPersistentListenerState((int)id, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }
        private void ReplaceMap(int targrtX, int targetY, GameObject go,int n)
        {
            GameObject target = GameObject.Find(targrtX + "," + targetY);
            if (target != null)
            {
                dataControl.mapData[targrtX][targetY] = n;
                GameObject replace = Instantiate(go);
                replace.name = targrtX + "," + targetY;
                replace.transform.parent = map.transform;
                replace.transform.localPosition = target.transform.localPosition;
                Destroy(target);
            }
        }
        public void BallCollectBtn()
        {
            dataControl.currentFood += int.Parse(mapTemp);
            dataControl.Statistics[GameData.Statist[(int)GameData.SID.FOOD]] += int.Parse(mapTemp);
            if (dataControl.currentFood > dataControl.BAG)
            {
                dataControl.currentFood = dataControl.BAG;
            }
            ReplaceMap(dataControl.playerX, dataControl.playerY, forest[dataControl.mapInfo[dataControl.playerX, dataControl.playerY]],5);
            SaveGameData();
        }
        public void BallExploreBtn()
        {
            if (dataControl.mapInfo[dataControl.playerX, dataControl.playerY] == 0)
            {
                dataControl.mapInfo[dataControl.playerX, dataControl.playerY] = 1;
                IsPause = true;
                if (GameAPI.IsMeet(GameData.mapInfo[dataControl.mapData[dataControl.playerX][dataControl.playerY]].bizarre + 0.3f))
                {
                    UIAPI.ShowMsgBox("怪物", "发现看守宝物的怪物，是否战斗", "战斗|逃离",arg=> {
                        if ((int)arg == 0)
                        {
                            fightPanel.SetActive(true);
                            fightPanel.GetComponent<FightPage>().Init(int.Parse(mapTemp));
                            SetChooseSprite(BallType.Fight);
                            GuideAPI.FightStartFunc();
                            //Debug.Log("3");
                        }
                        IsPause = false;
                    });
                }else
                {
                    UIAPI.ShowMsgBox("提示", "未发现任何有价值的东西", "确定",arg=> {
                        IsPause = false;
                    });
                }
                dataControl.Statistics[GameData.Statist[(int)GameData.SID.EXPLORE]]++;
                SaveGameData();
            }
            else
            {
                IsPause = true;
                UIAPI.ShowMsgBox("提示", "已经探索过了", "确定",arg=> {
                    IsPause = false;
                });
            }
        }

        public void BallNoneBtn()
        {

        }
        public void BallClimbBtn()
        {
            if (cam.transform.position.z == 0)
            {
                cam.transform.position += new Vector3(0, 0, -35);
                dataControl.currentFood -= int.Parse(mapTemp);
                SaveGameData();
            }
        }
    }
}

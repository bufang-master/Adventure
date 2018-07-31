using UnityEngine;
using SGF.UI.Framework;
using System.Linq;
using UnityEngine.UI;

namespace Scripts.UI.Collection
{
    public class CollectionPage:UIPage
    {
        public GameObject Collections;
        public Text pageOfPage;
        public Text pageName;

        private int pageID;
        private int pageCount;
        private int gainItemCount;

        private void Start()
        {
            gainItemCount = 0;
            pageID = 0;
            pageCount = GameData.collectPage.Count();
            ResetPage();
        }
        private void ResetPage()
        {
            //删除所有的子对象
            for (int i = 0; i < Collections.transform.childCount; i++)
            {
                GameObject child = Collections.transform.GetChild(i).gameObject;
                Destroy(child);
            }

            gainItemCount = 0;
            var data = GameData.collectItem.Where(n => n.pageID == pageID).ToList();
            var ud = AppConfig.Value.mainUserData;
            
            pageOfPage.text = (pageID + 1) + " / " + pageCount;
            switch (data.Count())
            {
                case 9:
                    for (int i = 0; i < data.Count(); i++)
                    {
                        int state = ud.collection_data[data[i].itemID];
                        if (state > 0)
                            gainItemCount++;
                        AddItem(data[i].itemID, data[i].path,state, GameData.collectItemPos[i]);
                    }
                    break;
                case 2:
                    for (int i = 0; i < data.Count(); i++)
                    {
                        int state = ud.collection_data[data[i].itemID];
                        if (state > 0)
                            gainItemCount++;
                        AddItem(data[i].itemID, data[i].path,state, GameData.collectItemPos[i]);
                    }
                    break;
            }
            pageName.text = GameData.collectPage[pageID] + "(" + gainItemCount + "/" + data.Count() + ")";
        }
        private void AddItem(int ID,string name,int state,Vector3 pos)
        {
            var temp = UIRes.LoadPrefab(UIDef.UICollectionBall);
            var itemSprite = Resources.Load<Sprite>(name);

            if(temp != null)
            {
                var ball = Instantiate(temp);
                ball.name = name.Split('/').Last();
                ball.transform.SetParent(Collections.transform,false);
                if (state == 2)
                {
                    foreach(Transform t in ball.transform)
                    {
                        if(t.name == "CollectionItem")
                        {
                            t.GetComponent<Image>().sprite = itemSprite;
                            break;
                        }
                    }
                }
                
                ball.transform.localPosition = pos;
                ball.GetComponent<CollectionBall>().itemID = ID;
            }
        }
        public void OnBtnNextPage()
        {
            if (pageID != pageCount - 1)
            {
                AudioControl.PlayEffect(GameDef.turnPageEffect);
                pageID++;
                ResetPage();
            }
        }
        public void OnBtnPrevPage()
        {
            if (pageID != 0)
            {
                AudioControl.PlayEffect(GameDef.turnPageEffect);
                pageID--;
                ResetPage();
            }
        }
    }
}

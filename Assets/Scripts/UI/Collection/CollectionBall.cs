using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripts.Service.User;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Collection
{
    public class CollectionBall:MonoBehaviour
    {
        public int itemID;
        public Image itemImage;

        private Animator animator;
        private int state;

        private void Start()
        {
            state = AppConfig.Value.mainUserData.collection_data[itemID];
            animator = GetComponent<Animator>();
            animator.SetInteger("state", state);
        }

        public void OnBtnCollectionInfo()
        {
            AudioControl.PlayEffect(GameDef.clickEffect);
            var item = GameData.collectItem[itemID];
            var ud = AppConfig.Value.mainUserData;

            if (state == 1)
            {
                AudioControl.PlayEffect(GameDef.shiftEffect);
                StartCoroutine(ChangeSprite());
            }
            else if(state == 2)
            {
                UIAPI.ShowCollectionInfoBox(itemImage.sprite, item.itemName, item.itemInfo);
            }
        }

        private IEnumerator ChangeSprite()
        {
            var itemSprite = Resources.Load<Sprite>(GameData.collectItem[itemID].path.ToLower());
            if (itemSprite == null)
                Debug.Log("null");
            animator.SetBool("change", true);
            yield return new WaitForSeconds(0.5f);
            itemImage.sprite = itemSprite;
            animator.SetBool("change", false);
            state = 2;
            animator.SetInteger("state", state);

            var ud = AppConfig.Value.mainUserData;
            ud.collection_data[itemID] = state;
            AppConfig.Value.mainUserData = ud;
            AppConfig.Save();
        }
    }
}

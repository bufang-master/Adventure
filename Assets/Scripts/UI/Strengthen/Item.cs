using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI.Strengthen
{
    public class Item:MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler
    {
        public GameObject target;
        public Sprite defaultSprite;

        private Vector3 Pos;
        private Boolean isMoving;
        public float height;

        float radian; // 弧度 
        float perRadian; // 每次变化的弧度   
        float radius; // 半径  

        private void Start()
        {
            isMoving = false;
            Pos = transform.position;
            radius = 20f;
            perRadian = 0.03f;
            radian = UnityEngine.Random.value * 3;
        }
        
        private void Update()
        {
            if (!isMoving)
            {
                radian += perRadian;
                float dy = Mathf.Cos(radian) * radius;  
                transform.position = Pos + new Vector3(0, dy, 0);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            SetDraggedPosition(eventData);
            isMoving = true;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (target.GetComponent<Danru>().isIn)
            {
                Sprite s = null;
                foreach(Transform i in transform)
                {
                    s = i.GetComponent<Image>().sprite;
                }
                target.GetComponent<Image>().sprite = s;
                this.GetComponent<Transform>().position = Pos;
                target.GetComponent<Danru>().GetNum();
            }
            else
            {
                this.GetComponent<Transform>().position = Pos;
            }
            isMoving = false;
        }

        private void SetDraggedPosition(PointerEventData eventData)
        {
            Debug.Log(eventData.position+",");
            transform.localPosition = new Vector3(eventData.position.x-540, eventData.position.y-1020, 0);
        }

        public void UpdateShow()
        {
            Debug.Log("drop");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}

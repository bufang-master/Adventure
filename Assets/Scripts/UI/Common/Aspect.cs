using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Common
{
    class Aspect:MonoBehaviour
    {
        private void Start()
        {
            //屏幕适配
            float standard_width = 1080f;
            float standard_height = 1920f;
            float device_width = 0f;
            float device_height = 0f;
            float adjustor = 0f;
            device_width = Screen.width;
            device_height = Screen.height;

            float standard_aspect = standard_width / standard_height;
            float device_aspect = device_width / device_height;

            if (device_aspect < standard_aspect)
            {
                adjustor = device_aspect / standard_aspect;
            }

            CanvasScaler canvasScaler = transform.GetComponent<CanvasScaler>();

            if(adjustor == 0)
            {
                canvasScaler.matchWidthOrHeight = 0;
            }else
            {
                canvasScaler.matchWidthOrHeight = 1;
            }
        }
    }
}

using System.Collections;
using SGF.UI.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Loading
{
    public class LoadingScene:MonoBehaviour
    {
        public Image processBar;
        public Text text;
        private AsyncOperation async;
        private uint _nowprocess;
        private float time;
        // Use this for initialization
        void Start()
        {
            _nowprocess = 0;
            StartCoroutine(loadScene());
            time = Time.time;
        }

        IEnumerator loadScene()
        {
            //异步读取场景。
            async = SceneManager.LoadSceneAsync(UIManager.MainScene);
            async.allowSceneActivation = false;
            //读取完毕后返回， 系统会自动进入C场景
            yield return async;
        }

        void Update()
        {
            if (async == null)
            {
                return;
            }

            float toProcess;
            if (async.progress < 0.9f)//坑爹的progress，最多到0.9f
            {
                toProcess = async.progress;
            }
            else
            {
                toProcess = 1;
            }

            if (_nowprocess < toProcess * 100)
            {
                _nowprocess++;
            }

            processBar.transform.SetScaleX(_nowprocess/100f);
            if ((Time.time-time) < 0.5f)
            {
                text.text = "Loading.";
            }
            else if ((Time.time - time) < 1f)
            {
                text.text = "Loading..";
            }
            else if ((Time.time - time) < 1.5f)
            {
                text.text = "Loading...";
            }
            else if ((Time.time - time) < 2f)
            {
                time = Time.time;
            }
            if (_nowprocess == 100)//async.isDone应该是在场景被激活时才为true
            {
                async.allowSceneActivation = true;
            }
        }
    }
}

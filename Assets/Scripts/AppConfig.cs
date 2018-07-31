using ProtoBuf;
using SGF;
using Scripts.Service.UserManager.Data;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using Scripts.Game;

namespace Scripts
{
    /// <summary>
    /// App的配置定义
    /// </summary>
    [Serializable]
    public class AppConfig
    {
        /// <summary>
        /// 主用户数据
        /// </summary>
        public UserData mainUserData = new UserData();
        public bool enableBgMusic = true;
        public bool enableSoundEffect = true;

        //============================================================================
        private static AppConfig m_Value = new AppConfig();
        public static AppConfig Value { get { return m_Value; } }

#if UNITY_EDITOR
        public readonly static string Path = Application.persistentDataPath + "/AppConfig_Editor.data";
#else
        public readonly static string Path = Application.persistentDataPath + "/AppConfig.data";
#endif

        public static void Init()
        {
            try
            {
                AppConfig cfg = LoadData<AppConfig>(Path);
                if (cfg != default(AppConfig))
                {
                    m_Value = cfg;
                }
            }catch 
            {
                Save();
            }
        }

        public static void Save()
        {
            if (m_Value != null)
            {
                SaveData<AppConfig>(m_Value, Path);
            }
        }

        private static void SaveData<T>(T data,string path)
        {
            IFormatter formatter = new BinaryFormatter();

            using (FileStream s = File.Create(path))
            {
                formatter.Serialize(s, data);
            }
        }
        private static T LoadData<T>(string path)
        {
            IFormatter formatter = new BinaryFormatter();

            using (FileStream s = File.OpenRead(path))
            {
                if (s != null)
                {
                    T da = (T)formatter.Deserialize(s);
                    return da;
                }
                return default(T);
            }
        }
    }


}

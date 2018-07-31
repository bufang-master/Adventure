using SGF;
using Scripts;
using UnityEngine;
using SGF.UI.Framework;
using SGF.Module.Framework;
using Scripts.Service.User;
using Scripts.Service.UserManager.Data;
using System;
using UnityEngine.SceneManagement;
using Scripts.Guide;

public class AppMain : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	    //Debuger.EnableLog = true;
		//Debuger.EnableSave = true;
		Debuger.Log (Debuger.LogFileDir);
        //reset();
        GameData.InitGameData();
        AppConfig.Init();
        UserData ud = AppConfig.Value.mainUserData;
        UserManager.Instance.UpdateMainUserData(ud);
        //Debug.Log(Application.persistentDataPath);
	    InitServices();
	    InitBusiness();
        if (ud.IsAdven)
        {
            UIAPI.ShowMsgBox("提示", "是否继续未完的冒险", "继续|不了", arg=> {
                if ((int)arg == 0)
                {
                    UIManager.MainScene = "Adventure";
                    SceneManager.LoadScene("Loading");
                }
                else
                {
                    AppConfig.Value.mainUserData.IsAdven = false;
                    AppConfig.Value.mainUserData.dataControl = null;
                    AppConfig.Save();
                    GameAPI.CheckTime();
                }
            });
        }
        else
        {
            if (!ud.GuideFlag[GuideAPI.EnterTransPage])
            {
                GuideAPI.EnterTransPageFunc();
            }
            else if(!(!ud.GuideFlag[GuideAPI.FightEnd] && ud.GuideFlag[GuideAPI.FightStart]))
            {
                GameAPI.CheckTime();
            }
        }

    }

    private void ContinueAdven(object arg)
    {
        
    }

    private void reset()
    {
        UserData ud = new UserData();
        AppConfig.Value.mainUserData = ud;
        AppConfig.Save();
    }
    private void InitServices()
    {
        ModuleManager.Instance.Init("Scripts.Module");

        UIManager.Instance.Init("ui/");
        UIManager.MainScene = "Main";

        UserManager.Instance.Init();
        
    }


    private void InitBusiness()
    {
        ModuleManager.Instance.CreateModule(ModuleDef.MainModule);
        ModuleManager.Instance.CreateModule(ModuleDef.StrengthenModule);
        ModuleManager.Instance.CreateModule(ModuleDef.CollectionModule);
        ModuleManager.Instance.CreateModule(ModuleDef.TaskModule);
        ModuleManager.Instance.CreateModule(ModuleDef.SettingModule);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

}

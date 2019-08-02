using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Analytics;
public class ConfigManager : Singleton<ConfigManager>
{
    private ConfigLevel configlevel_;
    public ConfigLevel configlevel
    {
        get
        {
            return configlevel_;
        }
        private set
        {
            configlevel_ = value;
        }
    }
    // Start is called before the first frame update
    public void InitStart(Action callback)
    {
        StartCoroutine(Init(callback));
    }
    IEnumerator Init(Action callback)
    {
        configlevel = Resources.Load("DataTable/ConfigLevel", typeof(ScriptableObject)) as ConfigLevel;
        yield return new WaitUntil(() => configlevel != null);
        callback?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

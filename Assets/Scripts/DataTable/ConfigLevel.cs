using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ConfigLevelRecord
{
    public int id;
    public int level;
    public int anchor;
    public int wall;
    public int enemy;
}
public class ConfiglevelKey
{
    public int id;
    public int level;
}
public class ConfigLevelComparison : ConfigCompare<ConfigLevelRecord>
{
    public override int ICompareHandle(ConfigLevelRecord x, ConfigLevelRecord y)
    {

        if (x.level > y.level)
        {
            return 1;
        }
        else if (x.level < y.level)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
    public override ConfigLevelRecord SetkeySearch(object key)
    {
        ConfiglevelKey newKey = (ConfiglevelKey)key;
        ConfigLevelRecord data = new ConfigLevelRecord();
        data.level = newKey.level;
        return data;
    }
}
public class ConfigLevel : BYDataTable<ConfigLevelRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigLevelComparison();
    }
}

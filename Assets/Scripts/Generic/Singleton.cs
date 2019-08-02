using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private  static T instance_;
    public static T instance
    {
        get
        {
            if (instance_ != null)

                return instance_;

            else
                return GetT();
        }
    }

    static T GetT()
    {
        GameObject go = new GameObject();

        go.AddComponent<T>();
        go.name = typeof(T).ToString();
        return go.GetComponent<T>();

    }
    // Start is called before the first frame update
    void Awake()
    {
        instance_ = gameObject.GetComponent<T>();
        OnAwake();
    }

    public virtual void OnAwake()
    {

    }

    public void Reset()
    {
        gameObject.name = typeof(T).ToString();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard
{
    private Dictionary<string, bool> boolDictionary;
    private Dictionary<string, int> intDictionary;
    private Dictionary<string, Vector3> vector3Dictionary;
    private Dictionary<string, GameObject> gameObjectDictionary;

    public event Action DataChanged;

    public BlackBoard()
    {
        boolDictionary = new Dictionary<string, bool>();
        intDictionary = new Dictionary<string, int>();
        vector3Dictionary = new Dictionary<string, Vector3>();
        gameObjectDictionary = new Dictionary<string, GameObject>();
    }

    private void SetValue<T>(Dictionary<string, T> dict, string key, T value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }

        if (DataChanged != null)
        {
            DataChanged();
        }
    }

    private bool GetValue<T>(Dictionary<string, T> dict, string key, out T value)
    {
        if (dict.ContainsKey(key))
        {
            value = dict[key];
            return true;
        }
        value = default(T);
        return false;
    }

    public void SetValue(string key, int value)
    {
        SetValue(intDictionary, key, value);
    }

    public void SetValue(string key, bool value)
    {
        SetValue(boolDictionary, key, value);
    }

    public void SetValue(string key, Vector3 value)
    {
        SetValue(vector3Dictionary, key, value);
    }

    public void SetValue(string key,GameObject value)
    {
        SetValue(gameObjectDictionary, key, value);
    }

    public bool GetValue(string key, out int value)
    {
        return GetValue(intDictionary, key, out value);
    }

    public bool GetValue(string key, out bool value)
    {
        return GetValue(boolDictionary, key, out value);
    }

    public bool GetValue(string key, out Vector3 value)
    {
        return GetValue(vector3Dictionary, key, out value);
    }

    public bool GetValue(string key,out GameObject value)
    {
        return GetValue(gameObjectDictionary, key, out value);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPrefsManager
{
    private static PlayerPrefsManager instans = new PlayerPrefsManager();
    public static PlayerPrefsManager Instans { get { return instans; } }
    private PlayerPrefsManager() { }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="targetType">要读取数据的类型</param>
    /// <param name="keyName">主键</param>
    /// <returns></returns>
    public object Load(Type targetType, string keyName)
    {
        //获取实例中的字段信息
        FieldInfo[] fieldInfos = targetType.GetFields();
        //通过传入的目标类型创建的object实例，本质还是传入的类型
        object targetObject = Activator.CreateInstance(targetType);

        string loadKeyName;

        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            loadKeyName = keyName + "_" + fieldInfo.FieldType.Name + "_" + fieldInfo.Name;

            fieldInfo.SetValue(targetObject, LoadValue(fieldInfo.FieldType, loadKeyName));
        }
        return targetObject;
    }
    private object LoadValue(Type fieldType, string loadKeyName)
    {
        if (fieldType == typeof(int))
        {
            return PlayerPrefs.GetInt(loadKeyName);
        }
        else if (fieldType == typeof(string))
        {
            return PlayerPrefs.GetString(loadKeyName);
        }
        else if (fieldType == typeof(float))
        {
            return PlayerPrefs.GetFloat(loadKeyName);
        }
        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            int Count = PlayerPrefs.GetInt(loadKeyName + "_Count");
            IList list = Activator.CreateInstance(fieldType) as IList;
            for (int i = 0; i < Count; i++)
            {
                list.Add(LoadValue(fieldType.GetGenericArguments()[0], loadKeyName + i));
            }
            return list;
        }
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            int Count = PlayerPrefs.GetInt(loadKeyName + "_Count");
            IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
            object key;
            object value;
            for (int i = 0; i < Count; i++)
            {
                key = LoadValue(fieldType.GetGenericArguments()[0], loadKeyName + "_key_" + i);
                value = LoadValue(fieldType.GetGenericArguments()[1], loadKeyName + "_value_" + i);
                dic.Add(key, value);
            }
            return dic;
        }
        else
        {
            return Load(fieldType, loadKeyName);
        }

    }
    /// <summary>
    /// 存储数据
    /// </summary>
    /// <param name="data">要保存数据的实例</param>
    /// <param name="keyName">主键</param>
    public void Save(object data, string keyName)
    {
        //获取传入实例类型
        Type dataType = data.GetType();
        //获取实例中的字段信息
        FieldInfo[] fieldInfos = dataType.GetFields();


        string saveKeyName;
        object dataValue;

        foreach (FieldInfo fieldInfo in fieldInfos)
        {

            dataValue = fieldInfo.GetValue(data);
            saveKeyName = keyName + "_" + fieldInfo.FieldType.Name + "_" + fieldInfo.Name;

            SaveValue(dataValue, saveKeyName);

        }

    }

    private void SaveValue(object dataValue, string saveKeyName)
    {
        if (dataValue is int)
        {
            PlayerPrefs.SetInt(saveKeyName, (int)dataValue);

            Debug.Log(saveKeyName);
        }
        else if (dataValue is string)
        {
            PlayerPrefs.SetString(saveKeyName, dataValue.ToString());

            Debug.Log(saveKeyName);
        }
        else if (dataValue is float)
        {
            PlayerPrefs.SetFloat(saveKeyName, (float)dataValue);

            Debug.Log(saveKeyName);
        }
        else if (dataValue is bool)
        {
            PlayerPrefs.SetInt(saveKeyName, (bool)dataValue ? 1 : 0);

            Debug.Log(saveKeyName);
        }
        else if (typeof(IList).IsAssignableFrom(dataValue.GetType()))
        {
            IList list = dataValue as IList;
            PlayerPrefs.SetInt(saveKeyName + "_Count", list.Count);
            int index = 0;
            foreach (object obj in list)
            {
                SaveValue(obj, saveKeyName + index);

                index += 1;
            }
        }
        else if (typeof(IDictionary).IsAssignableFrom(dataValue.GetType()))
        {
            IDictionary dic = dataValue as IDictionary;
            PlayerPrefs.SetInt(saveKeyName + "_Count", dic.Count);

            Debug.Log(saveKeyName + "_Count");

            int index = 0;
            foreach (object key in dic.Keys)
            {
                SaveValue(key, saveKeyName + "_key_" + index);
                SaveValue(dic[key], saveKeyName + "_value_" + index);
                index += 1;
            }
        }
        else
        {
            Save(dataValue, saveKeyName);
        }
    }

}

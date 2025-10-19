using System;
using System.Reflection;
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

        FieldInfo[] fieldInfos = targetType.GetFields();
        object targetObject = Activator.CreateInstance(targetType);

        string loadKeyName;

        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            loadKeyName = keyName + "_" + fieldInfo.FieldType.Name + "_" + fieldInfo.Name;

            LoadValue(fieldInfo, loadKeyName, targetObject);
        }
        return targetObject;
    }
    private void LoadValue(FieldInfo fieldInfo, string loadKeyName, object targetObject)
    {

        Type dataType = fieldInfo.FieldType;

        if (dataType == typeof(int))
        {
            int value = PlayerPrefs.GetInt(loadKeyName);
            fieldInfo.SetValue(targetObject, value);
        }
        if (dataType == typeof(string))
        {
            string value = PlayerPrefs.GetString(loadKeyName);
            fieldInfo.SetValue(targetObject, value);

        }
        if (dataType == typeof(float))
        {
            float value = PlayerPrefs.GetFloat(loadKeyName);
            fieldInfo.SetValue(targetObject, value);

        }
        if (dataType == typeof(bool))
        {
            bool value = PlayerPrefs.GetInt(loadKeyName) == 1 ? true : false;
            fieldInfo.SetValue(targetObject, value);
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
        }
        if (dataValue is string)
        {
            PlayerPrefs.SetString(saveKeyName, dataValue.ToString());
        }
        if (dataValue is float)
        {
            PlayerPrefs.SetFloat(saveKeyName, (float)dataValue);
        }
        if (dataValue is bool)
        {
            PlayerPrefs.SetInt(saveKeyName, (bool)dataValue ? 1 : 0);
        }
    }

}

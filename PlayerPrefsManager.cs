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
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            //字段名
            string dataName = fieldInfo.Name;
            //字段类型
            Type dataType = fieldInfo.FieldType;
            if (dataType == typeof(int))
            {
                int value = PlayerPrefs.GetInt($"{keyName}_{dataName}");
                fieldInfo.SetValue(targetObject, value);
            }
            if (dataType == typeof(string))
            {
                string value = PlayerPrefs.GetString($"{keyName}_{dataName}");
                fieldInfo.SetValue(targetObject, value);

            }
            if (dataType == typeof(float))
            {
                float value = PlayerPrefs.GetFloat($"{keyName}_{dataName}");
                fieldInfo.SetValue(targetObject, value);

            }
        }
        return targetObject;
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
        //遍历字段
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            //字段名
            string dataName = fieldInfo.Name;
            //字段值
            object dataValue = fieldInfo.GetValue(data);
            if (dataValue is int)
            {
                PlayerPrefs.SetInt($"{keyName}_{dataName}", (int)dataValue);
            }
            if (dataValue is string)
            {
                PlayerPrefs.SetString($"{keyName}_{dataName}", dataValue as string);
            }
            if (dataValue is float)
            {
                PlayerPrefs.SetFloat($"{keyName}_{dataName}", (float)dataValue);
            }
        }

    }
}

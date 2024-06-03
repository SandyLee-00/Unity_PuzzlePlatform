using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    /// <summary>
    /// �ڽ� ���ӿ�����Ʈ�� ã�ƿ´�.
    /// </summary>
    /// <param name="_gameObject"></param>
    /// <param name="name"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject _gameObject, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(_gameObject, name, recursive);
        if (transform == null)
        {
            Debug.LogError($"Util::FindChild() {name} is not found.");
            return null;
        }
        return transform.gameObject;
    }

    /// <summary>
    /// �ڽ� ������Ʈ�� �ִ� T ������Ʈ�� ã�ƿ´�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_gameObject"></param>
    /// <param name="name"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject _gameObject, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (_gameObject == null)
        {
            Debug.LogError("Util::FindChild() _gameObject is null.");
            return null;
        }

        if (recursive == false)
        {
            Transform transform = _gameObject.transform.Find(name);
            if (transform == null)
            {
                Debug.LogError($"Util::FindChild() {name} is not found.");
                return null;
            }

            return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in _gameObject.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }
        Debug.LogError($"Util::FindChild() {name} is not found.");
        return null;
    }
}

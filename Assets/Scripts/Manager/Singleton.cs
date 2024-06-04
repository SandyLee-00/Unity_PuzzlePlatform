using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base T 싱글톤 클래스
/// SoundManager / GameManager 등 싱글톤 클래스를 상속받아 사용한다 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _componentInstance;

    public static T Instance
    {
        get
        {
            if (_componentInstance == null)
            {
                _componentInstance = (T)FindObjectOfType(typeof(T));

                if (_componentInstance == null)
                {
                    GameObject _gameObject = new GameObject();
                    _gameObject.name = typeof(T).ToString();
                    _componentInstance = _gameObject.GetOrAddComponent<T>();
                    DontDestroyOnLoad(_componentInstance);
                }
            }
            return _componentInstance;
        }
    }
}
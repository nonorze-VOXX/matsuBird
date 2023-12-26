using System;
using UnityEngine;

    public class Singleton <T>: MonoBehaviour  where T :Singleton<T>
    {
       //singleton
       private static T _instance;

       public static T instance
       {
           get
           {
               if (_instance == null)
               {
                   _instance = new GameObject("Singleton").AddComponent<T>();
               }
               return _instance;
           }
       }

       private void Awake()
       {
           if (_instance != null)
           {
               Destroy(gameObject);
           }else
           {
               _instance = (T) this;
               _Awake();
           }
       }

       protected  virtual void _Awake()
       {
           
       }
    }
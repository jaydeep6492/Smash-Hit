using System;
using System.Collections.Generic;
using System.Reflection;
using Smash_Hit.Scripts;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UMX.Managers
{
    public abstract class Manager : MonoBehaviour
    {
        #region Props

        private static Dictionary<Type, Manager> m_Managers;

        private static Dictionary<Type, Manager> Managers
        {
            get
            {
                if (m_Managers == null)
                    m_Managers = new Dictionary<Type, Manager>();
                return m_Managers;
            }
        }

        #endregion

        #region Unity Calls

        public virtual void Awake()
        {
            if (!Managers.ContainsKey(this.GetType()))
                Managers.Add(this.GetType(), this);
            else
                Debug.LogError("Duplicate Managers Found. Make sure to not duplicate two managers" + this.GetType());
        }

        public virtual void OnDestroy()
        {
            if (Managers.ContainsKey(this.GetType()))
            {
                Managers[this.GetType()] = null;
                Managers.Remove(this.GetType());
            }

            ClearAllStaticReferences(GetType(), GetType());
        }

        #endregion

        #region Private Methods

        private static void ClearAllStaticReferences(Type type, Type compareType)
        {
            var data = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var fieldInfo in data)
            {
                if (fieldInfo.FieldType != compareType) continue;
                fieldInfo.SetValue(type, null);
            }

            if (type.BaseType != null)
            {
                ClearAllStaticReferences(type.BaseType, compareType);
            }
        }

        #endregion

        #region Public Methods

        private static T GetManager<T>()
        {
            if (m_Managers.ContainsKey(typeof(T)))
                return (T) Convert.ChangeType(m_Managers[typeof(T)], typeof(T));
            return (T) Convert.ChangeType(null, typeof(T));
        }

        #endregion

        private static GameManager m_GameManager;

        public static GameManager GameManager
        {
            get
            {
                if (!m_GameManager)
                    m_GameManager = GetManager<GameManager>();
                return m_GameManager;
            }
        }

        private static DataManager m_DataManager;

        public static DataManager DataManager
        {
            get
            {
                if (!m_DataManager)
                    m_DataManager = GetManager<DataManager>();
                return m_DataManager;
            }
        }
    }
}
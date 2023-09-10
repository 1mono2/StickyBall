//  PlayerPrefsUtility.cs
//  http://kan-kikuchi.hatenablog.com/entry/PlayerPrefsUtility
//
//  Created by kan kikuchi on 2015.10.22.

using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

/// <summary>
/// Utility Class about PlayerPrefs
/// </summary>

namespace MyUtility
{
    public static class PlayerPrefsExtension
    {

        //=================================================================================
        //Save
        //=================================================================================

        /// <summary>
        /// Save list
        /// </summary>
        public static void SetList<T>(string key, List<T> value)
        {
            string serizlizedList = Serialize<List<T>>(value);
            PlayerPrefs.SetString(key, serizlizedList);
        }

        /// <summary>
        /// Save dictionary
        /// </summary>
        public static void SetDict<Key, Value>(string key, Dictionary<Key, Value> value)
        {
            string serizlizedDict = Serialize<Dictionary<Key, Value>>(value);
            PlayerPrefs.SetString(key, serizlizedDict);
        }

        //=================================================================================
        //Load
        //=================================================================================

        /// <summary>
        /// Load list
        /// </summary>
        public static List<T> GetList<T>(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string serizlizedList = PlayerPrefs.GetString(key);
                return Deserialize<List<T>>(serizlizedList);
            }

            return new List<T>();
        }

        /// <summary>
        /// Load dictionaru
        /// </summary>
        public static Dictionary<Key, Value> GetDict<Key, Value>(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string serizlizedDict = PlayerPrefs.GetString(key);
                return Deserialize<Dictionary<Key, Value>>(serizlizedDict);
            }

            return new Dictionary<Key, Value>();
        }

        //=================================================================================
        //Serialize, Deserialize
        //=================================================================================

        private static string Serialize<T>(T obj)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            return Convert.ToBase64String(memoryStream.GetBuffer());
        }

        private static T Deserialize<T>(string str)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
            return (T)binaryFormatter.Deserialize(memoryStream);
        }
    }
}

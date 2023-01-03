using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.Tool
{
    public static class Loader_Editor_Tool
    {
        public static GameObject LoadGameObject(string path)
        {
            GameObject parentPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return GameObject.Instantiate(parentPrefab);
        }
         public static List<T> GetAllForm_Asset<T>(string directory) where T : Object
         {
             return GetAllForm_Asset<T>(directory, typeof(T).Name);
         }

        private static List<T> GetAllForm_Asset<T>(string directory,string filterType) where T : Object
        {
            if (string.IsNullOrEmpty(directory) || !directory.StartsWith("Assets"))
                throw new ArgumentException($"No Find {directory}");
     
            List<T> allResources = new List<T>();
            List<string> subFolders = new List<string>(Directory.GetDirectories(directory));
            subFolders.Add(directory);
            
            foreach (var folder in subFolders)
            {
                string[] guids = AssetDatabase.FindAssets($"t:{filterType}", new string[] {folder});

                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    T test = AssetDatabase.LoadAssetAtPath<T>(path);
                    if(test == null)
                        continue;
                    allResources.Add(test);
                    // Debug.Log($"{path} {test.name}");
                }
            }
            return allResources;
        }

        public static List<string> GetAllAssetDataPath<T>(string directory) where T : Object
        {

            Type type = typeof(T);
            if (string.IsNullOrEmpty(directory) || !directory.StartsWith("Assets"))
                throw new ArgumentException($"No Find {directory}");
     
            List<string> allResources = new List<string>();
            List<string> subFolders = new List<string>(Directory.GetDirectories(directory));
            subFolders.Add(directory);

            foreach (var folder in subFolders)
            {
                string[] guids = AssetDatabase.FindAssets($"t:{type.Name}", new string[] {folder});

                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    // Debug.Log(path);
                    allResources.Add(path);
                }
            }
            return allResources;
        }
    }
}
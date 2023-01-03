using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.Tool
{
    public static class Folder_Editor_Tool
    {
        public static void OpenFolder(string path)
        {
            EditorApplication.ExecuteMenuItem("Window/General/Project");
            var obj = AssetDatabase.LoadAssetAtPath(path,typeof(Object));
            EditorGUIUtility.PingObject(obj);
            AssetDatabase.OpenAsset(obj);
        }

        public static T GetSelect<T>() where T : UnityEngine.Object
        {
            return Selection.activeObject as T;
        }
        public static string SelectPath => AssetDatabase.GetAssetPath(Selection.activeObject);

        public static bool  DeleteAllFile(string fullPath)
        {
            //获取指定路径下面的所有资源文件  然后进行删除
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
     
                Debug.Log(files.Length);
     
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    string FilePath = fullPath + "/" + files[i].Name;
                    File.Delete(FilePath);
                }
                return true;
            }
            return false;
        }

        public static List<GameObject> GetSelectFolderAllObjects()
        {
            var select = Selection.activeObject;
            var selectPath = AssetDatabase.GetAssetPath(select);
            return Loader_Editor_Tool.GetAllForm_Asset<GameObject>(selectPath);
        }
        
        public static List<string> GetSelectFolderAllObjectsPath()
        {
            var select = Selection.activeObject;
            var selectPath = AssetDatabase.GetAssetPath(select);
            return Loader_Editor_Tool.GetAllAssetDataPath<GameObject>(selectPath);
        }
        
        public static string  AssetDir(string assetpath)
        {
            return Path.GetDirectoryName(assetpath);
            // if(assetpath.Contains('.'))
            //     return assetpath.Replace(assetpath.Split('/')[^1], "");
            // return assetpath;
        }

        public static string AssetPathWithoutPoint(string assetpath)
        {
            return assetpath.Split('.')[0];
        }
        public static string AssetName(string assetpath)
        {
            string name = assetpath.Split('/')[^1];
            return name.Split('.')[0];
        }
        public static IEnumerable<Type> GetFilteredTypeList<T>()
        {
            var q = typeof(T).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition)                             // Excludes C1<>
                .Where(x => typeof(T).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            // .Where(x => typeof(BaseBuff).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            // Adds various C1<T> type variants.
            // q = q.AppendWith(typeof(PlugBuff<>).MakeGenericType(typeof(Damager)));
            // q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(AnimationCurve)));
            // q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(List<float>)));
            return q;
        }
    }
}
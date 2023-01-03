using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.Tool
{
    public static class Texture2D_Editor_Tool
    {
        // using Object = UnityEngine.Object;
        public static List<Sprite> GetTextureSprites(Texture2D image)
        {
            string rootPath = Folder_Editor_Tool.AssetDir(AssetDatabase.GetAssetPath(image));//获取路径名称  
            string path = rootPath + "/" + image.name + ".PNG";//图片路径名称

            UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < objs.Length; i++)
            {
                if(objs[i] is Sprite)
                    sprites.Add(objs[i] as Sprite);
            }
            // Debug.Log(sprites.Count);
            return sprites;
        }
    
        public static List<Sprite> GetSprites(string path)
        {
            UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath($"{path}.PNG");
            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < objs.Length; i++)
            {
                if(objs[i] is Sprite)
                    sprites.Add(objs[i] as Sprite);
            }
            // Debug.Log(sprites.Count);
            return sprites;
        }
    } 

}
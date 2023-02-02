// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.UI;
// using Object = UnityEngine.Object;
//
// namespace Editor.Tool
// {
//     public static class GameObj_Editor_Tool
//     {
//         private static readonly int[] textureSize = {32,64,128,256,512,1024,2048,4096};
//         
//         [MenuItem("Assets/Check/TextureToSprite2D&Pixel")]
//         public static void ChangeDirTextureToSprite2D()
//         {
//             var select = Selection.activeObject;
//             var selectPath = AssetDatabase.GetAssetPath(select);
//             List<string> allPath = FindNoneRefrences.GetAllResourcePath(selectPath, "t:Texture");
//
//             foreach (var path in allPath)
//             {
//                 TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
//                 
//                 TextureImporterSettings importerSettings = new TextureImporterSettings();
//                 importerSettings.spriteMeshType = SpriteMeshType.FullRect;
//                 textureImporter.ReadTextureSettings(importerSettings);
//                 
//                 Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
//                 textureImporter.textureType = TextureImporterType.Sprite;
//                 textureImporter.spritePixelsPerUnit = 40;
//                 textureImporter.filterMode = FilterMode.Point;
//                 
//                 textureImporter.textureCompression = TextureImporterCompression.CompressedHQ; 
//                 textureImporter.maxTextureSize = BestSize(texture);
//                 textureImporter.wrapMode = TextureWrapMode.Repeat;
//                 
//                 
//                 
//                 textureImporter.SaveAndReimport();
//                 
//             }
//         }
//
//         public static int BestSize(Texture texture)
//         {
//             int bestSize = 2048;
//             for (int i = 0; i < textureSize.Length; i++)
//             {
//                 int curSize = textureSize[i];
//                 if (texture.height < curSize && texture.width < curSize)
//                 {
//                     bestSize = curSize;
//                     break;
//                 }
//             }
//             return bestSize;
//         }
//     }
//
//     public static class CheckObj
//     {
//         [MenuItem("Assets/Check/Delete Missing Scripts")]
//         static void CleanupMissingScript()
//         {
//            ChangeSelectFolderPrefab(ComoponentThing);
//         }
//         
//         public static void ChangeSelectFolderPrefab(UnityAction<GameObject> doChange)
//         {
//             // var objs = Folder_Editor_Tool.GetSelectFolderAllObjectsPath();
//             
//             var select = Selection.activeObject;
//             var selectPath = AssetDatabase.GetAssetPath(select);
//             var objs =  Loader_Editor_Tool.GetAllAssetDataPath<GameObject>(selectPath);
//
//             foreach (var path in objs)
//             {
//                 FindObj(path,doChange);
//                 // RenameObj<ScriptableObject>(path);
//             }
//         }
//         
//         private static void FindObj(string path,UnityAction<GameObject> doChange)
//         {
//             GameObject parentPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//             GameObject parent = GameObject.Instantiate(parentPrefab);
//             parent.name = parentPrefab.name;
//             doChange(parent);
//             EditorUtility.SetDirty(parent);
//             PrefabUtility.SaveAsPrefabAsset(parent, path);
//             GameObject.DestroyImmediate(parent);
//         }
//
//         private static void ComoponentThing(GameObject obj)
//         {
//             // if (obj.transform.Find("Body") != null)
//             // {
//             //     SpriteRenderer sp = obj.transform.Find("Body").GetComponent<SpriteRenderer>();
//             //     sp.sortingOrder = 1;
//             //     sp.spriteSortPoint = SpriteSortPoint.Center;                
//             // }
//             
//         }
//         
//         private static void RenameObj<T>(string path) where T: Object
//         {
//             T parentPrefab = AssetDatabase.LoadAssetAtPath<T>(path);
//             if (parentPrefab is FxData slill)
//             {
//                 slill.FxParamater.Name = slill.name;
//                 EditorUtility.SetDirty(slill);
//                 // AssetDatabase.RenameAsset(path, name);
//             }
//         }
//         
//         [MenuItem("Assets/Check/GetDependencies")]
//         private static void GetDepend()
//         {
//             GameObject select = Selection.activeGameObject;
//             foreach (var dependency in AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(select)))
//             {
//                 Debug.Log(dependency);
//             }
//         }
//         
//         
//         [MenuItem("GameObject/UiTool/UiScaleToSize")]
//         private static void ChangeLayer()
//         {
//             GameObject select = Selection.activeGameObject;
//             var steamImage = select.GetComponent<Image>();
//             if (steamImage == null)
//                 return;
//             
//             Vector3 scale = steamImage.rectTransform.localScale;
//             SetImageSize(scale,select);
//             UiTool.ForeachAllChild(steamImage.rectTransform,DoThing);
//             // CopyGameobj();
//             void DoThing(int index, Transform rect)
//             {
//                 SetImageSize(scale,rect.gameObject);
//             }
//         }
//
//         private static void SetImageSize(Vector2 scale,GameObject gameObject)
//         {
//             var steamImage = gameObject.GetComponent<Image>();
//             if (steamImage == null)
//                 return;
//             steamImage.SetNativeSize();
//             
//             Vector3 size = steamImage.rectTransform.sizeDelta;
//             steamImage.rectTransform.localScale = Vector3.one;
//             steamImage.rectTransform.sizeDelta = new Vector2(size.x * scale.x, size.y * scale.y);
//         }
//     }
// }

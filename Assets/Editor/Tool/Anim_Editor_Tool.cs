using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Editor.Tool
{
    public static class Anim_Editor_Tool
    {
        private const float interval = 0.07f;
        [MenuItem("Assets/Check/CreateAnimClip")]
        private static void CreateClip()
        {
            var select = Selection.activeObject;
            var selectPath = AssetDatabase.GetAssetPath(select);
            SpriteClip(selectPath);
        }
        
        [MenuItem("Assets/Check/CreateAllAnimClip")]
        private static void CreateAllClip()
        {
            var select = Selection.activeObject;
            var selectPath = AssetDatabase.GetAssetPath(select);
            List<string> allPath = Loader_Editor_Tool.GetAllAssetDataPath<Texture>(selectPath);
            // Debug.Log(path);
      
            foreach (var texPath in allPath)
            {
                SpriteClip(texPath);
            }
        }
        
        public static RuntimeAnimatorController LoadRuntimeAnimatorController(string path)
        {
            var animator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                $"{path}.overrideController");
            if(animator == null)
                animator =
                    AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                        $"{path}.controller");
            return animator;
        }
        
        [MenuItem("Assets/Check/ResetAllAnimClip")]
        private static void ResetAllAnimClip()
        {
            var select = Selection.activeObject;
            var selectPath = AssetDatabase.GetAssetPath(select);
            List<AnimationClip> clips = Loader_Editor_Tool.GetAllForm_Asset<AnimationClip>(selectPath);
            // Debug.Log(path);

            
            foreach (var clip in clips)
            {
                clip.frameRate = 1 / interval;
                
                EditorCurveBinding binding = EditorCurveBinding.PPtrCurve("Body", typeof(SpriteRenderer), "m_Sprite");
                ObjectReferenceKeyframe[] keyFrames = AnimationUtility.GetObjectReferenceCurve(clip,binding);
                if (keyFrames == null)
                {
                    Debug.Log(clip.name);
                    continue;
                }
                List<ObjectReferenceKeyframe> newframes = new List<ObjectReferenceKeyframe>(keyFrames);
                for(int i = newframes.Count -1; i >=0 ; i--)
                {
                    if(newframes[i].value == null)
                        newframes.RemoveAt(i);
                }

                keyFrames = newframes.ToArray();

                for (int i = 0; i < keyFrames.Length; i++)
                {
                    keyFrames[i].time = i * interval;
                }
                AnimationUtility.SetObjectReferenceCurve(clip, binding, keyFrames);
                EditorUtility.SetDirty(clip);
            }
        }

        public static void AddFirstBlank(AnimationClip clip)
        {
            EditorCurveBinding binding = EditorCurveBinding.PPtrCurve("Body", typeof(SpriteRenderer), "m_Sprite");
            ObjectReferenceKeyframe[] keyFrames = AnimationUtility.GetObjectReferenceCurve(clip,binding);
            if (keyFrames == null)
            {
                Debug.Log(clip.name);
                return;
            }
            //第一帧已经为空
            if(keyFrames[0].value == null)
                return;
            
            List<ObjectReferenceKeyframe> newframes = new List<ObjectReferenceKeyframe>(keyFrames);
            ObjectReferenceKeyframe first = new ObjectReferenceKeyframe();
            first.value = null;
            newframes.Insert(0,first);
            keyFrames = newframes.ToArray();

            for (int i = 0; i < keyFrames.Length; i++)
            {
                keyFrames[i].time = i * interval;
            }
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keyFrames);
            EditorUtility.SetDirty(clip);
            
        }
        public static float GetSpriteAnimTime(AnimationClip clip)
        {
            clip.frameRate = 1 / interval;
            EditorCurveBinding binding = EditorCurveBinding.PPtrCurve("Body", typeof(SpriteRenderer), "m_Sprite");
            ObjectReferenceKeyframe[] keyFrames = AnimationUtility.GetObjectReferenceCurve(clip,binding);
            if (keyFrames == null)
            {
                Debug.Log($"Anim {clip.name}  no Body");
                return -99;
            }
            Debug.Log($"{clip.name}  {keyFrames.Length * interval} ");
            return keyFrames.Length * interval;
        }
        
        public static void ChangeStateAnimClip(AnimatorOverrideController overAnimator,string stateId,AnimationClip newClip)
        {
            AnimatorController animatorController = (AnimatorController) overAnimator.runtimeAnimatorController;
            AnimatorControllerLayer layer = animatorController.layers[0];//获取这个Animator组件上对应某一层的AnimatorController资源
            AnimatorStateMachine sm = layer.stateMachine;  //获取层状态机
            ChildAnimatorState[] ams = sm.states;
            foreach (var animatorState in ams)
            {
                string stateName = animatorState.state.name;
                Debug.Log($"state {stateName}");
                if (stateName != stateId) continue;
                Debug.Log($"had {stateId}");
                
                overAnimator[stateName] = newClip;
                return;
            }
            EditorUtility.SetDirty(overAnimator);
        }

        public static AnimationClip GetAnimClip(string stateName,AnimatorOverrideController overAnimator)
        {
            return overAnimator[stateName];
        }
        public static Dictionary<string,AnimationClip> GetAnimDic(AnimatorOverrideController overAnimator)
        {
            Dictionary<string,AnimationClip> dic = new Dictionary<string, AnimationClip>();
            
            AnimatorController animatorController = (AnimatorController) overAnimator.runtimeAnimatorController;
            AnimatorControllerLayer layer = animatorController.layers[0];//获取这个Animator组件上对应某一层的AnimatorController资源
            AnimatorStateMachine sm = layer.stateMachine;  //获取层状态机
            ChildAnimatorState[] ams = sm.states;
            foreach (var animatorState in ams)
            {
                string stateName = animatorState.state.name;
                dic.Add(stateName,overAnimator[stateName]);
                // Debug.Log($"state {stateName}");
            }
            return dic;
        }
        
        public static Dictionary<string,AnimationClip> GetAnimDic(RuntimeAnimatorController runtimeAnimator)
        {
            Dictionary<string,AnimationClip> dic = new Dictionary<string, AnimationClip>();

            foreach (var clip in runtimeAnimator.animationClips)
            {
                if(!dic.ContainsKey(clip.name))
                    dic.Add(clip.name,clip);
            }
            return dic;
        }
        

        public static AnimationClip SpriteClip(Texture texture, string savePath = "")
        {
            return SpriteClip(AssetDatabase.GetAssetPath(texture), savePath);
        }

        private static AnimationClip SpriteClip(string texturePath,string savePath = "")
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).Where(x => x is Sprite).Cast<Sprite>().ToArray();
            // clip creation
            AnimationClip newClip = new AnimationClip();
            // newClip.legacy = true;
            // create and apply clip settings
            AnimationClipSettings newSettings = new AnimationClipSettings();
            
            newSettings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(newClip, newSettings);

            // create initial binding
            //要设置动画的对象的变换路径  要设置动画的对象的类型	要在对象上设置动画的属性的名称
            EditorCurveBinding binding = EditorCurveBinding.PPtrCurve("Body", typeof(SpriteRenderer), "m_Sprite");

            // make the actual clip itself
            // float AnimInterval = 1f / sprites.Length;
            ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[sprites.Length];
            for (int i = 0; i < keyFrames.Length; i++)
            {
                keyFrames[i].time = i * interval;                
                keyFrames[i].value = sprites[i];
            }

            newClip.frameRate = 1 / interval;
            // newClip.frameRate = sprites.Length;
            // newClip.frameRate = 60;
            newClip.name = Folder_Editor_Tool.AssetName(texturePath);
            
            if (savePath == "")
            {
                savePath = $"{Folder_Editor_Tool.AssetDir(texturePath)}/{newClip.name}.anim";
            }
            else if(!savePath.Contains(".anim"))
            {
                savePath += $"/{newClip.name}.anim";
            }
            Debug.Log(savePath);
            // save it
            AnimationUtility.SetObjectReferenceCurve(newClip, binding, keyFrames);
            
            AssetDatabase.CreateAsset(newClip,savePath); // to create asset
            // string savePath = "Assets/Saved Clips";
            // AssetDatabase.CreateAsset(newClip, $"{savePath}/test.anim");
            AssetDatabase.SaveAssets();
            return newClip;
        }

        public static void ResetClip(Texture2D texture2D, AnimationClip clip)
        {
            ResetClip(AssetDatabase.GetAssetPath(texture2D),clip);
        }
        public static void ResetClip(string texturePath,AnimationClip clip)
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).Where(x => x is Sprite).Cast<Sprite>().ToArray();

            AnimationClipSettings newSettings = new AnimationClipSettings();
            
            newSettings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, newSettings);
            // create initial binding
            //要设置动画的对象的变换路径  要设置动画的对象的类型	要在对象上设置动画的属性的名称
            EditorCurveBinding binding = EditorCurveBinding.PPtrCurve("Body", typeof(SpriteRenderer), "m_Sprite");

            // make the actual clip itself
            // float AnimInterval = 1f / sprites.Length;
            float interval = 0.07f;
            ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[sprites.Length];
            for (int i = 0; i < keyFrames.Length; i++)
            {
                keyFrames[i].time = i * interval;
                // Debug.Log(keyFrames[i].time);
                keyFrames[i].value = sprites[i];
            }

            clip.frameRate = 1 / interval;
            // newClip.frameRate = sprites.Length;
            // newClip.frameRate = 60;

            // save it
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keyFrames);
            AssetDatabase.SaveAssets();
        }
    }
}
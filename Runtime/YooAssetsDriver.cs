using System.Diagnostics;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class YooAssetsDriver : MonoBehaviour
    {
        private static int LastestUpdateFrame = 0;

        [UnityEngine.Scripting.Preserve]
        private void Update()
        {
            DebugCheckDuplicateDriver();
            YooAssets.Update();
        }

#if UNITY_EDITOR
        [UnityEngine.Scripting.Preserve]
        private void OnApplicationQuit()
        {
            YooAssets.OnApplicationQuit();
        }
#endif

        [UnityEngine.Scripting.Preserve]
        [Conditional("DEBUG")]
        private void DebugCheckDuplicateDriver()
        {
            if (LastestUpdateFrame > 0)
            {
                if (LastestUpdateFrame == Time.frameCount)
                {
                    YooLogger.Warning($"There are two {nameof(YooAssetsDriver)} in the scene. Please ensure there is always exactly one driver in the scene.");
                }
            }

            LastestUpdateFrame = Time.frameCount;
        }
    }
}
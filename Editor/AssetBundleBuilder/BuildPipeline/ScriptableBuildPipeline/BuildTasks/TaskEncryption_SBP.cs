#if ENABLE_GAME_FRAME_X_SCRIPTABLE_BUILD_PIPELINE
namespace YooAsset.Editor
{
    public class TaskEncryption_SBP : TaskEncryption, IBuildTask
    {
        void IBuildTask.Run(BuildContext context)
        {
            var buildParameters = context.GetContextObject<BuildParametersContext>();
            var buildMapContext = context.GetContextObject<BuildMapContext>();

            var buildMode = buildParameters.Parameters.BuildMode;
            if (buildMode == EBuildMode.ForceRebuild || buildMode == EBuildMode.IncrementalBuild)
            {
                EncryptingBundleFiles(buildParameters, buildMapContext);
            }
        }
    }
}
#endif
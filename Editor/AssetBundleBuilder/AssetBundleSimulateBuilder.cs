using UnityEditor;

namespace YooAsset.Editor
{
    public static class AssetBundleSimulateBuilder
    {
        /// <summary>
        /// 模拟构建
        /// </summary>
        public static SimulateBuildResult SimulateBuild(string buildPipelineName, string packageName)
        {
            string packageVersion = "Simulate";
            BuildResult buildResult;

            if (buildPipelineName == nameof(EBuildPipeline.BuiltinBuildPipeline))
            {
                BuiltinBuildParameters buildParameters = CreateBuildParameters<BuiltinBuildParameters>(buildPipelineName, packageName, packageVersion);
                BuiltinBuildPipeline pipeline = new BuiltinBuildPipeline();
                buildResult = pipeline.Run(buildParameters, false);
            }
            else if (buildPipelineName == nameof(EBuildPipeline.ScriptableBuildPipeline))
            {
                ScriptableBuildParameters buildParameters = CreateBuildParameters<ScriptableBuildParameters>(buildPipelineName, packageName, packageVersion);
                ScriptableBuildPipeline pipeline = new ScriptableBuildPipeline();
                buildResult = pipeline.Run(buildParameters, true);
            }
            else if (buildPipelineName == nameof(EBuildPipeline.RawFileBuildPipeline))
            {
                RawFileBuildParameters buildParameters = CreateBuildParameters<RawFileBuildParameters>(buildPipelineName, packageName, packageVersion);
                RawFileBuildPipeline pipeline = new RawFileBuildPipeline();
                buildResult = pipeline.Run(buildParameters, true);
            }
            else
            {
                throw new System.NotImplementedException(buildPipelineName);
            }

            // 返回结果
            if (buildResult.Success)
            {
                SimulateBuildResult reulst = new SimulateBuildResult();
                reulst.PackageRootDirectory = buildResult.OutputPackageDirectory;
                return reulst;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 创建通用构建参数
        /// </summary>
        private static T CreateBuildParameters<T>(string buildPipelineName, string packageName, string packageVersion) where T : BuildParameters, new()
        {
            T buildParameters = new T
            {
                BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot(),
                BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot(),
                BuildPipeline = buildPipelineName,
                BuildTarget = EditorUserBuildSettings.activeBuildTarget,
                BuildMode = EBuildMode.SimulateBuild,
                PackageName = packageName,
                PackageVersion = packageVersion,
                FileNameStyle = EFileNameStyle.HashName,
                BuildinFileCopyOption = EBuildinFileCopyOption.None,
                BuildinFileCopyParams = string.Empty,
            };
            return buildParameters;
        }
    }
}
using System;
using UnityEngine;
using UnityEditor;

namespace YooAsset.Editor
{
    public static class AssetBundleBuilderSetting
    {
        private static T GetEnumSetting<T>(string key, T defaultValue) where T : struct
        {
            return (T)(object)EditorPrefs.GetInt(key, (int)(object)defaultValue);
        }
        private static void SetEnumSetting<T>(string key, T value) where T : struct
        {
            EditorPrefs.SetInt(key, (int)(object)value);
        }

        private static string GetStringSetting(string key, string defaultValue = "")
        {
            return EditorPrefs.GetString(key, defaultValue);
        }
        private static void SetStringSetting(string key, string value)
        {
            EditorPrefs.SetString(key, value);
        }

        // EBuildPipeline
        public static EBuildPipeline GetPackageBuildPipeline(string packageName)
        {
            string key = $"{Application.productName}_{packageName}_{nameof(EBuildPipeline)}";
            return GetEnumSetting(key, EBuildPipeline.BuiltinBuildPipeline);
        }
        public static void SetPackageBuildPipeline(string packageName, EBuildPipeline buildPipeline)
        {
            string key = $"{Application.productName}_{packageName}_{nameof(EBuildPipeline)}";
            SetEnumSetting(key, buildPipeline);
        }

        // EBuildMode
        public static EBuildMode GetPackageBuildMode(string packageName, EBuildPipeline buildPipeline)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_{nameof(EBuildMode)}";
            return GetEnumSetting(key, EBuildMode.ForceRebuild);
        }
        public static void SetPackageBuildMode(string packageName, EBuildPipeline buildPipeline, EBuildMode buildMode)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_{nameof(EBuildMode)}";
            SetEnumSetting(key, buildMode);
        }

        // ECompressOption
        public static ECompressOption GetPackageCompressOption(string packageName, EBuildPipeline buildPipeline)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_{nameof(ECompressOption)}";
            return GetEnumSetting(key, ECompressOption.LZ4);
        }
        public static void SetPackageCompressOption(string packageName, EBuildPipeline buildPipeline, ECompressOption compressOption)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_{nameof(ECompressOption)}";
            SetEnumSetting(key, compressOption);
        }

        // EFileNameStyle
        public static EFileNameStyle GetPackageFileNameStyle(string packageName, EBuildPipeline buildPipeline)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_{nameof(EFileNameStyle)}";
            return GetEnumSetting(key, EFileNameStyle.HashName);
        }
        public static void SetPackageFileNameStyle(string packageName, EBuildPipeline buildPipeline, EFileNameStyle fileNameStyle)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_{nameof(EFileNameStyle)}";
            SetEnumSetting(key, fileNameStyle);
        }

        // EBuildinFileCopyOption
        public static EBuildinFileCopyOption GetPackageBuildinFileCopyOption(string packageName, EBuildPipeline buildPipeline)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_{nameof(EBuildinFileCopyOption)}";
            return GetEnumSetting(key, EBuildinFileCopyOption.None);
        }
        public static void SetPackageBuildinFileCopyOption(string packageName, EBuildPipeline buildPipeline, EBuildinFileCopyOption buildinFileCopyOption)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_{nameof(EBuildinFileCopyOption)}";
            SetEnumSetting(key, buildinFileCopyOption);
        }

        // BuildFileCopyParams
        public static string GetPackageBuildinFileCopyParams(string packageName, EBuildPipeline buildPipeline)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_BuildFileCopyParams";
            return GetStringSetting(key, string.Empty);
        }
        public static void SetPackageBuildinFileCopyParams(string packageName, EBuildPipeline buildPipeline, string buildinFileCopyParams)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_BuildFileCopyParams";
            SetStringSetting(key, buildinFileCopyParams);
        }

        // EncyptionClassName
        public static string GetPackageEncyptionClassName(string packageName, EBuildPipeline buildPipeline)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_EncyptionClassName";
            return GetStringSetting(key, string.Empty);
        }
        public static void SetPackageEncyptionClassName(string packageName, EBuildPipeline buildPipeline, string encyptionClassName)
        {
            string key = $"{Application.productName}_{packageName}_{buildPipeline}_EncyptionClassName";
            SetStringSetting(key, encyptionClassName);
        }
    }
}
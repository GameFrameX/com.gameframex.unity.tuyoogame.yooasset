using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class LoadWebCatalogFileOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadCatalog,
            Done,
        }

        private readonly DefaultWebFileSystem _fileSystem;
        private ESteps _steps = ESteps.None;

        /// <summary>
        /// 内置清单版本
        /// </summary>
        public string PackageVersion { private set; get; }

        [UnityEngine.Scripting.Preserve]
        internal LoadWebCatalogFileOperation(DefaultWebFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.LoadCatalog;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadCatalog)
            {
                var catalogFilePath = _fileSystem.GetCatalogFileLoadPath();
                var catalog = Resources.Load<DefaultBuildinFileCatalog>(catalogFilePath);
                if (catalog == null)
                {
                    // 目录文件不存在时按空目录处理，未记录的文件会回退到 Web 根目录加载，不能让启动流程失效。
                    YooLogger.Warning($"Failed to load web catalog file, use empty catalog : {catalogFilePath}");
                    MarkEmptyCatalogDone();
                    return;
                }

                if (catalog.PackageName != _fileSystem.PackageName)
                {
                    // 包名不匹配说明目录文件属于其它包裹，同样按空目录处理，避免打断初始化流程。
                    YooLogger.Warning($"Web catalog file package name {catalog.PackageName} cannot match the file system package name {_fileSystem.PackageName}, use empty catalog");
                    MarkEmptyCatalogDone();
                    return;
                }

                PackageVersion = catalog.PackageVersion;
                foreach (var wrapper in catalog.Wrappers)
                {
                    var fileWrapper = new DefaultWebFileSystem.FileWrapper(wrapper.FileName);
                    _fileSystem.RecordFile(wrapper.BundleGUID, fileWrapper);
                }

                YooLogger.Log($"Package '{_fileSystem.PackageName}' catalog files count : {catalog.Wrappers.Count}");
                _steps = ESteps.Done;
                Status = EOperationStatus.Succeed;
            }
        }

        [UnityEngine.Scripting.Preserve]
        private void MarkEmptyCatalogDone()
        {
            YooLogger.Log($"Package '{_fileSystem.PackageName}' catalog files count : 0");
            _steps = ESteps.Done;
            Status = EOperationStatus.Succeed;
        }
    }
}
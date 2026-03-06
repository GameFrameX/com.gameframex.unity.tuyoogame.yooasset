using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class DCFSClearAllBundleFilesOperation : FSClearAllBundleFilesOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            GetAllCacheFiles,
            ClearAllCacheFiles,
            Done,
        }

        private readonly DefaultCacheFileSystem _fileSystem;
        private List<string> _allBundleGUIDs;
        private int _fileTotalCount = 0;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal DCFSClearAllBundleFilesOperation(DefaultCacheFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.GetAllCacheFiles;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.GetAllCacheFiles)
            {
                _allBundleGUIDs = _fileSystem.GetAllCachedBundleGUIDs();
                _fileTotalCount = _allBundleGUIDs.Count;
                _steps = ESteps.ClearAllCacheFiles;
                YooLogger.Log($"Found all cache files count : {_fileTotalCount}");
            }

            if (_steps == ESteps.ClearAllCacheFiles)
            {
                for (var i = _allBundleGUIDs.Count - 1; i >= 0; i--)
                {
                    var bundleGUID = _allBundleGUIDs[i];
                    _fileSystem.DeleteCacheFile(bundleGUID);
                    _allBundleGUIDs.RemoveAt(i);
                    if (OperationSystem.IsBusy)
                    {
                        break;
                    }
                }

                if (_fileTotalCount == 0)
                {
                    Progress = 1.0f;
                }
                else
                {
                    Progress = 1.0f - _allBundleGUIDs.Count / _fileTotalCount;
                }

                if (_allBundleGUIDs.Count == 0)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class DCFSLoadLocalPackageVersionOperation : FSRequestPackageVersionOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            GetPackageVersion,
            Done,
        }

        private readonly DefaultCacheFileSystem _fileSystem;
        private readonly bool _appendTimeTicks;
        private readonly int _timeout;
        private LoadCachePackageVersionOperation _loadCachePackageVersionOp;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal DCFSLoadLocalPackageVersionOperation(DefaultCacheFileSystem fileSystem, bool appendTimeTicks, int timeout)
        {
            _fileSystem = fileSystem;
            _appendTimeTicks = appendTimeTicks;
            _timeout = timeout;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.GetPackageVersion;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.GetPackageVersion)
            {
                if (_loadCachePackageVersionOp == null)
                {
                    _loadCachePackageVersionOp = new LoadCachePackageVersionOperation(_fileSystem);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _loadCachePackageVersionOp);
                }

                Progress = _loadCachePackageVersionOp.Progress;
                if (_loadCachePackageVersionOp.IsDone == false)
                {
                    return;
                }

                if (_loadCachePackageVersionOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    PackageVersion = _loadCachePackageVersionOp.PackageVersion;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadCachePackageVersionOp.Error;
                }
            }
        }
    }
}
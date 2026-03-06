using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class DBFSRequestPackageVersionOperation : FSRequestPackageVersionOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            RequestBuildinPackageVersion,
            Done,
        }

        private readonly DefaultBuildinFileSystem _fileSystem;
        private RequestBuildinPackageVersionOperation _requestBuildinPackageVersionOp;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal DBFSRequestPackageVersionOperation(DefaultBuildinFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.RequestBuildinPackageVersion;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.RequestBuildinPackageVersion)
            {
                if (_requestBuildinPackageVersionOp == null)
                {
                    _requestBuildinPackageVersionOp = new RequestBuildinPackageVersionOperation(_fileSystem);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _requestBuildinPackageVersionOp);
                }

                if (_requestBuildinPackageVersionOp.IsDone == false)
                {
                    return;
                }

                if (_requestBuildinPackageVersionOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    PackageVersion = _requestBuildinPackageVersionOp.PackageVersion;
                    Status = EOperationStatus.Succeed;
                    Debug.Log("获取包内版本号成功：" + PackageVersion);
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _requestBuildinPackageVersionOp.Error;
                }
            }
        }
    }
}
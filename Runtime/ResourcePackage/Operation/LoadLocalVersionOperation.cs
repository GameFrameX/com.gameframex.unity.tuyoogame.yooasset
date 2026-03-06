namespace YooAsset
{
    /// <summary>
    /// 获取本地的最新版本
    /// </summary>
    public abstract class LoadLocalVersionOperation : AsyncOperationBase
    {
        /// <summary>
        /// 当前本地的最新版本
        /// </summary>
        public string PackageVersion { protected set; get; }
    }

    /// <summary>
    /// 获取本地的最新版本
    /// </summary>
    internal sealed class LoadLocalVersionImplOperation : LoadLocalVersionOperation
    {
        private enum ESteps
        {
            None,
            TryLoadCachePackageVersion,
            LoadBuildinPackageVersion,
            Done,
        }

        private readonly IFileSystem _buildinFileSystem;
        private readonly IFileSystem _cacheFileSystem;
        private readonly bool _appendTimeTicks;
        private readonly int _timeout;
        private FSRequestPackageVersionOperation _loadCachePackageVersionOp;
        private FSRequestPackageVersionOperation _loadBuildinPackageVersionOp;
        private ESteps _steps = ESteps.None;

        internal LoadLocalVersionImplOperation(IFileSystem fileSystem, IFileSystem cacheSystem, bool appendTimeTicks, int timeout)
        {
            _buildinFileSystem = fileSystem;
            _cacheFileSystem = cacheSystem;
            _appendTimeTicks = appendTimeTicks;
            _timeout = timeout;
        }

        public override void InternalOnStart()
        {
            if (_cacheFileSystem != null)
            {
                _steps = ESteps.TryLoadCachePackageVersion;
            }
            else
            {
                _steps = ESteps.LoadBuildinPackageVersion;
            }
        }

        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.TryLoadCachePackageVersion)
            {
                if (_loadCachePackageVersionOp == null)
                {
                    _loadCachePackageVersionOp = _cacheFileSystem.LoadLocalPackageVersionAsync(_appendTimeTicks, _timeout);
                }

                if (_loadCachePackageVersionOp.IsDone == false)
                {
                    return;
                }

                if (_loadCachePackageVersionOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    PackageVersion = _loadCachePackageVersionOp.PackageVersion;
                }
                else
                {
                    _steps = ESteps.LoadBuildinPackageVersion;
                    //Status = EOperationStatus.Failed;
                    Error = _loadCachePackageVersionOp.Error;
                }
            }

            if (_steps == ESteps.LoadBuildinPackageVersion)
            {
                if (_loadBuildinPackageVersionOp == null)
                {
                    _loadBuildinPackageVersionOp = _buildinFileSystem.LoadLocalPackageVersionAsync(_appendTimeTicks, _timeout);
                }

                if (_loadBuildinPackageVersionOp.IsDone == false)
                {
                    return;
                }

                if (_loadBuildinPackageVersionOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    PackageVersion = _loadBuildinPackageVersionOp.PackageVersion;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadBuildinPackageVersionOp.Error;
                }
            }
        }
    }
}
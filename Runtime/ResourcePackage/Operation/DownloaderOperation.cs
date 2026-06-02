using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class DownloaderOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            Check,
            Loading,
            Done,
        }

        private const int MAX_LOADER_COUNT = 64;

        [UnityEngine.Scripting.Preserve]
        public delegate void OnDownloadOver(DownloaderFinishData data);

        [UnityEngine.Scripting.Preserve]
        public delegate void OnDownloadProgress(DownloadUpdateData data);

        [UnityEngine.Scripting.Preserve]
        public delegate void OnDownloadError(DownloadErrorData data);

        [UnityEngine.Scripting.Preserve]
        public delegate void OnStartDownloadFile(DownloadFileData data);

        private readonly string _packageName;
        private readonly int _downloadingMaxNumber;
        private readonly int _failedTryAgain;
        private readonly int _timeout;
        private readonly List<BundleInfo> _bundleInfoList;
        private readonly List<FSDownloadFileOperation> _downloaders = new List<FSDownloadFileOperation>(MAX_LOADER_COUNT);
        private readonly List<FSDownloadFileOperation> _removeList = new List<FSDownloadFileOperation>(MAX_LOADER_COUNT);
        private readonly List<FSDownloadFileOperation> _failedList = new List<FSDownloadFileOperation>(MAX_LOADER_COUNT);

        // 数据相关
        private bool _isPause = false;
        private long _lastDownloadBytes = 0;
        private int _lastDownloadCount = 0;
        private long _cachedDownloadBytes = 0;
        private int _cachedDownloadCount = 0;
        private ESteps _steps = ESteps.None;


        /// <summary>
        /// 统计的下载文件总数量
        /// </summary>
        public int TotalDownloadCount { private set; get; }

        /// <summary>
        /// 统计的下载文件的总大小
        /// </summary>
        public long TotalDownloadBytes { private set; get; }

        /// <summary>
        /// 当前已经完成的下载总数量
        /// </summary>
        public int CurrentDownloadCount
        {
            get { return _lastDownloadCount; }
        }

        /// <summary>
        /// 当前已经完成的下载总大小
        /// </summary>
        public long CurrentDownloadBytes
        {
            get { return _lastDownloadBytes; }
        }

        /// <summary>
        /// 当下载器结束（无论成功或失败）
        /// </summary>
        public OnDownloadOver OnDownloadOverCallback { set; get; }

        /// <summary>
        /// 当下载进度发生变化
        /// </summary>
        public OnDownloadProgress OnDownloadProgressCallback { set; get; }

        /// <summary>
        /// 当某个文件下载失败
        /// </summary>
        public OnDownloadError OnDownloadErrorCallback { set; get; }

        /// <summary>
        /// 当开始下载某个文件
        /// </summary>
        public OnStartDownloadFile OnStartDownloadFileCallback { set; get; }


        [UnityEngine.Scripting.Preserve]
        internal DownloaderOperation(string packageName, List<BundleInfo> downloadList, int downloadingMaxNumber, int failedTryAgain, int timeout)
        {
            _packageName = packageName;
            _bundleInfoList = downloadList;
            _downloadingMaxNumber = UnityEngine.Mathf.Clamp(downloadingMaxNumber, 1, MAX_LOADER_COUNT);
            _failedTryAgain = failedTryAgain;
            _timeout = timeout;

            // 设置包裹名称 (fix #210)
            SetPackageName(packageName);

            // 统计下载信息
            CalculatDownloaderInfo();
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            YooLogger.Log($"Begine to download {TotalDownloadCount} files and {TotalDownloadBytes} bytes");
            _steps = ESteps.Check;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.Check)
            {
                if (_bundleInfoList == null)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = "Download list is null.";
                }
                else
                {
                    _steps = ESteps.Loading;
                }
            }

            if (_steps == ESteps.Loading)
            {
                // 检测下载器结果
                _removeList.Clear();
                var downloadBytes = _cachedDownloadBytes;
                foreach (var downloader in _downloaders)
                {
                    downloadBytes += downloader.DownloadedBytes;
                    if (downloader.IsDone == false)
                    {
                        continue;
                    }

                    // 检测是否下载失败
                    if (downloader.Status != EOperationStatus.Succeed)
                    {
                        _removeList.Add(downloader);
                        _failedList.Add(downloader);
                        continue;
                    }

                    // 下载成功
                    _removeList.Add(downloader);
                    _cachedDownloadCount++;
                    _cachedDownloadBytes += downloader.DownloadedBytes;
                }

                // 移除已经完成的下载器（无论成功或失败）
                foreach (var downloader in _removeList)
                {
                    _downloaders.Remove(downloader);
                }

                // 如果下载进度发生变化
                if (_lastDownloadBytes != downloadBytes || _lastDownloadCount != _cachedDownloadCount)
                {
                    _lastDownloadBytes = downloadBytes;
                    _lastDownloadCount = _cachedDownloadCount;
                    Progress = TotalDownloadBytes > 0 ? (float)_lastDownloadBytes / TotalDownloadBytes : 0f;
                    var downloadUpdateData = new DownloadUpdateData
                    {
                        PackageName = _packageName,
                        Progress = Progress,
                        TotalDownloadCount = TotalDownloadCount,
                        CurrentDownloadCount = _lastDownloadCount,
                        TotalDownloadBytes = TotalDownloadBytes,
                        CurrentDownloadBytes = _lastDownloadBytes,
                    };
                    OnDownloadProgressCallback?.Invoke(downloadUpdateData);
                }

                // 动态创建新的下载器到最大数量限制
                // 注意：如果期间有下载失败的文件，暂停动态创建下载器
                if (_bundleInfoList.Count > 0 && _failedList.Count == 0)
                {
                    if (_isPause)
                    {
                        return;
                    }

                    if (_downloaders.Count < _downloadingMaxNumber)
                    {
                        var index = _bundleInfoList.Count - 1;
                        var bundleInfo = _bundleInfoList[index];
                        var downloader = bundleInfo.CreateDownloader(_failedTryAgain, _timeout);
                        _downloaders.Add(downloader);
                        _bundleInfoList.RemoveAt(index);
                        var downloadFileData = new DownloadFileData
                        {
                            PackageName = _packageName,
                            FileName = bundleInfo.Bundle.BundleName,
                            FileSize = bundleInfo.Bundle.FileSize,
                        };
                        OnStartDownloadFileCallback?.Invoke(downloadFileData);
                    }
                }

                // 下载结算
                if (_downloaders.Count == 0)
                {
                    if (_failedList.Count > 0)
                    {
                        var failedDownloader = _failedList[0];
                        var bundleName = failedDownloader.Bundle.BundleName;
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = $"Failed to download file : {bundleName}";
                        var downloadErrorData = new DownloadErrorData
                        {
                            PackageName = _packageName,
                            FileName = bundleName,
                            ErrorInfo = failedDownloader.Error,
                        };
                        OnDownloadErrorCallback?.Invoke(downloadErrorData);
                        var downloadFinishData = new DownloaderFinishData
                        {
                            PackageName = _packageName,
                            Succeed = false,
                        };
                        OnDownloadOverCallback?.Invoke(downloadFinishData);
                    }
                    else
                    {
                        // 结算成功
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Succeed;
                        var downloadFinishData = new DownloaderFinishData
                        {
                            PackageName = _packageName,
                            Succeed = true,
                        };
                        OnDownloadOverCallback?.Invoke(downloadFinishData);
                    }
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        private void CalculatDownloaderInfo()
        {
            if (_bundleInfoList != null)
            {
                TotalDownloadBytes = 0;
                TotalDownloadCount = _bundleInfoList.Count;
                foreach (var packageBundle in _bundleInfoList)
                {
                    TotalDownloadBytes += packageBundle.Bundle.FileSize;
                }
            }
            else
            {
                TotalDownloadBytes = 0;
                TotalDownloadCount = 0;
            }
        }

        /// <summary>
        /// 合并其它下载器
        /// </summary>
        /// <param name="downloader">合并的下载器</param>
        [UnityEngine.Scripting.Preserve]
        public void Combine(DownloaderOperation downloader)
        {
            if (_packageName != downloader._packageName)
            {
                YooLogger.Error("The downloaders have different resource packages !");
                return;
            }

            if (Status != EOperationStatus.None)
            {
                YooLogger.Error("The downloader is running, can not combine with other downloader !");
                return;
            }

            if (downloader.Status != EOperationStatus.None)
            {
                YooLogger.Error("The other downloader is running, can not combine !");
                return;
            }

            var temper = new HashSet<string>();
            foreach (var bundleInfo in _bundleInfoList)
            {
                var combineGUID = bundleInfo.GetDownloadCombineGUID();
                if (temper.Contains(combineGUID) == false)
                {
                    temper.Add(combineGUID);
                }
            }

            // 合并下载列表
            foreach (var bundleInfo in downloader._bundleInfoList)
            {
                var combineGUID = bundleInfo.GetDownloadCombineGUID();
                if (temper.Contains(combineGUID) == false)
                {
                    _bundleInfoList.Add(bundleInfo);
                }
            }

            // 重新统计下载信息
            CalculatDownloaderInfo();
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void BeginDownload()
        {
            if (_steps == ESteps.None)
            {
                OperationSystem.StartOperation(_packageName, this);
            }
        }

        /// <summary>
        /// 暂停下载
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void PauseDownload()
        {
            _isPause = true;
        }

        /// <summary>
        /// 恢复下载
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void ResumeDownload()
        {
            _isPause = false;
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void CancelDownload()
        {
            if (_steps != ESteps.Done)
            {
                _steps = ESteps.Done;
                Status = EOperationStatus.Failed;
                Error = "User cancel.";

                foreach (var downloader in _downloaders)
                {
                    downloader.Release();
                }
            }
        }
    }

    [UnityEngine.Scripting.Preserve]
    public sealed class ResourceDownloaderOperation : DownloaderOperation
    {
        [UnityEngine.Scripting.Preserve]
        internal ResourceDownloaderOperation(string packageName, List<BundleInfo> downloadList, int downloadingMaxNumber, int failedTryAgain, int timeout)
            : base(packageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout)
        {
        }

        /// <summary>
        /// 创建空的下载器
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        internal static ResourceDownloaderOperation CreateEmptyDownloader(string packageName, int downloadingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = new List<BundleInfo>();
            var operation = new ResourceDownloaderOperation(packageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout);
            return operation;
        }
    }

    [UnityEngine.Scripting.Preserve]
    public sealed class ResourceUnpackerOperation : DownloaderOperation
    {
        [UnityEngine.Scripting.Preserve]
        internal ResourceUnpackerOperation(string packageName, List<BundleInfo> downloadList, int downloadingMaxNumber, int failedTryAgain, int timeout)
            : base(packageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout)
        {
        }

        /// <summary>
        /// 创建空的解压器
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        internal static ResourceUnpackerOperation CreateEmptyUnpacker(string packageName, int upackingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = new List<BundleInfo>();
            var operation = new ResourceUnpackerOperation(packageName, downloadList, upackingMaxNumber, failedTryAgain, int.MaxValue);
            return operation;
        }
    }

    [UnityEngine.Scripting.Preserve]
    public sealed class ResourceImporterOperation : DownloaderOperation
    {
        [UnityEngine.Scripting.Preserve]
        internal ResourceImporterOperation(string packageName, List<BundleInfo> downloadList, int downloadingMaxNumber, int failedTryAgain, int timeout)
            : base(packageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout)
        {
        }

        /// <summary>
        /// 创建空的导入器
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        internal static ResourceImporterOperation CreateEmptyImporter(string packageName, int upackingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = new List<BundleInfo>();
            var operation = new ResourceImporterOperation(packageName, downloadList, upackingMaxNumber, failedTryAgain, int.MaxValue);
            return operation;
        }
    }
}
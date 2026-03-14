using System;
using UnityEngine.Networking;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class UnityWebFileRequestOperation : UnityWebRequestOperation
    {
        private UnityWebRequestAsyncOperation _requestOperation;
        private readonly string _fileSavePath;

        [UnityEngine.Scripting.Preserve]
        internal UnityWebFileRequestOperation(string url, string fileSavePath, int timeout = 60, bool appendTimeTicks = false) : base(url, timeout, appendTimeTicks)
        {
            _fileSavePath = fileSavePath;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.CreateRequest;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.CreateRequest)
            {
                _latestDownloadBytes = 0;
                _latestDownloadRealtime = Time.realtimeSinceStartup;

                CreateWebRequest();
                _steps = ESteps.Download;
            }

            if (_steps == ESteps.Download)
            {
                Progress = _requestOperation.progress;
                if (_requestOperation.isDone == false)
                {
                    CheckRequestTimeout();
                    return;
                }

                if (CheckRequestResult())
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                }

                // 注意：最终释放请求器
                DisposeRequest();
            }
        }

        [UnityEngine.Scripting.Preserve]
        internal override void InternalOnAbort()
        {
            _steps = ESteps.Done;
            DisposeRequest();
        }

        [UnityEngine.Scripting.Preserve]
        private void CreateWebRequest()
        {
            var requestURL = _requestURL;
            if (_appendTimeTicks)
            {
                requestURL += $"?time_ticks={DateTime.Now.Ticks}";
            }

            _webRequest = DownloadSystemHelper.NewUnityWebRequestGet(requestURL);
            var handler = new DownloadHandlerFile(_fileSavePath);
            handler.removeFileOnAbort = true;
            _webRequest.downloadHandler = handler;
            _webRequest.disposeDownloadHandlerOnDispose = true;
            _requestOperation = _webRequest.SendWebRequest();
        }
    }
}
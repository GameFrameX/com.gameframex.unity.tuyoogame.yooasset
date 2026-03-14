using System;
using UnityEngine.Networking;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class UnityWebTextRequestOperation : UnityWebRequestOperation
    {
        private UnityWebRequestAsyncOperation _requestOperation;

        /// <summary>
        /// 请求结果
        /// </summary>
        public string Result { private set; get; }


        [UnityEngine.Scripting.Preserve]
        internal UnityWebTextRequestOperation(string url, int timeout = 60, bool appendTimeTicks = false) : base(url, timeout, appendTimeTicks)
        {
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
                    Result = _webRequest.downloadHandler.text;
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
            var handler = new DownloadHandlerBuffer();
            _webRequest.downloadHandler = handler;
            _webRequest.disposeDownloadHandlerOnDispose = true;
            _requestOperation = _webRequest.SendWebRequest();
        }
    }
}
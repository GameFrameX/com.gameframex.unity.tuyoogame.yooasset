namespace YooAsset
{
    public class ClearCacheFilesOptions
    {
        /// <summary>
        /// 清理模式
        /// </summary>
        public string ClearMode;

        /// <summary>
        /// 附加参数
        /// </summary>
        public object ClearParam;
    }

    public abstract class FSClearCacheFilesOperation : AsyncOperationBase
    {
    }

    public sealed class FSClearCacheFilesCompleteOperation : FSClearCacheFilesOperation
    {
        private readonly string _error;

        public FSClearCacheFilesCompleteOperation()
        {
            _error = null;
        }

        public FSClearCacheFilesCompleteOperation(string error)
        {
            _error = error;
        }

        protected override void InternalStart()
        {
            if (string.IsNullOrEmpty(_error))
            {
                Status = EOperationStatus.Succeed;
            }
            else
            {
                Status = EOperationStatus.Failed;
                Error = _error;
            }
        }

        protected override void InternalUpdate()
        {
        }
    }
}
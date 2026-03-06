using System;
using System.Text;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class RemoteDebuggerDefine
    {
        public static readonly Guid kMsgSendPlayerToEditor = new("e34a5702dd353724aa315fb8011f08c3");
        public static readonly Guid kMsgSendEditorToPlayer = new("4d1926c9df5b052469a1c63448b7609a");
    }
}
// ==========================================================================================
//   GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//   GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//   均受中华人民共和国及相关国际法律法规保护。
//   are protected by the laws and regulations of the People's Republic of China and related international regulations.
//   使用本项目须严格遵守相应法律法规与开源许可证之规定。
//   Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//   本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//   This project is dual-licensed under the MIT License and Apache License 2.0,
//   完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//   please refer to the LICENSE file in the root directory of the source code for the full license text.
//   禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//   It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//   侵犯他人合法权益等法律法规所禁止的行为！
//   or infringe upon the legal rights and interests of others, as prohibited by laws and regulations!
//   因基于本项目二次开发所产生的一切法律纠纷与责任，
//   Any disputes or liabilities arising from secondary development based on this project
//   本项目组织与贡献者概不承担。
//   shall be borne solely by the developer, the project organization and contributors assume no responsibility.
//   GitHub 仓库：https://github.com/GameFrameX
//   GitHub Repository:  https://github.com/GameFrameX
//   Gitee  仓库：https://gitee.com/GameFrameX
//   Gitee Repository:   https://gitee.com/GameFrameX
//   CNB  仓库：https://cnb.cool/GameFrameX
//   CNB Repository:     https://cnb.cool/GameFrameX
//   官方文档：https://gameframex.doc.alianblank.com/
//   Official Documentation: https://gameframex.doc.alianblank.com/
//  ==========================================================================================

namespace YooAsset
{
    /// <summary>
    /// 小游戏直载文件系统选项基类（直载 bundle 网络失败重试参数的公共数据载体）。
    /// <para>各平台 Option（如 WechatFileSystemOption）派生本类；参数经平台 Creater 工厂 optional 参数 → FileSystemParameters（键由平台包定义）→ 平台 FileSystem.SetParameter 读回 → 平台 FSLoadBundleOperation 消费。</para>
    /// <para>平台默认值差异经子类构造器赋值表达（如 <c>LoadBundleRetryCount = N;</c>）；属性非虚，钳制语义仅本类实现，子类不得经属性重写绕过。</para>
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public abstract class FileSystemOption
    {
        private int _loadBundleRetryCount = 4;

        /// <summary>
        /// 直载 bundle 网络失败的退避重试次数（0 = 关闭重试，单发即 Failed，等价热修前行为；负值一律按 0 处理）。
        /// <para>默认 4：微信端 CDN 边缘（并发建连按连接随机 RST ~13%、串行退避重试 10/10 命中）实测标定，zhenyingcanmeng 1.1.8 已现网实证；其余平台跨平台复用同一基线。</para>
        /// </summary>
        public int LoadBundleRetryCount
        {
            get { return _loadBundleRetryCount; }
            set { _loadBundleRetryCount = value < 0 ? 0 : value; }
        }

        private float _loadBundleRetryBackoffSeconds = 0.5f;

        /// <summary>
        /// 退避基准秒数：第 n 次重试等待 本值 × 2^(n-1)，默认 0.5 即 0.5/1/2/4s 指数退避（负值一律按 0 处理：失败后立即重试，不等待）。
        /// </summary>
        public float LoadBundleRetryBackoffSeconds
        {
            get { return _loadBundleRetryBackoffSeconds; }
            set { _loadBundleRetryBackoffSeconds = value < 0f ? 0f : value; }
        }
    }
}

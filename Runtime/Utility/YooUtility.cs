using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YooAsset
{
    /// <summary>
    /// 路径工具类
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public static class PathUtility
    {
        /// <summary>
        /// 路径归一化
        /// 注意：替换为Linux路径格式
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string RegularPath(string path)
        {
            return path.Replace('\\', '/');
        }

        /// <summary>
        /// 移除路径里的后缀名
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string RemoveExtension(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var index = str.LastIndexOf('.');
            if (index == -1)
            {
                return str;
            }
            else
            {
                return str.Remove(index); //"assets/config/test.unity3d" --> "assets/config/test"
            }
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string Combine(string path1, string path2, string path3)
        {
            return Path.Combine(path1, path2, path3);
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string Combine(string path1, string path2, string path3, string path4)
        {
            return Path.Combine(path1, path2, path3, path4);
        }
    }

    /// <summary>
    /// 文件工具类
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public static class FileUtility
    {
        /// <summary>
        /// 读取文件的文本数据
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string ReadAllText(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                return null;
            }

            return File.ReadAllText(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文件的字节数据
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static byte[] ReadAllBytes(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                return null;
            }

            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// 写入文本数据（会覆盖指定路径的文件）
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void WriteAllText(string filePath, string content)
        {
            // 创建文件夹路径
            CreateFileDirectory(filePath);

            var bytes = Encoding.UTF8.GetBytes(content);
            File.WriteAllBytes(filePath, bytes); //避免写入BOM标记
        }

        /// <summary>
        /// 写入字节数据（会覆盖指定路径的文件）
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void WriteAllBytes(string filePath, byte[] data)
        {
            // 创建文件夹路径
            CreateFileDirectory(filePath);

            File.WriteAllBytes(filePath, data);
        }

        /// <summary>
        /// 创建文件的文件夹路径
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void CreateFileDirectory(string filePath)
        {
            // 获取文件的文件夹路径
            var directory = Path.GetDirectoryName(filePath);
            CreateDirectory(directory);
        }

        /// <summary>
        /// 创建文件夹路径
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void CreateDirectory(string directory)
        {
            // If the directory doesn't exist, create it.
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// 获取文件大小（字节数）
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static long GetFileSize(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
    }

    /// <summary>
    /// 哈希工具类
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public static class HashUtility
    {
        [UnityEngine.Scripting.Preserve]
        private static string ToString(byte[] hashBytes)
        {
            var result = BitConverter.ToString(hashBytes);
            result = result.Replace("-", "");
            return result.ToLower();
        }

        #region SHA1

        /// <summary>
        /// 获取字符串的Hash值
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string StringSHA1(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            return BytesSHA1(buffer);
        }

        /// <summary>
        /// 获取文件的Hash值
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string FileSHA1(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return StreamSHA1(fs);
            }
        }

        /// <summary>
        /// 获取文件的Hash值
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string FileSHA1Safely(string filePath)
        {
            try
            {
                return FileSHA1(filePath);
            }
            catch (Exception e)
            {
                YooLogger.Exception(e);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取数据流的Hash值
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string StreamSHA1(Stream stream)
        {
            // 说明：创建的是SHA1类的实例，生成的是160位的散列码
            using (var hash = SHA1.Create())
            {
                var hashBytes = hash.ComputeHash(stream);
                return ToString(hashBytes);
            }
        }

        /// <summary>
        /// 获取字节数组的Hash值
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string BytesSHA1(byte[] buffer)
        {
            // 说明：创建的是SHA1类的实例，生成的是160位的散列码
            using (var hash = SHA1.Create())
            {
                var hashBytes = hash.ComputeHash(buffer);
                return ToString(hashBytes);
            }
        }

        #endregion

        #region MD5

        /// <summary>
        /// 获取字符串的MD5
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string StringMD5(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            return BytesMD5(buffer);
        }

        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string FileMD5(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return StreamMD5(fs);
            }
        }

        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string FileMD5Safely(string filePath)
        {
            try
            {
                return FileMD5(filePath);
            }
            catch (Exception e)
            {
                YooLogger.Exception(e);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取数据流的MD5
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string StreamMD5(Stream stream)
        {
            using (var provider = MD5.Create())
            {
                var hashBytes = provider.ComputeHash(stream);
                return ToString(hashBytes);
            }
        }

        /// <summary>
        /// 获取字节数组的MD5
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string BytesMD5(byte[] buffer)
        {
            using (var provider = MD5.Create())
            {
                var hashBytes = provider.ComputeHash(buffer);
                return ToString(hashBytes);
            }
        }

        #endregion

        #region CRC32

        /// <summary>
        /// 获取字符串的CRC32
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string StringCRC32(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            return BytesCRC32(buffer);
        }

        /// <summary>
        /// 获取文件的CRC32
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string FileCRC32(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return StreamCRC32(fs);
            }
        }

        /// <summary>
        /// 获取文件的CRC32
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string FileCRC32Safely(string filePath)
        {
            try
            {
                return FileCRC32(filePath);
            }
            catch (Exception e)
            {
                YooLogger.Exception(e);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取数据流的CRC32
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string StreamCRC32(Stream stream)
        {
            var hash = new CRC32Algorithm();
            var hashBytes = hash.ComputeHash(stream);
            return ToString(hashBytes);
        }

        /// <summary>
        /// 获取字节数组的CRC32
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string BytesCRC32(byte[] buffer)
        {
            var hash = new CRC32Algorithm();
            var hashBytes = hash.ComputeHash(buffer);
            return ToString(hashBytes);
        }

        #endregion
    }
}
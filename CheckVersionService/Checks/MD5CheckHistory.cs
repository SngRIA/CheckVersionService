using CheckVersionService.Interfaces;
using CheckVersionService.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FileInfo = CheckVersionService.Models.FileInfo;

namespace CheckVersionService.Checks
{
    public class MD5CheckHistory : ICheckHistory
    {
        private static MD5 _md5 = MD5.Create();
        private string _lastMD5Hash = string.Empty;

        /// <summary>
        /// Возвращает MD5 хэш файла
        /// </summary>
        /// <param name="file">Файл с данными</param>
        /// <returns>MD5</returns>
        public async Task<string> GetCalcValue(FileInfo file)
        {
            using (FileStream stream = File.OpenRead(file.Path))
            {
                _lastMD5Hash = await Task.Factory.StartNew(() => CalcMD5Hash(stream));
                return _lastMD5Hash;
            }
        }

        private string CalcMD5Hash(Stream inputStream)
        {
            byte[] hashBytes = _md5.ComputeHash(inputStream);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Проверка изменился ли файл
        /// </summary>
        /// <param name="newFile"></param>
        /// <param name="oldFile"></param>
        /// <returns></returns>
        public bool IsChanged(FileInfo newFile, FileInfo oldFile)
        {
            if(oldFile.Hash == newFile.Hash)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void IDispose()
        {
            _md5.Dispose();
        }
    }
}

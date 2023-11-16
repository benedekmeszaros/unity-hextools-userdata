using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace HexTools.Persitence
{
    public static class FileUtility
    {
        public static void Write(byte[] bytes, string file)
        {
            string filePath = GetFilePath(file);
            string folderPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            File.WriteAllBytes(filePath, bytes);
        }
        public static void Write(byte[] bytes, string folder, string file)
        {
            string filePath = Path.Combine(Application.persistentDataPath, folder, file);
            string folderPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            File.WriteAllBytes(filePath, bytes);
        }
        public static byte[] Read(string filePath)
        {
            filePath = GetFilePath(filePath);
            if (!File.Exists(filePath))
                return default;

            return File.ReadAllBytes(filePath);
        }
        public static bool Delete(string filePath)
        {
            filePath = GetFilePath(filePath);
            if (!File.Exists(filePath))
                return false;
            else
            {
                File.Delete(filePath);
                return true;
            }
        }
        public static bool Exists(string filePath)
        {
            return File.Exists(GetFilePath(filePath));
        }
        public static string GetFileName(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }
        public static string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
        public static byte[] Serialize(object userData)
        {
            string json = JsonUtility.ToJson(userData, true);
            return Encoding.ASCII.GetBytes(json);
        }
        public static T Deserialize<T>(byte[] bytes)
        {
            string json = Encoding.ASCII.GetString(bytes);
            return (T)Convert.ChangeType(JsonUtility.FromJson<T>(json), typeof(T));
        }
    }
}



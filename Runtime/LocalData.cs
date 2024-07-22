using System.Collections.Generic;

namespace HexTools.Persitence
{
    public static class LocalData
    {
        private static readonly Dictionary<string, IData> sharedData = new ();

        public static UserData<T> Init<T>(string file)
        {
            return Init<T>(file, default, false);
        }
        public static UserData<T> Init<T>(string file, T fallback)
        {
            return Init<T>(file, fallback, false);
        }
        public static UserData<T> Init<T>(string file, T fallback, bool isShared = false)
        {
            if (isShared && sharedData.TryGetValue(file, out IData data))
                return (UserData<T>)data;
            UserData<T> userData = new (file);
            if (userData.Exists)
                userData.Read();
            else
                userData.Overwrite(fallback);
            if (isShared)
                ShareUserData(userData);
            return userData;
        }
        public static bool TryGetSharedData<T>(string file, out UserData<T> userData)
        {      
            if (sharedData.TryGetValue(file, out IData data))
            {
                userData = (UserData<T>)data;
                return true;
            }
            else
            {
                userData = null;
                return false;
            }
        }
        public static bool IsUserDataShared(IData data)
        {
            return sharedData.ContainsKey(data.RelativePath);
        }
        public static bool ShareUserData(IData data)
        {
            return sharedData.TryAdd(data.RelativePath, data);
        }
        public static bool UnshareUserData(IData data)
        {
            return sharedData.Remove(data.RelativePath);
        }
        public static IData[] GetAllSharedData()
        {
            IData[] datas = new IData[sharedData.Count];
            int i = 0;
            foreach (KeyValuePair<string, IData> pair in sharedData)
                datas[i++] = pair.Value;
            return datas;
        }
        public static void ClearAllSharedData()
        {
            sharedData.Clear();
        }
    }
}

using System;

namespace HexTools.Persitence
{
    /// <summary>
    /// <b>Userdata</b> is a gateway between the memory and the hard disk.
    /// <br><b>NOTE</b>: <b>T</b> must have System.Serializable attribute.</br>
    /// </summary>
    /// <typeparam name="T">Serializable type</typeparam>
    public class UserData<T>
    {
        private readonly string file;
        private T value;

        /// <summary>
        ///  The object behind the given file location. 
        ///  <br><b>NOTE</b>: If the value is not loaded yet this function gona try it automatically</br>.
        /// </summary>
        public T Value
        {
            get
            {
                if (value == null)
                    Load();
                return value;
            }
        }
        /// <summary>
        /// The name of the file.
        /// </summary>
        public string Name { get => FileUtility.GetFileName(file); }
        /// <summary>
        /// The extension type of the file.
        /// </summary>
        public string Extension { get => FileUtility.GetFileExtension(file); }

        public UserData(string file)
        {
            this.file = file;
        }

        /// <summary>
        /// Create a new <b>Userdata</b> instance and a corresponding file on the disk.
        /// <br><b>NOTE</b>: If the file is already exists, its value get higher priority.</br>
        /// </summary>
        /// <param name="file">The full name of the file.</param>
        /// <param name="intiValue">The fallback value of the corresponding file.</param>
        /// <returns></returns>
        public static UserData<T> Init(string file, T intiValue)
        {
            UserData<T> userData = new(file);
            if (!userData.Exists())
                userData.Override(intiValue);
            else
                userData.Load();
            return userData;
        }

        /// <summary>
        /// Write to the disk.
        /// </summary>
        public void Save()
        {
            if (value != null)
                FileUtility.Write(Serialize(Value), file);
        }
        /// <summary>
        /// Overwrite the current value of the file.
        /// </summary>
        /// <param name="value">New value.</param>
        public void Override(T value)
        {
            this.value = value;
            Save();
        }
        /// <summary>
        /// Load the value of the corresponding file if it exists.
        /// </summary>
        /// <returns></returns>
        public T Load()
        {
            byte[] bytes = FileUtility.Read(file);
            if (bytes != null)
                value = Deserialize(bytes);
            return value;
        }
        /// <summary>
        /// Wipe the value from the memory.
        /// </summary>
        public void Unload()
        {
            value = default;
        }
        /// <summary>
        /// Delete the corresponding file from the disk.
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            return FileUtility.Delete(file);
        }
        /// <summary>
        /// Checking if the file exists in the given location. 
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return FileUtility.Exists(file);
        }
        /// <summary>
        /// Invoke the given action, then save the value.
        /// </summary>
        /// <param name="action">Modify action.</param>
        public void Modify(Action<T> action)
        {
            action.Invoke(Value);
            Save();
        }
        /// <summary>
        /// Invoke the given action, and if it return <b>true</b> save the value.
        /// </summary>
        /// <param name="action">Modify action.</param>
        /// <returns></returns>
        public bool Modify(Func<T, bool> action)
        {
            var result = action.Invoke(Value);
            if (result)
                Save();
            return result;
        }

        //Generic functions
        public override bool Equals(object obj)
        {
            if (obj is UserData<T> userData)
                return userData.file.Equals(file);
            else
                return false;
        }
        public override int GetHashCode()
        {
            int hash = 0;
            if (file != null)
                hash += 13 * file.GetHashCode();
            if (value != null)
                hash += 17 * value.GetHashCode();
            return hash;
        }
        public override string ToString()
        {
            return FileUtility.GetFilePath(file);
        }

        //Customizable functions
        public virtual byte[] Serialize(T value)
        {
            return FileUtility.Serialize(value);
        }
        public virtual T Deserialize(byte[] bytes)
        {
            return FileUtility.Deserialize<T>(bytes);
        }
    }
}

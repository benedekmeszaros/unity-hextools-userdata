using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HexTools.Persitence
{
    [Serializable]
    public class UserData<T> : IData
    {
        [Serializable]
        private struct SingleField
        {
            public T value;

            public SingleField(T value)
            {
                this.value = value;
            }
        }

        [SerializeField] private string relativePath = "data.json";
        [SerializeField] private T value;

        /// <summary>
        /// Gets the current value of the data.
        /// </summary>
        public T Value { get => value; }
        /// <summary>
        /// Gets the file name of the file without extension.
        /// </summary>
        public string Name { get => FileUtility.GetFileName(relativePath); }
        /// <summary>
        ///  Checks if the file exists.
        /// </summary>
        public bool Exists { get => FileUtility.Exists(relativePath); }
        /// <summary>
        /// Gets the file extension.
        /// </summary>
        public string Extension { get => FileUtility.GetFileExtension(relativePath); }
        /// <summary>
        /// Gets the relative path of the file.
        /// </summary>
        public string RelativePath { get => relativePath; }
        /// <summary>
        /// Gets the absolute path of the file.
        /// </summary>
        public string AbsolutePath { get => FileUtility.GetFilePath(relativePath); }


        public UserData(string relativePath)
        {
            this.relativePath = relativePath;
        }
        public UserData(string relativePath, T value)
        {
            this.relativePath = relativePath;
            this.value = value;
        }

        [Obsolete("Init method is deprecated and will be removed in the next patch. Use the constructor instead.")]
        public static UserData<T> Init(string relativePath, T value)
        {
            UserData<T> userData = new UserData<T>(relativePath, value);
            if (!userData.Exists)
                userData.Overwrite(value);
            else
                userData.Load();
            return userData;
        }

        /// <summary>
        /// Save the current value to the file. If the file does not exist, create a new one.
        /// </summary>
        public void Save()
        {
            if (value != null)
                FileUtility.Write(Serialize(Value), relativePath);
        }
        /// <summary>
        /// Save the current value to the file asynchronously. If the file does not exist, create a new one.
        /// </summary>
        public async Task SaveAsync()
        {
            if (value != null)
                await FileUtility.WriteAsync(Serialize(Value), relativePath);
        }
        /// <summary>
        /// Save the current value to the file asynchronously. If the file does not exist, create a new one.
        /// </summary>
        /// <param name="onReady">Callback which gets called when the operation finished.</param>
        public async void SaveAsync(Action onReady)
        {
            if (value != null)
            {
                await FileUtility.WriteAsync(Serialize(Value), relativePath);
                onReady.Invoke();
            }
        }
        /// <summary>
        ///  Overwrites the current value with the specified value and saves it. If the file does not exist, create a new one.
        /// </summary>
        /// <param name="value">Replacement parameter.</param>
        public void Overwrite(T value)
        {
            this.value = value;
            Save();
        }
        /// <summary>
        ///  Overwrites the current value with the specified value and saves it asynchronously. If the file does not exist, create a new one.
        /// </summary>
        /// <param name="value">Replacement parameter.</param>
        public async Task OverwriteAsync(T value)
        {
            this.value = value;
            await SaveAsync();
        }
        /// <summary>
        ///  Overwrites the current value with the specified value and saves it asynchronously. If the file does not exist, create a new one.
        /// </summary>
        /// <param name="value">Replacement parameter.</param>
        /// <param name="onReady">Callback which gets called when the operation finished.</param>
        public async void OverwriteAsync(T value, Action onReady)
        {
            this.value = value;
            await SaveAsync();
            onReady.Invoke();
        }

        /// <summary>
        /// Loads the value from the file.
        /// </summary>
        /// <returns>The read value.</returns>
        public T Load()
        {
            Read();
            return value;
        }
        /// <summary>
        /// Loads the value from the file asynchronously.
        /// </summary>
        /// <returns>The read value.</returns>
        public async Task<T> LoadAsync()
        {
            await ReadAsync();
            return value;
        }
        /// <summary>
        /// Loads the value from the file asynchronously.
        /// </summary>
        /// <param name="onReady">Callback which gets called when the operation finished.</param>
        public async void LoadAsync(Action<T> onReady)
        {
            await ReadAsync();
            onReady.Invoke(value);
        }
        /// <summary>
        /// Loads the value from the file.
        /// </summary>
        public void Read()
        {
            byte[] bytes = FileUtility.Read(relativePath);
            if (bytes != null)
                value = Deserialize(bytes);
        }
        /// <summary>
        /// Loads the value from the file asynchronously.
        /// </summary>
        public async Task<T> ReadAsync()
        {
            byte[] bytes = await FileUtility.ReadAsync(relativePath);
            if (bytes != null)
                value = Deserialize(bytes);
            return value;
        }
        /// <summary>
        /// Loads the value from the file asynchronously.
        /// </summary>
        /// <param name="onReady">Callback which gets called when the operation finished.</param>
        public async void ReadAsync(Action<T> onReady)
        {
            byte[] bytes = await FileUtility.ReadAsync(relativePath);
            if (bytes != null)
                value = Deserialize(bytes);
            onReady.Invoke(value);
        }
        /// <summary>
        /// Delete the file from the disk.
        /// </summary>
        /// <returns>Is the file deleted?</returns>
        public bool Remove()
        {
            return FileUtility.Delete(relativePath);
        }
        /// <summary>
        /// Unloads the current value, setting it to the <b>default</b> value of <b>T</b>.
        /// </summary>
        public void Unload()
        {
            value = default;
        }
        /// <summary>
        /// Modifies the current value using the provided action and saves it.
        /// </summary>
        /// <param name="action">Modification action.</param>
        public void Modify(Action<T> action)
        {
            action.Invoke(Value);
            Save();
        }
        /// <summary>
        /// Modifies the current value using the provided action and saves it asynchronously.
        /// </summary>
        /// <param name="action">Modification action.</param>
        public async Task ModifyAsync(Action<T> action)
        {
            action.Invoke(Value);
            await SaveAsync();
        }
        /// <summary>
        ///  Modifies the current value using the provided action asynchronously. Saves the value if the action returns <b>true</b>.
        /// </summary>
        /// <param name="action">Conditional modification action.</param>
        /// <param name="onReady">Callback which gets called when the operation finished.</param>
        public async void ModifyAsync(Action<T> action, Action onReady)
        {
            action.Invoke(Value);
            await SaveAsync();
            onReady?.Invoke();
        }
        /// <summary>
        ///  Modifies the current value using the provided action. Saves the value if the action returns <b>true</b>.
        /// </summary>
        /// <param name="action">Conditional modification action.</param>
        public bool Modify(Func<T, bool> action)
        {
            var result = action.Invoke(Value);
            if (result)
                Save();
            return result;
        }
        /// <summary>
        ///  Modifies the current value using the provided action asynchronously. Saves the value if the action returns <b>true</b>.
        /// </summary>
        /// <param name="action">Conditional modification action.</param>
        public async Task<bool> ModifyAsync(Func<T, bool> action)
        {
            var result = action.Invoke(Value);
            if (result)
                await SaveAsync();
            return result;
        }
        /// <summary>
        ///  Modifies the current value using the provided action asynchronously. Saves the value if the action returns <b>true</b>.
        /// </summary>
        /// <param name="action">Conditional modification action.</param>
        /// <param name="onReady">Callback which gets called when the operation finished.</param>
        public async void ModifyAsync(Func<T, bool> action, Action<bool> onReady)
        {
            var result = action.Invoke(Value);
            if (result)
                await SaveAsync();
            onReady?.Invoke(result);
        }
        public override string ToString()
        {
            return $"{AbsolutePath} - (UserData<{typeof(T).Name}>)";
        }
        /// <summary>
        /// Serializes the value to a <b>byte[]</b>.
        /// </summary>
        public virtual byte[] Serialize(T value)
        {
            if (IsSingleField())
                return FileUtility.Serialize(new SingleField(value));
            else
                return FileUtility.Serialize(value);
        }
        /// <summary>
        /// Deserialize the <b>byte[]</b> to a value of type <b>T</b>.
        /// </summary>
        public virtual T Deserialize(byte[] bytes)
        {
            if (IsSingleField())
                return FileUtility.Deserialize<SingleField>(bytes).value;
            else
                return FileUtility.Deserialize<T>(bytes);
        }

        /// <summary>
        /// Determines if the type <b>T</b> should be considered as a single field for serialization.
        /// </summary>
        private bool IsSingleField()
        {
            Type type = typeof(T);
            return type == typeof(string) ||
                type.IsValueType || 
                type.IsArray || 
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
        }
    }
}

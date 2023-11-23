# HexTools - UserData
The UserData class, which is available as open-source and specifically designed for use in Unity®, serves as a versatile and generic solution. The primary objective of this class is to facilitate seamless and dependable Input/Output operations within Unity®, ensuring consistency and reliability across various platforms, irrespective of the inherent differences or peculiarities associated with each platform. This user-friendly class streamlines the complexities of handling Input/Output tasks, providing a straightforward yet robust mechanism that developers can employ to interact with data consistently, regardless of the specific Unity® platform they are targeting.
<br/>

## Supported platforms
- Windows
- Linux
- Mac
- Android
- iOS
- WebGL
- VR
## Supported Editor
- Unity 2017 or later

## Consturctor
| Implementation | Description |
| :------------- | :---------- |
| `UserData<T>(string)`| Initializes a new instance of the UserData<T> class that is empty. |
## Properties
| Implementation | Type |Description |
| :------------- | :--- |:---------- |
| `Extension`| `string` | Return the extension type of the file. |
| `Name`| `string` | Return the name of the file. |
| `Value`| `T` | Return the object behind the given file location. <br/> <b>NOTE</b>: If the value is not loaded yet this function gona try it automatically. |
## Static Method
| Implementation | Return type | Description |
| :------------- | :---------- | :---------- |
| `Init(string, T)` |`Userdata<T>`| Initializes a new instance of the UserData<T> class and create a corresponding file for it. <br />  <b>NOTE</b>: If the file is already exists, its contetnt get loaded to the instance. |
## Methods
| Implementation | Return type | Description |
| :------------- | :---------- | :---------- |
| `Save()`| `void` |Write the `Value` object to the disk. |
| `Overwrite(T)`| `void` |Overwrite the current value of the file. <br />  <b>NOTE</b>: If the file not exists it will create one. |
| `Load()`| `T` |Return the value of the corresponding file and cach it, only if the file is exists. |
| `Unload()`| `void` | Clear the value from the cach memory. |
| `Remove()`| `bool` | Delete the corresponding file from the disk. |
| `Exists()`| `bool` | Checking if the corresponding file exists. |
| `Modify(Action<T>)`| `void` | Invoke the given action, then call the `Save()` function. |
| `Modify(Func<T, bool>)` | `bool` | Invoke the given action, then if it's returning value is `true` call `Save()` function.|
| `GetHashCode()` | `int` |Serves as the default hash function.|
| `Equals(object)`| `bool` | Determines whether the specified object is equal to the current object. | 
| `ToString()`| `string` |Returns the full path of the corresponding file. |
## Overridable Methods
| Implementation | Return type | Description |
| :------------- | :---------- | :---------- |
| `Deserialize(byte[])` | `T` | Convert the given `byte[]` to object. |
| `Serialize(T)` | `byte[]` | Convert the given object to `byte[]`. |

# Examples
```cs
[System.Serializable]
public class Progress
{
    public int coins;
    public float highScore;
}
```
```cs
public class ProgressTracer : MonoBehaviour
{
    private UserData<Progress> progressData;

    void Awake()
    {
        progressData = UserData<Progress>.Init("Saved/progress.json", new Progress());
    }
}
```
# License
[MIT](https://choosealicense.com/licenses/mit/)
# Author
[Mészáros Benedek](https://www.github.com/benedekmeszaros)

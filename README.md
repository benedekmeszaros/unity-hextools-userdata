# HexTools - UserData
The UserData is an open-source generic class for Unity®. The goal of this class is simple and reliable I/O operations regardless of the given platform. It is quick and easy to implement, ideal for projects that are based on serialized documents. 
Featuring base CRUD operation and more.
<br/>

## Supported platforms
- Windows
- Linux
- Mac
- Android
- iOS
- WebGL
## Consturctor
| Implementation | Description |
| :------------- | :---------- |
| `UserData<T>(string)`| Initializes a new instance of the UserData<T> class that is empty. |
## Properties
| Implementation | Description |
| :------------- | :---------- |
| `Value`| Return the object behind the given file location. <br/> <b>NOTE</b>: If the value is not loaded yet this function gona try it automatically. |
| `Name`| Return the name of the file. |
| `Extension`| Return the extension type of the file. |
## Methods
| Implementation | Description |
| :------------- | :---------- |
| `Save()`| Write the `Value` object to the disk. |
| `Overwrite(T)`| Overwrite the current value of the file. <br />  <b>NOTE</b>: If the file not exists it will create one. |
| `Load()`|  Return the value of the corresponding file and cach it, only if the file is exists. |
| `Unload()`| Clear the value from the cach memory. |
| `Remove()`| Delete the corresponding file from the disk. |
| `Exists()`| Checking if the corresponding file exists. |
| `Modify(Action<T>)`| Invoke the given action, then call the `Save()` function. |
| `Modify(Func<T, bool>)` | Invoke the given action, then if it's returning value is `true` call `Save()` function.|
| `GetHashCode()` | Serves as the default hash function.|
| `Equals(object)`| Determines whether the specified object is equal to the current object. | 
| `ToString()`| Returns the full path of the corresponding file. |
## Static Method
| Implementation | Description |
| :------------- | :---------- |
| `Init(string, T)`| Initializes a new instance of the UserData<T> class and create a corresponding file for it. <br />  <b>NOTE</b>: If the file is already exists, its value get higher priority. |
## Overridable Methods for Custom Serialization
| Implementation | Description |
| :------------- | :---------- |
| `Serialize(T)` | Convert the given object to `byte[]`. |
| `Deserialize(byte[])`| Convert the given `byte[]` to object. |

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

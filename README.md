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
- Console
- VR
## Supported Editors
- Unity 2017 or later

## Import package
In the Editor, you can access the Package Manager window through: Window > Package Manager, than:
1.  Open the add menu in the Package Manager's toolbar.
2. The options for adding packages appear. Add package from git URL button.
3. Select Add package from git URL from the add menu. A text box and an Add button appear.
4. Enter a valid Git URL in the text box and click Add.
 
Illustration:
<br/>![Img](https://docs.unity3d.com/uploads/Main/upm-ui-giturl.png)

Enter the following git URL:

```bash
  https://github.com/benedekmeszaros/unity-hextools-userdata.git
```

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
| `Save()`| `void` |Write the `Value` object to the disk. <br/> Each file is stored within the directory specified by [`Application.persistentDataPath`](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html). |
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
By default this class using Unity's `JsonUtility` for serialization.
| Implementation | Return type | Description |
| :------------- | :---------- | :---------- |
| `Deserialize(byte[])` | `T` | Convert the given `byte[]` to object. |
| `Serialize(T)` | `byte[]` | Convert the given object to `byte[]`. |

# Examples
The subsequent utilization of the `Progress` class, in conjunction with the `ProgressTracer` class, will be employed for purposes of demonstration.
```cs
[System.Serializable]
public class Progress
{
    public int coins;
    public float highScore;
}
```
### Instantiate
```cs
using HexTools.Persistence;

public class ProgressTracer : MonoBehaviour
{
    private UserData<Progress> progressData;

    void Awake()
    {
        progressData = UserData<Progress>.Init("Saved/progress.json", new Progress());
        // or
        progressData = new UserData<Progress>("Saved/progress.json");
        if(!progressData.Exists())
            progressData.Overwrite(new Progress());
        else
            progressData.Load();
    }
}
```
### Saving modifications
Periodic modifications may be preserved by invoking the `Save()` function. For a more resilient approach, users have the option to employ the `Modify(...)` function along with a lambda expression.
<br/>
<br/>
Basic approach:
```cs
public void AddCoins(int coins)
{
    Progress p = progressData.Value;
    p.coins += coins;
    progressData.Save();
}
```
Using the unconditional `Modify` function:
```cs
public void AddCoins(int coins)
{
    progressData.Modify(p => p.coins += coins);
}
```
Using the conditional `Modify` function:
```cs
public bool UpdateHighScore(float score)
{
    // Saving occurs exclusively when the provided anonymous function returns a true value.
    return progressData.Modify(p =>
    {
        bool update = p.highScore < score;

        if (update)
           p.highScore = score;

        return update;
    });
}
```
### Overwrite content
```cs
public void ResetProgress()
{
    progressData.Overwrite(new Progress());
}
```

### Delete the corresponding file from disk
```cs
public bool RemoveProgress()
{
    //Returns true if the file is successfuly deleted.
    return progressData.Remove();
}
```

# License
- [MIT](https://choosealicense.com/licenses/mit/)
# Author
- [Benedek Mészáros](https://www.github.com/benedekmeszaros)

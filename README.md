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
- Unity 2020 or later

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
| `UserData<T>(string, T)`| Initializes a new instance of the UserData<T> with a provided value. |
## Properties
| Implementation | Type |Description |
| :------------- | :--- |:---------- |
| `Value`| `T` | Gets the current value of the data. |
| `Name`| `string` | Gets the file name of the file without extension. |
| `Extension`| `string` | Gets the file extension. |
| `Exists`| `bool` | Checks if the file exists. |
| `RelativePath`| `string` | Gets the relative path of the file. |
| `AbsolutePath`| `string` | Gets the absolute path of the file. |
## Methods
| Implementation | Return type | Description |
| :------------- | :---------- | :---------- |
| `Save()`| `void` | Save the current value to the file. If the file does not exist, create a new one. <br/> Each file is stored within the directory specified by [`Application.persistentDataPath`](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html). |
| `SaveAsync()`| `Task` | Save the current value to the file asynchronously. |
| `SaveAsync(Action)`| `void` | Save the current value to the file asynchronously. |
| `Overwrite(T)`| `void` | Overwrites the current value with the specified value and saves it. If the file does not exist, create a new one. |
| `OverwriteAsync(T)`| `Task` | Overwrites the current value with the specified value and saves it asynchronously. |
| `OverwriteAsync(T, Action)`| `void` | Overwrites the current value with the specified value and saves it asynchronously. |
| `Load()`| `T` | Loads the value from the file. |
| `LoadAsync()`| `Task<T>` | Loads the value from the file asynchronously. |
| `LoadAsync(Action<T>)`| `void` | Loads the value from the file asynchronously. |
| `Read()`| `void` | Loads the value from the file. |
| `ReadAsync()`| `Task<T>` | Loads the value from the file asynchronously. |
| `ReadAsync(Action<T>)`| `void` | Loads the value from the file asynchronously. |
| `Unload()`| `void` | Unloads the current value, setting it to the <b>default</b> value of <b>T</b>. |
| `Remove()`| `bool` | Delete the file from the disk. |
| `Modify(Action<T>)`| `void` | Modifies the current value using the provided action and saves it. |
| `ModifyAsync(Action<T>)`| `Task` | Modifies the current value using the provided action and saves it asynchronously. |
| `ModifyAsync(Action<T>, Action)`| `Task` | Modifies the current value using the provided action and saves it asynchronously. |
| `Modify(Func<T, bool>)` | `bool` | Modifies the current value using the provided action. Saves the value if the action returns <b>true</b>. |
| `ModifyAsync(Func<T, bool>)` | `Task<bool>` | Modifies the current value using the provided action asynchronously. Saves the value if the action returns <b>true</b>. |
| `ModifyAsync(Func<T, bool>, Action<bool>)` | `void` | Modifies the current value using the provided action asynchronously. Saves the value if the action returns <b>true</b>. |
| `GetHashCode()` | `int` |Serves as the default hash function.|
| `Equals(object)`| `bool` | Determines whether the specified object is equal to the current object. | 
| `ToString()`| `string` |Returns the full path of the corresponding file. |
## Overridable Methods
By default this class using the built in `JsonUtility` for serialization.
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

Create a new instance, then load its content. If the target file does not exist, the object keeps its initial value.

```cs
using HexTools.Persistence;

public class ProgressTracer : MonoBehaviour
{
    private UserData<Progress> progressData = new UserData<Progress>("saved/progress.json", new Progress());

    void Awake()
    {
        // Load the content
        progressData.Read();
    }
}
```
### Save content/modifications
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

### Load content

```cs
public void LoadProgress()
{
    progressData.Read();
    //or
    progressData.Load();
}
```

### Overwrite content
```cs
public void ResetProgress()
{
    progressData.Overwrite(new Progress());
}
```

### Delete content
```cs
public bool RemoveProgress()
{
    //Returns true if the file is successfuly deleted.
    return progressData.Remove();
}
```

## Inspector view
<b>UserData</b> has its own property drawer for visual debugging directly in the Inspector, which support deep serialization. Make sure to give `SerializeField` attribute to the desired field or make it `public`, otherwise the editor won't display it.

```cs
[SerializeField] private UserData<Progress> progressData;
```

<br>![Img](https://github.com/user-attachments/assets/12d03c74-2810-4cf9-9826-bef00b52f29a)

Make sure to provide a valid relative path for the given instance.

<br>![Img](https://github.com/user-attachments/assets/ced51e1c-7eca-4186-8396-2be7d1cb5c7e)

Enter `Play mode` to access the following utility controlls:
- Create/Save
- Read
- Delete

<br>![Img](https://github.com/user-attachments/assets/62f39db9-1dd9-4146-b284-6c5a3f399d50)

# License
- [MIT](https://choosealicense.com/licenses/mit/)
# Author
- [Benedek Mészáros](https://www.github.com/benedekmeszaros)

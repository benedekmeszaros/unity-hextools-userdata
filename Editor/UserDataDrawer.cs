using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HexTools.Persitence.Edior
{
    [CustomPropertyDrawer(typeof(IData), true)]
    public class UserDataDrawer : PropertyDrawer
    {
        private readonly string PATH_PATTERN = @"^((\./|\.\./)?([a-zA-Z0-9_\-]+\/)*[a-zA-Z0-9_\-]+\.[a-zA-Z0-9]+)$";
        private readonly string WRAPPER_KEY = "hexdog-idata-wrapper";
        private readonly GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold
        };
        private readonly GUIStyle boldLabelStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Bold
        };
        private bool isInitialized = false;
        private bool exists = false;
        private Texture2D saveIcon;
        private Texture2D readIcon;
        private Texture2D trashIcon;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string wrapper = $"{WRAPPER_KEY}-{property.propertyPath}";

            EditorGUI.BeginProperty(position, label, property);
            property.serializedObject.Update();
            float singleFieldHeight = EditorGUIUtility.singleLineHeight;
            Rect fieldRect = new Rect(position.x, position.y, position.width, singleFieldHeight);
            bool isConnected = Application.isPlaying;
            Rect foldoutRect = new Rect(position.x, fieldRect.y, position.width, singleFieldHeight);
            SerializedProperty pathProperty = property.FindPropertyRelative("relativePath");
            bool isPathValid = Regex.IsMatch(pathProperty.stringValue, PATH_PATTERN);

            SetSession(wrapper, EditorGUI.Foldout(fieldRect, GetSession(wrapper), label.text, foldoutStyle));
            Rect statusArea = new Rect(position.x + position.width - (exists ? 65 : 80), fieldRect.y, exists ? 65 : 80, singleFieldHeight);
            EditorGUI.LabelField(statusArea, new GUIContent(exists ? "[Mounted]" : "[Unmounted]"), boldLabelStyle);           

            if (Event.current.type == EventType.MouseDown && foldoutRect.Contains(Event.current.mousePosition))
            {
                SetSession(wrapper, !GetSession(wrapper));
                Event.current.Use();
            }
            EditorGUI.BeginChangeCheck();
            if (GetSession(wrapper))
            {
                fieldRect.y += singleFieldHeight + 2;
                if (!isPathValid)
                {
                    EditorGUI.HelpBox(new Rect(fieldRect.position, new Vector2(fieldRect.width, 30)), $"Path \"{pathProperty.stringValue}\" is Invalid", MessageType.Error);
                    fieldRect.y += 35;
                }
                EditorGUI.indentLevel++;               
                GUI.enabled = !isConnected;
                fieldRect.width -= 30;
                
                EditorGUI.PropertyField(fieldRect, pathProperty, new GUIContent("Path"));

                fieldRect.width += 30;
                Rect buttonArea = new Rect(position.x + position.width - 25, fieldRect.y, 25, singleFieldHeight);
                string relativePath = pathProperty.stringValue;
                GUI.enabled = isPathValid;
                if (GUI.Button(buttonArea, "..."))
                {
                    try
                    {
                        if (!isPathValid)
                            Process.Start(Application.persistentDataPath);
                        else
                        {
                            string absolutePath = Path.Combine(Application.persistentDataPath, Path.GetDirectoryName(relativePath));
                            if (Directory.Exists(absolutePath))
                                Process.Start(absolutePath);
                            else
                                Process.Start(Application.persistentDataPath);
                        }
                    } catch
                    {
                        Process.Start(Application.persistentDataPath);
                    }
                }
                fieldRect.y += singleFieldHeight + 3;
                if(isConnected)
                {
                    DrawUtilityControlls(fieldRect, property);
                    fieldRect.y += 27;
                }

                GUI.enabled = true;
                DrawProperties(fieldRect, property);
                fieldRect.y += singleFieldHeight;
            }
            if (EditorGUI.EndChangeCheck())
            {
                exists = VerifyExistance(property.FindPropertyRelative("relativePath").stringValue);
                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(!isInitialized)
            {
                isInitialized = true;
                OnInit(property);
            }

            float totalHeight = 0;
            string wrapper = $"{WRAPPER_KEY}-{property.propertyPath}";
            if (GetSession(wrapper))
            {
                SerializedProperty iterator = property.Copy();
                SerializedProperty endProperty = iterator.GetEndProperty();

                if (iterator.NextVisible(true))
                {
                    do
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;

                    } while (iterator.NextVisible(false) && !SerializedProperty.EqualContents(iterator, endProperty));
                }
                if (!Regex.IsMatch(property.FindPropertyRelative("relativePath").stringValue, PATH_PATTERN))
                {
                    totalHeight += 35;
                }
                else if(Application.isPlaying)
                    totalHeight += 27;
            }
            return totalHeight + GetSingleFieldHeight();
        }

        private void OnInit(SerializedProperty property)
        {
            string relPath = property.FindPropertyRelative("relativePath").stringValue;
            exists = VerifyExistance(relPath);
            saveIcon = (Texture2D)EditorGUIUtility.Load("Packages/com.hexdogstudio.userdata/Editor/Resources/save-icon.png");
            readIcon = (Texture2D)EditorGUIUtility.Load("Packages/com.hexdogstudio.userdata/Editor/Resources/read-icon.png");
            trashIcon = (Texture2D)EditorGUIUtility.Load("Packages/com.hexdogstudio.userdata/Editor/Resources/trash-icon.png");

            if(saveIcon == null)
                saveIcon = (Texture2D)EditorGUIUtility.Load("Assets/Persistence/Editor/Resources/save-icon.png");
            if(readIcon == null)
                readIcon = (Texture2D)EditorGUIUtility.Load("Assets/Persistence/Editor/Resources/read-icon.png");
            if(trashIcon == null)
                trashIcon = (Texture2D)EditorGUIUtility.Load("Assets/Persistence/Editor/Resources/trash-icon.png");
        }
        private void DrawProperties(Rect position, SerializedProperty property)
        {
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = iterator.GetEndProperty();

            int initialIndentLevel = --EditorGUI.indentLevel;
            if (iterator.NextVisible(true))
            {
                do
                {
                    if (iterator.name.Equals("relativePath"))
                        continue;
                    EditorGUI.indentLevel = initialIndentLevel + iterator.depth;
                    position.height = EditorGUI.GetPropertyHeight(iterator, true);
                    EditorGUI.PropertyField(position, iterator, true);
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                } while (iterator.NextVisible(false) && !SerializedProperty.EqualContents(iterator, endProperty));
            }

            EditorGUI.indentLevel = initialIndentLevel;
        }
        private void DrawUtilityControlls(Rect position, SerializedProperty property)
        {
            object target = property.serializedObject.targetObject;
            FieldInfo field = target.GetType().GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            object userData = field.GetValue(target);
            IData data = userData as IData;
            float width = 25;
            Rect buttonRect = new Rect(position.position, new Vector2(width, 25));
            if (GUI.Button(buttonRect, new GUIContent(saveIcon, exists ? "Save" : "Create")))
            {
                data.Save();
            }
            buttonRect.x += width + 2;
            GUI.enabled = exists;
            if (GUI.Button(buttonRect, new GUIContent(readIcon, "Read")))
            {
                data.Read();
            }
            buttonRect.x += width + 2;         
            if (GUI.Button(buttonRect, new GUIContent(trashIcon, "Remove")))
            {
                string absPath = data.AbsolutePath.Replace('\\','/');
                if (EditorUtility.DisplayDialog("Remove file from disk.", $"Are you sure you want to delete \"{absPath}\" from the disk? This operation can't be undone.", "Confirm","Cancel"))
                {
                    data.Remove();
                }
            }
            GUI.enabled = true;
        }
        private float GetSingleFieldHeight()
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
        }
        private bool GetSession(string key)
        {
            return SessionState.GetBool(key, true);
        }
        private void SetSession(string key, bool value)
        {
            SessionState.SetBool(key, value);
        }
        private bool VerifyExistance(string relPath)
        {
            string absPath = Path.Combine(Application.persistentDataPath, relPath);
            return FileUtility.Exists(absPath);
        }
    }
}

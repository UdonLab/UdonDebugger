
using System;
using Sonic853.Udon.ArrayPlus;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon;

namespace Sonic853.Udon.Debuger
{
    public class UdonDebuger : UdonSharpBehaviour
    {
        public GameObject[] showList;
        public GameObject Content;
        public GameObject logText;
        [NonSerialized] public string[] logs = new string[0];
        public InputField searchField;
        public Toggle infoToggle;
        public Toggle warningToggle;
        public Toggle errorToggle;
        public static UdonDebuger Instance()
        {
            var obj = GameObject.Find("UdonLab Debuger");
            if (obj == null)
            {
                Debug.LogError("UdonDebuger not found");
                return null;
            }
            return (UdonDebuger)obj.GetComponent(typeof(UdonBehaviour));
        }
        protected virtual GameObject Logobj(string text)
        {
            logs = UdonArrayPlus.Add(logs, text);
            var obj = Instantiate(logText, Content.transform);
            // 将排序放在最前面
            obj.transform.SetSiblingIndex(0);
            var textobj = obj.GetComponent<Text>();
            textobj.text = text;
            CheckText(textobj);
            return obj;
        }
        protected void CheckText(Text textobj)
        {
            if (!infoToggle.isOn && textobj.text.Contains("][<color=white>Info</color>]\n<color=white>"))
            {
                textobj.gameObject.SetActive(false);
                return;
            }
            if (!warningToggle.isOn && textobj.text.Contains("][<color=yellow>Warning</color>]\n<color=yellow>"))
            {
                textobj.gameObject.SetActive(false);
                return;
            }
            if (!errorToggle.isOn && textobj.text.Contains("][<color=red>Error</color>]\n<color=red>"))
            {
                textobj.gameObject.SetActive(false);
                return;
            }
            if (!string.IsNullOrEmpty(searchField.text) && !textobj.text.Contains(searchField.text))
            {
                textobj.gameObject.SetActive(false);
                return;
            }
            textobj.gameObject.SetActive(true);
        }
        public void Log(string text, string program = "")
        {
            var _program = string.IsNullOrEmpty(program) ? "" : $"[{program}]";
            // [时分秒，补零][运行时间，秒.毫秒，精确到3位数，补零][Info]: <color=white>text</color>
            Debug.Log($"{_program}{text}");
            Logobj($"[{DateTime.Now:HH:mm:ss}][{Time.realtimeSinceStartup:0.000}]{_program}[<color=white>Info</color>]\n<color=white>{text}</color>");
        }
        public void LogWarning(string text, string program = "")
        {
            var _program = string.IsNullOrEmpty(program) ? "" : $"[{program}]";
            Debug.LogWarning($"{_program}{text}");
            Logobj($"[{DateTime.Now:HH:mm:ss}][{Time.realtimeSinceStartup:0.000}]{_program}[<color=yellow>Warning</color>]\n<color=yellow>{text}</color>");
        }
        public void LogError(string text, string program = "")
        {
            var _program = string.IsNullOrEmpty(program) ? "" : $"[{program}]";
            Debug.LogError($"{_program}{text}");
            Logobj($"[{DateTime.Now:HH:mm:ss}][{Time.realtimeSinceStartup:0.000}]{_program}[<color=red>Error</color>]\n<color=red>{text}</color>");
        }
        public void UpdateList()
        {
            foreach (Transform child in Content.transform)
            {
                var textui = child.GetComponent<Text>();
                if (textui != null)
                {
                    CheckText(textui);
                }
            }
        }
        public void Clear()
        {
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }
            logs = new string[0];
        }
        public void ToggleShow()
        {
            if (showList.Length == 0) return;
            var show = !showList[0].activeSelf;
            foreach (var i in showList)
            {
                i.SetActive(show);
            }
        }
    }
}

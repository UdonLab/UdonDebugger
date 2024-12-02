
using Sonic853.Udon.ArrayPlus;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.Debuger
{
    public class UdonDebugerTMP : UdonDebuger
    {
        protected override GameObject Logobj(string text)
        {
            logs = UdonArrayPlus.Add(logs, text);
            var obj = Instantiate(logText, Content.transform);
            // 将排序放在最前面
            obj.transform.SetSiblingIndex(0);
            var textobj = obj.GetComponent<TextMeshProUGUI>();
            textobj.text = text;
            CheckText(textobj);
            return obj;
        }
        protected void CheckText(TMP_Text textobj)
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
    }
}

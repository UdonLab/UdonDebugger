
using UdonSharp;
using UnityEngine;

namespace UdonLab
{
    public class TestTryLog : UdonSharpBehaviour
    {
        UdonDebuger udonDebuger;
        void Start()
        {
            udonDebuger = UdonDebuger.Instance();
            TryLog();
        }
        public void TryLog()
        {
            if (udonDebuger == null)
            {
                Debug.LogError("UdonDebuger not found");
                return;
            }
            udonDebuger.Log("TestTryLog");
            udonDebuger.LogWarning("TestTryLog");
            udonDebuger.LogError("TestTryLog");
        }
        public void SendFunction() => TryLog();
        public void SendFunctions() => TryLog();

    }
}

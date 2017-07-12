namespace RustProtect
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Snapshot : MonoBehaviour
    {
        public static RustProtect.Snapshot Singleton;

        private void Awake()
        {
            Singleton = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }

        public void CaptureSnapshot()
        {
            base.StartCoroutine(this.method_0());
        }

        private IEnumerator method_0()
        {
            yield return new WaitForEndOfFrame();
            Texture2D textured1 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            textured1.ReadPixels(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), 0, 0);
            textured1.Apply();
            Protection.Screenshot = textured1.EncodeToJPG();
            UnityEngine.Object.DestroyObject(textured1);
            yield return 0;
        }

        private void OnGUI()
        {
            if (NetCull.isClientRunning && (Protection.playerClient != null))
            {
                PlayerClient playerClient = Protection.playerClient;
                GUIStyle style = new GUIStyle {
                    fontSize = 10
                };
                style.normal.textColor = Color.white;
                if (Protection.firstBB && (Protection.lastBB))
                {
                    GUIStyle bb = new GUIStyle();
                    bb.normal.background = null;    //这是设置背景填充的
                    bb.normal.textColor = new Color(1, 0, 0);   //设置字体颜色的
                    bb.fontSize = 40; 

                    string aa = "RUST军火库提示您,这是给GM看的图片:看毛看,没看过挂B啊";
                    GUI.Label(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), aa, bb);
                }
                GUI.Label(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), "=RUST军火库= 荣誉出品", style);
                GUI.Label(new Rect(0f, 13f, (float)Screen.width, (float)Screen.height), "=去你妈的防挂端=", style);
                GUI.Label(new Rect(0f, 26f, (float)Screen.width, (float)Screen.height), DateTime.Now.ToString(Class3.smethod_10(0x266)), style);
                
                // GUI.Label(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), playerClient.userName + Class3.smethod_10(0x23e) + playerClient.userID, style);
                // GUI.Label(new Rect(0f, 13f, (float) Screen.width, (float) Screen.height), Class3.smethod_10(0x248) + Assembly.GetCallingAssembly().GetName().Version, style);
                // GUI.Label(new Rect(0f, 26f, (float) Screen.width, (float) Screen.height), DateTime.Now.ToString(Class3.smethod_10(0x266)), style);
            }
        }

    }
}


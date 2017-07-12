using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace RustProtect
{
	public class Snapshot : MonoBehaviour
	{
		public static Snapshot Singleton;

        private void Awake()
        {
            Snapshot.Singleton = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }


        private void OnGUI()
		{
			if (NetCull.isClientRunning && Protection.playerClient != null)
			{
				PlayerClient playerClient = Protection.playerClient;
				GUIStyle gUIStyle = new GUIStyle();
				gUIStyle.fontSize = 10;
				gUIStyle.normal.textColor = Color.white;
				GUI.Label(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), playerClient.userName + Class3.smethod_10(574) + playerClient.userID, gUIStyle);
				GUI.Label(new Rect(0f, 13f, (float)Screen.width, (float)Screen.height), Class3.smethod_10(584) + Assembly.GetCallingAssembly().GetName().Version, gUIStyle);
				GUI.Label(new Rect(0f, 26f, (float)Screen.width, (float)Screen.height), DateTime.Now.ToString(Class3.smethod_10(614)), gUIStyle);
			}
		}

		public void CaptureSnapshot()
		{
			base.StartCoroutine(this.method_0());
		}

        private IEnumerator method_0()
        {
            yield return new WaitForEndOfFrame();
            Texture2D textured1 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            textured1.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
            textured1.Apply();
            Protection.Screenshot = textured1.EncodeToJPG();
            UnityEngine.Object.DestroyObject(textured1);
            yield return 0;
        }
    }
}

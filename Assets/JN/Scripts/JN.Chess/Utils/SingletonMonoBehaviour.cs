using UnityEngine;

namespace JN.Utils
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
		private static bool isApplicationQuit;
        private static T _instance;
        public static T Instance
        {
            get
            {
				if (isApplicationQuit)
				{
					return default(T);
				}

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T) + "");
                    _instance = singleton.AddComponent<T>();
					isApplicationQuit = false;
					Application.quitting += () => isApplicationQuit = true;
                }

                return _instance;
            }
        }
    }
}
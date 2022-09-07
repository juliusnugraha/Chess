using UnityEngine;
using System.Collections.Generic;

namespace JN.Utils
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        protected List<T> list = new List<T>();
        protected string objectName;
        GameObject original;
        protected Transform parent;

        public void Init(string ObjectName, GameObject Original, Transform Parent = null, int InitialCount = 2)
        {
            list = new List<T>();
            original = Original;
            objectName = ObjectName;
            parent = Parent;

            for (int i = 0; i < InitialCount; i++)
            {
                AddNewObject();
            }
        }

        public T GetNewObject()
        {
            T obj = GetAvailableObject();
            if (obj == default(T))
            {
                return AddNewObject();
            }

			return obj;
        }

        public void KillAll()
        {
            if (list.Count == 0)
                return;
                
            foreach (T t in list)
            {
                t.gameObject.SetActive(false);
            }
        }

        public List<T> GetList ()
        {
            return list;
        }

        public T[] ToArray()
        {
            return list.ToArray();
        }

        public int CountActiveObject()
        {
            int count = 0;
            foreach (T t in list)
            {
                if (t.gameObject.activeSelf)
                {
                    count++;
                }
            }
            return count;
        }

        public void Iterate(System.Action<T> Method, bool ActiveOnly = false)
        {
            foreach (T t in list)
            {
                if (t.gameObject.activeSelf || !ActiveOnly)
                {
                    Method(t);
                }
            }
        }

        protected T GetAvailableObject()
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
					T cur = list[i];
                    if (!cur.gameObject.activeSelf)
                    {
                        return cur;
                    }
                }
            }
            return default(T);
        }

        protected T AddNewObject()
        {
            GameObject o = GameObject.Instantiate(original);
            o.transform.SetParent(parent, false);
            o.name = objectName + " " + list.Count;
            T t = o.GetComponent<T>();
            if (t != null)
            {
                list.Add(t);
            }
            o.SetActive(false);
			return t;
        }

        public void DestroyAll()
        {
            if (list.Count == 0)
                return;
                
            T[] deleted = list.ToArray();
            for (int i = deleted.Length - 1; i >= 0; i--)
            {
                GameObject.Destroy(deleted[i].gameObject);
            }
            list.Clear();
        }
    }
}

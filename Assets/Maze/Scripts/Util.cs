using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    public static class Util {

        public static Vector3 ToVector3XY(this Vector2 v) {
            return new Vector3(v.x, v.y, 0);
        }
        public static Vector3 ToVector3XZ(this Vector2 v) {
            return new Vector3(v.x, 0, v.y);
        }
        public static Vector2 ToVector2XY(this Vector3 v) {
            return new Vector2(v.x, v.y);
        }
        public static Vector2 ToVector2XZ(this Vector3 v) {
            return new Vector2(v.x, v.z);
        }
        public static Vector3 NullifyY(this Vector3 v) {
            return new Vector3(v.x, 0, v.z);
        }
        public static Vector3 NullifyZ(this Vector3 v) {
            return new Vector3(v.x, v.y, 0);
        }
        public static void DestroyChild(Transform child) {
            child.gameObject.SetActive(false);
            GameObject.Destroy(child.gameObject);
        }

        public static void DestroyAllChildren(Transform parent) {
            for (int i = parent.childCount - 1; i >= 0; --i) {
                Util.DestroyChild(parent.GetChild(i));
            }
        }

        public static void CopyTransform(Transform from, Transform to) {
            from.position = to.position;
            from.rotation = to.rotation;
            //from.localScale = to.localScale;
        }

        public static void ParentAndCenter(Transform child, Transform parent, bool worldPositionStays=true) {
            child.SetParent(parent, worldPositionStays);
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            //child.localScale = Vector3.one;
        }

        public static List<int> GenerateSeries(int length, int start = 0) {
            List<int> ret = new List<int>(length);
            for (int i = 0; i < length; ++i) {
                ret.Add(start++);
            }
            return ret;
        }
        public static T[] RandomFilter<T>(T[] source, int count) {
            int len = Mathf.Min(count, source.Length);
            T[] ret = new T[len];
            Pool<T> pool = new Pool<T>(source);
            for (int i = 0; i < len; ++i) {
                ret[i] = pool.Get();
            }
            return ret;
        }

        public static T[] Join<T>(T[] a, T[] b) {
            T[] ret = new T[a.Length + b.Length];
            int j = 0;
            for (int i = 0; i < a.Length; ++i) {
                ret[j++] = a[i];
            }
            for (int i = 0; i < b.Length; ++i) {
                ret[j++] = b[i];
            }
            return ret;
        }

        public static void AddToList<T>(List<T> to, T[] from) {
            for (int i = 0; i < from.Length; ++i) {
                to.Add(from[i]);
            }
        }
        public static void AddToList<T>(List<T> to, List<T> from) {
            foreach(T t in from) {
                to.Add(t);
            }
        }
    }
}
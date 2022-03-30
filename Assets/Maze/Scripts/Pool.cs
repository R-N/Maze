using UnityEngine;
using System.Collections.Generic;

namespace Maze {
    public class Pool<T> {
        protected T[] source;
        protected List<int> pool;
        protected int count = 0;


        public int recycleCount = 0;
        public Pool(T[] source) {
            this.source = source;
        }
        public Pool(List<T> source) {
            this.source = source.ToArray();
        }

        public virtual void Refresh() {
            this.count = source.Length;
            this.pool = Util.GenerateSeries(count);
            recycleCount++;
        }

        public virtual int GetIndex() {
            if (count == 0) {
                Refresh();
            }
            int j = Random.Range(0, --count);
            int i = pool[j];
            pool.RemoveAt(j);
            return i;
        }

        public virtual T Get() {
            int i = GetIndex();
            T ret = this[i];
            return ret;
        }

        public virtual T this[int index] {
            get {
                return source[index];
            }
        }

        public virtual int Length {
            get {
                return source.Length;
            }
        }

        public virtual T[] Get(int count) {
            T[] ret = new T[count];
            for (int i = 0; i < count; ++i) {
                ret[i] = Get();
            }
            return ret;
        }
    }
    public class PoolOfPool<T> : Pool<Pool<T>>  {
        public PoolOfPool(Pool<T>[] source) : base(source) {
        }
        public PoolOfPool(List<Pool<T>> source) : base(source) {
        }

        public override void Refresh() {
            for (int i = 0; i < source.Length; ++i) {
                source[i].Refresh();
            }
            count = Length;
        }

        public override int Length {
            get {
                int l = 0;
                for (int i = 0; i < source.Length; ++i) {
                    l += source.Length;
                }
                return l;
            }
        }

        public new T Get() {
            int j = GetIndex();
            return this[j];
        }

        public new T this[int j] {
            get {

                for (int i = 0; i < source.Length; ++i) {
                    if (j >= source.Length) {
                        j -= source.Length;
                        continue;
                    }
                    return source[i][j];
                }
                throw new System.Exception();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Maze.Database {
    public class Reader : IDataReader {
        IDataReader source;
        Cursor parent;
        public Reader(Cursor parent, IDataReader source) {
            this.parent = parent;
            this.source = source;
        }

        public object this[string name] {
            get {
                return source[name];
            }
        }

        public object this[int i] {
            get {
                return source[i];
            }
        }

        public int Depth {
            get {
                return source.Depth;
            }
        }

        public bool IsClosed {
            get {
                return source.IsClosed;
            }
        }

        public int RecordsAffected {
            get {
                return source.RecordsAffected;
            }
        }

        public int FieldCount {
            get {
                return source.FieldCount;
            }
        }

        public void Close() {
            source.Close();
            parent.Close();
        }

        public void Dispose() {
            Close();
        }

        public bool GetBoolean(int i) {
            return source.GetBoolean(i);
        }

        public byte GetByte(int i) {
            return source.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) {
            return source.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i) {
            return source.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) {
            return source.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i) {
            return new Reader(parent, source.GetData(i));
        }

        public string GetDataTypeName(int i) {
            return source.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i) {
            return source.GetDateTime(i);
        }

        public decimal GetDecimal(int i) {
            return source.GetDecimal(i);
        }

        public double GetDouble(int i) {
            return source.GetDouble(i);
        }

        public Type GetFieldType(int i) {
            return source.GetFieldType(i);
        }

        public float GetFloat(int i) {
            return source.GetFloat(i);
        }

        public Guid GetGuid(int i) {
            return source.GetGuid(i);
        }

        public short GetInt16(int i) {
            return source.GetInt16(i);
        }

        public int GetInt32(int i) {
            return source.GetInt32(i);
        }

        public long GetInt64(int i) {
            return source.GetInt64(i);
        }

        public string GetName(int i) {
            return source.GetName(i);
        }

        public int GetOrdinal(string name) {
            return source.GetOrdinal(name);
        }

        public DataTable GetSchemaTable() {
            return source.GetSchemaTable();
        }

        public string GetString(int i) {
            return source.GetString(i);
        }

        public object GetValue(int i) {
            return source.GetValue(i);
        }

        public int GetValues(object[] values) {
            return source.GetValues(values);
        }

        public bool IsDBNull(int i) {
            return source.IsDBNull(i);
        }

        public bool NextResult() {
            return source.NextResult();
        }

        int _successfulReadCount = 0;
        public int successfulReadCount{
            get{
                return _successfulReadCount;
            }
        }

        public bool Read() {
            bool ret = source.Read();
            _successfulReadCount++;
            return ret;
        }

        public T Get<T>(string name){
            object o = this[name];
            try{
                return (T)o;
            }catch (InvalidCastException ex){
                string error = string.Format("Error casting {0} to {1}", o.GetType(), typeof(T));
                throw new InvalidCastException(error);
            }
        }

        public double GetDouble(string name) {
            return Get<double>(name);
        }
        public float GetFloat(string name) {
            return (float)GetDouble(name);
        }
        public int GetInt32(string name) {
            return Get<int>(name);
        }
        public long GetInt64(string name) {
            return Get<long>(name);
        }
        public short GetInt16(string name) {
            return Get<short>(name);
        }
        public string GetString(string name) {
            return Get<string>(name);
        }
        public bool GetBool(string name) {
            return GetBoolean(name);
        }
        public bool GetBool(int i) {
            return GetBoolean(i);
        }
        public bool GetBoolean(string name) {
            return GetInt32(name) != 0;
        }

        
    }
}

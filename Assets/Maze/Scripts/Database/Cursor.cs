using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using System.Linq;
using System.Text;

namespace Maze.Database {
    public class Cursor : IDisposable {
        Connection connection;
        private IDbCommand _command = null;
        private IDbCommand command {
            get {
                if (_command == null) {
                    _command = connection.CreateCommand();
                }
                return _command;
            }
            set {
                if (_command != null && _command != value) {
                    _command.Dispose();
                }
                _command = value;
            }
        }
        private Reader _reader = null;
        private Reader reader {
            get {
                return _reader;
            }
            set {
                if (_reader != null && _reader != value && !_reader.IsClosed) {
                    _reader.Close();
                }
                _reader = value;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        

        public Cursor(Connection connection) {
            this.connection = connection;
            _command = connection.CreateCommand();
        }

        public void Open() {
            connection.Open();
        }
        public void Close() {

            if (_reader != null && !_reader.IsClosed) {
                _reader.Close();
            }
            _reader = null;

            if (_command != null) {
                _command.Dispose();
            }
            _command = null;

            connection.Close();
        }

        public Reader ExecuteReader() {
            try{
                Open();
                reader = new Reader(this, command.ExecuteReader());
                return reader;
            }finally{
                Close();
            }
        }

        public long lastInsertId{
            get{
                using(Cursor cur = Database.GetCursor()){
                    cur.commandText = "SELECT last_insert_rowid()";
                    using(Reader reader= cur.ExecuteReader()){
                        reader.Read();
                        return reader.GetInt64(0);
                    }
                }
            }
        }

        public string commandText {
            get {
                return command.CommandText;
            }
            set {
                command.CommandText = value;
            }
        }

        public object this[string key] {
            get {
                if (reader == null) {
                    return null;
                }
                return reader[key];

            }
            set {
                command.Parameters[key] = value;
            }
        }
        public object this[int key] {
            get {
                if (reader == null) {
                    return null;
                }
                return reader[key];

            }
            set {
                command.Parameters[key] = value;
            }
        }

        public int ExecuteNonQuery() {
            try{
                Open();
                int ret = command.ExecuteNonQuery();
                return ret;
            }catch(SqliteExecutionException ex){
                throw;
            }finally{
                Close();
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Cursor() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}

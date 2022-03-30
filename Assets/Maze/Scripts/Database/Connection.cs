using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using System.Text;
using UnityEngine;

namespace Maze.Database {
    public class Connection {

        private IDbConnection _connection = null;
        private IDbConnection connection {
            get {
                if (_connection == null) {
                    Init1();
                }
                return _connection;
            }
            set {
                if (!isClosed && _connection != value) {
                    Close();
                }
                
                _connection = value;
            }
        }
        private string dbLocation;
        private string dir;

        private string dbName;

        string connectionString;

        private const string packageName = "com.wp.maze";

        public Connection(string dbName) {
            this.dbName = dbName;
            Init1();
        }

        public void Init1() {
            Debug.Log("Call to OpenDB:" + dbName);
            // check if file exists in Application.persistentDataPath
            if (Application.platform == RuntimePlatform.Android)
                dir = "/data/data/" + packageName + "/files";
            else
                dir = Application.persistentDataPath;
            dbLocation = dir + Path.AltDirectorySeparatorChar + dbName + ".db";

            if (File.Exists(dbLocation) && PlayerPrefs.GetString("version") != Application.version) {
                DeleteDB();
                PlayerPrefs.SetString("version", Application.version);
            }

            if (!File.Exists(dbLocation)) {
                Debug.Log("Deploying db");
                string path;
                if (Application.platform == RuntimePlatform.Android)
                    path = "jar:file://" + Application.dataPath + "!/assets/" + dbName + ".db";
                else
                    path = "file://" + Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + dbName + ".db";

                // if it doesn't ->
                // open StreamingAssets directory and load the db -> 
                WWW loadDB = new WWW(path);
                while (!loadDB.isDone) {
                }
                // then save to Application.persistentDataPath
                if (File.Exists(dbLocation)) {
                    Debug.Log("File exists!");
                } else {
                    File.WriteAllBytes(dbLocation, loadDB.bytes);
                    if (File.Exists(dbLocation)) {
                        FileStream file = File.Open(dbLocation, FileMode.Open);
                        if (file.Length == loadDB.bytes.LongLength) {
                            Debug.Log("Write succeeded. bytes : " + file.Length);
                        } else {
                            Debug.Log("Write failed - size mismatch " + file.Length + " should be " + loadDB.bytes.LongLength);
                        }
                        loadDB.Dispose();
                        file.Close();
                    } else {
                        Debug.Log("Write failed - file doesn't exist");
                    }
                }
            }

            connectionString = "URI=file:" + dbLocation;

            Init2();

        }
        public void DeleteDB() {
            Debug.Log("Deleting DB");
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            if (!string.IsNullOrEmpty(dir) && !string.IsNullOrEmpty(dbName)) {
                if (File.Exists(dbLocation))
                    File.Delete(dbLocation);
                if (File.Exists(dbLocation + "-shm"))
                    File.Delete(dbLocation + "-shm");
                if (File.Exists(dbLocation + "-wal"))
                    File.Delete(dbLocation + "-wal");
            } else {
                Debug.Log("dir " + dir);
                Debug.Log("dbName " + dbName);
            }
        }

        public IDbCommand CreateCommand() {
            return connection.CreateCommand();
        }
        
        /// <summary>
        /// Basic initialization of SQLite
        /// </summary>
        public void Init2() {
            Debug.Log("SQLiter - Opening SQLite Connection at " + connectionString);
            _connection = new SqliteConnection(connectionString);

            Open();

            IDbCommand _command = _connection.CreateCommand();
            IDataReader _reader;

             
            // WAL = write ahead logging, very huge speed increase
            _command.CommandText = "PRAGMA journal_mode = WAL;";
            _command.ExecuteNonQuery();
            
            // journal mode = look it up on google, I don't remember
            _command.CommandText = "PRAGMA journal_mode";
            _reader = _command.ExecuteReader();
            if (_reader.Read())
                Debug.Log("SQLiter - WAL value is: " + _reader.GetString(0));
            _reader.Close();

            // more speed increases
            _command.CommandText = "PRAGMA synchronous = OFF";
            _command.ExecuteNonQuery();

            // and some more
            _command.CommandText = "PRAGMA synchronous";
            _reader = _command.ExecuteReader();
            if (_reader.Read())
                Debug.Log("SQLiter - synchronous value is: " + _reader.GetInt32(0));
            _reader.Close();
            
            _command.Dispose();

            Close();
        }

        public bool isOpen {
            get {
                return _connection != null && _connection.State == ConnectionState.Open;
            }
        }

        public bool isClosed {
            get {
                return _connection == null || _connection.State == ConnectionState.Open;
            }
        }

        public void Open() {
            if (!isOpen) {
                _connection.Open();
                Debug.Log("Successfully opened");
            }
        }

        public void Close() {

            if (!isClosed) {
                _connection.Close();
                Debug.Log("Successfully closed");
            }
        }

        public Cursor GetCursor() {
            return new Cursor(this);
        }
    }

}
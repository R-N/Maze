using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Database {
    public static class Database {
        private const string DB_NAME = "Maze";
        private static Connection _connection;
        private static Connection connection {
            get {
                if (_connection == null) {
                    Init();
                }
                return _connection;
            }
            set {
                if (!isClosed && _connection != value) {
                    _connection.Close();
                }

                _connection = value;
            }
        }

        public static void DeleteDB() {
            if (_connection != null) _connection.DeleteDB();
        }

        public static bool isOpen {
            get {
                return _connection != null && _connection.isOpen;
            }
        }

        public static bool isClosed {
            get {
                return _connection == null || _connection.isClosed;
            }
        }

        public static void Init() {
            connection = new Connection(DB_NAME);
        }

        public static Cursor GetCursor() {
            return connection.GetCursor();
        }

    }
}

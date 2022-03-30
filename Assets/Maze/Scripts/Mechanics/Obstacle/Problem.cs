using Maze.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Maze.Mechanics.Obstacle {
    public enum Category {
        math
    }
    public class Problem  {
        public int id;
        public string text;
        public int difficulty;
        public Category category;

        public static Dictionary<int, Problem> problemCache = new Dictionary<int, Problem>();
        public static Dictionary<int, Statement> statementCache = new Dictionary<int, Statement>();


        public Statement[] statements;

        public int statementCount {
            get {
                return statements.Length;
            }
        }

        public Problem() { }

        public Problem(Database.Cursor cur, Reader reader) {
            Read(cur, reader);
        }
        
        public void Read(Database.Cursor cur, Reader reader) {
            this.id = reader.GetInt32("Problem_Id");
            this.text = reader.GetString("Text");
        }

        public void RetrieveStatements() {
            using (Cursor cur = Database.Database.GetCursor()) {
                RetrieveStatements(cur);
            }
        }
        public void RetrieveStatements(Database.Cursor cur){
            List<Statement> ret = new List<Statement>();

            string cmd = "SELECT * FROM ANSWER WHERE Problem_Id=" + id + " ORDER BY RANDOM()";
            cur.commandText = cmd;
            using(Reader reader = cur.ExecuteReader()){
                while (reader.Read()) {
                    Statement s = new Statement(this, cur, reader);
                    statementCache[s.id] = s;
                    ret.Add(s);
                }
            }
            
            this.statements = ret.ToArray<Statement>();
        }

        public Statement[] GetStatements(bool? correct = null, int count=1) {
            List<Statement> ret = new List<Statement>();

            int j = 0;
            for (int i = 0; i < statements.Length; ++i) {
                if (!correct.HasValue || statements[i].correct == correct.Value) {
                    ret.Add(statements[i]);
                    if (j == count) {
                        break;
                    }
                }
            }

            return ret.ToArray<Statement>();
        }

        public static Problem[] GetRandom(int count=1, int difficulty = 0, Category[] categories = null) {
            List<Problem> ret = new List<Problem>();

            string cmd = "SELECT * FROM PROBLEM";
            List<string> wheres = new List<string>();
            if(categories != null && categories.Length > 0){
                UnityEngine.Debug.Log("CATEGORIES IS NOT NULL: " + categories.Length);
                int mask = 0;
                for(int i = 0; i < categories.Length; ++i){
                    mask = mask | (1 << ((int)categories[i]));
                }
                wheres.Add("CATEGORY_ID & " + mask);
            } 
            if (wheres.Count > 0) {
                cmd += " WHERE " + String.Join(" AND ", wheres.ToArray<string>());
            }
            cmd += " ORDER BY ";
            if(difficulty > 0){
                //wheres.Add("DIFFICULTY=" + difficulty);
                cmd += String.Format("ABS(DIFFICULTY-{0}), DIFFICULTY, ", difficulty);
            }
            cmd += "RANDOM() ASC LIMIT " + count;

            UnityEngine.Debug.Log("Cmd: " + cmd);


            using (Cursor cur = Database.Database.GetCursor()) {
                cur.commandText = cmd;
                using (Reader reader = cur.ExecuteReader()) {
                    while (reader.Read()) {
                        Problem p = new Problem(cur, reader);
                        ret.Add(p);
                    }
                }
            }

            Problem[] ret2 = ret.ToArray<Problem>();

            for (int i = 0; i < ret2.Length; ++i) {
                ret2[i].RetrieveStatements();
            }

            return ret2;
        }

        public static Problem GetProblem(int id) {
            if (problemCache.ContainsKey(id)) {
                return problemCache[id];
            }
            using (Cursor cur = Database.Database.GetCursor()) {
                cur.commandText = "SELECT * FROM PROBLEM WHERE PROBLEM_ID=" + id;
                using (Reader reader = cur.ExecuteReader()) {
                    if (reader.Read()) {
                        Problem prob = new Problem(cur, reader);
                        problemCache[prob.id] = prob;
                        prob.RetrieveStatements(cur);
                        return prob;
                    }
                }
            }
            return null;
        }

        public Statement GetStatementById(int answerId) {
            for (int i = 0; i < statements.Length; ++i) {
                if (statements[i].id == answerId) {
                    return statements[i];
                }
            }
            return null;
        }

        public static Statement GetStatement(int answerId) {
            if (statementCache.ContainsKey(answerId)) {
                return statementCache[answerId];
            }

            using (Cursor cur = Database.Database.GetCursor()) {
                cur.commandText = String.Format("SELECT * FROM PROBLEM WHERE PROBLEM_ID=(SELECT DISTINCT PROBLEM_ID FROM ANSWER WHERE ANSWER_ID={0})", answerId);
                using (Reader reader = cur.ExecuteReader()) {
                    if (reader.Read()) {
                        Problem prob = new Problem(cur, reader);
                        problemCache[prob.id] = prob;
                        prob.RetrieveStatements(cur);
                        return prob.GetStatementById(answerId);
                    }
                }
            }
            return null;
        }
        
    }
}

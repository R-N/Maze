using Maze.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Maze.Mechanics.Obstacle {
    public class Statement  {
        Problem parent;
        public int id;
        string answer;
        public bool correct;

        public Statement(Problem parent) {
            this.parent = parent;
        }
        public Statement(Problem parent, Database.Cursor cur, Reader reader) : this(parent){
            Read(cur, reader);
        }
        
        
        public void Read(Database.Cursor cur, Reader reader) {
            this.id = reader.GetInt32("Answer_Id");
            this.answer = reader.GetString("Text");

            this.correct = reader.GetBool("Correct");
        }
        
        public string text {
            get {
                return String.Format(parent.text, answer) ;
            }
        }
    }
}

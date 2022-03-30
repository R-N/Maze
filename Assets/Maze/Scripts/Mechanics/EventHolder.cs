using Maze.Inputs;
using UnityEngine;
using UnityEngine.Events;
namespace Maze.Mechanics {
    public class EventHolder : MonoBehaviour {
        public UnityEvent event1;
        public UnityEvent event2;

        public BoolEvent boolEvent;
        public FloatEvent floatEvent;
        public IntEvent intEvent;
        public GameObjectEvent gameObjectEvent;
        

        public void Invoke1() {
            event1.Invoke();
        }
        public void Invoke2() {
            event2.Invoke();
        }

        public void Invoke(bool arg) {
            boolEvent.Invoke(arg);
        }
        public void Invoke(float arg) {
            floatEvent.Invoke(arg);
        }
        public void Invoke(int arg) {
            intEvent.Invoke(arg);
        }
        public void Invoke(GameObject arg) {
            gameObjectEvent.Invoke(arg);
        }
    }
}

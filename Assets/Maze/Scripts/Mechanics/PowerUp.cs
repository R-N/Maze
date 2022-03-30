using Maze.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Maze.Mechanics {
    public class PowerUp : ActionOnControllerContact {
        public Transform body;
        public float deltaRot = 30;
        public float lifeTime = 1000000;
        public float sineFreq = 20;
        public float sineAmpli = 2;
        

        Buff buff;

        float curSine = 0;

        bool done = false;

        public void SetBuff(Buff buff) {
            this.buff = buff;
        }

        public Rigidbody GetRigidbody() {
            return GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void LateUpdate() {
            body.Rotate(Vector3.up, deltaRot * Time.deltaTime, Space.Self);
            curSine += sineFreq * Mathf.Deg2Rad * Time.deltaTime;
            if (curSine > 360) curSine -= 360;
            body.localPosition = new Vector3(0, sineAmpli * Mathf.Sin(curSine), 0);
        }


        public override void Action(Controller controller)
        {
            if(done) return;
            controller.status.AddBuff(buff);
            Destroy(gameObject);
            done=true;
        }
    }
}
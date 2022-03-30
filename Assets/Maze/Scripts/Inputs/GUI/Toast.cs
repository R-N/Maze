using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maze.Mechanics;

namespace Maze.Inputs.GUI{
	public class Toast : MonoBehaviour {
		
		public Text title;
		public Text text;
		public MovingBlock move;

		void Start () {
			
		}

		public void SetMovement(Transform start, Transform end){
			move.startTransform = start;
			move.targetTransform = end;
			move.Move();
		}

		public void SetTitle(string title){
			this.title.text = title;
			this.title.gameObject.SetActive(true);
		}

		public void SetText(string text){
			this.text.text = text;
			this.text.gameObject.SetActive(true);
		}
		
		public void Destroy(){
			Destroy(gameObject);
		}
	}
}
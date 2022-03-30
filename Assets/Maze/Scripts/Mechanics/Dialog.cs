using Maze.Inputs;
using Maze.Inputs.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Mechanics {
    public class Dialog : MonoBehaviour, IButton{
        public Text title;
        public Text text;
        public ButtonAction button;

        public WorldCanvasHelper canvas = null;

         string _title = null;
         string _text = null;
         bool? _buttonActive;

        // Use this for initialization
        void Start() {
            if(canvas == null){
                canvas = GetComponent<WorldCanvasHelper>();
            }
        }

        public void SetActiveTimed(bool active){
            canvas.SetActiveTimed(active);
        }
        void Refresh() {
        }

        public void OnEnable(){
            if(_title != null){
                SetTitle(_title);
                _title = null;
            }
            if(_text != null){
                SetText(_text);
                _text = null;
            }
            if(_buttonActive.HasValue){
                SetButtonActive(_buttonActive.Value);
                _buttonActive = null;
            }
        }

        public void SetTitle(string title) {
            if(isActiveAndEnabled) StartCoroutine("_SetTitle", title);
            else _title = title;
        }

        IEnumerator _SetTitle(string title) {
            yield return new WaitForEndOfFrame();
            this.title.gameObject.SetActive(true);
            this.title.text = title;
            this.title.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            Refresh();
            yield return null;
        }

        public void SetText(string text) {
            if(isActiveAndEnabled) StartCoroutine("_SetText", text);
            else _text = text;
        }

        IEnumerator _SetText(string text) {
            yield return new WaitForEndOfFrame();
            this.text.gameObject.SetActive(true);
            this.text.text = text;
            this.text.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            Refresh();
            yield return null;
        }

        public void SetButtonEventHandler(ButtonEvent handler) {
            button.onClick = handler;
            Refresh();
        }

        public void SetButtonActive(bool active) {
            if(isActiveAndEnabled) StartCoroutine("_SetButtonActive", active);
            else _buttonActive = active;
        }

        IEnumerator _SetButtonActive(bool active) {
            yield return new WaitForEndOfFrame();
            button.gameObject.SetActive(active);
            Refresh();
            yield return null;
        }
    }
}
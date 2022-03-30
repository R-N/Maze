using System;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control;
using Maze.Mechanics;
using Maze.Level;

namespace Maze.Inputs {
    public struct ButtonEventData {
        public GameObject source;
        public Controller actor;
        public int sourceId;
    }
    public struct ToggleEventData {
        public GameObject source;
        public Controller actor;
        public int sourceId;
        public bool enabled;
    }
    public struct PlayerDetectedEventData {
        public PlayerDetector source;
        public bool playerInside;
    }
}

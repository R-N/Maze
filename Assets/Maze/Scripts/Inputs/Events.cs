using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Maze.Level;

namespace Maze.Inputs {
    [Serializable]
    public class ButtonEvent : UnityEvent<ButtonEventData> {
    }
    [Serializable]
    public class ToggleEvent : UnityEvent<ToggleEventData> {
    }
    [Serializable]
    public class PlayerDetectedEvent : UnityEvent<PlayerDetectedEventData> {
    }
    [Serializable]
    public class MyEvent<T0> : UnityEvent<T0>{ }
    [Serializable]
    public class MyEvent<T0, T1> : UnityEvent<T0, T1> { }
    [Serializable]
    public class MyEvent<T0, T1, T2> : UnityEvent<T0, T1, T2> { }
    [Serializable]
    public class MyEvent<T0, T1, T2, T3> : UnityEvent<T0, T1, T2, T3> { }
    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }
    [Serializable]
    public class IntEvent : UnityEvent<int> { }
    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    [Serializable]
    public class LevelPackSelectedEvent : UnityEvent<LevelPackData>{}

    [Serializable]
    public class LevelSelectedEvent : UnityEvent<LevelData>{}
}

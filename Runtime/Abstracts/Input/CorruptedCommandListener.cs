using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrupted
{
    public abstract class CorruptedCommandListener<T> : MonoBehaviour
    {
        public CommandListener[] buttons;
        public CommandAxisListener[] axes;

        protected T receiver;

        public abstract T GetReceiver();

        private void Start()
        {
            receiver = GetReceiver();
        }

        private void Update()
        {
            foreach(CommandListener l in buttons)
            {
                if (Input.GetKeyDown(l.input))
                    l.command.StartExecute(receiver);
                if (Input.GetKey(l.input))
                    l.command.WhileExecute(receiver);
                if (Input.GetKeyUp(l.input))
                    l.command.EndExecute(receiver);
            }
            foreach(CommandAxisListener l in axes)
            {
                float axis = Input.GetAxis(l.axis);
                if (axis != 0 && l.lastFrameValue == 0)
                    l.command.StartExecute(receiver);
                else if (axis == 0 && l.lastFrameValue != 0)
                    l.command.EndExecute(receiver);

                l.command.WhileExecute(receiver, axis);
            }
        }

        [System.Serializable]
        public struct CommandListener
        {
            public string name;
            public KeyCode input;
            public CorruptedCommand<T> command;
        }

        [System.Serializable]
        public struct CommandAxisListener
        {
            public string name;
            public string axis;
            public CorruptedAxisCommand<T> command;
            [HideInInspector] public float lastFrameValue;
        }
    }

    
}

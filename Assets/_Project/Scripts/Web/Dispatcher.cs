using System.Collections.Generic;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    private readonly Queue<System.Action> _executionQueue = new Queue<System.Action>();

    public void InvokeAsync(System.Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    protected void Update()
    {
        while (true)
        {
            System.Action action;

            lock (_executionQueue)
            {
                if (_executionQueue.Count == 0)
                {
                    break;
                }

                action = _executionQueue.Dequeue();
            }

            action();
        }
    }

    private static Dispatcher _instance;

    public static Dispatcher CurrentDispatcher
    {
        get
        {
            if (_instance == null)
            {
                GameObject dispatcherObject = new GameObject("Dispatcher");
                _instance = dispatcherObject.AddComponent<Dispatcher>();
                DontDestroyOnLoad(dispatcherObject);
            }

            return _instance;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
  public class CoroutinesQueue : MonoBehaviour
  {
    private Queue<IEnumerator> _coroutines = new Queue<IEnumerator>();
    public bool Running { get; private set; } = false;

    public void add(IEnumerator coroutine)
    {
      _coroutines.Enqueue(coroutine);
    }

    public IEnumerator start()
    {
      if (Running)
        yield return null;

      Running = true;

      while (_coroutines.Count > 0)
        yield return StartCoroutine(_coroutines.Dequeue());

      Running = false;

      yield return null;
    }
  }
}
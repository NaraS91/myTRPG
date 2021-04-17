using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
  public class CoroutinesQueue : MonoBehaviour
  {
    private Queue<IEnumerator> _coroutines = new Queue<IEnumerator>();

    public void add(IEnumerator coroutine)
    {
      _coroutines.Enqueue(coroutine);
    }

    public IEnumerator start()
    {
      while (_coroutines.Count > 0)
        yield return StartCoroutine(_coroutines.Dequeue());

      yield return null;
    }

    public bool isEmpty()
    {
      return _coroutines.Count == 0;
    }
  }
}
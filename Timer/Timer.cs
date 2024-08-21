using Bipolar.Prototyping;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar
{
	public struct Timer : ITimer
	{
		public sealed class TimerManager : SelfCreatingSingleton<TimerManager> 
		{ 
			private readonly Dictionary<Guid, Coroutine> runningTimers = new Dictionary<Guid, Coroutine>();

			public bool IsRunning(Guid timerID) => runningTimers.ContainsKey(timerID);

			public void StartTimer(Guid timerID, IEnumerator timerCoroutine)
			{
				if (IsRunning(timerID) == false) 
				{
					runningTimers.Add(timerID, StartCoroutine(timerCoroutine));
				}
			}
		
			public void StopTimer(Guid timerID)
			{
				if (runningTimers.TryGetValue(timerID, out var coroutine))
				{
					StopCoroutine(coroutine);
					runningTimers.Remove(timerID);
				}
			}
		}

		private Guid _id;
		public Guid ID
		{
			get
			{
				if (_id == Guid.Empty)
					_id = Guid.NewGuid();
				return _id;
			}
		}

		public Action OnElapsed { get; set; }
		public float Speed { get; set; }
		public float Duration { get; set; }
		public float CurrentTime { get; set; }
		public bool AutoReset { get; set; }

		public void Start()
		{
			TimerManager.Instance.StartTimer(ID, TimerLoop());
		}

		public void Stop()
		{
			TimerManager.Instance.StopTimer(ID);
		}

		private IEnumerator TimerLoop()
		{
			while (true)
			{
				yield return null;
				yield return null;
				yield return null;
				yield return null;
				break;
			}
		}
	}
}

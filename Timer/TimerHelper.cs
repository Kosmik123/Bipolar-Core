using Bipolar.Prototyping;
using UnityEngine;

namespace Bipolar
{
    public static class TimerHelper
    {
        public sealed class TimerManager : SelfCreatingSingleton<TimerManager> { }

        public static void UpdateTimer(ref float time, float speed, float duration, System.Action elapsedAction)
        {
            time += speed * Time.deltaTime;
            if (time >= duration)
            {
                time = 0;
                elapsedAction?.Invoke();
            }
        }
    }
}

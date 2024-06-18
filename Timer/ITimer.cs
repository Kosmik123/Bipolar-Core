using System;

namespace Bipolar
{
    public interface ITimer
    {
        Action OnElapsed { get; set; }
        float Speed { get; set; }
        float Duration { get; set; }
        float CurrentTime { get; set; }
        bool AutoReset { get; set; }

        void Start();
        void Stop();
    }
}

using System;

namespace Bipolar
{
    internal interface ITimer
    {
        Action OnElapsed { get; set; }
        float Speed { get; set; }
        float Duration { get; set; }
        float CurrentTime { get; set; }
    }
}

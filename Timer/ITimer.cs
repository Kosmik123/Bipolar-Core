#if NAUGHTY_ATTRIBUTES
#endif

namespace Bipolar
{
    internal interface ITimer
    {
        event System.Action OnElapsed;
        float Speed { get; set; }
        float Duration { get; set; }
        float CurrentTime { get; set; }
    }
}

#if NAUGHTY_ATTRIBUTES
#endif

namespace Bipolar
{
    internal interface ITimer
    {
        System.Action OnElapsed { get; set; }
        float Speed { get; set; }
        float Duration { get; set; }
        float CurrentTime { get; set; }
    }
}

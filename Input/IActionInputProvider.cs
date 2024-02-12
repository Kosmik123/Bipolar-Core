using System;

namespace Bipolar.Input
{
    public interface IActionInputProvider
    {
        event Action OnPerformed;
    }
}

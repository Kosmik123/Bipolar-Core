using System;

namespace Bipolar.Input
{

    public interface IActionInputProvider
    {
        public event Action OnPerformed;
    }
}

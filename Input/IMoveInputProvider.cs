using UnityEngine;

namespace Bipolar.Input
{
    public interface IMoveInputProvider
    {
        float GetHorizontal();
        float GetVertical();
    }
}

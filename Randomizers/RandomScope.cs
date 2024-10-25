using UnityEngine;

namespace Bipolar
{
    public struct RandomScope : System.IDisposable
    {
        private Random.State state;
        
        public RandomScope(int seed) 
        {
            state = Random.state;
            Random.InitState(seed);
        }

        public readonly void Dispose() => 
            Random.state = state;
    }
}

using UnityEngine;

namespace Bipolar
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteColorRandomizer : Randomizer<SpriteRenderer>
    {
        [SerializeField]
        private Color[] colors;

        public override void Randomize()
        {
            RandomizedComponent.color = colors.GetRandom();
        }
    }
}

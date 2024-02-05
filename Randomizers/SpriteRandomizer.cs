using UnityEngine;

namespace Bipolar
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRandomizer : Randomizer<SpriteRenderer>
    {
        [SerializeField]
        private Sprite[] sprites;

        public override void Randomize()
        {
            RandomizedComponent.sprite = sprites.GetRandom();
        }
    }
}

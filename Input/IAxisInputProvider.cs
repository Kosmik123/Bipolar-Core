namespace Bipolar.Input
{
    public interface IAxisInputProvider
    {
        float GetAxis();
    }

	[System.Serializable]
	public class AxisInputProvider : Serialized<IAxisInputProvider>, IAxisInputProvider
	{
		public float GetAxis() => Value.GetAxis();
	}
}

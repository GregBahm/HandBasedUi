public class MainPinchDetector : PinchDetector
{
    public static MainPinchDetector Instance;

    private void Awake()
    {
        Instance = this;
    }
}

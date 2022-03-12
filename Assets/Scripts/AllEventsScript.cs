using UnityEngine;

// Class to contain all event functions
public class AllEventsScript : MonoBehaviour
{
    public delegate void BlankFunc();

    public static BlankFunc OnCarEnterMask;
    public static BlankFunc OnCarExitMask;

    public delegate void Func1(NewPoliceAIScript policeAI);
    public delegate void Func2(GameObject caller);
    public delegate void Func3(uint num);

    public static Func1 OnArrestInitiated;
    public static Func1 OnArrestFailed;
    public static BlankFunc OnArrestComplete;

    public static Func2 OnCarEnterMaskWObj;
    public static Func2 OnCarExitMaskWObj;

    public static Func3 OnCarLifeDecrease;
    public static BlankFunc OnCarDestroyed;

    public static BlankFunc OnCountdownOver;

    // public static Func1 OnArrestInitiated;

}
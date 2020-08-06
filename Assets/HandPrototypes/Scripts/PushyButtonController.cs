using System;
using UnityEngine;

public class PushyButtonController : MonoBehaviour
{
    public bool IsFocused { get { return FocusManager.Instance.FocusedItem == focus; } }

    [SerializeField]
    private ButtonInteractionStyles interactionStyle;
    public ButtonInteractionStyles InteractionStyle => interactionStyle;

    [SerializeField]
    private bool toggled;
    public bool Toggled { get => toggled; set => toggled = value; }

    [SerializeField]
    private FocusableItemBehavior focus;
    public FocusableItemBehavior Focus => focus;

    [SerializeField]
    private AudioSource pressSound;

    private ButtonState oldState;
    public ButtonState State { get; private set; }

    public float Pressedness { get; private set; }

    [SerializeField]
    private float outroDuration = .3f;
    public float OutroDuration => outroDuration;
    public float Outro { get; private set; }

    public event EventHandler Pressed;


#if UNITY_EDITOR
    [SerializeField]
    private bool testClick;
#endif

    private void Start()
    {
        State = ButtonState.Ready;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (testClick)
        {
            testClick = false;
            OnPressed();
        }
#endif
        State = UpdateState();
        if (State == ButtonState.Pressed && oldState != ButtonState.Pressed)
        {
            OnPressed();
        }
        UpdateOutro();
        oldState = State;
    }

    private void UpdateOutro()
    {
        if(State == ButtonState.Pressed)
        {
            Outro += Time.deltaTime;
            if (Outro > outroDuration)
            {
                State = ButtonState.Ready;
            }
        }
        else
        {
            Outro = 0;
        }
    }

    private void OnPressed()
    {
        if (interactionStyle == ButtonInteractionStyles.ToggleButton)
        {
            Toggled = !Toggled;
        }
        pressSound.Play();
        EventHandler handler = Pressed;
        handler?.Invoke(this, EventArgs.Empty);
    }

    private ButtonState UpdateState()
    {
        if (State == ButtonState.Pressed && Outro < outroDuration)
        {
            return ButtonState.Pressed;
        }
        if (IsFocused)
        {
            Vector3 localPos = GetLocalFingerPosition();
            Pressedness = localPos.z;
            if (localPos.z < 0 && (State == ButtonState.Ready || State == ButtonState.Hovered))
            {
                return ButtonState.Hovered;
            }
            if ((State == ButtonState.Hovered || State == ButtonState.Pressed) && localPos.z > 0)
            {
                return ButtonState.Pressed;
            }
        }
        return ButtonState.Ready;
    }

    private Vector3 GetLocalFingerPosition()
    {
        Vector3 fingerPos = Hands.Instance.RightHandProxy.IndexTip.position;
        return transform.InverseTransformPoint(fingerPos);
    }

    public enum ButtonInteractionStyles
    {
        ToggleButton,
        ClickButton
    }
}

public enum ButtonState
{
    Disabled,
    Ready,
    Hovered,
    Pressed
}
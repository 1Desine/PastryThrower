//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""51ba13fd-d555-423c-b067-27df10622bcf"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""7b18cabe-3ce6-42f8-9662-229e1a7655ed"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""InvertVector2(invertX=false)"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ThrowSpeed"",
                    ""type"": ""Value"",
                    ""id"": ""494fc69b-8654-448f-84c7-e6dd9791ce9e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ThrowPastry"",
                    ""type"": ""Button"",
                    ""id"": ""96922d8b-2d49-4689-8edb-a30b5a25f15d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpawnPastry"",
                    ""type"": ""Button"",
                    ""id"": ""cee441bf-2432-452c-9c37-b8f4dc449f71"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""efb8cc06-7ad5-429f-bccd-28e9b665e9c3"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2fde6fb-fe9e-4207-8ce4-33fb50313e83"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowPastry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""507c43b1-eedf-49f6-baa5-c83a9dd1f54b"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnPastry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4b1863f-96e3-4365-b036-a0abc6628796"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowSpeed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_ThrowSpeed = m_Player.FindAction("ThrowSpeed", throwIfNotFound: true);
        m_Player_ThrowPastry = m_Player.FindAction("ThrowPastry", throwIfNotFound: true);
        m_Player_SpawnPastry = m_Player.FindAction("SpawnPastry", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_ThrowSpeed;
    private readonly InputAction m_Player_ThrowPastry;
    private readonly InputAction m_Player_SpawnPastry;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @ThrowSpeed => m_Wrapper.m_Player_ThrowSpeed;
        public InputAction @ThrowPastry => m_Wrapper.m_Player_ThrowPastry;
        public InputAction @SpawnPastry => m_Wrapper.m_Player_SpawnPastry;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @ThrowSpeed.started += instance.OnThrowSpeed;
            @ThrowSpeed.performed += instance.OnThrowSpeed;
            @ThrowSpeed.canceled += instance.OnThrowSpeed;
            @ThrowPastry.started += instance.OnThrowPastry;
            @ThrowPastry.performed += instance.OnThrowPastry;
            @ThrowPastry.canceled += instance.OnThrowPastry;
            @SpawnPastry.started += instance.OnSpawnPastry;
            @SpawnPastry.performed += instance.OnSpawnPastry;
            @SpawnPastry.canceled += instance.OnSpawnPastry;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @ThrowSpeed.started -= instance.OnThrowSpeed;
            @ThrowSpeed.performed -= instance.OnThrowSpeed;
            @ThrowSpeed.canceled -= instance.OnThrowSpeed;
            @ThrowPastry.started -= instance.OnThrowPastry;
            @ThrowPastry.performed -= instance.OnThrowPastry;
            @ThrowPastry.canceled -= instance.OnThrowPastry;
            @SpawnPastry.started -= instance.OnSpawnPastry;
            @SpawnPastry.performed -= instance.OnSpawnPastry;
            @SpawnPastry.canceled -= instance.OnSpawnPastry;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnThrowSpeed(InputAction.CallbackContext context);
        void OnThrowPastry(InputAction.CallbackContext context);
        void OnSpawnPastry(InputAction.CallbackContext context);
    }
}

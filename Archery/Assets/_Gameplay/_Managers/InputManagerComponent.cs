//using UnityEngine;
//using UnityEngine.InputSystem;

//public class InputManagerComponent : MonoBehaviour
//{
//    private PlayerInput playerInput;
//    private InputAction lookAction;
//    private Aim playerAim;

//    void Awake()
//    {
//        playerInput = GetComponent<PlayerInput>();
//        if (playerInput != null)
//        {
//            lookAction = playerInput.actions["Look"];
//        }
//    }

//    void Start()
//    {
//        playerAim = GameObject.FindWithTag("Player").GetComponent<Aim>();
//    }

//    void Update()
//    {
//        if (playerAim != null && lookAction != null)
//        {
//            Vector2 lookInput = lookAction.ReadValue<Vector2>();
//            playerAim.SendMessage("OnLookPerformed", lookInput);
//        }
//    }
//}

using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool walk;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		InputManager inputManager;

		[SerializeField] QuickSlot quickSlot1;
        [SerializeField] QuickSlot quickSlot2;
        [SerializeField] QuickSlot quickSlot3;
        [SerializeField] QuickSlot quickSlot4;
        [SerializeField] QuickSlot quickSlot5;
        [SerializeField] QuickSlot quickSlot6;
        [SerializeField] QuickSlot quickSlot7;

        private void Awake()
        {
			inputManager = GameManager.Instance.inputManager;
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnWalk(InputValue value)
		{
			WalkInput(value.isPressed);
		}

        public void OnToggleInventory(InputValue value)
        {
			GameManager.Instance.inputManager.ToggleInventory();
        }

        public void OnToggleQuestWindow(InputValue value)
        {
            GameManager.Instance.inputManager.ToggleQuestWindow();
        }

        public void OnToggleSkillWindow(InputValue value)
        {
            GameManager.Instance.inputManager.ToggleSkillWindow();
        }

        public void OnToggleEnchantWindow(InputValue value)
        {
            GameManager.Instance.inputManager.ToggleEnchantWindow();
        }

        public void OnESC(InputValue value)
        {
            GameManager.Instance.inputManager.ToggleESC();
        }

        public void OnAlpha1(InputValue value)
        {
			if (quickSlot1.command == null) return;
            quickSlot1.command.Execute();
        }

        public void OnAlpha2(InputValue value)
        {
            if (quickSlot2.command == null) return;
            quickSlot2.command.Execute();
        }

        public void OnAlpha3(InputValue value)
        {
            if (quickSlot3.command == null) return;
            quickSlot3.command.Execute();
        }

        public void OnAlpha4(InputValue value)
        {
            if (quickSlot4.command == null) return;
            quickSlot4.command.Execute();
        }

        public void OnAlpha5(InputValue value)
        {
            if (quickSlot5.command == null) return;
            quickSlot5.command.Execute();
        }

        public void OnAlpha6(InputValue value)
        {
            if (quickSlot6.command == null) return;
            quickSlot6.command.Execute();
        }

        public void OnAlpha7(InputValue value)
        {
            if (quickSlot7.command == null) return;
            quickSlot7.command.Execute();
        }


#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			if (!inputManager.canControl())
			{
				look = Vector2.zero;
				return;
			}
			look = newLookDirection;
        }

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
        }

		public void WalkInput(bool newWalkState)
		{
			walk = newWalkState;
        }

		/*
        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
		*/
	}
	
}
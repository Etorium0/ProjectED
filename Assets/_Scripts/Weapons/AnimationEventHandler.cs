using System;
using UnityEngine;

namespace Etorium.Weapons
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public event Action OnFinish;
        public event Action OnStartMovement;
        public event Action OnStopMovement;
        public event Action OnAttackAction;
        public event Action OnMinHoldPassed;
        
        // Trigger này được sử dụng để chỉ ra trong hoạt ảnh vũ khí khi nào thì đầu vào nên được "sử dụng", nghĩa là người chơi phải nhả phím đầu vào và nhấn lại để kích hoạt đòn tấn công tiếp theo.
        // Nói chung, sự kiện hoạt ảnh này được thêm vào khung "hành động" đầu tiên của hoạt ảnh. Ví dụ: khung chém kiếm đầu tiên hoặc khung bắn cung.
        public event Action OnUseInput;

        public event Action<AttackPhases> OnEnterAttackPhase;
        
        private void AnimationFinishedTrigger() => OnFinish?.Invoke();
        private void StartMovementTrigger() => OnStartMovement?.Invoke();
        private void StopMovementTrigger() => OnStopMovement?.Invoke();
        private void AttackActionTrigger() => OnAttackAction?.Invoke();
        private void MinHoldPassedTrigger() => OnMinHoldPassed?.Invoke();
        private void UseInputTrigger() => OnUseInput?.Invoke();
        
        private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);
    }
}
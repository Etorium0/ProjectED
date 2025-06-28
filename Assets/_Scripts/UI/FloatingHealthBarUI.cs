// using UnityEngine;
// using UnityEngine.UI;

// namespace Etorium.UI
// {
//     public class FloatingHealthBarUI : MonoBehaviour
//     {
//         [Header("UI Components")]
//         [SerializeField] private HealthBarUI healthBarUI;
//         [SerializeField] private Canvas canvas;
//         [SerializeField] private RectTransform rectTransform;
        
//         [Header("Target Settings")]
//         [SerializeField] private Transform target;
//         [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);
//         [SerializeField] private bool followTarget = true;
//         [SerializeField] private bool alwaysFaceCamera = true;
        
//         [Header("Visibility Settings")]
//         [SerializeField] private bool hideWhenFullHealth = false;
//         [SerializeField] private bool hideWhenDead = true;
//         [SerializeField] private float fadeInDuration = 0.2f;
//         [SerializeField] private float fadeOutDuration = 0.5f;
        
//         private Camera mainCamera;
//         private CanvasGroup canvasGroup;
//         private bool isVisible = true;
        
//         private void Awake()
//         {
//             if (canvas == null)
//                 canvas = GetComponent<Canvas>();
                
//             if (rectTransform == null)
//                 rectTransform = GetComponent<RectTransform>();
                
//             canvasGroup = GetComponent<CanvasGroup>();
//             if (canvasGroup == null)
//                 canvasGroup = gameObject.AddComponent<CanvasGroup>();
                
//             mainCamera = Camera.main;
//         }
        
//         private void Start()
//         {
//             if (healthBarUI != null)
//             {
//                 healthBarUI.OnHealthChanged += HandleHealthChanged;
//             }
//         }
        
//         private void OnDestroy()
//         {
//             if (healthBarUI != null)
//             {
//                 healthBarUI.OnHealthChanged -= HandleHealthChanged;
//             }
//         }
        
//         private void LateUpdate()
//         {
//             if (target == null || !followTarget) return;
            
//             UpdatePosition();
            
//             if (alwaysFaceCamera && mainCamera != null)
//             {
//                 UpdateRotation();
//             }
//         }
        
//         private void UpdatePosition()
//         {
//             if (target == null || mainCamera == null) return;
            
//             Vector3 targetPosition = target.position + offset;
//             Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            
//             if (screenPosition.z < 0)
//             {
//                 // Object is behind camera, hide health bar
//                 SetVisibility(false);
//                 return;
//             }
            
//             rectTransform.position = screenPosition;
//             SetVisibility(true);
//         }
        
//         private void UpdateRotation()
//         {
//             if (mainCamera == null) return;
            
//             transform.rotation = mainCamera.transform.rotation;
//         }
        
//         private void HandleHealthChanged(float currentHealth, float maxHealth)
//         {
//             if (healthBarUI == null) return;
            
//             float healthPercentage = healthBarUI.GetHealthPercentage();
            
//             // Handle visibility based on health
//             if (hideWhenFullHealth && healthPercentage >= 1f)
//             {
//                 SetVisibility(false);
//             }
//             else if (hideWhenDead && healthBarUI.IsDead())
//             {
//                 SetVisibility(false);
//             }
//             else
//             {
//                 SetVisibility(true);
//             }
//         }
        
//         private void SetVisibility(bool visible)
//         {
//             if (isVisible == visible) return;
            
//             isVisible = visible;
            
//             if (visible)
//             {
//                 ShowHealthBar();
//             }
//             else
//             {
//                 HideHealthBar();
//             }
//         }
        
//         private void ShowHealthBar()
//         {
//             if (canvasGroup != null)
//             {
//                 LeanTween.alphaCanvas(canvasGroup, 1f, fadeInDuration);
//             }
//         }
        
//         private void HideHealthBar()
//         {
//             if (canvasGroup != null)
//             {
//                 LeanTween.alphaCanvas(canvasGroup, 0f, fadeOutDuration);
//             }
//         }
        
//         public void SetTarget(Transform newTarget)
//         {
//             target = newTarget;
//         }
        
//         public void SetOffset(Vector3 newOffset)
//         {
//             offset = newOffset;
//         }
        
//         public void SetFollowTarget(bool follow)
//         {
//             followTarget = follow;
//         }
        
//         public void SetAlwaysFaceCamera(bool faceCamera)
//         {
//             alwaysFaceCamera = faceCamera;
//         }
        
//         public HealthBarUI GetHealthBarUI() => healthBarUI;
//         public Transform GetTarget() => target;
//     }
// } 
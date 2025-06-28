# ProjectED - Unity Game Architecture Analysis

## Tổng quan dự án

**ProjectED** là một dự án Unity game 2D platformer với kiến trúc được thiết kế theo các design pattern hiện đại, tập trung vào modularity, extensibility và maintainability. Dự án sử dụng hệ thống component-based architecture kết hợp với finite state machine để quản lý behavior của player và weapon system.

## Kiến trúc tổng thể

### Design Patterns chính được sử dụng:

1. **Component Pattern** - Hệ thống weapon và projectile được chia thành các component độc lập
2. **Strategy Pattern** - Các weapon component có thể hoán đổi behavior 
3. **Observer Pattern** - Event system và callback mechanism
4. **State Machine Pattern** - Player state management
5. **Object Pooling** - Tối ưu performance cho projectile
6. **Interface Segregation** - IDataPersistence interface
7. **Lazy Loading** - Core components chỉ load khi cần
8. **Data-Driven Design** - ScriptableObject cho configuration
9. **Hierarchical FSM** - SuperStates và SubStates cho player
10. **Event-Driven UI** - UI phản ứng với game events

## Hệ thống Weapon System

### Kiến trúc Component-Based

Weapon System được thiết kế theo pattern component-based với cấu trúc:

```
Weapon (MonoBehaviour)
├── WeaponComponent<TData, TAttackData> (Abstract Base)
│   ├── WeaponSprite
│   ├── ProjectileSpawner 
│   ├── ChargeToProjectileSpawner
│   ├── ActionHitBox
│   ├── Damage
│   ├── PoiseDamage
│   ├── KnockBack
│   └── Movement
├── WeaponGenerator (Factory)
├── AnimationEventHandler (Event System)
└── Data (ScriptableObject)
```

### WeaponComponent Base Class

```csharp
public abstract class WeaponComponent<TData, TAttackData> : MonoBehaviour
    where TData : ComponentData<TAttackData>
    where TAttackData : AttackData
```

**Lợi ích của Generic Design:**
- **Type Safety**: Compile-time checking cho data types
- **Code Reuse**: Base functionality cho tất cả components  
- **Consistent Interface**: Unified API cho component lifecycle
- **Data Binding**: Automatic data association với attack phases

### WeaponSprite Component và RegisterSpriteChangeCallback

**WeaponSprite.cs** là component chịu trách nhiệm đồng bộ sprite của weapon với animation của nhân vật.

#### Cơ chế RegisterSpriteChangeCallback:

```csharp
// Trong Start()
baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);

// Callback handler
private void HandleBaseSpriteChange(SpriteRenderer sr)
{
    if (!isAttackActive)
    {
        weaponSpriteRenderer.sprite = null;
        return;
    }

    if (currentWeaponSpriteIndex >= currentPhaseSprites.Length)
    {
        Debug.LogWarning($"{weapon.name} weapon sprites length mismatch");
        return;
    }
    
    weaponSpriteRenderer.sprite = currentPhaseSprites[currentWeaponSpriteIndex];
    currentWeaponSpriteIndex++;
}
```

#### Workflow của Sprite Synchronization:

1. **Animation Frame Change**: Player animation controller thay đổi sprite của character
2. **Callback Trigger**: SpriteRenderer trigger RegisterSpriteChangeCallback 
3. **Weapon Sprite Update**: HandleBaseSpriteChange được gọi để update weapon sprite
4. **Index Increment**: currentWeaponSpriteIndex tăng lên để match với frame tiếp theo
5. **Phase Management**: AttackPhases control việc chọn sprite array phù hợp

#### Lý do thiết kế này:

- **Perfect Synchronization**: Weapon sprite luôn khớp với character animation frame
- **Automatic Management**: Không cần manual timing code
- **Performance**: Chỉ update khi animation thực sự thay đổi
- **Flexibility**: Support multiple attack phases với sprite sets khác nhau
- **Error Handling**: Graceful handling khi sprite count mismatch

### WeaponGenerator (Factory Pattern)

```csharp
public class WeaponGenerator : MonoBehaviour
{
    public Weapon GenerateWeapon(WeaponDataSO data);
}
```

**Chức năng:**
- **Dynamic Weapon Creation**: Tạo weapon từ ScriptableObject data
- **Component Assembly**: Tự động attach các component cần thiết
- **Data Injection**: Inject appropriate data vào các component
- **Validation**: Ensure weapon integrity trước khi return

### AnimationEventHandler

```csharp
public class AnimationEventHandler : MonoBehaviour
{
    public UnityEvent<AttackPhases> OnEnterAttackPhase;
    public UnityEvent OnExit;
}
```

**Event-Driven Architecture:**
- **Animation Integration**: Connect Unity Animation Events với weapon logic
- **Phase Transitions**: Trigger weapon behavior changes based on animation
- **Decoupled Communication**: Weapons react to animation events without tight coupling

## Projectile System

### Component Architecture

Projectile System mirror weapon system architecture:

```
ProjectileComponent<TData> (Abstract Base)
├── Damage (IDamageService)
├── PoiseDamage (IPoiseDamageService) 
├── KnockBack (IKnockBackService)
└── Graphics (Sprite Management)
```

### Data Package System

```csharp
public abstract class ProjectileDataPackage : ScriptableObject
{
    // Base data for projectile configuration
}

// Specific implementations
- DamageDataPackage
- PoiseDamageDataPackage  
- SpriteDataPackage
```

**Lợi ích:**
- **Modular Data**: Mỗi component có data package riêng
- **Designer Friendly**: ScriptableObject có thể edit trong Inspector
- **Runtime Flexibility**: Data có thể swap runtime
- **Version Control**: Easy to track changes in data files

### Service Interfaces

```csharp
public interface IDamageService
{
    void SetDamage(float damage);
}

public interface IPoiseDamageService  
{
    void SetPoiseDamage(float poiseDamage);
}

public interface IKnockBackService
{
    void SetKnockBackStrength(Vector2 strength);
}
```

**Interface Segregation Benefits:**
- **Single Responsibility**: Mỗi interface có một mục đích cụ thể
- **Flexible Implementation**: Components implement chỉ interfaces cần thiết
- **Easy Testing**: Mock interfaces cho unit testing
- **Future Extension**: Thêm services mới không ảnh hưởng existing code

## Core System

### Core Architecture

```csharp
public class Core : MonoBehaviour
{
    private Dictionary<Type, CoreComponent> coreComponentsMap;
    
    public T GetCoreComponent<T>() where T : CoreComponent
    {
        // Lazy loading with caching
    }
}
```

### Core Components

1. **Movement** - Physics-based movement với collision detection
2. **CollisionSenses** - Environment detection (ground, wall, ledge)
3. **Stats** - Health, damage, speed management
4. **DamageReceiver** - Damage processing và health management  
5. **PoiseDamageReceiver** - Stagger/stun system
6. **KnockBackReceiver** - Physics-based knockback
7. **Death** - Death state management và respawn
8. **ParticleManager** - Visual effects management
9. **InteractableDetector** - Object interaction system
10. **WeaponInventory** - Weapon storage và management
11. **WeaponSwap** - Weapon switching logic

### Lazy Loading Pattern

```csharp
public T GetCoreComponent<T>() where T : CoreComponent
{
    var type = typeof(T);
    if (coreComponentsMap.TryGetValue(type, out var component))
    {
        return component as T;
    }
    
    component = GetComponentInChildren<T>();
    coreComponentsMap[type] = component;
    return component as T;
}
```

**Performance Benefits:**
- **Memory Efficiency**: Components chỉ load khi được request
- **Startup Speed**: Faster initialization time
- **Cache Optimization**: Subsequent calls use cached reference
- **Modular Loading**: Unused components không consume memory

## Player State Machine

### Hierarchical FSM Architecture

```
PlayerStateMachine
├── SuperStates (Abstract Base States)
│   ├── PlayerGroundedState
│   ├── PlayerAbilityState  
│   └── PlayerTouchingWallState
└── SubStates (Concrete Implementation)
    ├── PlayerIdleState
    ├── PlayerMoveState
    ├── PlayerJumpState
    ├── PlayerAttackState
    ├── PlayerCrouchIdleState
    ├── PlayerCrouchMoveState
    ├── PlayerWallSlideState
    ├── PlayerWallGrabState
    ├── PlayerWallClimbState
    ├── PlayerWallJumpState
    ├── PlayerLedgeClimbState
    ├── PlayerInAirState
    ├── PlayerLandState
    └── PlayerRestState
```

### State Lifecycle

```csharp
public abstract class PlayerState
{
    public virtual void Enter() { }
    public virtual void Exit() { }  
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void DoChecks() { }
    public virtual void AnimationTrigger() { }
    public virtual void AnimationFinishTrigger() { }
}
```

### SuperState Pattern

**PlayerGroundedState Example:**
```csharp
public class PlayerGroundedState : PlayerState
{
    // Common grounded behavior
    // Shared by IdleState, MoveState, CrouchStates
}
```

**Benefits:**
- **Code Reuse**: Common behavior trong SuperStates
- **State Inheritance**: SubStates inherit và extend SuperState logic
- **Easier Maintenance**: Change shared behavior trong một place
- **Clear Hierarchy**: Logical organization của related states

### State Transitions

```csharp
// Example transition logic
public override void LogicUpdate()
{
    base.LogicUpdate();
    
    if (inputHandler.JumpInput && Movement.CanJump)
    {
        stateMachine.ChangeState(player.JumpState);
    }
    else if (!CollisionSenses.Ground)
    {
        stateMachine.ChangeState(player.InAirState);
    }
    // ... other transition conditions
}
```

## Data Persistence System

### Architecture Overview

```csharp
public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}

public class DataPersistenceManager : MonoBehaviour
{
    private FileDataHandler dataHandler;
    private List<IDataPersistence> dataPersistenceObjects;
    private GameData gameData;
}
```

### Save System Workflow

1. **Data Collection**: Gather data từ tất cả IDataPersistence objects
2. **Serialization**: Convert GameData object thành JSON
3. **File Writing**: Write encrypted data to file
4. **Verification**: Validate write operation success

### GameData Structure

```csharp
[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> collectedItems;
    public SerializableDictionary<string, int> weaponInventory;
    // ... other persistent data
}
```

### SerializableDictionary

```csharp
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // Custom serialization for Unity compatibility
}
```

**Unity Compatibility:**
- **Inspector Support**: Dictionary data visible trong Inspector
- **JSON Serialization**: Compatible với Unity's JsonUtility
- **Version Control**: Readable diffs trong version control
- **Debug Friendly**: Easy to inspect saved data

## UI System

### Event-Driven UI Architecture

```csharp
public class WeaponSwapUI : MonoBehaviour
{
    private void OnEnable()
    {
        WeaponSwap.OnWeaponChanged += UpdateWeaponDisplay;
    }
    
    private void OnDisable()  
    {
        WeaponSwap.OnWeaponChanged -= UpdateWeaponDisplay;
    }
}
```

### Debug UI Systems

1. **DebugSaveUI** - Runtime save/load testing
2. **DebugUI** - Development debugging tools
3. **WeaponSwapUI** - Weapon inventory display

**Benefits:**
- **Reactive Updates**: UI tự động update khi data changes
- **Decoupled Design**: UI không depend directly on game logic
- **Easy Debugging**: Developer tools cho testing
- **Runtime Flexibility**: UI có thể enable/disable dynamically

## Performance Optimizations

### Object Pooling

Projectile system sử dụng object pooling pattern:

```csharp
// Conceptual pooling implementation
public class ProjectilePool
{
    private Queue<Projectile> availableProjectiles;
    private List<Projectile> activeProjectiles;
    
    public Projectile GetProjectile()
    {
        // Return from pool or create new
    }
    
    public void ReturnProjectile(Projectile projectile)
    {
        // Reset và return to pool
    }
}
```

### Component Caching

Core system cache components để avoid repeated GetComponent calls:

```csharp
private Dictionary<Type, CoreComponent> coreComponentsMap;
```

### Event System Optimization

- **Unsubscribe Pattern**: Proper cleanup trong OnDestroy
- **Null Checks**: Prevent null reference exceptions
- **Event Aggregation**: Batch multiple events when possible

## Extensibility và Modularity

### Adding New Weapon Components

1. **Inherit từ WeaponComponent<TData, TAttackData>**
2. **Create corresponding ComponentData class**
3. **Implement required lifecycle methods**
4. **Add to WeaponGenerator factory logic**

### Adding New Projectile Components

1. **Inherit từ ProjectileComponent<TData>**
2. **Create corresponding DataPackage**
3. **Implement service interfaces nếu cần**
4. **Configure trong ProjectileDataPackage**

### Adding New Player States

1. **Inherit từ appropriate SuperState hoặc PlayerState**
2. **Implement state logic trong required methods**
3. **Add transition logic trong related states**
4. **Configure trong PlayerStateMachine**

## Error Handling và Debugging

### Defensive Programming

```csharp
// Example từ WeaponSprite
if (currentWeaponSpriteIndex >= currentPhaseSprites.Length)
{
    Debug.LogWarning($"{weapon.name} weapon sprites length mismatch");
    return;
}
```

### Debug Systems

- **Debug UI**: Runtime debugging tools
- **Logging**: Comprehensive error reporting
- **Validation**: Data integrity checks
- **Graceful Degradation**: Fallback behavior cho edge cases

## Best Practices được áp dụng

### SOLID Principles

1. **Single Responsibility**: Mỗi component có một nhiệm vụ rõ ràng
2. **Open/Closed**: Extensible without modifying existing code
3. **Liskov Substitution**: Components có thể substitute cho base class
4. **Interface Segregation**: Focused interfaces cho specific functionality
5. **Dependency Inversion**: Depend on abstractions, not concretions

### Unity-Specific Best Practices

1. **MonoBehaviour Lifecycle**: Proper use của Start, Update, OnDestroy
2. **ScriptableObject Data**: Configuration data outside code
3. **Event System**: UnityEvent và C# events cho decoupling
4. **Component Architecture**: Composition over inheritance
5. **Performance Considerations**: Object pooling, caching, lazy loading

## Kết luận

ProjectED thể hiện một kiến trúc Unity game design chất lượng cao với:

- **Modular Architecture**: Dễ maintain và extend
- **Performance Optimized**: Object pooling, caching, lazy loading
- **Designer Friendly**: ScriptableObject configuration
- **Robust Error Handling**: Defensive programming practices
- **Clean Code**: SOLID principles và design patterns
- **Scalable Systems**: Easy to add new features
- **Debug Support**: Comprehensive debugging tools

Hệ thống **RegisterSpriteChangeCallback** trong WeaponSprite là một example tuyệt vời của event-driven programming, đảm bảo perfect synchronization giữa character animation và weapon sprites mà không cần complex timing logic.

Kiến trúc này tạo foundation mạnh mẽ cho việc phát triển một 2D platformer game với khả năng mở rộng cao và maintainability tốt.

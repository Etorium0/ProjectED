# Save System - Final Guide

## 🎯 **Overview**
Game sử dụng hệ thống `DataPersistenceManager` ban đầu với JSON save files. Player chỉ có thể save tại Rest Points/Bonfires (như Dark Souls).

## 📁 **Current Architecture**

### Core System Files:
- `DataPersistenceManager.cs` - Quản lý save/load tự động
- `FileDataHandler.cs` - Xử lý JSON file I/O  
- `GameData.cs` - Chứa dữ liệu save (đã thêm `lastRestPosition`, `lastRestId`)
- `IDataPersistence.cs` - Interface cho objects implement save/load

### Game-Specific Files:
- `GameManager.cs` - Implement `IDataPersistence`, quản lý respawn/rest points
- `RestPoint.cs` - Manual rest points (nhấn E)
- `Bonfire.cs` - Auto rest points (trigger enter)
- `Death.cs` - Handle player death, không save health
- `DebugUI.cs` - Debug controls với F-keys

## 🎮 **How It Works**

### **Save/Load Logic (TRUE DARK SOULS STYLE):**
1. **MANUAL SAVE ONLY**: Chỉ save khi player rest tại Bonfire/RestPoint
2. **Auto-load**: Khi vào scene (OnSceneLoaded)  
3. **NO AUTO-SAVE**: Không save khi chuyển scene hoặc quit game
4. **NEW GAME**: Tạo GameData mới, spawn tại initial position
5. **CONTINUE**: Load GameData, spawn tại lastRestPosition
6. **RISK/REWARD**: Chưa rest = mất progress nếu quit game

### **Rest System:**
1. **RestPoint**: Player đến gần → nhấn `E` → heal + **SAVE GAME** + update rest point
2. **Bonfire**: Player trigger enter → auto heal + **SAVE GAME** + update rest point
3. **Both**: Heal to full + manual save + update respawn point

### **Death System:**
1. Player chết → Death count tăng trong GameManager
2. Respawn tại last rest position với full health
3. **Không save khi chết** - chỉ save tại rest points

### **Key Difference - TRUE DARK SOULS:**
- ❌ **NO auto-save** khi chuyển scene
- ❌ **NO auto-save** khi quit game  
- ✅ **ONLY save** khi rest tại Bonfire/RestPoint
- ⚠️ **Risk**: Chưa rest = mất progress nếu quit

## 🔧 **Unity Setup**

### 1. DataPersistenceManager Setup:
```
1. Tạo Empty GameObject: "DataPersistenceManager"
2. Add DataPersistenceManager component
3. Settings:
   - Initialize Data If Null: ✓ (cho debug)
   - File Name: "data" 
   - Use Encryption: ✓ (optional)
```

### 2. GameManager Setup:  
```
- GameManager đã implement IDataPersistence
- Assign Initial Spawn Point transform
- Assign Player GameObject reference
- Set Respawn Time
```

### 3. Rest Points Setup:
```
RestPoint: GameObject + RestPoint component + Collider2D (trigger)
Bonfire: GameObject + Bonfire component + Collider2D (trigger)
```

### 4. DebugUI Setup (Optional):
```
Canvas → Panel → DebugUI component
Add Text components cho: death count, rest position, player position, instructions
```

## ⌨️ **Controls**

### In-Game:
- **E** - Rest tại RestPoint (manual)
- **Walk into Bonfire** - Auto rest

### Debug (F-Keys):
- **F1** - Toggle Debug UI
- **F5** - Manual save (force)
- **F9** - Reload game data
- **F10** - Start new game

## 🎯 **Key Features**

### ✅ **What Works:**
- JSON save/load tự động
- CONTINUE button chỉ enable khi có save file
- Player spawn đúng vị trí (initial vs last rest)
- Death count tracking
- Rest points heal to full + update spawn
- Full health khi respawn/continue

### ✅ **Game Flow (TRUE DARK SOULS STYLE):**
1. Start game → Load last rest position (or initial if new)
2. Explore → **NO auto-save**, **HIGH RISK** of losing progress
3. Find Rest/Bonfire → **DECISION**: Rest now (save) or continue exploring?
4. Rest → Heal + **MANUAL SAVE** + update respawn point
5. Die → Respawn at last rest with full health
6. Quit without resting → **LOSE ALL PROGRESS** since last rest!

**This creates intense risk/reward gameplay where every rest decision matters!**

## 📂 **Save File Location**
```
Windows: %userprofile%/AppData/LocalLow/[CompanyName]/[ProductName]/data
```

## 🐛 **Troubleshooting**

### CONTINUE button disabled:
- Check DataPersistenceManager exists in scene
- Check save file exists in AppData folder
- Try F10 (new game) → find rest point → return to menu

### Player spawns wrong position:
- Check GameManager has Initial Spawn Point assigned
- Check Debug UI (F1) for rest position values
- Check GameData.lastRestPosition in JSON file

### F-keys not working:
- Check DebugUI component attached to active GameObject
- Check Console for errors
- Try manual: DataPersistenceManager.instance.SaveGame()

### Save not working:
- Check DataPersistenceManager Initialize Data If Null = true
- Check file permissions in AppData folder
- Check Console for save/load errors

## 🔮 **Future Extensions**

### Boss/Progress Tracking:
```csharp
// Add to GameData.cs
public bool[] bossesDefeated = new bool[10];
public string[] levelsUnlocked = new string[0];
```

### Inventory System:
```csharp
// Add to GameData.cs  
public int[] inventoryItems = new int[20];
public int[] inventoryQuantities = new int[20];
```

### Multiple Save Slots:
```csharp
// Change filename in DataPersistenceManager
fileName = "save_slot_" + slotNumber + ".json";
```

---

## 📝 **Summary**

Hệ thống hiện tại đơn giản và hoạt động tốt:
- ✅ Sử dụng architecture ban đầu của bạn
- ✅ Chỉ thêm rest position vào GameData
- ✅ GameManager implement IDataPersistence
- ✅ Rest-based saving như Dark Souls
- ✅ Full health on respawn/continue
- ✅ CONTINUE button hoạt động đúng

**No more custom SaveManager complexity - just your original system with rest point additions!**

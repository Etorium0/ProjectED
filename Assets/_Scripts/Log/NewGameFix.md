# New Game Fix & DataPersistenceManager Duplicate Issue

## 🔧 **Problem Fixed:**

### Issue 1: New Game Loading Save Data
**Problem**: New Game vẫn load save data thay vì tạo fresh data  
**Cause**: `OnSceneLoaded` luôn gọi `LoadGame()` từ file  
**Solution**: Thêm `isNewGame` flag để phân biệt New Game vs Continue

### Issue 2: Multiple DataPersistenceManager 
**Problem**: "Found more than one Data Persistence Manager in the scene"  
**Cause**: Có nhiều GameObject với DataPersistenceManager component  
**Solution**: Cần xóa duplicates trong Unity Editor

## ✅ **Code Changes:**

### DataPersistenceManager.cs:
```csharp
// Added flag to track new game
private bool isNewGame = false;

public void NewGame() 
{
    this.gameData = new GameData();
    this.isNewGame = true; // Set flag
    Debug.Log("New game created - fresh data will be used");
}

public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
{
    this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    
    if (isNewGame)
    {
        // New game - use fresh data, don't load from file
        Debug.Log("New game - using fresh data, not loading from file");
        isNewGame = false; // Reset flag
        
        // Push fresh data to all objects
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }
    else
    {
        // Continue game - load from file
        LoadGame();
    }
}
```

## 🔧 **Unity Editor Fix Required:**

### To Fix Multiple DataPersistenceManager:
1. **Check MainMenu scene**: Tìm tất cả GameObject có DataPersistenceManager component
2. **Check Main scene**: Tìm tất cả GameObject có DataPersistenceManager component  
3. **Keep only ONE**: Xóa tất cả duplicate, chỉ giữ lại 1 cái
4. **Recommended**: Đặt DataPersistenceManager trong MainMenu scene (sẽ DontDestroyOnLoad)

### Steps:
```
1. Open MainMenu scene
2. Hierarchy → Search "DataPersistenceManager"  
3. Should only have 1 result
4. Open Main scene
5. Hierarchy → Search "DataPersistenceManager"
6. Should have 0 results (vì đã DontDestroyOnLoad từ MainMenu)
7. If found duplicates → Delete them
```

## 🎮 **Expected Behavior After Fix:**

### New Game:
```
1. Click "NEW GAME"
2. Log: "New game created - fresh data will be used"
3. Scene loads
4. Log: "New game - using fresh data, not loading from file"  
5. Player spawns at initial position
6. Death count = 0, Rest position = Vector3.zero
```

### Continue Game:
```
1. Click "CONTINUE"  
2. Scene loads
3. Log: "Game loaded successfully from: [path]"
4. Log: "Player loaded at rest position: [position]"
5. Player spawns at last rest position
6. Death count và rest position từ save file
```

## 🚨 **Important:**
- Only ONE DataPersistenceManager should exist across all scenes
- It should be created in MainMenu and persist via DontDestroyOnLoad
- Check both MainMenu and Main scenes for duplicates

**After fixing duplicates in Unity Editor, New Game should work correctly!** ✅

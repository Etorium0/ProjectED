# Giải thích chi tiết logic hệ thống ProjectED

## 1. Core System
- **Core** là thành phần trung tâm, quản lý các CoreComponent như Movement, Stats, WeaponInventory, v.v. Mỗi entity (player, enemy) đều có một Core riêng, giúp truy xuất và quản lý các chức năng một cách module hóa.
- **CoreComponent**: Mỗi chức năng (di chuyển, máu, inventory, vũ khí,...) là một component riêng, dễ mở rộng và bảo trì.

## 2. Player State Machine
- **PlayerStateMachine** quản lý trạng thái của player (Idle, Move, Jump, Attack, Dash, v.v).
- Mỗi trạng thái là một class kế thừa từ base state, xử lý input, logic vật lý, trigger animation riêng biệt.
- Chuyển trạng thái dựa trên input, điều kiện vật lý (chạm đất, va tường, hết stamina, v.v).
- **Ưu điểm:** Dễ thêm trạng thái mới, dễ debug, code rõ ràng, tách biệt logic.

## 3. Enemy State Machine & Enemy Specify
- **EnemyStateMachine** quản lý trạng thái của enemy (Patrol, Chase, Attack, Die, v.v).
- Mỗi enemy có thể có các state riêng hoặc dùng chung base state.
- **EnemySpecify**: Cho phép định nghĩa hành vi đặc biệt cho từng loại enemy (boss nhiều phase, quái thường chỉ patrol/attack, v.v).
- Có thể mở rộng bằng cách kế thừa và override các state đặc biệt.

## 4. Weapon System
- **WeaponInventory**: Quản lý các vũ khí player sở hữu (dưới dạng mảng, số slot cấu hình được trong Inspector).
- **WeaponSwap**: Xử lý logic nhặt, đổi, vứt vũ khí. Khi nhặt vũ khí mới, nếu còn slot trống sẽ thêm vào, nếu đầy sẽ hiện UI cho phép chọn vũ khí để đổi.
- **WeaponPickup**: Object vũ khí trên map, khi player lại gần và ấn E sẽ trigger logic nhặt vũ khí.
- **WeaponDataSO**: ScriptableObject lưu trữ dữ liệu từng loại vũ khí (icon, damage, mô tả, v.v).
- **Charge/Projectile/Attack**: Mỗi loại vũ khí có thể có logic riêng (bắn, chém, charge, v.v) thông qua các component chuyên biệt.

## 5. Interaction System
- **InteractableDetector**: Gắn cho player, tự động phát hiện object có thể tương tác gần nhất (vũ khí, bonfire, checkpoint, v.v).
- Khi ấn E, sẽ gọi hàm Interact() của object gần nhất, thực hiện logic tương ứng (nhặt vũ khí, rest, v.v).
- **IInteractable**: Interface chuẩn hóa các object có thể tương tác.

## 6. Data Persistence (Save/Load)
- **DataPersistenceManager**: Quản lý lưu/đọc dữ liệu game (JSON), chỉ lưu khi player rest tại bonfire/checkpoint.
- **GameData**: Lưu vị trí, máu, inventory, trạng thái boss, v.v.
- **RestPoint/Bonfire**: Khi player rest sẽ lưu game, hồi máu, cập nhật vị trí respawn.
- **Không auto-save khi chuyển scene hoặc quit game** (giống Dark Souls).

## 7. UI System
- **EquippedWeaponUI, WeaponInfoUI, WeaponSwapUI**: Hiển thị vũ khí đang cầm, thông tin vũ khí, UI chọn đổi vũ khí khi inventory đầy.
- **DebugUI**: Hiển thị thông tin debug (vị trí, số lần chết, trạng thái save, v.v).
- **UI cập nhật realtime** khi inventory hoặc trạng thái player thay đổi.

## 8. Object Pool System
- Tối ưu hiệu năng khi spawn/destroy nhiều object (projectile, hiệu ứng, v.v), tránh lag spike.
- Các object như đạn, hiệu ứng được lấy ra từ pool thay vì tạo mới liên tục.

## 9. Particles/Effects
- Quản lý hiệu ứng khi tấn công, chết, nhặt đồ, v.v.
- Dùng chung hệ thống pool để tối ưu hiệu năng.

## 10. Luồng hoạt động tổng thể
1. **Player di chuyển, thực hiện hành động qua State Machine.**
2. **Khi lại gần object có thể tương tác (vũ khí, bonfire...), InteractableDetector sẽ highlight object đó.**
3. **Ấn E:**  
   - Nếu là vũ khí: Thêm vào inventory hoặc hiện UI đổi vũ khí nếu đầy.
   - Nếu là bonfire/checkpoint: Hồi máu, lưu game, cập nhật vị trí respawn.
4. **Enemy hoạt động theo EnemyStateMachine, có thể mở rộng hành vi riêng.**
5. **Khi player chết:** Respawn tại vị trí rest gần nhất, inventory giữ nguyên.
6. **Toàn bộ dữ liệu quan trọng được lưu vào GameData khi rest, không auto-save khi chuyển scene hoặc quit game.**

---

## Ưu điểm kiến trúc
- **Tách biệt rõ ràng từng hệ thống (core, state machine, inventory, interaction, save/load, UI...)**
- **Dễ mở rộng, bảo trì, thêm tính năng mới.**
- **Tối ưu hiệu năng với object pool.**
- **Dễ dàng debug, kiểm soát luồng dữ liệu.**

---

## Hướng dẫn mở rộng
- Thêm enemy mới: Tạo state machine riêng hoặc kế thừa từ base, cấu hình EnemySpecify.
- Thêm vũ khí mới: Tạo WeaponDataSO mới, kéo vào prefab WeaponPickup.
- Thêm tính năng save mới: Mở rộng GameData, implement IDataPersistence cho object cần lưu.

---

**File này giúp trình bày rõ ràng kiến trúc, luồng hoạt động, và cách mở rộng dự án khi báo cáo!** 
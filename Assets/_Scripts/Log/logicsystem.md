# Giải thích chi tiết logic hệ thống ProjectED (có lý do thiết kế)

## 1. Core System
- **Core** là thành phần trung tâm, quản lý các CoreComponent như Movement, Stats, WeaponInventory, v.v. Mỗi entity (player, enemy) đều có một Core riêng, giúp truy xuất và quản lý các chức năng một cách module hóa.
- **CoreComponent**: Mỗi chức năng (di chuyển, máu, inventory, vũ khí,...) là một component riêng, dễ mở rộng và bảo trì.
- **Vì sao dùng Core & CoreComponent?**
  - **Tách biệt chức năng:** Mỗi chức năng (di chuyển, máu, inventory, vũ khí,...) là một component riêng, giúp code module hóa, dễ bảo trì, dễ mở rộng.
  - **Tái sử dụng:** Các entity (player, enemy, NPC) đều dùng chung Core, chỉ cần gắn các component phù hợp.
  - **Dễ debug:** Khi lỗi ở đâu chỉ cần kiểm tra component đó, không ảnh hưởng toàn hệ thống.
  - **Thực tiễn:** Nếu muốn thêm tính năng mới (ví dụ: hệ thống buff/debuff), chỉ cần tạo CoreComponent mới và gắn vào Core.

## 2. Player State Machine
- **PlayerStateMachine** quản lý trạng thái của player (Idle, Move, Jump, Attack, Dash, v.v).
- Mỗi trạng thái là một class kế thừa từ base state, xử lý input, logic vật lý, trigger animation riêng biệt.
- Chuyển trạng thái dựa trên input, điều kiện vật lý (chạm đất, va tường, hết stamina, v.v).
- **Ưu điểm:** Dễ thêm trạng thái mới, dễ debug, code rõ ràng, tách biệt logic.
- **Vì sao dùng State Machine cho player?**
  - **Quản lý trạng thái phức tạp:** Player có nhiều trạng thái (Idle, Move, Jump, Attack, Dash, v.v), mỗi trạng thái có logic và animation riêng.
  - **Dễ mở rộng:** Muốn thêm trạng thái mới (ví dụ: Climb, Swim) chỉ cần tạo class mới, không ảnh hưởng code cũ.
  - **Tách biệt input và logic:** Input được xử lý ở từng state, tránh code rối.
  - **Thực tiễn:** Nếu không dùng state machine, code sẽ bị rối, khó kiểm soát bug khi nhiều trạng thái chồng chéo.

## 3. Enemy State Machine & Enemy Specify
- **EnemyStateMachine** quản lý trạng thái của enemy (Patrol, Chase, Attack, Die, v.v).
- Mỗi enemy có thể có các state riêng hoặc dùng chung base state.
- **EnemySpecify**: Cho phép định nghĩa hành vi đặc biệt cho từng loại enemy (boss nhiều phase, quái thường chỉ patrol/attack, v.v).
- Có thể mở rộng bằng cách kế thừa và override các state đặc biệt.
- **Vì sao enemy cũng dùng State Machine?**
  - **AI phức tạp:** Enemy có thể patrol, chase, attack, die, v.v. Mỗi hành vi là một state, dễ quản lý.
  - **Tùy biến từng loại enemy:** Dùng EnemySpecify để override hoặc mở rộng hành vi cho boss, quái đặc biệt.
  - **Dễ mở rộng:** Thêm enemy mới chỉ cần kế thừa base state hoặc thêm state riêng.
  - **Thực tiễn:** Nếu enemy chỉ có 1-2 hành vi thì không cần state machine, nhưng game có nhiều loại enemy phức tạp thì state machine là tối ưu.

## 4. Weapon System
- **WeaponInventory**: Quản lý các vũ khí player sở hữu (dưới dạng mảng, số slot cấu hình được trong Inspector).
- **WeaponSwap**: Xử lý logic nhặt, đổi, vứt vũ khí. Khi nhặt vũ khí mới, nếu còn slot trống sẽ thêm vào, nếu đầy sẽ hiện UI cho phép chọn vũ khí để đổi.
- **WeaponPickup**: Object vũ khí trên map, khi player lại gần và ấn E sẽ trigger logic nhặt vũ khí.
- **WeaponDataSO**: ScriptableObject lưu trữ dữ liệu từng loại vũ khí (icon, damage, mô tả, v.v).
- **Charge/Projectile/Attack**: Mỗi loại vũ khí có thể có logic riêng (bắn, chém, charge, v.v) thông qua các component chuyên biệt.
- **Vì sao dùng WeaponInventory, WeaponSwap, WeaponPickup, WeaponDataSO?**
  - **Quản lý nhiều vũ khí:** Player có thể nhặt, đổi, vứt nhiều loại vũ khí. Dùng mảng weaponData để dễ kiểm soát số lượng, loại vũ khí.
  - **Dữ liệu tách biệt:** Dùng ScriptableObject (WeaponDataSO) để lưu thông tin vũ khí, dễ chỉnh sửa, không cần sửa code.
  - **Tối ưu gameplay:** Khi inventory đầy, hiện UI cho phép chọn vũ khí để đổi, tạo chiều sâu chiến thuật.
  - **Thực tiễn:** Nếu chỉ cho cầm 1 vũ khí thì không cần inventory, nhưng để gameplay hấp dẫn, cần hệ thống này.

## 5. Interaction System
- **InteractableDetector**: Gắn cho player, tự động phát hiện object có thể tương tác gần nhất (vũ khí, bonfire, checkpoint, v.v).
- Khi ấn E, sẽ gọi hàm Interact() của object gần nhất, thực hiện logic tương ứng (nhặt vũ khí, rest, v.v).
- **IInteractable**: Interface chuẩn hóa các object có thể tương tác.
- **Vì sao dùng InteractableDetector & IInteractable?**
  - **Tự động phát hiện object có thể tương tác:** Không cần hardcode từng loại object, chỉ cần implement IInteractable là player có thể tương tác.
  - **Dễ mở rộng:** Thêm object mới (shop, NPC, cửa, v.v) chỉ cần implement IInteractable.
  - **Tránh bug:** Luôn chọn object gần nhất, tránh trường hợp ấn E mà không đúng ý người chơi.
  - **Thực tiễn:** Game có nhiều loại object tương tác, hệ thống này giúp code gọn, dễ bảo trì.

## 6. Data Persistence (Save/Load)
- **DataPersistenceManager**: Quản lý lưu/đọc dữ liệu game (JSON), chỉ lưu khi player rest tại bonfire/checkpoint.
- **GameData**: Lưu vị trí, máu, inventory, trạng thái boss, v.v.
- **RestPoint/Bonfire**: Khi player rest sẽ lưu game, hồi máu, cập nhật vị trí respawn.
- **Không auto-save khi chuyển scene hoặc quit game** (giống Dark Souls).
- **Vì sao chỉ save khi rest, không auto-save?**
  - **Tăng tính thử thách:** Giống Dark Souls, chỉ save khi rest tạo cảm giác hồi hộp, quyết định của người chơi có giá trị.
  - **Dễ kiểm soát bug:** Save/load tập trung vào 1 điểm, tránh lỗi khi chuyển scene hoặc quit game.
  - **Dễ mở rộng:** Muốn thêm dữ liệu gì chỉ cần mở rộng GameData và implement IDataPersistence.
  - **Thực tiễn:** Nếu auto-save liên tục sẽ khó debug, dễ phát sinh bug mất dữ liệu.

## 7. UI System
- **EquippedWeaponUI, WeaponInfoUI, WeaponSwapUI**: Hiển thị vũ khí đang cầm, thông tin vũ khí, UI chọn đổi vũ khí khi inventory đầy.
- **DebugUI**: Hiển thị thông tin debug (vị trí, số lần chết, trạng thái save, v.v).
- **UI cập nhật realtime** khi inventory hoặc trạng thái player thay đổi.
- **Vì sao tách UI thành nhiều component?**
  - **Dễ bảo trì:** Mỗi UI (vũ khí, info, swap, debug) là một script riêng, dễ sửa, dễ mở rộng.
  - **Realtime update:** UI tự động cập nhật khi inventory hoặc trạng thái player thay đổi, không cần code thủ công.
  - **Thực tiễn:** Nếu gộp UI vào 1 script sẽ rất rối, khó mở rộng khi thêm tính năng mới.

## 8. Object Pool System
- Tối ưu hiệu năng khi spawn/destroy nhiều object (projectile, hiệu ứng, v.v), tránh lag spike.
- Các object như đạn, hiệu ứng được lấy ra từ pool thay vì tạo mới liên tục.
- **Vì sao cần object pool?**
  - **Tối ưu hiệu năng:** Spawn/destroy nhiều object (đạn, hiệu ứng) liên tục sẽ gây lag, pool giúp tái sử dụng object, giảm GC.
  - **Dễ mở rộng:** Thêm loại object mới chỉ cần tạo pool mới.
  - **Thực tiễn:** Game action, platformer thường có nhiều đạn, hiệu ứng, pool là bắt buộc nếu muốn mượt.

## 9. Particles/Effects
- Quản lý hiệu ứng khi tấn công, chết, nhặt đồ, v.v.
- Dùng chung hệ thống pool để tối ưu hiệu năng.
- **Vì sao tách riêng hệ thống hiệu ứng?**
  - **Tăng trải nghiệm:** Hiệu ứng khi tấn công, chết, nhặt đồ giúp game sống động.
  - **Tối ưu:** Dùng chung object pool, tránh lag khi nhiều hiệu ứng xuất hiện cùng lúc.
  - **Thực tiễn:** Nếu không tách riêng, code hiệu ứng sẽ lẫn vào logic gameplay, khó bảo trì.

## 10. Luồng hoạt động tổng thể
1. **Player di chuyển, thực hiện hành động qua State Machine.**
2. **Khi lại gần object có thể tương tác (vũ khí, bonfire...), InteractableDetector sẽ highlight object đó.**
3. **Ấn E:**  
   - Nếu là vũ khí: Thêm vào inventory hoặc hiện UI đổi vũ khí nếu đầy.
   - Nếu là bonfire/checkpoint: Hồi máu, lưu game, cập nhật vị trí respawn.
4. **Enemy hoạt động theo EnemyStateMachine, có thể mở rộng hành vi riêng.**
5. **Khi player chết:** Respawn tại vị trí rest gần nhất, inventory giữ nguyên.
6. **Toàn bộ dữ liệu quan trọng được lưu vào GameData khi rest, không auto-save khi chuyển scene hoặc quit game.**
- **Vì sao thiết kế như vậy?**
  - **Tối ưu trải nghiệm:** Mỗi hệ thống tách biệt, phối hợp với nhau qua Core, giúp gameplay mượt, dễ mở rộng.
  - **Dễ debug:** Khi lỗi ở đâu chỉ cần kiểm tra hệ thống đó, không ảnh hưởng toàn bộ game.
  - **Thực tiễn:** Dự án lớn, nhiều người làm, kiến trúc này giúp teamwork hiệu quả, dễ chia module.

---

## Ưu điểm kiến trúc
- **Tách biệt rõ ràng từng hệ thống (core, state machine, inventory, interaction, save/load, UI...)**
- **Dễ mở rộng, bảo trì, thêm tính năng mới.**
- **Tối ưu hiệu năng với object pool.**
- **Dễ dàng debug, kiểm soát luồng dữ liệu.**

---

## tương lai mở rộng
- **Thêm enemy mới:** Dễ dàng nhờ state machine và EnemySpecify, chỉ cần kế thừa hoặc override state.
- **Thêm vũ khí mới:** Tạo WeaponDataSO mới, kéo vào prefab WeaponPickup, không cần sửa code.
- **Thêm hệ thống mới:** Chỉ cần tạo CoreComponent mới, gắn vào Core.
- **Thêm tính năng save mới:** Mở rộng GameData, implement IDataPersistence cho object cần lưu.
- **Thêm UI mới:** Tạo script UI riêng, kết nối với hệ thống core qua event hoặc observer.

---

**Với các giải thích này, bạn có thể trả lời mọi câu hỏi về:**
- Vì sao chọn kiến trúc này?
- Ưu điểm, nhược điểm từng hệ thống?
- Cách mở rộng, bảo trì, debug?
- So sánh với các cách làm khác (hardcode, không tách module, không dùng state machine, v.v).

**File này giúp trình bày rõ ràng kiến trúc, luồng hoạt động, và cách mở rộng dự án khi báo cáo!** 
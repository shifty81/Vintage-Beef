# Quick Test Guide

This guide will walk you through testing the core features of Vintage Beef v0.1.0.

## Test 1: Main Menu

**Objective:** Verify the main menu loads and buttons work

1. Open Unity Editor
2. Open MainMenu scene (Assets/Scenes/MainMenu.unity)
3. Press Play
4. **Verify:**
   - Title "VINTAGE BEEF" appears
   - "Play" button is visible and clickable
   - "Quit" button is visible
5. Click "Play" button
6. **Expected:** Scene transitions to Lobby

**Status:** [ ] Pass [ ] Fail

---

## Test 2: Profession Selection

**Objective:** Verify all 12 professions can be selected

1. From Main Menu, click "Play" to enter Lobby
2. **Verify:** All 12 professions are listed:
   - Farmer
   - Blacksmith
   - Builder
   - Miner
   - Hunter
   - Cook
   - Tailor
   - Merchant
   - Explorer
   - Engineer
   - Alchemist
   - Woodworker
3. Click on each profession
4. **Expected:** 
   - Profession highlights or shows as selected
   - Description updates
   - "Start Game" button becomes enabled
5. Select "Explorer" profession
6. Click "Start Game"
7. **Expected:** Scene transitions to GameWorld

**Status:** [ ] Pass [ ] Fail

---

## Test 3: Player Movement

**Objective:** Verify player controls work correctly

1. Start from GameWorld scene (after completing Test 2)
2. Test keyboard movement:
   - Press W → Character moves forward
   - Press S → Character moves backward
   - Press A → Character moves left
   - Press D → Character moves right
3. Test mouse look:
   - Move mouse left/right → Camera rotates horizontally
   - Move mouse up/down → Camera rotates vertically
4. Test jumping:
   - Press Space → Character jumps
5. Test sprinting:
   - Hold Left Shift + W → Character moves faster

**Expected Results:**
- Movement is smooth and responsive
- Camera follows mouse movement
- Jump has appropriate height
- Sprint is noticeably faster than walk
- No stuttering or lag

**Status:** [ ] Pass [ ] Fail

---

## Test 4: Dungeon Entrances

**Objective:** Verify dungeon entrances exist and can be interacted with

1. In GameWorld, look around for purple cube markers
2. **Verify:** At least 5 purple cubes are visible in the world
3. Walk toward a purple cube
4. When near (within ~3 meters):
   - **Expected:** Interaction prompt appears "[E] Enter [Dungeon Name]"
5. Press E key
6. **Expected:** Console message "Entering [Dungeon Name]!"

**Note:** Dungeons don't load yet - this is expected behavior in v0.1.0

**Status:** [ ] Pass [ ] Fail

---

## Test 5: World Boundaries

**Objective:** Verify the world has reasonable boundaries

1. In GameWorld, note your starting position
2. Walk in one direction for 30 seconds
3. **Verify:** 
   - Ground plane continues
   - No falling through floor
   - Can return to starting position

**Expected:**
- World extends far enough for exploration
- Ground is stable
- No visual glitches at world edges

**Status:** [ ] Pass [ ] Fail

---

## Test 6: Scene Transitions

**Objective:** Verify all scenes load correctly

1. **MainMenu → Lobby:**
   - From MainMenu, click "Play"
   - **Expected:** Lobby loads with profession selection
   
2. **Lobby → GameWorld:**
   - Select any profession
   - Click "Start Game"
   - **Expected:** GameWorld loads with player spawned

3. **Return to Menu:**
   - Press ESC to unlock cursor
   - Note: Currently no back button (limitation in v0.1.0)

**Status:** [ ] Pass [ ] Fail

---

## Test 7: Performance Check

**Objective:** Ensure acceptable performance

1. In GameWorld, press F to show FPS (may need to enable in Stats)
2. Walk around the world
3. **Verify FPS:**
   - High Quality: 60+ FPS
   - Medium Quality: 60+ FPS  
   - Low Quality: 60+ FPS

4. Check Unity Stats panel (Game view)
   - Draw calls: Should be < 100
   - Batches: Should be low
   - Triangles: Should be reasonable

**Status:** [ ] Pass [ ] Fail

---

## Test 8: UI Responsiveness

**Objective:** Verify UI elements work correctly

1. **Main Menu UI:**
   - Hover over buttons → Should highlight
   - Click buttons → Should respond
   
2. **Lobby UI:**
   - Click profession buttons → Should register selection
   - Text should be readable
   - Layout should be organized

3. **In-Game UI:**
   - Dungeon prompts appear correctly
   - Text is legible
   - No UI elements block gameplay

**Status:** [ ] Pass [ ] Fail

---

## Test 9: Camera Controls

**Objective:** Verify camera behaves correctly

1. In GameWorld, test camera:
   - Look up → Camera tilts up (max ~60°)
   - Look down → Camera tilts down (max ~60°)
   - Rotate 360° → Full horizontal rotation
   
2. **Verify:**
   - Camera doesn't clip through player
   - Smooth movement
   - No jittering
   - Cursor locked during gameplay

**Status:** [ ] Pass [ ] Fail

---

## Test 10: System Persistence

**Objective:** Verify core systems persist

1. From MainMenu, select a profession in Lobby
2. Enter GameWorld
3. **Verify:**
   - ProfessionManager exists (check Hierarchy)
   - PlayerData exists (check Hierarchy)
   - GameManager exists (check Hierarchy)
   - Selected profession is remembered

4. Check Console for any errors

**Status:** [ ] Pass [ ] Fail

---

## Known Issues (v0.1.0)

Document any issues found during testing:

- [ ] Issue 1: _______________
- [ ] Issue 2: _______________
- [ ] Issue 3: _______________

---

## Testing Environment

- Unity Version: _______________
- Operating System: _______________
- Graphics Card: _______________
- Tested on: [Date] _______________
- Tester Name: _______________

---

## Summary

Total Tests: 10
Passed: ____
Failed: ____
Pass Rate: ____%

**Overall Status:** [ ] Ready for Release [ ] Needs Fixes

---

## Notes

Add any additional observations or suggestions:

_____________________________________________________________________
_____________________________________________________________________
_____________________________________________________________________

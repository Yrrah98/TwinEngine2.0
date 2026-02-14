# Entity Type Reference

This document lists all available entity types that can be used in level JSON files.

## Available Entity Types

### Player
**Purpose:** Player-controlled character
**Components:** Position, Size, Sprite, Collider, Gravity, Input, Velocity
**Properties:**
- Size: 32x32
- Color: White
- Has Gravity: Yes
- Collidable: Yes
- Player Controlled: Yes
- Move Speed: 200 px/s

**Usage:**
```json
{
  "type": "Player",
  "x": 100,
  "y": 100
}
```

---

### Static_NPC
**Purpose:** Non-moving, solid obstacles or NPCs
**Components:** Position, Size, Sprite, Collider
**Properties:**
- Size: 16x16
- Color: Purple
- Has Gravity: No
- Collidable: Yes

**Usage:**
```json
{
  "type": "Static_NPC",
  "x": 200,
  "y": 200
}
```

---

### Platform
**Purpose:** Horizontal platforms for jumping/standing
**Components:** Position, Size, Sprite, Collider
**Properties:**
- Size: 64x16
- Color: Brown
- Has Gravity: No
- Collidable: Yes

**Usage:**
```json
{
  "type": "Platform",
  "x": 150,
  "y": 300
}
```

---

### Wall
**Purpose:** Vertical walls and barriers
**Components:** Position, Size, Sprite, Collider
**Properties:**
- Size: 16x64
- Color: Gray
- Has Gravity: No
- Collidable: Yes

**Usage:**
```json
{
  "type": "Wall",
  "x": 300,
  "y": 200
}
```

---

### Floating_Enemy
**Purpose:** Flying/hovering enemies that ignore gravity
**Components:** Position, Size, Sprite, Collider, Velocity
**Properties:**
- Size: 16x16
- Color: Red
- Has Gravity: No
- Collidable: Yes
- Move Speed: 100 px/s

**Usage:**
```json
{
  "type": "Floating_Enemy",
  "x": 250,
  "y": 150
}
```

---

### Ground_Enemy
**Purpose:** Ground-based enemies affected by gravity
**Components:** Position, Size, Sprite, Collider, Gravity, Velocity
**Properties:**
- Size: 16x16
- Color: Orange
- Has Gravity: Yes
- Collidable: Yes
- Move Speed: 80 px/s

**Usage:**
```json
{
  "type": "Ground_Enemy",
  "x": 180,
  "y": 300
}
```

---

### Decoration
**Purpose:** Non-interactive visual elements
**Components:** Position, Size, Sprite
**Properties:**
- Size: 16x16
- Color: Light Green
- Has Gravity: No
- Collidable: No

**Usage:**
```json
{
  "type": "Decoration",
  "x": 400,
  "y": 100
}
```

---

### Collectible
**Purpose:** Items that can be picked up (coins, power-ups, etc.)
**Components:** Position, Size, Sprite, Collider
**Properties:**
- Size: 12x12
- Color: Gold
- Has Gravity: No
- Collidable: Yes

**Usage:**
```json
{
  "type": "Collectible",
  "x": 350,
  "y": 250
}
```

---

## Example Level

```json
{
  "name": "Example Level",
  "description": "Shows various entity types",
  "entities": [
    {
      "type": "Player",
      "x": 100,
      "y": 300
    },
    {
      "type": "Platform",
      "x": 50,
      "y": 400
    },
    {
      "type": "Wall",
      "x": 300,
      "y": 250
    },
    {
      "type": "Floating_Enemy",
      "x": 250,
      "y": 200
    },
    {
      "type": "Collectible",
      "x": 150,
      "y": 350
    }
  ]
}
```

## Adding New Entity Types

To add a new entity type:

1. Open `Game/Templates/EntityTemplates.cs`
2. Add a new entry to the `_templates` dictionary
3. Define which components it should have and their default values
4. Rebuild the project
5. Use the new type name in your level JSON files

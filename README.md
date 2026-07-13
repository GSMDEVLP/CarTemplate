# CarTemplate

A vehicle combat game developed with Unity, featuring weaponized cars, AI-controlled opponents, and physics-based gameplay.

The project is currently in development.

## About

The player controls a weaponized vehicle and fights against AI-controlled opponents. The project focuses on vehicle physics, combat systems, different projectile types, and separation of gameplay responsibilities.

## Implemented Features

- Vehicle controller
- AI-controlled opponents
- Weapon system
- Standard and homing projectiles
- Health and damage system
- Vehicle collision handling
- Checkpoints and respawning
- Audio and visual feedback
- Gameplay user interface

## Technologies

- Unity 6
- C#
- Unity Physics
- ScriptableObject
- Event-driven architecture

## Architecture

The source code is organized by feature:

```text
Assets/Scripts/
├── Features/
│   ├── AI/
│   ├── Combat/
│   ├── Feedback/
│   ├── Respawn/
│   └── Vehicles/
├── Shared/
└── UI/
```

Main project areas:

- `Combat` — weapons, projectiles, and damage processing
- `Vehicles` — vehicle logic and health
- `AI` — opponent behavior
- `Respawn` — checkpoints and respawning
- `Feedback` — audio and visual effects
- `UI` — gameplay user interface

The event bus is intended for facts that have already occurred. Player commands are passed through dedicated interfaces and C# events.

## In Development

- Physics-based vehicle collision damage
- Reliable collision detection for fast projectiles
- Separation of events by responsibility
- Moving commands out of the global event bus
- Combat system tests

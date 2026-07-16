# CarTemplate

A vehicle combat game developed with Unity. The project combines physics-based driving, AI-controlled opponents, configurable weapons, and an event-driven combat pipeline.

The project is currently in development.

## Overview

The player controls a weaponized vehicle and fights AI-controlled opponents. The project focuses on separating gameplay, presentation, and infrastructure responsibilities while keeping combat systems configurable and extensible.

## Implemented Features

- Player vehicle controller
- AI-controlled opponents
- Configurable weapon system
- Standard, homing, rocket, and mine projectiles
- Centralized damage service
- Vehicle health and destruction
- Physics-based collision damage
- Checkpoints and respawning
- Player health, weapon, and vehicle telemetry UI

## Technologies

- Unity 6
- C#
- Unity Physics
- ScriptableObject configurations
- Event-driven architecture
- MVVM-style UI presentation

## Architecture

The source code is organized by feature:

```text
Assets/Scripts/
|-- Features/
|   |-- AI/
|   |-- Combat/
|   |-- Feedback/
|   |-- Respawn/
|   `-- Vehicles/
|-- Shared/
`-- UI/
```

Main project areas:

- `Combat` - weapons, projectiles, firing logic, and damage processing
- `Vehicles` - vehicle health, collision handling, and telemetry
- `AI` - combat decisions and target tracking
- `Respawn` - checkpoints and vehicle respawning
- `Feedback` - event-driven audio and visual feedback infrastructure
- `UI` - views, view models, and presentation contracts

The event bus primarily publishes facts that have already occurred. Moving the remaining input commands to dedicated interfaces and C# events is an ongoing architectural improvement.

## Weapon System

Weapons are configured with ScriptableObject assets and runtime modules. The configuration adapter converts editor data into weapon definitions, while the weapon service owns ammo, cooldown, heat, and firing state.

```text
WeaponConfig + Modules
          |
          v
 WeaponConfigAdapter
          |
          v
   WeaponDefinition
          |
          v
     WeaponService
          |
          v
  WeaponFireHandler
          |
          v
 IProjectileFactory
          |
          v
   Projectile Parts
```

Projectile behavior is composed from focused components for straight movement, homing, mines, lifetime, and damage. Adding a weapon primarily requires a new configuration and the appropriate projectile modules rather than duplicating firing logic.

## Damage Pipeline

Damage from projectiles, explosions, mines, and vehicle collisions is processed through a shared damage service:

```text
Projectile / Mine / Explosion / Collision
                    |
                    v
              IDamageService
                    |
                    v
          VehicleHealthAdapter
                    |
                    v
 DamageTaken / HitConfirmed / VehicleDestroyed
                    |
                    v
              UI / AI / Feedback
```

Entities are addressed by `EntityId`, allowing damage, destruction, respawn, and UI updates to be filtered for the correct vehicle.

## UI Separation

The gameplay UI does not depend directly on weapon, health, or vehicle controller implementations. It receives data through presentation contracts:

- `IWeaponHudSource`
- `IPlayerHealthSource`
- `IVehicleTelemetrySource`

View models transform this data into observable properties, and views react only to presentation state changes.

## Third-Party Vehicle Controller

The project currently uses third-party vehicle controller assets for player and AI driving behavior. Some vehicle-specific audio and visual effects are still managed directly by that code.

Refactoring those integrations into the event-driven feedback layer is intentionally deferred until the core gameplay tasks are complete.

## In Development

- Combat and damage pipeline tests
- Event-driven integration of vehicle VFX and SFX
- Refactoring of third-party vehicle controller feedback
- Moving the remaining commands out of the event bus
- Further separation of event buses by responsibility

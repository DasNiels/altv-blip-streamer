# ALT:V MP Server-side Blip Streamer
A server-side C# implementation of a blip streamer for ALT:V MP. Supports both global blips that show on the map at all times and blips that only show within X range of the player.

## Installation
- This resource makes use of the ``AltV.Net.EntitySync`` and ``AltV.Net.EntitySync.ServerEvent`` nuget package, make sure to install those prior to using this resource.
- Copy ``server-scripts/BlipStreamer.cs`` to your gamemode.
- Make sure to add the following code to your gamemode's OnStart() method(the object streamer won't work without it!):
```csharp
// Documentation: https://fabianterhorst.github.io/coreclr-module/articles/entity-sync.html
AltEntitySync.Init(1, 100, false,
   (threadCount, repository) => new ServerEventNetworkLayer(threadCount, repository),
   (entity, threadCount) => (entity.Id % threadCount), 
   (entityId, entityType, threadCount) => (entityId % threadCount),
   (threadId) => new LimitedGrid3(50_000, 50_000, 100, 10_000, 10_000, 300),
   new IdProvider());
```
- Copy ``blip-streamer`` to your ``server-root/resources`` directory.
- Add ``"blip-streamer"`` to your server config resources list.

## Usage
The following global methods are available:
```csharp
// Create a new dynamic blip on the map and returns the instance. A DynamicBlip can only be seen when the player is within X range of the blip.
DynamicBlip CreateDynamicBlip( ulong blipId, string name, int color, int scale, bool shortRange, int spriteId, Vector3 position, int dimension, uint range = 300 );

// Destroy a dynamic blip by it's instance.
void DestroyDynamicBlip( DynamicBlip blip );

// Create a new static blip that will show on the map at all times.
StaticBlip CreateStaticBlip( int blipId, string name, int spriteId, Vector3 position, int dimension, int color, int scale, bool shortRange );

// Destroy a static blip by it's instance.
void DestroyStaticBlip( StaticBlip blip );

// Destroy a static blip by it's blip ID.
void DestroyStaticBlip( int blipId );
```
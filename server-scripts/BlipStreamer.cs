using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.EntitySync;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace DasNiels.AltV.Streamers
{
    /// <summary>
    /// StaticBlip class that stores all data related to one static blip.
    /// </summary>
    public class StaticBlip : IWritable
    {
        public int BlipId { get; set; }
        public string Name { get; set; }
        public int SpriteId { get; set; }
        public int Color { get; set; }
        public int Scale { get; set; } = 1;
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int Dimension { get; set; } = 0;
        public bool ShortRange { get; set; } = true;

        public void OnWrite( IMValueWriter writer )
        {
            writer.BeginObject( );
            writer.Name( "blipId" );
            writer.Value( BlipId );
            writer.Name( "name" );
            writer.Value( Name );
            writer.Name( "spriteId" );
            writer.Value( SpriteId );
            writer.Name( "color" );
            writer.Value( Color );
            writer.Name( "scale" );
            writer.Value( Scale );
            writer.Name( "x" );
            writer.Value( X );
            writer.Name( "y" );
            writer.Value( Y );
            writer.Name( "z" );
            writer.Value( Z );
            writer.Name( "dimension" );
            writer.Value( Dimension );
            writer.Name( "shortRange" );
            writer.Value( ShortRange );
            writer.EndObject( );
        }

        /// <summary>
        /// Destroy this static blip
        /// </summary>
        public void Destroy( )
        {
            BlipStreamer.DestroyStaticBlip( this );
        }
    }

    /// <summary>
    /// DynamicBlip class that stores all data related to a single blip.
    /// </summary>
    public class DynamicBlip : global::AltV.Net.EntitySync.Entity, global::AltV.Net.EntitySync.IEntity
    {
        /// <summary>
        /// Unique ID to identify the blip by
        /// </summary>
        public ulong BlipId { get; set; }

        /// <summary>
        /// The text to display on the blip in the map menu
        /// </summary>
        public string Name
        {
            get
            {
                if( !TryGetData( "name", out string name ) )
                    return null;

                return name;
            }
            set
            {
                SetData( "name", value );
            }
        }

        /// <summary>
        /// ID of the sprite to use, can be found on the ALTV wiki
        /// </summary>
        public int SpriteId
        {
            get
            {
                if( !TryGetData( "spriteId", out int spriteId ) )
                    return 0;

                return spriteId;
            }
            set
            {
                SetData( "spriteId", value );
            }
        }

        /// <summary>
        /// Blip Color code, can also be found on the ALTV wiki
        /// </summary>
        public int Color
        {
            get
            {
                if( !TryGetData( "color", out int color ) )
                    return 0;

                return color;
            }
            set
            {
                SetData( "color", value );
            }
        }

        /// <summary>
        /// Scale of the blip, 1 is regular size.
        /// </summary>
        public int Scale
        {
            get
            {
                if( !TryGetData( "scale", out int scale ) )
                    return 1;

                return scale;
            }
            set
            {
                SetData( "scale", value );
            }
        }

        /// <summary>
        /// Whether this blip can be seen on the minimap from anywhere on the map, or only when close to it(it will always show on the main map).
        /// </summary>
        public bool ShortRange
        {
            get
            {
                if( !TryGetData( "shortRange", out bool shortRange ) )
                    return true;

                return shortRange;
            }
            set
            {
                SetData( "shortRange", value );
            }
        }

        public DynamicBlip( Vector3 position, int dimension, uint range, ulong entityType ) : base( entityType, position, dimension, range )
        {
        }

        /// <summary>
        /// Destroy this blip.
        /// </summary>
        public void Destroy( )
        {
            AltEntitySync.RemoveEntity( this );
        }
    }

    public static class BlipStreamer
    {
        public static readonly ulong ENTITY_TYPE_BLIP = 5;

        /// <summary>
        /// List to keep track of the static blips that dont require the streamer.
        /// </summary>
        public static List<StaticBlip> StaticBlips { get; set; } = new List<StaticBlip>( );

        /// <summary>
        /// Create a new dynamic blip that only shows up when a player is within the specified range.
        /// </summary>
        /// <param name="blipId">ID of the blip</param>
        /// <param name="name">Text to display</param>
        /// <param name="color">Blip color</param>
        /// <param name="scale">Blip scale</param>
        /// <param name="shortRange">Shortrange, doesnt have alot of use-cases on dynamic blips.</param>
        /// <param name="spriteId">The sprite ID of the blip</param>
        /// <param name="position">The position to spawn the blip at</param>
        /// <param name="dimension">The dimension the player has to be in in order to see the blip</param>
        /// <param name="range">The distance at which the blip can be seen from</param>
        /// <returns></returns>
        public static DynamicBlip CreateDynamicBlip( ulong blipId, string name, int color, int scale, bool shortRange, int spriteId, Vector3 position, int dimension, uint range = 300 )
        {
            DynamicBlip blip = new DynamicBlip( position, dimension, range, ENTITY_TYPE_BLIP )
            {
                BlipId = blipId,
                Name = name,
                Color = color,
                Scale = scale,
                ShortRange = shortRange,
                SpriteId = spriteId,
            };
            AltEntitySync.AddEntity( blip );
            return blip;
        }

        /// <summary>
        /// Create a static blip that can be seen from anywhere on the map
        /// </summary>
        /// <param name="blipId">ID of the blip</param>
        /// <param name="name">Text to display</param>
        /// <param name="spriteId">The sprite ID of the blip</param>
        /// <param name="position">The position to spawn the blip at</param>
        /// <param name="dimension">The dimension the player has to be in in order to see the blip</param>
        /// <param name="color">Blip color</param>
        /// <param name="scale">Blip scale</param>
        /// <param name="shortRange">Whether this blip can only be seen on the minimap when closer to it, or from anywhere on the map. Is usually false</param>
        /// <returns></returns>
        public static StaticBlip CreateStaticBlip( int blipId, string name, int spriteId, Vector3 position, int dimension, int color, int scale, bool shortRange )
        {
            StaticBlip blip = new StaticBlip( )
            {
                BlipId = blipId,
                Name = name,
                X = position.X,
                Y = position.Y,
                Z = position.Z,
                Color = color,
                Dimension = dimension,
                Scale = scale,
                ShortRange = shortRange,
                SpriteId = spriteId
            };

            StaticBlips.Add( blip );
            Alt.EmitAllClients( "StaticBlips:add", blip );

            return blip;
        }

        /// <summary>
        /// Destroy a dynamic blip
        /// </summary>
        /// <param name="blip">The blip to destroy</param>
        public static void DestroyDynamicBlip( DynamicBlip blip )
        {
            AltEntitySync.RemoveEntity( blip );
        }

        /// <summary>
        /// Destroy a static blip
        /// </summary>
        /// <param name="blip">The blip to destroy</param>
        public static void DestroyStaticBlip( StaticBlip blip )
        {
            Alt.EmitAllClients( "StaticBlips:remove", blip.BlipId );
            StaticBlips.Remove( blip );
        }

        /// <summary>
        /// Destroy a static blip by blip ID.
        /// </summary>
        /// <param name="blipId">The blip ID of the blip to destroy</param>
        public static void DestroyStaticBlip( int blipId )
        {
            StaticBlip blip = StaticBlips.FirstOrDefault( b => b.BlipId == blipId );

            if( blip == null )
                return;

            DestroyStaticBlip( blip );
        }
    }

    public class BlipStreamerEvents : IScript
    {
        public BlipStreamerEvents( )
        {
            AltAsync.OnPlayerConnect += OnPlayerConnect;
        }

        private async Task OnPlayerConnect( IPlayer player, string reason )
        {
            player.EmitLocked( "StaticBlips:init", BlipStreamer.StaticBlips );
        }
    }
}

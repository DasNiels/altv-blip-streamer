import * as alt from 'alt';
import * as natives from 'natives';

// array to store the created blips
let blips = new Map( );

// event that gets called from server when it needs to update the active blips(eg when a player joins or when a new blip is created that only needs to be shown within range)
alt.onServer( "StaticBlips:add", ( blipData ) => {
    if( !blips.has( blipData.blipId ) )
    {
        let blip = new alt.PointBlip( blipData.x, blipData.y, blipData.z );
        blip.sprite = +blipData.spriteId;
        blip.color = +blipData.color;
        blip.scale = +blipData.scale;
        blip.name = blipData.name;
        blip.shortRange = !!blipData.shortRange;
        natives.setBlipAsShortRange( +blip.scriptID, !!blipData.shortRange );

        blips.set( blipData.blipId, blip );
    }
} );

alt.onServer( "StaticBlips:remove", ( blipId ) => {
    alt.log( `remove blip: `, blipId );

    if( !blips.has( blipId ) )
        return;

    const blip = blips.get( blipId );
    blip.destroy( );
    blips.delete( blipId );
} );

alt.onServer( "StaticBlips:init", ( newBlips ) => {

    blips.forEach( b => {
        b.destroy( );
    } );

    blips = new Map( );

    newBlips.forEach( b => {
        let blip = new alt.PointBlip( b.x, b.y, b.z );
        blip.sprite = +b.spriteId;
        blip.color = +b.color;
        blip.scale = +b.scale;
        blip.name = b.name;
        blip.shortRange = !!b.shortRange;
        natives.setBlipAsShortRange( +blip.scriptID, !!b.shortRange );

        blips.set( b.blipId, blip );
    } );
} );

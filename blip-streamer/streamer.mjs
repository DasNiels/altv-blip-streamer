/*
    Developed by DasNiels/Niels/DingoDongBlueBalls
*/

import * as alt from 'alt';
import * as natives from 'natives';

class BlipStreamer {
    constructor() {
        this.blips = {};
    }

    add( entityId, name, spriteId, color, scale, shortRange, position, entityType ) {
        this.remove( +entityId );
        this.clear( +entityId );

        let blip = new alt.PointBlip( position.x, position.y, position.z );
        blip.sprite = +spriteId;
        blip.color = +color;
        blip.scale = +scale;
        blip.name = name;
        blip.shortRange = !!shortRange;
        natives.setBlipAsShortRange( +blip.scriptID, !!shortRange );

        this.blips[ entityId ] = {
            blip: blip,
            position: position,
            color: color,
            spriteId: spriteId,
            scale: scale,
            name: name,
            shortRange: shortRange,
            entityType: entityType,
        };
    }

    restore( entityId ) {
        if( this.blips.hasOwnProperty( entityId ) ) {
            let bData = this.blips[ entityId ];

            let blip = new alt.PointBlip( bData.position.x, bData.position.y, bData.position.z );
            blip.sprite = +bData.spriteId;
            blip.color = +bData.color;
            blip.scale = +bData.scale;
            blip.name = bData.name;
            blip.shortRange = !!bData.shortRange;
            natives.setBlipAsShortRange( +blip.scriptID, !!bData.shortRange );
            this.blips[ entityId ].blip = blip;
        }
    }

    remove( entityId ) {
        if( this.blips.hasOwnProperty( entityId ) ) {
            let bData = this.blips[ entityId ];
            bData.blip.destroy( );
            bData.blip = null;
        }
    }

    clear( entityId ) {
        if( this.blips.hasOwnProperty( entityId ) ) {
            delete this.blips[ entityId ];
        }
    }

    clearAll() {
        this.blips = {};
    }

    setPosition( entityId, pos ) {
        if( this.blips.hasOwnProperty( entityId ) ) {
            this.blips[ entityId ].position = pos;
            this.blips[ entityId ].blip.pos = pos;
        }
    }
}

export const blipStreamer = new BlipStreamer();

alt.on( "resourceStop", () => {
    blipStreamer.clearAll();
} );

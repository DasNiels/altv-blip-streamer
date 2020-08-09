import * as alt from 'alt';
import { blipStreamer } from "./streamer";

import 'blips';

const ENTITY_TYPE = 5;

// when an object is streamed in
alt.onServer( "entitySync:create", ( entityId, entityType, position, currEntityData ) => {
    if( currEntityData ) {
        let data = currEntityData;
        if( typeof data !== 'undefined') {
            if( +entityType === ENTITY_TYPE ) {
                blipStreamer.add( +entityId, data.name, data.spriteId, data.color, data.scale, data.shortRange, position, +entityType );
            }
        }
    } else {
        if( +entityType === ENTITY_TYPE ) {
            blipStreamer.restore( +entityId );
        }
    }
} );

// when an object is streamed out
alt.onServer( "entitySync:remove", ( entityId, entityType ) => {
    if( +entityType === ENTITY_TYPE ) {
        blipStreamer.remove( +entityId );
    }
} );

// when a streamed in object changes position data
alt.onServer( "entitySync:updatePosition", ( entityId, entityType, position ) => {
    if( +entityType === ENTITY_TYPE ) {
        blipStreamer.setPosition( +entityId, position );
    }
} );

// when a streamed in object changes data
alt.onServer( "entitySync:updateData", ( entityId, entityType, newEntityData ) => {
    if( +entityType === ENTITY_TYPE ) {
    }
} );

// when a streamed in object needs to be removed
alt.onServer( "entitySync:clearCache", ( entityId, entityType ) => {
    if( +entityType === ENTITY_TYPE ) {
        blipStreamer.clear( +entityId );
    }
} );

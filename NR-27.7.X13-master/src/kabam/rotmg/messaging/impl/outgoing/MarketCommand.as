package kabam.rotmg.messaging.impl.outgoing {
import Market.MarketOffer;

import flash.utils.IDataOutput;

public class MarketCommand extends OutgoingMessage {

    public static const REQUEST_MY_ITEMS:int = 0;
    public static const ADD_OFFER:int = 1;
    public static const REMOVE_OFFER:int = 2;

    public var commandId:int;
    public var offerId:uint;
    public var newOffers:Vector.<Market.MarketOffer>;

    public function MarketCommand(packetId:uint, callback:Function) {
        super(packetId, callback);
    }

    override public function writeToOutput(wtr:IDataOutput):void {
        wtr.writeByte(commandId);
        switch (commandId) {
            case REQUEST_MY_ITEMS:
                break;
            case ADD_OFFER:
                wtr.writeInt(newOffers.length);
                for each(var offer:MarketOffer in newOffers){
                    offer.writeToOutput(wtr);
                }
                break;
            case REMOVE_OFFER:
                wtr.writeUnsignedInt(offerId);
                break;
        }
    }
}
}

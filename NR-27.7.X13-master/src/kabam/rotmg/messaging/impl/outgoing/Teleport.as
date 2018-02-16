package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class Teleport extends OutgoingMessage {

    public var objectId_:int;
    public var isRecon:Boolean;

    public function Teleport(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(this.objectId_);
        _arg1.writeBoolean(this.isRecon);
    }

    override public function toString():String {
        return (formatToString("TELEPORT", "objectId_"));
    }


}
}

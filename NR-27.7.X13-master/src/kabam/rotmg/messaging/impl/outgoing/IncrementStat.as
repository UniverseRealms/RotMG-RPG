package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class IncrementStat extends OutgoingMessage {

    public var statType:int;

    public function IncrementStat(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeByte(this.statType);
    }

    override public function toString():String {
        return (formatToString("INCREMENTSTAT", "statType"));
    }


}
}

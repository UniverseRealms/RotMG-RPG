package Market
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class ObjectSlot
   {
       
      
      public var objectId_:int;
      
      public var slotId:int;
      
      public var objectType_:int;
      
      public function ObjectSlot()
      {
         super();
      }
      
      public function parseFromInput(param1:IDataInput) : void
      {
         this.objectId_ = param1.readInt();
         this.slotId = param1.readUnsignedByte();
         this.objectType_ = param1.readShort();
      }
      
      public function writeToOutput(param1:IDataOutput) : void
      {
         param1.writeInt(this.objectId_);
         param1.writeByte(this.slotId);
         param1.writeShort(this.objectType_);
      }
      
      public function toString() : String
      {
         return "objectId_: " + this.objectId_ + " slotId_: " + this.slotId + " objectType_: " + this.objectType_;
      }
   }
}

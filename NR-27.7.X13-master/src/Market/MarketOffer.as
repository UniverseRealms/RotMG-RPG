package Market
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;

   public class MarketOffer
   {
       
      
      public var price:int;
      
      public var objectSlot:ObjectSlot;
      
      public function MarketOffer()
      {
         super();
         this.objectSlot = new ObjectSlot();
      }
      
      public function parseFromInput(param1:IDataInput) : void
      {
         this.price = param1.readInt();
         this.objectSlot.parseFromInput(param1);
      }
      
      public function writeToOutput(param1:IDataOutput) : void
      {
         param1.writeInt(this.price);
         this.objectSlot.writeToOutput(param1);
      }
      
      public function toString() : String
      {
         return "price: " + this.price + " objectSlot: " + this.objectSlot.toString();
      }
   }
}

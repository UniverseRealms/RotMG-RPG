package Market
{
   import org.osflash.signals.Signal;
   
   public class MarketItemsResultSignal extends Signal
   {
       
      
      public function MarketItemsResultSignal()
      {
         super(Vector.<PlayerShopItem>);
      }
   }
}

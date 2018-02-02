package Market.ui
{
import Market.MarketNPCPanel;
import Market.MarketNPCPanelMediator;

import robotlegs.bender.framework.api.IConfig
   import org.swiftsuspenders.Injector;
   import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
   import Market.MarketItemsResultSignal;
   import Market.MarketResultSignal;
   
   public class MarketMediatorConfig implements IConfig
   {
       
      
      [Inject]
      public var injector:Injector;
      
      [Inject]
      public var mediatorMap:IMediatorMap;
      
      public function MarketMediatorConfig()
      {
         super();
      }
      
      public function configure() : void
      {
         this.injector.map(MarketItemsResultSignal).asSingleton();
         this.injector.map(MarketResultSignal).asSingleton();
         this.injector.map(MarketActionSignal).asSingleton();
         this.mediatorMap.map(MarketNPCPanel).toMediator(MarketNPCPanelMediator);
         this.mediatorMap.map(MarketOverview).toMediator(MarketMediator);
      }
   }
}

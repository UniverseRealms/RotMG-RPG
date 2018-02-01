package Market.ui
{
   import com.company.assembleegameclient.game.AGameSprite;
   import flash.events.MouseEvent;
   import robotlegs.bender.bundles.mvcs.Mediator
   import kabam.rotmg.dialogs.control.CloseDialogsSignal
   public class MarketMediator extends Mediator
   {
       
      
      [Inject]
      public var view:MarketOverview;
      
      [Inject]
      public var doAction:MarketActionSignal;
      
      [Inject]
      public var gs_:AGameSprite;

      public function MarketMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.gs_ = gs_;
         this.doAction.add(this.onAction);
      }
      
      private function onAction(param1:String) : void
      {
         this.gs_.gsc_.playerText(param1);
      }
      
      override public function destroy() : void
      {
         this.doAction.remove(this.onAction);
      }
   }
}

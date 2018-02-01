package Market
{
   import robotlegs.bender.bundles.mvcs.Mediator
   import kabam.rotmg.dialogs.control.OpenDialogSignal
   import kabam.rotmg.account.core.Account
   import flash.events.MouseEvent;
   import Market.ui.MarketOverview;
   
   public class MarketNPCPanelMediator extends Mediator
   {
       
      
      [Inject]
      public var view:MarketNPCPanel;
      
      [Inject]
      public var _06Z_:OpenDialogSignal;
      
      [Inject]
      public var dialogNoDim:OpenDialogSignal;
      
      [Inject]
      public var account:Account;
      
      public function MarketNPCPanelMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.view.init();
         this._Z_7();
      }
      
      private function _Z_7() : void
      {
         if(this.view._0J___)
         {
            this.view._0J___.addEventListener(MouseEvent.CLICK,this._0my);
            this.view._0R_w.addEventListener(MouseEvent.CLICK,this._ln);
         }
         else
         {
            this.view._0R_w.addEventListener(MouseEvent.CLICK,this._ln);
         }
      }
      
      override public function destroy() : void
      {
         super.destroy();
      }
      
      protected function _ln(param1:MouseEvent) : void
      {
         this._06Z_.dispatch(new InfoDialog());
      }
      
      protected function _0my(param1:MouseEvent) : void
      {
         this.dialogNoDim.dispatch(new MarketOverview());
      }
   }
}
